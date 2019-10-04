using Veldrid;

namespace PlainCore
{
    public interface IGraphicsContext
    {
        GraphicsDevice Device { get; }
        ResourceFactory Factory { get; }
    }
}
