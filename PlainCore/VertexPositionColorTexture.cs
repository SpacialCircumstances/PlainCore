using System.Collections.Generic;
using System.Numerics;
using Veldrid;

namespace PlainCore
{
    public struct VertexPositionColorTexture
    {
        public VertexPositionColorTexture(Vector2 position, RgbaFloat color, Vector2 textureCoordinates)
        {
            Position = position;
            Color = color;
            TextureCoordinates = textureCoordinates;
        }

        public Vector2 Position { get; }
        public RgbaFloat Color { get; }
        public Vector2 TextureCoordinates { get; }

        public override bool Equals(object obj)
        {
            return obj is VertexPositionColorTexture texture &&
                   Position.Equals(texture.Position) &&
                   Color.Equals(texture.Color) &&
                   TextureCoordinates.Equals(texture.TextureCoordinates);
        }

        public override int GetHashCode()
        {
            var hashCode = 962000893;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + EqualityComparer<RgbaFloat>.Default.GetHashCode(Color);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(TextureCoordinates);
            return hashCode;
        }
    }
}
