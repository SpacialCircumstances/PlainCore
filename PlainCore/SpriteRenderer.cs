﻿using PlainCore.Vertices;
using System;
using Veldrid;
using Veldrid.SPIRV;

namespace PlainCore
{
    public sealed class SpriteRenderer : IDisposable
    {
        private const int MAX_BATCH = 1024;
        private static readonly uint VERTEX_SIZE = default(VertexPosition3ColorTexture).Size;

        private readonly GraphicsDevice device;
        private readonly ResourceFactory factory;
        private CommandList commandList;
        private ushort[] indices = Array.Empty<ushort>();
        private VertexPosition3ColorTexture[] vertices = new VertexPosition3ColorTexture[64 * 4];

        private DeviceBuffer indexBuffer;
        private DeviceBuffer vertexBuffer;
        private DeviceBuffer worldMatrixBuffer;
        private readonly Framebuffer framebuffer;
        private Pipeline pipeline;
        private ResourceLayout viewResourceLayout;
        private ResourceLayout graphicsResourceLayout;
        private ShaderSetDescription shaderSet;

        public SpriteRenderer(Window window, BlendStateDescription? bsd = null) : this(window, window.Framebuffer, bsd)
        {
        }

        public SpriteRenderer(IGraphicsContext context, Framebuffer framebuffer, BlendStateDescription? blendStateDescription = null)
        {
            this.device = context.Device;
            this.factory = context.Factory;
            this.framebuffer = framebuffer;
            var shaders = factory.CreateFromSpirv(Shaders.SpritebatchDefaultVertexShader, Shaders.SpritebatchDefaultFragmentShader);
            this.shaderSet = new ShaderSetDescription(new[] { VertexPosition3ColorTexture.VertexLayout }, shaders);
            Initialize(blendStateDescription ?? BlendStateDescription.SingleAlphaBlend);
        }

        public SpriteRenderer(GraphicsDevice device, ResourceFactory factory, Framebuffer framebuffer, ShaderSetDescription shaderSet, BlendStateDescription? blendStateDescription = null)
        {
            this.device = device;
            this.factory = factory;
            this.framebuffer = framebuffer;
            this.shaderSet = shaderSet;
            Initialize(blendStateDescription ?? BlendStateDescription.SingleAlphaBlend);
        }

        private void Initialize(BlendStateDescription? blendStateDescription = null)
        {
            commandList = factory.CreateCommandList();

            var indexBufferDescription = new BufferDescription(sizeof(ushort) * MAX_BATCH * 6, BufferUsage.IndexBuffer | BufferUsage.Dynamic);
            indexBuffer = factory.CreateBuffer(indexBufferDescription);

            var vertexBufferDescription = new BufferDescription(VERTEX_SIZE * MAX_BATCH * 4, BufferUsage.VertexBuffer | BufferUsage.Dynamic);
            vertexBuffer = factory.CreateBuffer(vertexBufferDescription);

            var worldMatrixBufferDescription = new BufferDescription(64, BufferUsage.UniformBuffer);
            worldMatrixBuffer = factory.CreateBuffer(worldMatrixBufferDescription);

            viewResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(new ResourceLayoutElementDescription("WorldView", ResourceKind.UniformBuffer, ShaderStages.Vertex)));

            graphicsResourceLayout = factory.CreateResourceLayout(new ResourceLayoutDescription(
                new ResourceLayoutElementDescription("Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                new ResourceLayoutElementDescription("TextureSampler", ResourceKind.Sampler, ShaderStages.Fragment)));

            var rasterizerStateDescription = new RasterizerStateDescription(FaceCullMode.None, PolygonFillMode.Solid, FrontFace.Clockwise, true, true);

            var pipelineDescription = new GraphicsPipelineDescription(
                blendStateDescription ?? BlendStateDescription.SingleAlphaBlend,
                DepthStencilStateDescription.DepthOnlyGreaterEqual,
                rasterizerStateDescription,
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] { viewResourceLayout, graphicsResourceLayout },
                framebuffer.OutputDescription);
            pipeline = factory.CreateGraphicsPipeline(pipelineDescription);
        }

        public unsafe void Render(SpriteBatch batch, View view, Sampler sampler = null, IntRect? scissorRect = null)
        {
            var sprites = batch.GetSprites();
            int spriteCount = sprites.Count;
            int batchIndex = 0;

            device.UpdateBuffer(worldMatrixBuffer, 0, view.WorldMatrix);

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
                            FlushVertexArray(vertexArrayFixedPtr, index, tex, view, sampler, scissorRect);

                            tex = item.Texture;
                            index = 0;
                            vertexArrayPtr = vertexArrayFixedPtr;
                        }

                        *(vertexArrayPtr + 0) = item.TopLeft;
                        *(vertexArrayPtr + 1) = item.TopRight;
                        *(vertexArrayPtr + 2) = item.BottomLeft;
                        *(vertexArrayPtr + 3) = item.BottomRight;
                    }

                    FlushVertexArray(vertexArrayFixedPtr, index, tex, view, sampler, scissorRect);
                }

                spriteCount -= batchSize;
            }
        }

        private unsafe void FlushVertexArray(VertexPosition3ColorTexture* vertexArray, int vertexIndex, Texture2D texture, View view, Sampler sampler, IntRect? scissorRect)
        {
            if (texture == null) return;

            var vrsd = new ResourceSetDescription(viewResourceLayout, worldMatrixBuffer);
            var viewResourceSet = factory.CreateResourceSet(vrsd);

            var grsd = new ResourceSetDescription(graphicsResourceLayout, texture.TextureView, sampler ?? device.PointSampler);
            var graphicsResourceSet = factory.CreateResourceSet(grsd);

            //TODO: Depth buffer
            commandList.Begin();
            commandList.UpdateBuffer(vertexBuffer, 0, (IntPtr)vertexArray, (uint)vertexIndex * VERTEX_SIZE);
            commandList.SetFramebuffer(framebuffer);
            commandList.SetViewport(0, view.ScreenView);
            if (scissorRect.HasValue)
            {
                var sr = scissorRect.Value;
                commandList.SetScissorRect(0, (uint)sr.X, (uint)sr.Y, (uint)sr.Width, (uint)sr.Height);
            }
            commandList.SetPipeline(pipeline);
            commandList.SetGraphicsResourceSet(0, viewResourceSet);
            commandList.SetGraphicsResourceSet(1, graphicsResourceSet);
            commandList.SetIndexBuffer(indexBuffer, IndexFormat.UInt16);
            commandList.SetVertexBuffer(0, vertexBuffer);
            commandList.DrawIndexed((uint)(vertexIndex * 1.5));
            commandList.End();
            device.SubmitCommands(commandList);

            viewResourceSet.Dispose();
            graphicsResourceSet.Dispose();
        }

        private void EnsureIndices(int spriteCount)
        {
            int indiceCount = spriteCount * 6;

            if (indices.Length < indiceCount)
            {
                indices = new ushort[indiceCount];
                for (int i = 0; i < spriteCount; i++)
                {
                    int idx = i * 6;
                    int vertex = i * 4;
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

        public void Dispose()
        {
            commandList.Dispose();
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
            worldMatrixBuffer.Dispose();
            pipeline.Dispose();
            viewResourceLayout.Dispose();
            graphicsResourceLayout.Dispose();
        }
    }
}
