﻿using System.Collections.Generic;
using Veldrid;

namespace PlainCore
{
    public class Texture2D
    {
        public static Texture2D FromTextureAndView(Texture texture, TextureView textureView)
        {
            return new Texture2D
            {
                Texture = texture,
                TextureView = textureView
            };
        }

        public Texture Texture { get; private set; }
        public TextureView TextureView { get; private set; }

        public int Height => (int)Texture.Height;
        public int Width => (int)Texture.Width;

        public override bool Equals(object obj)
        {
            return obj is Texture2D d &&
                   Texture.Equals(d.Texture) &&
                   TextureView.Equals(d.TextureView);
        }

        public override int GetHashCode()
        {
            var hashCode = -507090303;
            hashCode = hashCode * -1521134295 + Texture.GetHashCode();
            hashCode = hashCode * -1521134295 + TextureView.GetHashCode();
            return hashCode;
        }
    }
}
