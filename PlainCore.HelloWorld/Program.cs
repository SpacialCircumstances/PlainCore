using System.Numerics;
using Veldrid;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;

namespace PlainCore.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var wci = new WindowCreateInfo()
            {
                X = 100,
                Y = 100,
                WindowWidth = 800,
                WindowHeight = 600,
                WindowTitle = "Hello World"
            };
            var window = VeldridStartup.CreateWindow(ref wci);

            var graphicsDevice = VeldridStartup.CreateGraphicsDevice(window);

            var spriteRenderer = new SpriteRenderer(graphicsDevice, graphicsDevice.SwapchainFramebuffer, (gb) =>
            {
                return graphicsDevice.ResourceFactory.CreateFromSpirv(Shaders.SpritebatchDefaultVertexShader, Shaders.SpritebatchDefaultFragmentShader);
            });
            spriteRenderer.Initialize();

            var spritebatch = new SpriteBatch();

            var texture = Texture2D.FromFile(graphicsDevice, "Planet.png");

            while (window.Exists)
            {
                window.PumpEvents();
            }

            graphicsDevice.Dispose();
        }
    }
}
