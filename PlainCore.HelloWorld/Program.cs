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

            var window = new WindowBuilder()
                            .Build();

            var graphicsDevice = window.Device;
            var factory = window.Factory;

            var spriteRenderer = new SpriteRenderer(graphicsDevice, factory, graphicsDevice.SwapchainFramebuffer, (gb) =>
            {
                return graphicsDevice.ResourceFactory.CreateFromSpirv(Shaders.SpritebatchDefaultVertexShader, Shaders.SpritebatchDefaultFragmentShader);
            });
            spriteRenderer.Initialize();

            var spritebatch = new SpriteBatch();

            var texture = Texture2D.FromFile(graphicsDevice, graphicsDevice.ResourceFactory, "Planet.png");

            while (window.IsOpen)
            {
                window.WindowHandle.PumpEvents();

                window.Clear(RgbaFloat.Black);

                spritebatch.Begin();
                spritebatch.Draw(texture, new Vector2(0, 0), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.End();

                spriteRenderer.Render(spritebatch, window.MainView);

                window.Display();
            }

            graphicsDevice.Dispose();
        }
    }
}
