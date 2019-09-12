using System.IO;
using Veldrid;
using Veldrid.SPIRV;

namespace PlainCore
{
    public class SpirvShaderRepository : IShaderRepository
    {
        private readonly GraphicsDevice device;
        private readonly string ShaderFolderPath;

        public SpirvShaderRepository(GraphicsDevice device, string shaderFolder)
        {
            this.device = device;
        }

        protected byte[] LoadShaderFile(string name)
        {
            var filename = Path.Combine(ShaderFolderPath, name);
            return File.ReadAllBytes(filename);
        }

        public Shader[] LoadShaders(GraphicsBackend backend)
        {
            var fragmentShaderBytes = LoadShaderFile("Fragment");
            var fragmentShaderDescription = new ShaderDescription(ShaderStages.Fragment, fragmentShaderBytes, "main");

            var vertexShaderBytes = LoadShaderFile("Vertex");
            var vertexShaderDescription = new ShaderDescription(ShaderStages.Vertex, vertexShaderBytes, "main");

            var options = new CrossCompileOptions(); //TODO
            return device.ResourceFactory.CreateFromSpirv(vertexShaderDescription, fragmentShaderDescription, options);
        }
    }
}
