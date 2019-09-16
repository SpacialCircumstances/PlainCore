using System.Numerics;
using Veldrid;

namespace PlainCore
{
    public class View
    {
        public View(Viewport screenView, FloatRect worldView)
        {
            ScreenView = screenView;
            WorldView = worldView;
        }

        public Viewport ScreenView { get; }
        public FloatRect WorldView { get; }
    }
}
