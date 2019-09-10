using PlainCore.Vertices;
using System;
using Veldrid;

namespace PlainCore
{
    public class SpriteRenderer
    {
        private const int MAX_BATCH = 1024;

        private readonly GraphicsDevice device;
        private CommandList commandList;
        private ushort[] indices = new ushort[64 * 6];
        private VertexPosition3ColorTexture[] vertices = new VertexPosition3ColorTexture[64 * 4];

        public SpriteRenderer(GraphicsDevice device)
        {
            this.device = device;
        }

        public void Initialize()
        {
            commandList = device.ResourceFactory.CreateCommandList();
        }

        public void Render(SpriteBatch batch)
        {
            var sprites = batch.GetSprites();
            int spriteCount = sprites.Length;

            EnsureIndices(Math.Min(spriteCount, MAX_BATCH));

            while (spriteCount > 0)
            {

            }
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
            }

            if (vertices.Length < spriteCount * 4)
            {
                vertices = new VertexPosition3ColorTexture[spriteCount * 4];
            }
        }
    }
}
