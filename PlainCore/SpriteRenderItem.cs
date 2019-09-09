using System;
using System.Collections.Generic;
using System.Text;

namespace PlainCore
{
    public struct SpriteRenderItem
    {
        public SpriteRenderItem(VertexPositionColorTexture topLeft, VertexPositionColorTexture topRight, VertexPositionColorTexture bottomLeft, VertexPositionColorTexture bottomRight, Texture2D texture)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
            Texture = texture;
        }

        public VertexPositionColorTexture TopLeft { get; }
        public VertexPositionColorTexture TopRight { get; }
        public VertexPositionColorTexture BottomLeft { get; }
        public VertexPositionColorTexture BottomRight { get; }
        public Texture2D Texture { get; }

        public override bool Equals(object obj)
        {
            return obj is SpriteRenderItem item &&
                   EqualityComparer<VertexPositionColorTexture>.Default.Equals(TopLeft, item.TopLeft) &&
                   EqualityComparer<VertexPositionColorTexture>.Default.Equals(TopRight, item.TopRight) &&
                   EqualityComparer<VertexPositionColorTexture>.Default.Equals(BottomLeft, item.BottomLeft) &&
                   EqualityComparer<VertexPositionColorTexture>.Default.Equals(BottomRight, item.BottomRight) &&
                   EqualityComparer<Texture2D>.Default.Equals(Texture, item.Texture);
        }

        public override int GetHashCode()
        {
            var hashCode = 1905714886;
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPositionColorTexture>.Default.GetHashCode(TopLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPositionColorTexture>.Default.GetHashCode(TopRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPositionColorTexture>.Default.GetHashCode(BottomLeft);
            hashCode = hashCode * -1521134295 + EqualityComparer<VertexPositionColorTexture>.Default.GetHashCode(BottomRight);
            hashCode = hashCode * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(Texture);
            return hashCode;
        }
    }
}
