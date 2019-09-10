using Veldrid;

namespace PlainCore.Vertices
{
    public interface IVertex
    {
        VertexLayoutDescription VertexLayout { get; }
        uint Size { get; }
    }
}
