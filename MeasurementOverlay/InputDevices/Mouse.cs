namespace MeasurementOverlay.InputDevices
{
    using System;
    using System.Drawing;

    using MeasurementOverlay.PInvoke;
    using MeasurementOverlay.PInvoke.Structs;

    public static class Mouse
    {
        public static Point GetCursorPosition()
        {
            POINT lpPoint;

            var success = User32.GetCursorPos(out lpPoint);

            if (!success) throw new InvalidOperationException("Failed to get cursor position.");

            return new Point(lpPoint.X, lpPoint.Y);
        }
    }
}