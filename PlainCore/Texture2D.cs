using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Threading;
using System.IO;
using Veldrid;
using Veldrid.ImageSharp;

namespace PlainCore
{
    public class Texture2D
    {
        private static int textureIdCounter = 0;

        private static Texture2D CreateTexture(GraphicsDevice device, ResourceFactory factory, ImageSharpTexture tex)
        {
            var texture = tex.CreateDeviceTexture(device, factory);
            var textureView = factory.CreateTextureView(texture);

            return new Texture2D()
            {
                Texture = texture,
                TextureView = textureView
            };
        }

        public static Texture2D FromStream(Window window, Stream stream, bool mipmap = true, bool srgb = false)
        {
            return FromStream(window.Device, window.Factory, stream, mipmap, srgb);
        }

        public static Texture2D FromStream(GraphicsDevice device, ResourceFactory factory, Stream stream, bool mipmap = true, bool srgb = false)
        {
            var tex = new ImageSharpTexture(stream, mipmap, srgb);
            return CreateTexture(device, factory, tex);
        }
        
        public static Texture2D FromImage(Window window, Image<Rgba32> image, bool mipmap = true, bool srgb = false)
        {
            return FromImage(window.Device, window.Factory, image, mipmap, srgb);
        }

        public static Texture2D FromImage(GraphicsDevice device, ResourceFactory factory, Image<Rgba32> image, bool mipmap = true, bool srgb = false)
        {
            var tex = new ImageSharpTexture(image, mipmap, srgb);
            return CreateTexture(device, factory, tex);
        }

        public static Texture2D FromFile(Window window, string filename, bool mipmap = true, bool srgb = false)
        {
            return FromFile(window.Device, window.Factory, filename, mipmap, srgb);
        }

        public static Texture2D FromFile(GraphicsDevice device, ResourceFactory factory, string filename, bool mipmap = true, bool srgb = false)
        {
            var tex = new ImageSharpTexture(filename, mipmap, srgb);
            return CreateTexture(device, factory, tex);
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
