namespace MeasurementOverlay.ScreenCapture
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using MeasurementOverlay.PInvoke;

    public class ScreenCapture
    {
        public ScreenCapture(Rectangle screen)
        {
            if (screen == null) throw new ArgumentNullException(nameof(screen));

            this.Capture = new Bitmap(screen.Width, screen.Height, PixelFormat.Format32bppArgb);

            using (var gfx = Graphics.FromImage(this.Capture))
            {
                gfx.CopyFromScreen(screen.X, screen.Y, 0, 0, screen.Size, CopyPixelOperation.SourceCopy);
            }
        }

        public Bitmap Capture { get; }

        public Color GetCapturedColorAt(int x, int y)
        {
            return this.Capture.GetPixel(x, y);
        }

        public static Color GetColorAt(Point point)
        {
            if (point == null) throw new ArgumentNullException(nameof(point));

            return GetColorAt(point.X, point.Y);
        }

        public static Color GetColorAt(int x, int y)
        {
            var desk = User32.GetDesktopWindow();
            var dc = User32.GetWindowDC(desk);
            var a = (int)Gdi32.GetPixel(dc, x, y);
            User32.ReleaseDC(desk, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
    }
}