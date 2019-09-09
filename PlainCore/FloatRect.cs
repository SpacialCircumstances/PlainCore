using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PlainCore
{
    public struct FloatRect
    {
        public FloatRect(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }

        public FloatRect(float x, float y, float w, float h)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(w, h);
        }

        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public Vector2 Corner => Position + Size;
        public float X => Position.X;
        public float Y => Position.Y;
        public float Width => Size.X;
        public float Height => Size.Y;
        public float Left => Position.X;
        public float Top => Position.Y;
        public float Bottom => Position.Y + Size.Y;
        public float Right => Position.X + Size.X;

        public override bool Equals(object obj)
        {
            return obj is FloatRect rect &&
                   Position.Equals(rect.Position) &&
                   Size.Equals(rect.Size);
        }

        public override int GetHashCode()
        {
            var hashCode = -1930765514;
            hashCode = hashCode * -1521134295 + Position.GetHashCode();
            hashCode = hashCode * -1521134295 + Size.GetHashCode();
            return hashCode;
        }
    }
}
