using System.Numerics;
using Veldrid;

namespace PlainCore.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = new WindowBuilder()
                            .SetTitle("Hello World")
                            .UseDepthBuffer()
                            .UseDefaultClearFunction()
                            .Build();

            var spriteRenderer = new SpriteRenderer(window);

            var spritebatch = new SpriteBatch();

            var texture = Texture2D.FromFile(window, "Planet.png");

            var font = Font.GenerateFromFont(window, "OpenSans-Regular.ttf", 40);

            var scissor = new IntRect(50, 50, 400, 400);

            while (window.IsOpen)
            {
                window.WindowHandle.PumpEvents();

                window.Clear(RgbaFloat.Black);

                spritebatch.Begin();
                spritebatch.Draw(texture, new Vector2(400, 400), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.Draw(texture, new Vector2(200, 200), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.Draw(texture, new Vector2(0, 0), null, RgbaFloat.White, 0f, Vector2.Zero, Vector2.One, 0.0f);
                spritebatch.DrawText("Hello World!", font, RgbaFloat.White, 0, 200, 1f, 0f);
                spritebatch.End();

                spriteRenderer.Render(spritebatch, window.MainView, scissorRect: scissor);

                window.Display();
            }

            spriteRenderer.Dispose();
        }
    }
}
