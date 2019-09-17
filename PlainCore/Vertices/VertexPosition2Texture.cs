using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore.Vertices
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition2Texture : IVertex
    {
        //See https://github.com/mellinoe/veldrid/issues/121 for the usage of VertexElementSemantic.TextureCoordinates
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate),
                new VertexElementDescription("TextureCoordinates", VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate)
            );

        public VertexPosition2Texture(Vector2 position, Vector2 textureCoordinates)
        {
            Position = position;
            TextureCoordinates = textureCoordinates;
        }

        public Vector2 Position { get; }
        public Vector2 TextureCoordinates { get; }
        VertexLayoutDescription IVertex.VertexLayout => VertexLayout;
        public uint Size => 8 + 8;
        public override bool Equals(object obj)
        {
            return obj is VertexPosition2Texture texture &&
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
