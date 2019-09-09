using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PlainCore
{
    public struct VertexPositionTexture
    {
        public VertexPositionTexture(Vector2 position, Vector2 textureCoordinates)
        {
            Position = position;
            TextureCoordinates = textureCoordinates;
        }

        public Vector2 Position { get; }
        public Vector2 TextureCoordinates { get; }

        public override bool Equals(object obj)
        {
            return obj is VertexPositionTexture texture &&
                   Position.Equals(texture.Position) &&
                   TextureCoordinates.Equals(texture.TextureCoordinates);
        }

        public override int GetHashCode()
        {
            var hashCode = -484238027;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Position);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(TextureCoordinates);
            return hashCode;
        }
    }
}
