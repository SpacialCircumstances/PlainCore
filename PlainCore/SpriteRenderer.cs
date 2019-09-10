using PlainCore.Vertices;
using System;
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

        public SpriteRenderer(GraphicsDevice device)
        {
            this.device = device;
        }

        public void Initialize()
        {
            var factory = device.ResourceFactory;
            commandList = factory.CreateCommandList();

            var indexBufferDescription = new BufferDescription(sizeof(ushort) * MAX_BATCH * 6, BufferUsage.IndexBuffer | BufferUsage.Dynamic);
            indexBuffer = factory.CreateBuffer(indexBufferDescription);

            var vertexBufferDescription = new BufferDescription(VERTEX_SIZE * MAX_BATCH * 4, BufferUsage.VertexBuffer | BufferUsage.Dynamic);
            vertexBuffer = factory.CreateBuffer(vertexBufferDescription);
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
            device.UpdateBuffer(vertexBuffer, 0, (IntPtr)vertexArray, (uint)vertexIndex * VERTEX_SIZE);

            //TODO: Draw the stuff
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
