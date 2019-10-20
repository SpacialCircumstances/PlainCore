namespace PlainCore
{
    public struct IntRect: IRect<int>
    {
        public IntRect(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int Left => X;
        public int Right => X + Width;
        public int Top => Y;
        public int Bottom => Y + Height;

        public FloatRect ToFloatRect()
        {
            return new FloatRect(X, Y, Width, Height);
        }

        public override bool Equals(object obj)
        {
            return obj is IntRect rect &&
                   X == rect.X &&
                   Y == rect.Y &&
                   Width == rect.Width &&
                   Height == rect.Height;
        }

        public override int GetHashCode()
        {
            var hashCode = 466501756;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public bool Contains(int x, int y)
        {
            return x >= Left && x < Right && y >= Top && y < Bottom;
        }

        public bool Intersects(IRect<int> other)
        {
            return Contains(other.Left, other.Top) || Contains(other.Left, other.Bottom) || Contains(other.Right, other.Top) || Contains(other.Right, other.Bottom);
        }

        public static bool operator ==(IntRect left, IntRect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntRect left, IntRect right)
        {
            return !(left == right);
        }
    }
}
