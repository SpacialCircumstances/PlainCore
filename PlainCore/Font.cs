using System;
using Veldrid;

namespace PlainCore
{
    public class Font
    {
        public static Font FromDescriptionFile(Window window, string filename)
        {
            return new Font(FontDescription.FromDescriptionFile(filename), window.Device, window.Factory);
        }

        public static Font GenerateFromFont(Window window, string fontFileName, uint fontSize)
        {
            return new Font(FontGenerator.GenerateFont(fontFileName, fontSize), window.Device, window.Factory);
        }

        public Font(FontDescription description, GraphicsDevice device, ResourceFactory factory)
        {
            if (description == null) throw new ArgumentNullException(nameof(description));

            this.Texture = Texture2D.FromImage(device, factory, description.Bitmap);
            this.Description = description;
        }

        public Texture2D Texture { get; }
        public FontDescription Description { get; }
    }
}
