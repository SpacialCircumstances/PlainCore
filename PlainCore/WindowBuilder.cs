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
        private Func<ResourceFactory, ResourceFactory> resourceFactoryFactory = (r) => r;
        private Func<GraphicsDevice, ResourceFactory, Action<RgbaFloat>> clearFunctionFactory = (_, _2) => null;

        public WindowBuilder()
        {

        }

        public WindowBuilder SetResourceFactoryFactory(Func<ResourceFactory, ResourceFactory> createResourceFactory)
        {
            resourceFactoryFactory = createResourceFactory ?? throw new ArgumentNullException(nameof(createResourceFactory));
            return this;
        }

        public WindowBuilder SetClearFunctionFactory(Func<GraphicsDevice, ResourceFactory, Action<RgbaFloat>> clearFunctionFactory)
        {
            this.clearFunctionFactory = clearFunctionFactory;
            return this;
        }

        public WindowBuilder UseAutomaticBackend()
        {
            preferredBackend = null;
            return this;
        }

        public WindowBuilder UseBackend(GraphicsBackend backend)
        {
            preferredBackend = backend;
            return this;
        }

        public WindowBuilder WithWindowState(WindowState windowState)
        {
            wci.WindowInitialState = windowState;
            return this;
        }

        public WindowBuilder WithPositionY(int y)
        {
            wci.Y = y;
            return this;
        }

        public WindowBuilder WithPositionX(int x)
        {
            wci.X = x;
            return this;
        }

        public WindowBuilder WithPosition(int x, int y)
        {
            wci.X = x;
            wci.Y = y;
            return this;
        }

        public WindowBuilder WithSize(int width, int height)
        {
            wci.WindowWidth = width;
            wci.WindowHeight = height;
            return this;
        }

        public WindowBuilder WithHeight(int height)
        {
            wci.WindowHeight = height;
            return this;
        }

        public WindowBuilder WithWidth(int width)
        {
            wci.WindowWidth = width;
            return this;
        }

        public WindowBuilder WithTitle(string title)
        {
            wci.WindowTitle = title ?? throw new ArgumentNullException(nameof(title));
            return this;
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

            var factory = resourceFactoryFactory(device.ResourceFactory);
            var clearFunction = clearFunctionFactory(device, factory);
            return new Window(device, factory, win, clearFunction);
        }
    }
}
