using System;
using Veldrid;

namespace PlainCore
{
    public interface IShaderRepository
    {
        Shader[] LoadShaders(GraphicsBackend backend);
    }
}
