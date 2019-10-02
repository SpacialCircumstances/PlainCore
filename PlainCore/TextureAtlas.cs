using System;
using System.Collections.Generic;
using System.Linq;

namespace PlainCore
{
    public class TextureAtlas
    {
        private readonly Dictionary<string, TextureRegion> atlas;

        public static TextureAtlas Create(Texture2D texture, IDictionary<string, IntRect> regions)
        {
            var textureRegions = regions.Select(entry =>
            {
                if (entry.Value.Left >= 0 &&
                    entry.Value.Top >= 0 &&
                    entry.Value.Right <= texture.Width &&
                    entry.Value.Bottom <= texture.Height)
                {
                    return new KeyValuePair<string, TextureRegion>(entry.Key, new TextureRegion(entry.Value, texture));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(regions), "One region is outside the bounds of the texture");
                }
            }).ToDictionary(e => e.Key, e => e.Value);
            return new TextureAtlas(textureRegions);
        }

        public TextureAtlas(Dictionary<string, TextureRegion> atlas)
        {
            this.atlas = atlas;
        }

        public TextureRegion this[string name]
        {
            get => atlas[name];
        }
    }
}
