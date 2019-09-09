using Veldrid;

namespace PlainCore
{
    public class Texture2D
    {
        public static Texture2D FromTextureAndView(Texture texture, TextureView textureView)
        {
            return new Texture2D
            {
                Texture = texture,
                TextureView = textureView
            };
        }

        public Texture Texture { get; private set; }
        public TextureView TextureView { get; private set; }

        public int Height => (int)Texture.Height;
        public int Width => (int)Texture.Width;
    }
}
