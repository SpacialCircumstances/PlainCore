using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition3ColorTexture : IVertex
    {
        //See https://github.com/mellinoe/veldrid/issues/121 for the usage of VertexElementSemantic.TextureCoordinates
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription(Vertices.POSITION_ATTRIBUTE_NAME, VertexElementFormat.Float3, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription(Vertices.COLOR_ATTRIBUTE_NAME, VertexElementFormat.Float4, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription(Vertices.TEXCOORDS_ATTRIBUTE_NAME, VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate)
            );

        public VertexPosition3ColorTexture(Vector3 position, RgbaFloat color, Vector2 textureCoordinates)
        {
            Position = position;
            Color = color;
            TextureCoordinates = textureCoordinates;
        }

        public Vector3 Position { get; }
        public RgbaFloat Color { get; }
        public Vector2 TextureCoordinates { get; }
        VertexLayoutDescription IVertex.VertexLayout => VertexLayout;
        public uint Size => 12 + 16 + 8;
        public override bool Equals(object obj)
        {
            return obj is VertexPosition3ColorTexture texture &&
                   Position.Equals(texture.Position) &&
                   Color.Equals(texture.Color) &&
                   TextureCoordinates.Equals(texture.TextureCoordinates);
        }

        public override int GetHashCode()
        {
            var hashCode = 962000893;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            hashCode = hashCode * -1521134295 + TextureCoordinates.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(VertexPosition3ColorTexture left, VertexPosition3ColorTexture right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPosition3ColorTexture left, VertexPosition3ColorTexture right)
        {
            return !(left == right);
        }
    }
}
