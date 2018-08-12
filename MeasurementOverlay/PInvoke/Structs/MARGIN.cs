namespace MeasurementOverlay.PInvoke.Structs
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MARGIN
    {
        public int cxLeftWidth;

        public int cxRightWidth;

        public int cyTopHeight;

        public int cyBottomHeight;
    }
}