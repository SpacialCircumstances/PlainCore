using System;
using Veldrid;

namespace PlainCore
{
    public interface IShaderRepository
    {
        Shader LoadShader(GraphicsBackend backend, ShaderStages shaderType);
    }
}
