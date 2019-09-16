using System.Numerics;
using Veldrid;

namespace PlainCore
{
    public class View
    {
        public View(Viewport screenView, FloatRect worldView, float worldRotation = 0f)
        {
            ScreenView = screenView;
            WorldRotation = worldRotation;
            WorldView = worldView;
            var rot = Matrix4x4.CreateRotationZ(WorldRotation);
            var proj = Matrix4x4.CreateOrthographic(WorldView.Width, WorldView.Height, 0f, 1f);
            var translate = Matrix4x4.CreateTranslation(WorldView.Left - (WorldView.Width / 2), WorldView.Top - (WorldView.Height / 2), 0f);
            WorldMatrix = translate * proj * rot;
        }

        public Viewport ScreenView { get; }
        public float WorldRotation { get; }
        public FloatRect WorldView { get; }
        public Matrix4x4 WorldMatrix { get; }
    }
}
