using System;
using System.Collections.Generic;
using System.Text;

namespace PlainCore
{
    public interface IRect<T>
    {
        T Left { get; }
        T Right { get; }
        T Top { get; }
        T Bottom { get; }
        T Width { get; }
        T Height { get; }
        bool Contains(T x, T y);
        bool Intersects(IRect<T> other);
    }
}
