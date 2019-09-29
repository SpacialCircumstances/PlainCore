using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PlainCore
{
    /// <summary>
    /// Contains the data of a generated bitmap font.
    /// </summary>
    public class FontDescription
    {
        public FontDescription(FontMetadata metadata, Image<Rgba32> bitmap, Glyphs glyphs)
        {
            Metadata = metadata;
            Bitmap = bitmap;
            Glyphs = glyphs;
        }

        public FontMetadata Metadata { get; }
        public Image<Rgba32> Bitmap { get; }
        public Glyphs Glyphs { get; }

        public override string ToString()
        {
            return $"{Metadata.FamilyName} {Metadata.SubfamilyName} ({Metadata.Size}) ({Metadata.Style})";
        }
    }
}
