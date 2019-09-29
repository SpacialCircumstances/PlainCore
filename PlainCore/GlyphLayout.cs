using System.Numerics;

namespace PlainCore
{
    /// <summary>
    /// Contains data for rendering a glyph.
    /// </summary>
    public struct GlyphLayout
    {
        public GlyphLayout(char character, IntRect? bitmapRegion, Vector2 size, Vector2 bearing, float advance)
        {
            Character = character;
            BitmapRegion = bitmapRegion;
            Size = size;
            Bearing = bearing;
            Advance = advance;
        }

        public char Character { get; }
        public IntRect? BitmapRegion { get; }
        public Vector2 Size { get; }
        public Vector2 Bearing { get; }
        public float Advance { get; }

        public override bool Equals(object obj)
        {
            throw new System.NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public static bool operator ==(GlyphLayout left, GlyphLayout right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GlyphLayout left, GlyphLayout right)
        {
            return !(left == right);
        }
    }
}