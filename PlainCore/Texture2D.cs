using System.Threading;
using Veldrid;
using Veldrid.ImageSharp;

namespace PlainCore
{
    public class Texture2D
    {
        private static int textureIdCounter = 0;

        public static Texture2D FromFile(GraphicsDevice device, string filename, bool mipmap = false)
        {
            var tex = new ImageSharpTexture(filename, mipmap);
            var texture = tex.CreateDeviceTexture(device, device.ResourceFactory);
            var textureView = device.ResourceFactory.CreateTextureView(texture);

            return new Texture2D()
            {
                Texture = texture,
                TextureView = textureView
            };
        }
        public static Texture2D FromTextureAndView(Texture texture, TextureView textureView)
        {
            return new Texture2D
            {
                Texture = texture,
                TextureView = textureView
            };
        }
        private Texture2D()
        {
            this.Id = textureIdCounter;
            Interlocked.Increment(ref textureIdCounter);
        }

        public Texture Texture { get; private set; }
        public TextureView TextureView { get; private set; }

        public int Height => (int)Texture.Height;
        public int Width => (int)Texture.Width;
        public int Id { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is Texture2D d &&
                   d.Id == Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -507090303;
            hashCode = hashCode * -1521134295 + Texture.GetHashCode();
            hashCode = hashCode * -1521134295 + TextureView.GetHashCode();
            return hashCode;
        }
    }
}
