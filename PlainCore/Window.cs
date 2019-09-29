using System;
using Veldrid;
using Veldrid.Sdl2;

namespace PlainCore
{
    public class Window
    {
        public Window(GraphicsDevice device, ResourceFactory factory, Sdl2Window windowHandle, Action<RgbaFloat> clearCallback)
        {
            Device = device;
            Factory = factory;
            WindowHandle = windowHandle;
            int w = (int)device.SwapchainFramebuffer.Width;
            int h = (int)device.SwapchainFramebuffer.Height;
            MainView = new View(device, new Viewport(0f, 0f, w, h, 0f, 1f), new FloatRect(0f, 0f, w, h));
            this.clearCallback = clearCallback;
        }

        private readonly Action<RgbaFloat> clearCallback;
        public View MainView { get; }
        public GraphicsDevice Device { get; }
        public ResourceFactory Factory { get; }
        public Sdl2Window WindowHandle { get; }
        public Framebuffer Framebuffer => Device.SwapchainFramebuffer;
        public bool IsOpen => WindowHandle.Exists;

        public InputSnapshot HandleInput()
        {
            return WindowHandle.PumpEvents();
        }

        public void Display()
        {
            Device.WaitForIdle();
            Device.SwapBuffers();
        }

        public void Clear(RgbaFloat color)
        {
            clearCallback?.Invoke(color);
        }
    }
}
