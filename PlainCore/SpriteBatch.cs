using PlainCore.Vertices;
using System;
using System.Numerics;
using Veldrid;

namespace PlainCore
{
    public class SpriteBatch
    {
        public SpriteBatch()
        {
            sprites = new ArrayBufferList<SpriteRenderItem>();
        }

        private readonly ArrayBufferList<SpriteRenderItem> sprites;
        private bool batching;

        public void Draw(Texture2D texture, Vector2 position, RgbaFloat color, Vector2 scale, float rotation = 0f, float depth = 0f)
        {
            Draw(texture, position, null, color, rotation, Vector2.Zero, scale, depth);
        }

        public void Draw(Texture2D texture, FloatRect destination, RgbaFloat color, float rotation = 0f, float depth = 0f)
        {
            Draw(texture, destination, null, color, rotation, Vector2.Zero, depth);
        }

        public void Draw(Texture2D texture, FloatRect destination, IntRect? sourceRectangle, RgbaFloat color, float rotation, Vector2 origin, float depth)
        {
            float width;
            float height;
            if (sourceRectangle.HasValue)
            {
                var sourceRect = sourceRectangle.Value;
                width = sourceRect.Width;
                height = sourceRect.Height;
            }
            else
            {
                width = texture.Width;
                height = texture.Height;
            }

            var scale = new Vector2(destination.Width / width, destination.Height / height);
            Draw(texture, destination.Position, sourceRectangle, color, rotation, origin, scale, depth);
        }

        public void Draw(Texture2D texture, Vector2 position, IntRect? sourceRectangle, RgbaFloat color, float rotation, Vector2 origin, Vector2 scale, float depth)
        {
            var actualOrigin = origin * scale;
            float w;
            float h;
            Vector2 texCoordsTL;
            Vector2 texCoordsBR;

            if (sourceRectangle.HasValue)
            {
                var sourceRect = sourceRectangle.GetValueOrDefault();
                w = sourceRect.Width * scale.X;
                h = sourceRect.Height * scale.Y;
                float wF = 1f / texture.Width;
                float hF = 1f / texture.Height;
                texCoordsTL = new Vector2(sourceRect.Left * wF, sourceRect.Top * hF);
                texCoordsBR = new Vector2(sourceRect.Right * wF, sourceRect.Bottom * hF);
            }
            else
            {
                w = texture.Width * scale.X;
                h = texture.Height * scale.Y;
                texCoordsTL = Vector2.Zero;
                texCoordsBR = Vector2.One;
            }

            Vector2 posTL;
            Vector2 posTR;
            Vector2 posBL;
            Vector2 posBR;

            if (rotation == 0f)
            {
                float x = position.X - actualOrigin.X;
                float y = position.Y - actualOrigin.Y;
                posTL = new Vector2(x, y);
                posTR = new Vector2(x + w, y);
                posBL = new Vector2(x, y + h);
                posBR = new Vector2(x + w, y + h);
            }
            else
            {
                float rotSin = (float)Math.Sin(rotation);
                float rotCos = (float)Math.Cos(rotation);
                float dx = -actualOrigin.X;
                float dy = -actualOrigin.Y;
                posTL = new Vector2(
                    position.X + (dx * rotCos) - (dy * rotSin),
                    position.Y + (dx * rotSin) - (dy * rotCos));
                posTR = new Vector2(
                    position.X + ((dx + w) * rotCos) - (dy * rotSin),
                    position.Y + ((dx + w) * rotSin) - (dy * rotCos));
                posBL = new Vector2(
                    position.X + (dx * rotCos) - ((dy + h) * rotSin),
                    position.Y + (dx * rotSin) - ((dy + h) * rotCos));
                posBR = new Vector2(
                    position.X + ((dx + w) * rotCos) - ((dy + h) * rotSin),
                    position.Y + ((dx + w) * rotSin) - ((dy + h) * rotCos));
            }

            var vertexTL = new VertexPosition3ColorTexture(new Vector3(posTL, depth), color, texCoordsTL);
            var vertexTR = new VertexPosition3ColorTexture(new Vector3(posTR, depth), color, new Vector2(texCoordsBR.X, texCoordsTL.Y));
            var vertexBL = new VertexPosition3ColorTexture(new Vector3(posBL, depth), color, new Vector2(texCoordsTL.X, texCoordsBR.Y));
            var vertexBR = new VertexPosition3ColorTexture(new Vector3(posBR, depth), color, texCoordsBR);

            var sprite = new SpriteRenderItem(vertexTL, vertexTR, vertexBL, vertexBR, texture);
            sprites.Add(sprite);
        }

        public void DrawText(string text, Font font, RgbaFloat color, float x, float y, float scale, float depth)
        {
            var texture = font.Texture;

            float currentX = x;
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                var glyph = font.Description.GetGlyph(character);
                Draw(texture, new Vector2(currentX, y), null, color, 0f, Vector2.Zero, new Vector2(glyph.GlyphSize.W * scale, glyph.GlyphSize.H * scale), depth);
                currentX += glyph.GlyphSize.W * scale;
            }
        }

        public void Draw(SpriteRenderItem sprite)
        {
            if (!batching)
            {
                throw new InvalidOperationException("Cannot draw before calling Begin()");
            }

            sprites.Add(sprite);
        }

        public void Begin()
        {
            if (batching)
            {
                throw new InvalidOperationException("Begin() was already called");
            }

            sprites.Clear();
            batching = true;
        }

        public void End()
        {
            if (!batching)
            {
                throw new InvalidOperationException("Begin() must be called before calling End()");
            }

            Array.Sort(sprites.Buffer, 0, sprites.Count);

            batching = false;
        }

        public ArrayBufferList<SpriteRenderItem> GetSprites()
        {
            if (batching)
            {
                throw new InvalidOperationException("Cannot draw before calling End()");
            }

            return sprites;
        }
    }
}
