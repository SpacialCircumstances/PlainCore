﻿using SharpFont;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

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
        public static FontDescription GenerateFont(string fontFileName, float fontSize, char lowerChar = (char)32, char upperChar = (char)127)
        {
            var font = new FontFace(File.OpenRead(fontFileName));

            var glyphs = new Dictionary<char, (GlyphLayout, Glyph)>();

            var currentX = 0;
            var currentY = 0;
            var maxY = 0;

            for (char i = lowerChar; i < upperChar; i++)
            {
                var glyph = font.GetGlyph(new CodePoint(i), fontSize);
                int w = glyph.RenderWidth;
                int h = glyph.RenderHeight;

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

                IntRect? region;
                if (w == 0 || h == 0)
                {
                    //Glyph is invisible, so it has no region on the bitmap
                    region = null;
                }
                else
                {
                    region = new IntRect(currentX, currentY, w, h);
                }

                var layout = new GlyphLayout(i, region, new Vector2(glyph.Width, glyph.Height), glyph.HorizontalMetrics.Bearing, glyph.HorizontalMetrics.Advance);
                currentX += w + HORIZONTAL_OFFSET;

                glyphs.Add(i, (layout, glyph));
            }

            var finalHeight = currentY + maxY;

            var bitmap = new Image<Rgba32>(MAX_BITMAP_WIDTH, finalHeight);

            var glyphTable = new Dictionary<char, GlyphLayout>();

            bitmap.Mutate(ctx =>
            {
                foreach (var glyphEntry in glyphs)
                {
                    var (layout, glyph) = glyphEntry.Value;
                    if (layout.BitmapRegion.HasValue)
                    {
                        //Glyph is renderable
                        var region = layout.BitmapRegion.Value;
                        var pos = new Point(region.X, region.Y);
                        var img = RenderGlyph(glyph);
                        ctx.DrawImage(img, 1f, pos);
                    }
                    glyphTable[glyphEntry.Key] = layout;
                }
            });

            var metadata = new FontMetadata(
                font.Family,
                font.Subfamily,
                fontSize,
                FontMetadata.StyleFrom(font.Style),
                (int)font.Weight,
                FontMetadata.StretchFrom(font.Stretch));

            return new FontDescription(metadata, bitmap, new Glyphs(glyphTable));
        }

        private static unsafe Image<Rgba32> RenderGlyph(Glyph glyph)
        {
            int width = glyph.RenderWidth;
            int height = glyph.RenderHeight;
            var data = new byte[width * height];

            fixed (byte* dataPtr = data)
            {

                var surface = new Surface
                {
                    Bits = (IntPtr)dataPtr,
                    Width = width,
                    Height = height,
                    Pitch = width
                };

                glyph.RenderTo(surface);

                int len = width * height;
                var pixelData = new byte[len * 4];
                int index = 0;
                for (int i = 0; i < len; i++)
                {
                    byte c = *(dataPtr + i);
                    pixelData[index++] = 255;
                    pixelData[index++] = 255;
                    pixelData[index++] = 255;
                    pixelData[index++] = c;
                }

                return Image.LoadPixelData<Rgba32>(pixelData, width, height);
            }
        }
    }
}