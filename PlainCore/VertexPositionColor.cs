using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPositionColor : IVertex
    {
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementFormat.Float2, VertexElementSemantic.Position),
                new VertexElementDescription("Color", VertexElementFormat.Float4, VertexElementSemantic.Color)
            );

        public VertexPositionColor(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }

        public Vector2 Position { get; }
        public RgbaFloat Color { get; }
        VertexLayoutDescription IVertex.VertexLayout => VertexLayout;

        public override bool Equals(object obj)
        {
            return obj is VertexPositionColor color &&
                   Position.Equals(color.Position) &&
                   Color.Equals(color.Color);
        }

        public override int GetHashCode()
        {
            var hashCode = -866678350;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            return hashCode;
        }
    }
}
