namespace MeasurementOverlay.PInvoke.Structs
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;

        public int Y;
    }
}