using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Threading;
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

            return new Texture2D(texture, textureView);
        }

        public static Texture2D FromStream(IGraphicsContext context, Stream stream, bool mipmap = true, bool srgb = false)
        {
            return FromStream(context.Device, context.Factory, stream, mipmap, srgb);
        }

        public static Texture2D FromStream(GraphicsDevice device, ResourceFactory factory, Stream stream, bool mipmap = true, bool srgb = false)
        {
            var tex = new ImageSharpTexture(stream, mipmap, srgb);
            return CreateTexture(device, factory, tex);
        }

        public static Texture2D FromImage(IGraphicsContext context, Image<Rgba32> image, bool mipmap = true, bool srgb = false)
        {
            return FromImage(context.Device, context.Factory, image, mipmap, srgb);
        }

        public static Texture2D FromImage(GraphicsDevice device, ResourceFactory factory, Image<Rgba32> image, bool mipmap = true, bool srgb = false)
        {
            var tex = new ImageSharpTexture(image, mipmap, srgb);
            return CreateTexture(device, factory, tex);
        }

        public static Texture2D FromFile(IGraphicsContext context, string filename, bool mipmap = true, bool srgb = false)
        {
            return FromFile(context.Device, context.Factory, filename, mipmap, srgb);
        }

        public static Texture2D FromFile(GraphicsDevice device, ResourceFactory factory, string filename, bool mipmap = true, bool srgb = false)
        {
            var tex = new ImageSharpTexture(filename, mipmap, srgb);
            return CreateTexture(device, factory, tex);
        }
        public static Texture2D FromTextureAndView(Texture texture, TextureView textureView)
        {
            return new Texture2D(texture, textureView);
        }
        private Texture2D(Texture texture, TextureView textureView)
        {
            Id = textureIdCounter;
            Texture = texture;
            TextureView = textureView;
            Interlocked.Increment(ref textureIdCounter);
        }

        public Texture Texture { get; }
        public TextureView TextureView { get; }

        public int Height => (int)Texture.Height;
        public int Width => (int)Texture.Width;
        public int Id { get; }

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
