using System.Numerics;
using Veldrid;
using Veldrid.SPIRV;

namespace PlainCore.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new WindowBuilder()
                            .SetTitle("Hello World")
                            .UseDefaultClearFunction()
                            .Build();

            var spriteRenderer = new SpriteRenderer(window, (gb) =>
            {
                return window.Factory.CreateFromSpirv(Shaders.SpritebatchDefaultVertexShader, Shaders.SpritebatchDefaultFragmentShader);
            });
            spriteRenderer.Initialize();

            var spritebatch = new SpriteBatch();

            var texture = Texture2D.FromFile(window, "Planet.png");

            while (window.IsOpen)
            {
                window.WindowHandle.PumpEvents();

                window.Clear(RgbaFloat.Red);

                spritebatch.Begin();
                spritebatch.Draw(texture, new Vector2(100, 100), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.Draw(texture, new Vector2(50, 50), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.Draw(texture, new Vector2(0, 0), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.End();

                spriteRenderer.Render(spritebatch, window.MainView);

                window.Display();
            }
        }
    }
}
