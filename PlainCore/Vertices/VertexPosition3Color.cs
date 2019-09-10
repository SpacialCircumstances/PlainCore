using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition3Color : IVertex
    {
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementFormat.Float3, VertexElementSemantic.Position),
                new VertexElementDescription("Color", VertexElementFormat.Float4, VertexElementSemantic.Color)
            );

        public VertexPosition3Color(Vector3 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }

        public Vector3 Position { get; }
        public RgbaFloat Color { get; }
        VertexLayoutDescription IVertex.VertexLayout => VertexLayout;
        public uint Size => 12 + 16;

        public override bool Equals(object obj)
        {
            return obj is VertexPosition3Color color &&
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
