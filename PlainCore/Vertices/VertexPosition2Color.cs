using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition2Color : IVertex
    {
        //See https://github.com/mellinoe/veldrid/issues/121 for the usage of VertexElementSemantic.TextureCoordinates
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription(Vertices.POSITION_ATTRIBUTE_NAME, VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription(Vertices.COLOR_ATTRIBUTE_NAME, VertexElementFormat.Float4, VertexElementSemantic.TextureCoordinate)
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

        public static bool operator ==(VertexPosition2Color left, VertexPosition2Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPosition2Color left, VertexPosition2Color right)
        {
            return !(left == right);
        }
    }
}
