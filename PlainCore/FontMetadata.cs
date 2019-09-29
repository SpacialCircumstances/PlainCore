using System;

namespace PlainCore
{
    public class FontMetadata
    {
        public FontMetadata(string familyName, string subfamilyName, float size, FontStyle style, int weight, FontStretch stretch)
        {
            FamilyName = familyName;
            SubfamilyName = subfamilyName;
            Size = size;
            Style = style;
            Weight = weight;
            Stretch = stretch;
        }

        public string FamilyName { get; }
        public string SubfamilyName { get; }
        public float Size { get; }
        public FontStyle Style { get; }
        public int Weight { get; }
        public FontStretch Stretch { get; }

        internal static FontStyle StyleFrom(SharpFont.FontStyle style)
        {
            FontStyle result;
            switch (style)
            {
                case SharpFont.FontStyle.Bold:
                    result = FontStyle.Bold;
                    break;
                case SharpFont.FontStyle.Italic:
                    result = FontStyle.Italic;
                    break;
                case SharpFont.FontStyle.Regular:
                    result = FontStyle.Regular;
                    break;
                case SharpFont.FontStyle.Oblique:
                    result = FontStyle.Oblique;
                    break;
                default:
                    throw new ArgumentException(nameof(style));
            }
            return result;
        }

        internal static FontStretch StretchFrom(SharpFont.FontStretch stretch)
        {
            FontStretch result;
            switch (stretch)
            {
                case SharpFont.FontStretch.UltraCondensed:
                    result = FontStretch.UltraCondensed;
                    break;
                case SharpFont.FontStretch.ExtraCondensed:
                    result = FontStretch.ExtraCondensed;
                    break;
                case SharpFont.FontStretch.Condensed:
                    result = FontStretch.Condensed;
                    break;
                case SharpFont.FontStretch.SemiCondensed:
                    result = FontStretch.SemiCondensed;
                    break;
                case SharpFont.FontStretch.Normal:
                    result = FontStretch.Normal;
                    break;
                case SharpFont.FontStretch.SemiExpanded:
                    result = FontStretch.SemiExpanded;
                    break;
                case SharpFont.FontStretch.ExtraExpanded:
                    result = FontStretch.ExtraExpanded;
                    break;
                case SharpFont.FontStretch.UltraExpanded:
                    result = FontStretch.UltraExpanded;
                    break;
                default:
                    result = FontStretch.Unknown;
                    break;
            }
            return result;
        }
    }

    //These enums match those from SharpFont, but we copy them here to make it possible 
    //to change font libraries without breaking old code

    /// <summary>
    /// Specifies the font stretching level.
    /// </summary>
    public enum FontStretch
    {
        /// <summary>
        /// The stretch is unknown or unspecified.
        /// </summary>
        Unknown,

        /// <summary>
        /// Ultra condensed.
        /// </summary>
        UltraCondensed,

        /// <summary>
        /// Extra condensed.
        /// </summary>
        ExtraCondensed,

        /// <summary>
        /// Condensed.
        /// </summary>
        Condensed,

        /// <summary>
        /// Somewhat condensed.
        /// </summary>
        SemiCondensed,

        /// <summary>
        /// Normal.
        /// </summary>
        Normal,

        /// <summary>
        /// Somewhat expanded.
        /// </summary>
        SemiExpanded,

        /// <summary>
        /// Expanded.
        /// </summary>
        Expanded,

        /// <summary>
        /// Extra expanded.
        /// </summary>
        ExtraExpanded,

        /// <summary>
        /// Ultra expanded.
        /// </summary>
        UltraExpanded
    }

    /// <summary>
    /// Specifies various font styles.
    /// </summary>
    public enum FontStyle
    {
        /// <summary>
        /// No particular styles applied.
        /// </summary>
        Regular,

        /// <summary>
        /// The font is emboldened.
        /// </summary>
        Bold,

        /// <summary>
        /// The font is stylistically italic.
        /// </summary>
        Italic,

        /// <summary>
        /// The font is algorithmically italic / angled.
        /// </summary>
        Oblique
    }
}
