namespace MeasurementOverlay.PInvoke
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    using MeasurementOverlay.PInvoke.Structs;

    [SuppressUnmanagedCodeSecurity]
    internal static class DwmApi
    {
        [DllImport("dwmapi.dll", SetLastError = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGIN pMargins);
    }
}