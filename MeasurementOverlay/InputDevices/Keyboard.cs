namespace MeasurementOverlay.InputDevices
{
    using System;

    using MeasurementOverlay.PInvoke;
    using MeasurementOverlay.PInvoke.Enums;

    public static class Keyboard
    {
        public static bool KeyPressed(VirtualKeyStates key)
        {
            return Convert.ToBoolean(User32.GetKeyState(key) & 0x8000);
        }
    }
}