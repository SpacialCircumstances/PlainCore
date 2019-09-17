using System.Numerics;
using Veldrid;

namespace PlainCore
{
    public class View
    {
        public View(GraphicsDevice device, Viewport screenView, FloatRect worldView, float worldRotation = 0f)
        {
            ScreenView = screenView;
            WorldRotation = worldRotation;
            WorldView = worldView;
            //If GraphicsDevice does not use Vulkan clip space, invert the y direction. This will happen on Direct3D
            //and, when the device is created with "preferStandardClipSpaceYDirection: true", on Vulkan.
            float projYFactor = device.IsClipSpaceYInverted ? 1f : -1f;
            var rot = Matrix4x4.CreateRotationZ(WorldRotation);
            var proj = Matrix4x4.CreateScale(2f / WorldView.Width, (2f / WorldView.Height) * projYFactor, -1f);
            var translate = Matrix4x4.CreateTranslation(WorldView.Left - (WorldView.Width / 2), WorldView.Top - (WorldView.Height / 2), 0f);
            WorldMatrix = translate * proj * rot;
        }

        public Viewport ScreenView { get; }
        public float WorldRotation { get; }
        public FloatRect WorldView { get; }
        public Matrix4x4 WorldMatrix { get; }
    }
}
