using PlainCore.Vertices;
using System;
using System.Numerics;
using Veldrid;

namespace PlainCore
{
    public class SpriteRenderer
    {
        private const int MAX_BATCH = 1024;
        private static readonly uint VERTEX_SIZE = default(VertexPosition3ColorTexture).Size;

        private readonly GraphicsDevice device;
        private CommandList commandList;
        private ushort[] indices = new ushort[64 * 6];
        private VertexPosition3ColorTexture[] vertices = new VertexPosition3ColorTexture[64 * 4];

        private DeviceBuffer indexBuffer;
        private DeviceBuffer vertexBuffer;
        private DeviceBuffer worldMatrixBuffer;
        private readonly Framebuffer framebuffer;
        private Pipeline pipeline;
        private ResourceLayout viewResourceLayout;
        private ResourceLayout graphicsResourceLayout;
        private readonly Func<GraphicsBackend, Shader[]> loadShaders;

        public SpriteRenderer(GraphicsDevice device, Framebuffer framebuffer, Func<GraphicsBackend, Shader[]> loadShaders)
        {
            this.device = device;
            this.framebuffer = framebuffer;
            this.loadShaders = loadShaders;
        }

        public void Initialize()
        {
            var factory = device.ResourceFactory;
            commandList = factory.CreateCommandList();

            var indexBufferDescription = new BufferDescription(sizeof(ushort) * MAX_BATCH * 6, BufferUsage.IndexBuffer | BufferUsage.Dynamic);
            indexBuffer = factory.CreateBuffer(indexBufferDescription);

            var vertexBufferDescription = new BufferDescription(VERTEX_SIZE * MAX_BATCH * 4, BufferUsage.VertexBuffer | BufferUsage.Dynamic);
            vertexBuffer = factory.CreateBuffer(vertexBufferDescription);

            var worldMatrixBufferDescription = new BufferDescription(64, BufferUsage.UniformBuffer);
            worldMatrixBuffer = factory.CreateBuffer(worldMatrixBufferDescription);

            var viewMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, 800f, 600f, 0f, 0f, 10.0f);
            device.UpdateBuffer(worldMatrixBuffer, 0, viewMatrix);

            viewResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(new ResourceLayoutElementDescription("WorldView", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

            graphicsResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                new ResourceLayoutElementDescription("Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                new ResourceLayoutElementDescription("TextureSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            var shaders = loadShaders(device.BackendType);
            var shaderSet = new ShaderSetDescription(new[] { VertexPosition3ColorTexture.VertexLayout }, shaders);

            var pipelineDescription = new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend, 
                DepthStencilStateDescription.DepthOnlyLessEqual,
                RasterizerStateDescription.CullNone,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { viewResourceLayout, graphicsResourceLayout },
                framebuffer.OutputDescription);
            pipeline = factory.CreateGraphicsPipeline(pipelineDescription);
        }

        public unsafe void Render(SpriteBatch batch)
        {
            var sprites = batch.GetSprites();
            int spriteCount = sprites.Length;
            int batchIndex = 0;

            EnsureIndices(Math.Min(spriteCount, MAX_BATCH));

            while (spriteCount > 0)
            {
                var index = 0;
                Texture2D tex = null;

                int batchSize = Math.Min(spriteCount, MAX_BATCH);

                fixed (VertexPosition3ColorTexture* vertexArrayFixedPtr = vertices)
                {
                    var vertexArrayPtr = vertexArrayFixedPtr;

                    for (int i = 0; i < batchSize; i++, batchIndex++, index += 4, vertexArrayPtr += 4)
                    {
                        var item = sprites[batchIndex];

                        var shouldFlush = !ReferenceEquals(item.Texture, tex);
                        if (shouldFlush)
                        {
                            FlushVertexArray(vertexArrayFixedPtr, index, tex);

                            tex = item.Texture;
                            index = 0;
                            vertexArrayPtr = vertexArrayFixedPtr;
                        }

                        *(vertexArrayPtr + 0) = item.TopLeft;
                        *(vertexArrayPtr + 1) = item.TopRight;
                        *(vertexArrayPtr + 2) = item.BottomLeft;
                        *(vertexArrayPtr + 3) = item.BottomRight;
                    }

                    FlushVertexArray(vertexArrayFixedPtr, index, tex);
                }

                spriteCount -= batchSize;
            }
        }

        protected unsafe void FlushVertexArray(VertexPosition3ColorTexture* vertexArray, int vertexIndex, Texture2D texture)
        {
            if (texture == null) return;

            var vrsd = new ResourceSetDescription(viewResourceLayout, worldMatrixBuffer);
            var viewResourceSet = device.ResourceFactory.CreateResourceSet(vrsd);

            var grsd = new ResourceSetDescription(graphicsResourceLayout, texture.TextureView, device.PointSampler);
            var graphicsResourceSet = device.ResourceFactory.CreateResourceSet(grsd);

            device.UpdateBuffer(vertexBuffer, 0, (IntPtr)vertexArray, (uint)vertexIndex * VERTEX_SIZE);

            //TODO: Depth buffer
            commandList.Begin();
            commandList.SetFramebuffer(framebuffer);
            commandList.SetFullViewports();
            commandList.SetPipeline(pipeline);
            commandList.SetGraphicsResourceSet(0, viewResourceSet);
            commandList.SetGraphicsResourceSet(1, graphicsResourceSet);
            commandList.SetIndexBuffer(indexBuffer, IndexFormat.UInt16);
            commandList.SetVertexBuffer(0, vertexBuffer);
            commandList.DrawIndexed((uint)vertexIndex);
            commandList.End();
            device.SubmitCommands(commandList);
        }

        protected void EnsureIndices(int spriteCount)
        {
            int indiceCount = spriteCount * 6;

            if (indices.Length < indiceCount)
            {
                indices = new ushort[indiceCount];
                for (int i = 0; i < spriteCount; i++)
                {
                    int idx = i * 6;
                    int vertex = i * 6;
                    indices[idx] = (ushort)vertex;
                    indices[idx + 1] = (ushort)(vertex + 1);
                    indices[idx + 2] = (ushort)(vertex + 2);
                    indices[idx + 3] = (ushort)(vertex + 1);
                    indices[idx + 4] = (ushort)(vertex + 3);
                    indices[idx + 5] = (ushort)(vertex + 2);
                }

                device.UpdateBuffer(indexBuffer, 0, indices);
            }

            if (vertices.Length < spriteCount * 4)
            {
                vertices = new VertexPosition3ColorTexture[spriteCount * 4];
            }
        }
    }
}
