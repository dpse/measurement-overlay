namespace MeasurementOverlay.Rendering
{
    using System;
    using System.Runtime.InteropServices;

    using MeasurementOverlay.PInvoke;
    using MeasurementOverlay.PInvoke.Structs;

    public class OverlayWindow : IDisposable
    {
        private static readonly Random Random = new Random();

        private Wndproc windowprochandle;

        public OverlayWindow(bool vsync = false, IntPtr parent = default(IntPtr))
        {
            this.X = 0;
            this.Y = 0;
            this.Width = 800;
            this.Height = 600;

            if (parent == default(IntPtr))
            {
                this.Width = User32.GetSystemMetrics(0);
                this.Height = User32.GetSystemMetrics(1);
            }

            var className = GenerateRandomString(5, 11);
            var menuName = GenerateRandomString(5, 11);
            var windowName = GenerateRandomString(5, 11);

            var wndClassEx = new WNDCLASSEX
                                 {
                                     cbSize = WNDCLASSEX.Size(),
                                     style = 0,
                                     lpfnWndProc = this.GetWindowProcPointer(),
                                     cbClsExtra = 0,
                                     cbWndExtra = 0,
                                     hInstance = IntPtr.Zero,
                                     hIcon = IntPtr.Zero,
                                     hCursor = IntPtr.Zero,
                                     hbrBackground = IntPtr.Zero,
                                     lpszMenuName = menuName,
                                     lpszClassName = className,
                                     hIconSm = IntPtr.Zero
                                 };

            if (User32.RegisterClassEx(ref wndClassEx) == 0)
                throw new Exception("RegisterClassExA failed with error code: " + Marshal.GetLastWin32Error());

            this.Handle = User32.CreateWindowEx(
                0x8 | 0x20 | 0x80000 | 0x80
                | 0x8000000, // WS_EX_TOPMOST | WS_EX_TRANSPARENT | WS_EX_LAYERED | WS_EX_TOOLWINDOW
                className,
                windowName,
                0x80000000 | 0x10000000,
                this.X,
                this.Y,
                this.Width,
                this.Height,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (this.Handle == IntPtr.Zero)
                throw new Exception("CreateWindowEx failed with error code: " + Marshal.GetLastWin32Error());

            User32.SetLayeredWindowAttributes(this.Handle, 0, 255, 0x2); // | 0x01

            this.Graphics = new Direct2DRenderer(this.Handle, vsync);

            this.extendFrameIntoClientArea();

            User32.UpdateWindow(this.Handle);

            this.IsVisible = true;
            this.Topmost = true;

            if (parent == default(IntPtr)) return;

            this.ParentWindowHandle = parent;
        }

        public Direct2DRenderer Graphics { get; }

        public IntPtr Handle { get; }

        public int Height { get; private set; }

        public bool IsVisible { get; private set; }

        public IntPtr ParentWindowHandle { get; }

        public bool Topmost { get; }

        public int Width { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public void Dispose()
        {
            this.Graphics?.Dispose();
        }

        private void extendFrameIntoClientArea()
        {
            var margin = new MARGIN
                             {
                                 cxLeftWidth = this.X,
                                 cxRightWidth = this.Width,
                                 cyBottomHeight = this.Height,
                                 cyTopHeight = this.Y
                             };

            DwmApi.DwmExtendFrameIntoClientArea(this.Handle, ref margin);
        }

        private static string GenerateRandomString(int minLen, int maxLen)
        {
            var len = Random.Next(minLen, maxLen);

            var chars = new char[len];

            for (var i = 0; i < chars.Length; i++) chars[i] = (char)Random.Next(97, 123);

            return new string(chars);
        }

        private IntPtr GetWindowProcPointer()
        {
            this.windowprochandle = this.WindowProcedure;
            return Marshal.GetFunctionPointerForDelegate(this.windowprochandle);
        }

        public void Hide()
        {
            if (!this.IsVisible) return;

            User32.ShowWindow(this.Handle, 0);
            this.IsVisible = false;
        }

        private void ParentService()
        {
            var parentBounds = default(RECT);

            User32.GetWindowRect(this.ParentWindowHandle, out parentBounds);

            if (this.X != parentBounds.Left || this.Width != parentBounds.Right - parentBounds.Left
                || this.Y != parentBounds.Top || this.Height != parentBounds.Bottom - parentBounds.Top)
            {
                this.X = parentBounds.Left;
                this.Y = parentBounds.Top;
                this.Width = parentBounds.Right - parentBounds.Left;
                this.Height = parentBounds.Bottom - parentBounds.Top;

                this.SetBounds(this.X, this.Y, this.Width, this.Height);
            }
        }

        public void SetBounds(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;

            //User32.SetWindowPos(Handle, -1, x, y, Width, Height, 0x40);
            //User32.UpdateLayeredWindow(this.Handle, IntPtr.Zero, ref pos, ref size, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero, 0x1 | 0x2);
            User32.MoveWindow(this.Handle, this.X, this.Y, this.Width, this.Height, 1);

            if (this.Graphics != null) this.Graphics.Resize(this.Width, this.Height);

            this.extendFrameIntoClientArea();
        }

        public void Show()
        {
            if (this.IsVisible) return;

            User32.ShowWindow(this.Handle, 5);
            this.IsVisible = true;
        }

        private int WindowProcedure(IntPtr handle, uint message, uint wparam, uint lparam)
        {
            switch (message)
            {
                case 0x12: return 0;
                case 0x14:
                    User32.SendMessage(handle, 0x12, 0, 0);
                    break;
                case 0x100: return 0;
                default: break;
            }

            //extendFrameIntoClientArea();
            return User32.DefWindowProc(handle, message, wparam, lparam);
        }

        private delegate int Wndproc(IntPtr handle, uint message, uint wparam, uint lparam);
    }
}