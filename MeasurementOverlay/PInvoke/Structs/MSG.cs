namespace MeasurementOverlay.PInvoke.Structs
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        public IntPtr hwnd;

        public uint message;

        public IntPtr wparam;

        public IntPtr lparam;

        public uint time;

        public int x;

        public int y;
    }
}