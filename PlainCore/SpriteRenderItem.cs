using PlainCore.Vertices;
using System;
using System.Collections.Generic;

namespace PlainCore
{
    public struct SpriteRenderItem : IComparable<SpriteRenderItem>
    {
        public SpriteRenderItem(VertexPosition3ColorTexture topLeft, VertexPosition3ColorTexture topRight, VertexPosition3ColorTexture bottomLeft, VertexPosition3ColorTexture bottomRight, Texture2D texture)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            Texture = texture;
        }

        public VertexPosition3ColorTexture TopLeft { get; }
        public VertexPosition3ColorTexture TopRight { get; }
        public VertexPosition3ColorTexture BottomLeft { get; }
        public VertexPosition3ColorTexture BottomRight { get; }
        public Texture2D Texture { get; }

        public int CompareTo(SpriteRenderItem other)
        {
            return Texture.Id.CompareTo(other.Texture.Id);
        }

        public override bool Equals(object obj)
        {
            return obj is SpriteRenderItem item &&
                   EqualityComparer<VertexPosition3ColorTexture>.Default.Equals(TopLeft, item.TopLeft) &&
                   EqualityComparer<VertexPosition3ColorTexture>.Default.Equals(TopRight, item.TopRight) &&
                   EqualityComparer<VertexPosition3ColorTexture>.Default.Equals(BottomLeft, item.BottomLeft) &&
                   EqualityComparer<VertexPosition3ColorTexture>.Default.Equals(BottomRight, item.BottomRight) &&
                   EqualityComparer<Texture2D>.Default.Equals(Texture, item.Texture);
        }

        public override int GetHashCode()
        {
            var hashCode = 1905714886;
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPosition3ColorTexture>.Default.GetHashCode(TopLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPosition3ColorTexture>.Default.GetHashCode(TopRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPosition3ColorTexture>.Default.GetHashCode(BottomLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPosition3ColorTexture>.Default.GetHashCode(BottomRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(Texture);
            return hashCode;
        }

        public static bool operator ==(SpriteRenderItem left, SpriteRenderItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SpriteRenderItem left, SpriteRenderItem right)
        {
            return !(left == right);
        }

        public static bool operator <(SpriteRenderItem left, SpriteRenderItem right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(SpriteRenderItem left, SpriteRenderItem right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(SpriteRenderItem left, SpriteRenderItem right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(SpriteRenderItem left, SpriteRenderItem right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
