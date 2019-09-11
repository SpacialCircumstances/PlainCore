using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition2Color: IVertex
    {
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementFormat.Float2, VertexElementSemantic.Position),
                new VertexElementDescription("Color", VertexElementFormat.Float4, VertexElementSemantic.Color)
            );
        public VertexPosition2Color(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }

        public Vector2 Position { get; }
        public RgbaFloat Color { get; }
        VertexLayoutDescription IVertex.VertexLayout => VertexLayout;
        public uint Size => 8 + 16;

        public override bool Equals(object obj)
        {
            return obj is VertexPosition2Color color &&
                   Position.Equals(color.Position) &&
                   Color.Equals(color.Color);
        }

        public override int GetHashCode()
        {
            var hashCode = 7026542;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            return hashCode;
        }
    }
}
