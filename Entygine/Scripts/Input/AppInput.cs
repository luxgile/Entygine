using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace Entygine.Input
{
    public static class AppInput
    {
        public static InputState CurrentInput { get; private set; }

        public static void SetFrameInput(InputState input)
        {
            CurrentInput = input;
        }
    }

    public struct InputState
    {
        public KeyboardState keyboard;
        public MouseState mouse;

        public InputState(KeyboardState keyboard, MouseState mouse)
        {
            this.keyboard = keyboard ?? throw new ArgumentNullException(nameof(keyboard));
            this.mouse = mouse ?? throw new ArgumentNullException(nameof(mouse));
        }
    }
}
