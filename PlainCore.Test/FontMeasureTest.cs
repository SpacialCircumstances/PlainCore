using System.Collections.Generic;
using System.Numerics;
using PlainCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace PlainCore.Test
{
    public class FontMeasureTest
    {
        private static void AddGlyph(Dictionary<char, GlyphLayout> glyphDict, GlyphLayout glyph) 
        {
            glyphDict.Add(glyph.Character, glyph);
        }

        [Fact]
        public void FontMeasuring() 
        {
            var fontMetadata = new FontMetadata("Test", "Default", 10.0f, FontStyle.Regular, 10, FontStretch.Normal);
            var bitmap = new Image<Rgba32>(200, 200);
            var glyphDict = new Dictionary<char, GlyphLayout>();
            AddGlyph(glyphDict, new GlyphLayout('a', null, new Vector2(10.0f, 10.0f), new Vector2(0.0f, 0.0f), 3.0f));
            AddGlyph(glyphDict, new GlyphLayout('b', null, new Vector2(8.0f, 10.0f), new Vector2(0.0f, 0.0f), 4.0f));
            AddGlyph(glyphDict, new GlyphLayout('g', null, new Vector2(5.0f, 11.0f), new Vector2(0.5f, 1.0f), 5.0f));
            AddGlyph(glyphDict, new GlyphLayout('!', null, new Vector2(3.0f, 12.0f), new Vector2(0.0f, -1.0f), 2.0f));
            var glyphs = new Glyphs(glyphDict);
            var fontDescription = new FontDescription(fontMetadata, bitmap, glyphs);
            Assert.Equal(new Vector2(6.0f, 10.0f), fontDescription.MeasureString("aa"));
            Assert.Equal(new Vector2(12.0f, 20.0f), fontDescription.MeasureString("aa", 2.0f));
            Assert.Equal(new Vector2(7.0f, 10.0f), fontDescription.MeasureString("ab"));
            Assert.Equal(new Vector2(17.0f, 12.0f), fontDescription.MeasureString("abgg"));
            Assert.Equal(new Vector2(25.0f, 30.0f), fontDescription.MeasureString("gg", 2.5f));
            Assert.Equal(new Vector2(4.0f, 11.0f), fontDescription.MeasureString("!!"));
        }
    }
}