using System.Collections.Generic;

namespace PlainCore
{
    public class Glyphs
    {
        private readonly Dictionary<char, GlyphLayout> glyphLayouts;

        public Glyphs(Dictionary<char, GlyphLayout> glyphLayouts)
        {
            this.glyphLayouts = glyphLayouts;
        }

        public GlyphLayout this[char c]
        {
            get => glyphLayouts[c];
        }
    }
}
