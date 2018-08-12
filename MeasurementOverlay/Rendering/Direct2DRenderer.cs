namespace MeasurementOverlay.Rendering
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    using MeasurementOverlay.PInvoke;
    using MeasurementOverlay.PInvoke.Structs;

    using SharpDX;
    using SharpDX.Direct2D1;
    using SharpDX.DirectWrite;
    using SharpDX.DXGI;
    using SharpDX.Mathematics.Interop;

    using AlphaMode = SharpDX.Direct2D1.AlphaMode;
    using Bitmap = SharpDX.Direct2D1.Bitmap;
    using Factory = SharpDX.Direct2D1.Factory;
    using FactoryType = SharpDX.Direct2D1.FactoryType;
    using FontFactory = SharpDX.DirectWrite.Factory;
    using FontStyle = SharpDX.DirectWrite.FontStyle;
    using Image = System.Drawing.Image;
    using PixelFormat = SharpDX.Direct2D1.PixelFormat;
    using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

    public class Direct2DRenderer : IDisposable
    {
        private static Color gdiTransparentColor = Color.Transparent;

        private static readonly RawColor4 Direct2DTransparent = new RawColor4(
            gdiTransparentColor.R,
            gdiTransparentColor.G,
            gdiTransparentColor.B,
            gdiTransparentColor.A);

        private SolidColorBrush brush;

        private WindowRenderTarget device;

        private Factory factory;

        private TextFormat font;

        private FontFactory fontFactory;

        private bool resize;

        private int resizeX;

        private int resizeY;

        private HwndRenderTargetProperties targetProperties;

        public Direct2DRenderer(IntPtr targetHwnd, bool vsync)
        {
            this.TargetHandle = targetHwnd;
            this.VSync = vsync;

            this.SetupInstance();
        }

        public RenderTarget RenderTarget => this.device;

        public IntPtr TargetHandle { get; }

        public bool VSync { get; }

        public void Dispose()
        {
            this.brush?.Dispose();
            this.device?.Dispose();
            this.factory?.Dispose();
            this.font?.Dispose();
            this.fontFactory?.Dispose();
        }

        public void BeginScene()
        {
            if (this.device == null) return;

            var msg = default(MSG);

            if (User32.PeekMessage(out msg, IntPtr.Zero, 0, 0, 1) != 0)
            {
                User32.TranslateMessage(ref msg);
                User32.DispatchMessage(ref msg);
            }

            if (this.resize)
            {
                this.device.Resize(new Size2(this.resizeX, this.resizeY));
                this.resize = false;
            }

            this.device.BeginDraw();
        }

        public void BorderedCircle(float x, float y, float radius, float stroke, Color color, Color borderColor)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawEllipse(new Ellipse(new RawVector2(x, y), radius, radius), this.brush, stroke);

            this.brush.Color = new RawColor4(borderColor.R, borderColor.G, borderColor.B, borderColor.A / 255.0f);

            this.device.DrawEllipse(
                new Ellipse(new RawVector2(x, y), radius + stroke, radius + stroke),
                this.brush,
                stroke);

            this.device.DrawEllipse(
                new Ellipse(new RawVector2(x, y), radius - stroke, radius - stroke),
                this.brush,
                stroke);
        }

        public void BorderedLine(
            int start_x,
            int start_y,
            int end_x,
            int end_y,
            float stroke,
            Color color,
            Color borderColor)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawLine(new RawVector2(start_x, start_y), new RawVector2(end_x, end_y), this.brush, stroke);

            this.brush.Color = new RawColor4(borderColor.R, borderColor.G, borderColor.B, borderColor.A / 255.0f);

            this.device.DrawLine(
                new RawVector2(start_x, start_y - stroke),
                new RawVector2(end_x, end_y - stroke),
                this.brush,
                stroke);
            this.device.DrawLine(
                new RawVector2(start_x, start_y + stroke),
                new RawVector2(end_x, end_y + stroke),
                this.brush,
                stroke);

            this.device.DrawLine(
                new RawVector2(start_x - stroke / 2, start_y - stroke * 1.5f),
                new RawVector2(start_x - stroke / 2, start_y + stroke * 1.5f),
                this.brush,
                stroke);
            this.device.DrawLine(
                new RawVector2(end_x - stroke / 2, end_y - stroke * 1.5f),
                new RawVector2(end_x - stroke / 2, end_y + stroke * 1.5f),
                this.brush,
                stroke);
        }

        public void BorderedRectangle(
            float x,
            float y,
            float width,
            float height,
            float stroke,
            float borderStroke,
            Color color,
            Color borderColor)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawRectangle(new RawRectangleF(x, y, x + width, y + height), this.brush, stroke);

            this.brush.Color = new RawColor4(borderColor.R, borderColor.G, borderColor.B, borderColor.A / 255.0f);

            this.device.DrawRectangle(
                new RawRectangleF(
                    x - (stroke - borderStroke),
                    y - (stroke - borderStroke),
                    x + width + stroke - borderStroke,
                    y + height + stroke - borderStroke),
                this.brush,
                borderStroke);

            this.device.DrawRectangle(
                new RawRectangleF(
                    x + (stroke - borderStroke),
                    y + (stroke - borderStroke),
                    x + width - stroke + borderStroke,
                    y + height - stroke + borderStroke),
                this.brush,
                borderStroke);
        }

        public void ClearScene()
        {
            this.device.Clear(Direct2DTransparent);
        }

        public Bitmap CreateBitmap(int width, int height)
        {
            return new Bitmap(this.device, new Size2(width, height), new BitmapProperties(this.device.PixelFormat));
        }

        public void CreateFont(string fontFamilyName, float size, bool bold = false, bool italic = false)
        {
            if (this.font != null) this.font.Dispose();
            this.font = new TextFormat(
                this.fontFactory,
                fontFamilyName,
                bold ? FontWeight.Bold : FontWeight.Normal,
                italic ? FontStyle.Italic : FontStyle.Normal,
                size);
        }

        public void DrawBarH(
            float x,
            float y,
            float width,
            float height,
            float value,
            float stroke,
            Color color,
            Color interiorColor)
        {
            var first = new RawRectangleF(x, y, x + width, y + height);

            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawRectangle(first, this.brush, stroke);

            if (value == 0) return;

            first.Top += height - height / 100.0f * value;

            this.brush.Color = new RawColor4(
                interiorColor.R,
                interiorColor.G,
                interiorColor.B,
                interiorColor.A / 255.0f);

            this.device.FillRectangle(first, this.brush);
        }

        public void DrawBarV(
            float x,
            float y,
            float width,
            float height,
            float value,
            float stroke,
            Color color,
            Color interiorColor)
        {
            var first = new RawRectangleF(x, y, x + width, y + height);

            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawRectangle(first, this.brush, stroke);

            if (value == 0) return;

            first.Right -= width - width / 100.0f * value;

            this.brush.Color = new RawColor4(
                interiorColor.R,
                interiorColor.G,
                interiorColor.B,
                interiorColor.A / 255.0f);

            this.device.FillRectangle(first, this.brush);
        }

        public void DrawBitmap(
            float x,
            float y,
            Bitmap bitmap,
            float opacity = 1f,
            BitmapInterpolationMode interpolationMode = BitmapInterpolationMode.Linear)
        {
            this.device.DrawBitmap(
                bitmap,
                new RawRectangleF(x, y, x + bitmap.PixelSize.Width, y + bitmap.PixelSize.Height),
                opacity,
                interpolationMode);
        }

        public void DrawBox2D(
            float x,
            float y,
            float width,
            float height,
            float stroke,
            Color color,
            Color interiorColor)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            this.device.DrawRectangle(new RawRectangleF(x, y, x + width, y + height), this.brush, stroke);
            this.brush.Color = new RawColor4(
                interiorColor.R,
                interiorColor.G,
                interiorColor.B,
                interiorColor.A / 255.0f);
            this.device.FillRectangle(
                new RawRectangleF(x + stroke, y + stroke, x + width - stroke, y + height - stroke),
                this.brush);
        }

        public void DrawBox3D(
            float x,
            float y,
            float width,
            float height,
            int length,
            float stroke,
            Color color,
            Color interiorColor)
        {
            var first = new RawRectangleF(x, y, x + width, y + height);
            var second = new RawRectangleF(x + length, y - length, first.Right + length, first.Bottom - length);

            var line_start = new RawVector2(x, y);
            var line_end = new RawVector2(second.Left, second.Top);

            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawRectangle(first, this.brush, stroke);
            this.device.DrawRectangle(second, this.brush, stroke);

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            line_start.X += width;
            line_end.X = line_start.X + length;

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            line_start.Y += height;
            line_end.Y += height;

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            line_start.X -= width;
            line_end.X -= width;

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            this.brush.Color = new RawColor4(
                interiorColor.R,
                interiorColor.G,
                interiorColor.B,
                interiorColor.A / 255.0f);

            this.device.FillRectangle(first, this.brush);
            this.device.FillRectangle(second, this.brush);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawCircle(float x, float y, float radius, float stroke, Color color)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            this.device.DrawEllipse(new Ellipse(new RawVector2(x, y), radius, radius), this.brush, stroke);
        }

        public void DrawEdge(float x, float y, float width, float height, int length, float stroke, Color color)
        {
            var first = new RawVector2(x, y);
            var second = new RawVector2(x, y + length);
            var third = new RawVector2(x + length, y);

            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawLine(first, second, this.brush, stroke);
            this.device.DrawLine(first, third, this.brush, stroke);

            first.Y += height;
            second.Y = first.Y - length;
            third.Y = first.Y;
            third.X = first.X + length;

            this.device.DrawLine(first, second, this.brush, stroke);
            this.device.DrawLine(first, third, this.brush, stroke);

            first.X = x + width;
            first.Y = y;
            second.X = first.X - length;
            second.Y = first.Y;
            third.X = first.X;
            third.Y = first.Y + length;

            this.device.DrawLine(first, second, this.brush, stroke);
            this.device.DrawLine(first, third, this.brush, stroke);

            first.Y += height;
            second.X += length;
            second.Y = first.Y - length;
            third.Y = first.Y;
            third.X = first.X - length;

            this.device.DrawLine(first, second, this.brush, stroke);
            this.device.DrawLine(first, third, this.brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawLine(int start_x, int start_y, int end_x, int end_y, float stroke, Color color)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            this.device.DrawLine(
                new RawVector2(start_x + 0.5f, start_y + 0.5f),
                new RawVector2(end_x + 0.5f, end_y + 0.5f),
                this.brush,
                stroke);
        }

        public void DrawPlus(float x, float y, int length, float stroke, Color color)
        {
            var first = new RawVector2(x - length, y);
            var second = new RawVector2(x + length, y);

            var third = new RawVector2(x, y - length);
            var fourth = new RawVector2(x, y + length);

            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawLine(first, second, this.brush, stroke);
            this.device.DrawLine(third, fourth, this.brush, stroke);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DrawRectangle(float x, float y, float width, float height, float stroke, Color color)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            this.device.DrawRectangle(new RawRectangleF(x, y, x + width, y + height), this.brush, stroke);
        }

        public void DrawRectangle3D(float x, float y, float width, float height, int length, float stroke, Color color)
        {
            var first = new RawRectangleF(x, y, x + width, y + height);
            var second = new RawRectangleF(x + length, y - length, first.Right + length, first.Bottom - length);

            var line_start = new RawVector2(x, y);
            var line_end = new RawVector2(second.Left, second.Top);

            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);

            this.device.DrawRectangle(first, this.brush, stroke);
            this.device.DrawRectangle(second, this.brush, stroke);

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            line_start.X += width;
            line_end.X = line_start.X + length;

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            line_start.Y += height;
            line_end.Y += height;

            this.device.DrawLine(line_start, line_end, this.brush, stroke);

            line_start.X -= width;
            line_end.X -= width;

            this.device.DrawLine(line_start, line_end, this.brush, stroke);
        }

        public void DrawText(float x, float y, string text, Color color)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            var layout = new TextLayout(this.fontFactory, text, this.font, float.MaxValue, float.MaxValue);
            this.device.DrawTextLayout(new RawVector2(x, y), layout, this.brush);
            layout.Dispose();
        }

        public void EndScene()
        {
            if (this.device == null) return;

            long tag_1 = 0L, tag_2 = 0L;

            var result = this.device.TryEndDraw(out tag_1, out tag_2);

            if (result.Failure) this.SetupInstance(true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillCircle(float x, float y, float radius, Color color)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            this.device.FillEllipse(new Ellipse(new RawVector2(x, y), radius, radius), this.brush);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillRectangle(float x, float y, float width, float height, Color color)
        {
            this.brush.Color = new RawColor4(color.R, color.G, color.B, color.A / 255.0f);
            this.device.FillRectangle(new RawRectangleF(x, y, x + width, y + height), this.brush);
        }

        public Bitmap LoadBitmap(string file)
        {
            // Loads from file using System.Drawing.Image
            using (var bitmap = (System.Drawing.Bitmap)Image.FromFile(file))
            {
                var sourceArea = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties =
                    new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
                var size = new Size2(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                var stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(
                        sourceArea,
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels
                    for (var y = 0; y < bitmap.Height; y++)
                    {
                        var offset = bitmapData.Stride * y;
                        for (var x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized
                            var B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            var rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return new Bitmap(this.device, size, tempStream, stride, bitmapProperties);
                }
            }
        }

        public void Resize(int x, int y)
        {
            this.resizeX = x;
            this.resizeY = y;
            this.resize = true;
        }

        private void SetupInstance(bool deleteOld = false)
        {
            if (deleteOld)
                try
                {
                    this.brush.Dispose();
                    this.font.Dispose();

                    this.fontFactory.Dispose();
                    this.factory.Dispose();
                    this.device.Dispose();
                }
                catch
                {
                }

            this.factory = new Factory(FactoryType.MultiThreaded, DebugLevel.None);
            this.fontFactory = new FontFactory();

            var bounds = default(RECT);

            User32.GetWindowRect(this.TargetHandle, out bounds);

            this.targetProperties =
                new HwndRenderTargetProperties
                    {
                        Hwnd = this.TargetHandle,
                        PixelSize = new Size2(
                            Math.Abs(bounds.Right - bounds.Left),
                            Math.Abs(bounds.Bottom - bounds.Top)),
                        PresentOptions =
                            this.VSync ? PresentOptions.None : PresentOptions.Immediately
                    };

            var renderTargetProperties = new RenderTargetProperties(
                RenderTargetType.Hardware,
                new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                0,
                0,
                RenderTargetUsage.None,
                FeatureLevel.Level_DEFAULT);

            this.device = new WindowRenderTarget(this.factory, renderTargetProperties, this.targetProperties);

            this.device.TextAntialiasMode = TextAntialiasMode.Default;
            this.device.AntialiasMode = AntialiasMode.PerPrimitive;

            this.brush = new SolidColorBrush(this.device, new RawColor4(0, 0, 0, 0));
        }
    }
}