using System;
using Veldrid;
using Veldrid.StartupUtilities;

namespace PlainCore
{
    public class WindowBuilder
    {
        private WindowCreateInfo wci = new WindowCreateInfo(100, 100, 800, 600, WindowState.Normal, "PlainCore Window");
        private GraphicsDeviceOptions gdo = new GraphicsDeviceOptions();
        private GraphicsBackend? preferredBackend = null;
        private Action<RgbaFloat> clearFunction = null;

        public WindowBuilder()
        {

        }

        public Window Build()
        {
            var win = VeldridStartup.CreateWindow(ref wci);
            GraphicsDevice device;
            if (preferredBackend is GraphicsBackend backend)
            {
                device = VeldridStartup.CreateGraphicsDevice(win, gdo, backend);
            }
            else
            {
                device = VeldridStartup.CreateGraphicsDevice(win, gdo);
            }

            return new Window(device, device.ResourceFactory, win, clearFunction);
        }
    }
}
