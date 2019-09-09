using System;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace PlainCore.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var wci = new WindowCreateInfo()
            {
                X = 100,
                Y = 100,
                WindowWidth = 800,
                WindowHeight = 600,
                WindowTitle = "Hello World"
            };
            var window = VeldridStartup.CreateWindow(ref wci);

            var graphicsDevice = VeldridStartup.CreateGraphicsDevice(window);

            while (window.Exists)
            {
                window.PumpEvents();
            }

            graphicsDevice.Dispose();
        }
    }
}
