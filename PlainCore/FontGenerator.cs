using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace PlainCore
{
    /// <summary>
    /// Allows generating fonts from font files.
    /// </summary>
    public static class FontGenerator
    {
        private const int MAX_BITMAP_WIDTH = 1024;
        private const int HORIZONTAL_OFFSET = 2; //Reduces artifacts when scaling up

        /// <summary>
        /// Generate a font from a font file.
        /// </summary>
        /// <param name="fontFileName">Name of the font file</param>
        /// <param name="fontSize">Size of the bitmap font</param>
        /// <param name="lowerChar">The lowest character to render</param>
        /// <param name="upperChar">The hightest character</param>
        /// <returns>A description for the font</returns>
        public static FontDescription GenerateFont(string fontFileName, uint fontSize, int lowerChar = 32, int upperChar = 127)
        {
            var fontCollection = new FontCollection();
            fontCollection.Install(fontFileName);
            var fontFamily = fontCollection.Families.First();
            var font = fontFamily.CreateFont(fontSize);

            var glyphs = new Dictionary<char, GlyphLayout>();

            var currentX = 0;
            var currentY = 0;
            var maxY = 0;

            for (int i = lowerChar; i < upperChar; i++)
            {
                char c = (char)i;
                var (w, h) = GetGlyphSize(font, c, (int)fontSize);

                //Glyph would be to big
                if (currentX + w + HORIZONTAL_OFFSET > MAX_BITMAP_WIDTH)
                {
                    currentY += maxY;
                    maxY = 0;
                    currentX = 0;
                }

                //Glyph is biggest in its line
                if (h > maxY)
                {
                    maxY = h;
                }

                var layout = new GlyphLayout(c, (currentX, currentY), (w, h));
                currentX += w + HORIZONTAL_OFFSET;

                glyphs.Add(c, layout);
            }

            var finalHeight = currentY + maxY;

            var bitmap = new Image<Rgba32>(MAX_BITMAP_WIDTH, finalHeight);

            bitmap.Mutate(ctx =>
            {
                foreach (var glyph in glyphs)
                {
                    var pos = new Point(glyph.Value.BitmapPosition.X, glyph.Value.BitmapPosition.Y);
                    ctx.DrawText($"{glyph.Value.Character}", font, Rgba32.White, pos);
                }
            });

            return new FontDescription(bitmap, fontSize, glyphs);
        }

        private static (int, int) GetGlyphSize(SixLabors.Fonts.Font face, char character, int size)
        {
            var glyphSize = TextMeasurer.Measure($"{character}", new RendererOptions(face));
            return ((int)glyphSize.Width, (int)glyphSize.Height);
        }
    }
}