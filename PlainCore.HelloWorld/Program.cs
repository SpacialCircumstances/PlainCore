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

            var deviceOptions = new GraphicsDeviceOptions(
                debug: false,
                swapchainDepthFormat: PixelFormat.R16_UNorm,
                syncToVerticalBlank: true,
                resourceBindingModel: ResourceBindingModel.Improved,
                preferDepthRangeZeroToOne: true,
                preferStandardClipSpaceYDirection: false);
            var graphicsDevice = VeldridStartup.CreateGraphicsDevice(window, deviceOptions, GraphicsBackend.OpenGL);

            var spriteRenderer = new SpriteRenderer(graphicsDevice, graphicsDevice.ResourceFactory, graphicsDevice.SwapchainFramebuffer, (gb) =>
            {
                return graphicsDevice.ResourceFactory.CreateFromSpirv(Shaders.SpritebatchDefaultVertexShader, Shaders.SpritebatchDefaultFragmentShader);
            });
            spriteRenderer.Initialize();

            var spritebatch = new SpriteBatch();

            var texture = Texture2D.FromFile(graphicsDevice, graphicsDevice.ResourceFactory, "Planet.png");

            var view = new View(new Viewport(0f, 0f, 800f, 600f, 0f, 1.0f), new FloatRect(0f, 0f, 800f, 600f), 0f);

            while (window.Exists)
            {
                window.PumpEvents();

                spritebatch.Begin();
                spritebatch.Draw(texture, new Vector2(0, 0), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.End();

                spriteRenderer.Render(spritebatch, view);

                graphicsDevice.SwapBuffers();
            }

            graphicsDevice.Dispose();
        }
    }
}
