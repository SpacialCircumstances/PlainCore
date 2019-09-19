using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition3Texture : IVertex
    {
        //See https://github.com/mellinoe/veldrid/issues/121 for the usage of VertexElementSemantic.TextureCoordinates
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription(Vertices.POSITION_ATTRIBUTE_NAME, VertexElementFormat.Float3, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription(Vertices.TEXCOORDS_ATTRIBUTE_NAME, VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate)
            );

        public VertexPosition3Texture(Vector3 position, Vector2 textureCoordinates)
        {
            Position = position;
            TextureCoordinates = textureCoordinates;
        }

        public Vector3 Position { get; }
        public Vector2 TextureCoordinates { get; }
        VertexLayoutDescription IVertex.VertexLayout => VertexLayout;
        public uint Size => 12 + 8;
        public override bool Equals(object obj)
        {
            return obj is VertexPosition3Texture texture &&
                   Position.Equals(texture.Position) &&
                   TextureCoordinates.Equals(texture.TextureCoordinates);
        }

        public override int GetHashCode()
        {
            var hashCode = -484238027;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + TextureCoordinates.GetHashCode();
            return hashCode;
        }
    }
}
