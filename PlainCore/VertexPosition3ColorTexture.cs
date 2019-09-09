using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;

namespace PlainCore
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct VertexPosition3ColorTexture : IVertex
    {
        public static readonly VertexLayoutDescription VertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementFormat.Float3, VertexElementSemantic.Position),
                new VertexElementDescription("Color", VertexElementFormat.Float4, VertexElementSemantic.Color),
                new VertexElementDescription("TextureCoordinates", VertexElementFormat.Float2, VertexElementSemantic.TextureCoordinate)
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
    }
}
