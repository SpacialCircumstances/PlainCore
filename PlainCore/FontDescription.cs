using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Numerics;

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

        public Vector2 MeasureString(string text, float scale = 1.0f)
        {
            float pen = 0f;
            float y = 0f;

            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                var glyph = Glyphs[character];
                float h = (glyph.Size.Y + glyph.Bearing.Y) * scale;
                if (h > y)
                {
                    y = h;
                }
                pen += glyph.Advance * scale;
            }

            return new Vector2(pen, y);
        }
    }
}
