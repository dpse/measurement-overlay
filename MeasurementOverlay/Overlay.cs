namespace MeasurementOverlay
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using MeasurementOverlay.Forms;
    using MeasurementOverlay.InputDevices;
    using MeasurementOverlay.PInvoke;
    using MeasurementOverlay.PInvoke.Enums;
    using MeasurementOverlay.Rendering;

    internal class Overlay : IDisposable
    {
        private readonly Target newTarget = new Target();

        private readonly OverlayWindow overlayWindow;

        private readonly object padLock = new object();

        private readonly List<Target> Targets = new List<Target>();

        private bool adding;

        private bool crosshair;

        private int currentPaletteNumber;

        private InvisibleForm inputForm;

        private Point mousePosition;

        private CancellationTokenSource overlayCancellationTokenSource;

        private Task overlayTask;

        private Rectangle screen;

        private bool update;

        public Overlay()
        {
            this.overlayWindow = new OverlayWindow();
            this.overlayWindow.Graphics.CreateFont("Consolas", 16);
            this.ResetNewTarget(OverlayType.Guidelines, true);
        }

        public bool Crosshair
        {
            get
            {
                lock (this.padLock)
                {
                    return this.crosshair;
                }
            }
            set
            {
                lock (this.padLock)
                {
                    if (value == this.crosshair) return;

                    this.crosshair = value;
                    this.update = true;
                }

                // Automatically enable or disable if necessary
                this.AutoEnableDisable();
            }
        }

        public bool Enabled { get; private set; }

        public void Dispose()
        {
            this.overlayCancellationTokenSource?.Cancel();

            this.inputForm?.Dispose();
            this.overlayCancellationTokenSource?.Dispose();

            this.overlayWindow?.Dispose();
        }

        private void Add(OverlayType type)
        {
            lock (this.padLock)
            {
                if (this.adding) return;

                this.newTarget.Type = type;
                this.ResetNewTargetPosition();
                this.update = true;
            }

            // Automatically enable if needed
            if (!this.Enabled) this.Enable();

            this.RunInputForm();
        }

        public void AddGrid()
        {
            this.Add(OverlayType.Grid);
        }

        public void AddGuideLines()
        {
            this.Add(OverlayType.Guidelines);
        }

        private void AddTarget()
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                this.Targets.Add(
                    new Target
                        {
                            Position = this.newTarget.Position,
                            Stroke = this.newTarget.Stroke,
                            Type = this.newTarget.Type,
                            Pitch = this.newTarget.Pitch,
                            Color = this.newTarget.Color
                        });

                this.update = true;
            }
        }

        private void AutoEnableDisable()
        {
            if (this.Enabled)
            {
                var disable = false;
                lock (this.padLock)
                {
                    disable = !this.crosshair && this.Targets.Count == 0 && !this.adding;
                }
                if (disable) this.Disable();
            }
            else
            {
                var enable = false;
                lock (this.padLock)
                {
                    enable = this.crosshair || this.Targets.Count != 0 || this.adding;
                }
                if (enable) this.Enable();
            }
        }

        public void CancelAdd()
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                this.update = true;
            }

            this.CloseInputForm();
        }

        private void ChangeNewTargetColor(int number)
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                var newColor = this.GetPaletteColor(number);

                if (this.newTarget.Color.Equals(newColor)) return;

                this.newTarget.Color = newColor;
                this.update = true;
            }

            this.currentPaletteNumber = number;
        }

        private void ChangeNewTargetPitch(int offset)
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                var newPitch = Limit(1, 10000, this.newTarget.Pitch + offset);

                if (this.newTarget.Pitch == newPitch) return;

                this.newTarget.Pitch = newPitch;
                this.update = true;
            }
        }

        private void ChangeNewTargetPosition(int xOffset, int yOffset)
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                var x = Limit(this.screen.Left, this.screen.Right, this.newTarget.Position.X + xOffset);
                var y = Limit(this.screen.Top, this.screen.Bottom, this.newTarget.Position.Y + yOffset);

                var newPoint = new Point(x, y);

                if (this.newTarget.Position.Equals(newPoint)) return;

                this.newTarget.Position = newPoint;
                this.update = true;
            }
        }

        private void ChangeNewTargetStroke(int offset)
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                var newStroke = Limit(1, 200, this.newTarget.Stroke + offset);

                if (this.newTarget.Stroke == newStroke) return;

                this.newTarget.Stroke = newStroke;
                this.update = true;
            }
        }

        private void ChangeNewTargetStrokeOrPitch(int offset)
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                if (this.newTarget.Type == OverlayType.Grid) this.ChangeNewTargetPitch(offset);
                else this.ChangeNewTargetStroke(offset);
            }
        }

        private void ChangeNewTargetType()
        {
            lock (this.padLock)
            {
                if (!this.adding) return;

                this.newTarget.Type = this.newTarget.Type == OverlayType.Guidelines
                                          ? OverlayType.Grid
                                          : OverlayType.Guidelines;
                this.update = true;
            }
        }

        private void CheckMousePosition()
        {
            try
            {
                var mousePosition = Mouse.GetCursorPosition();
                if (!mousePosition.Equals(this.mousePosition))
                {
                    lock (this.padLock)
                    {
                        this.newTarget.Position = mousePosition;

                        if (this.crosshair || this.adding) this.update = true;
                    }
                    this.mousePosition = mousePosition;
                }
            }
            catch (InvalidOperationException)
            {
                // Failed to ger mouse position
            }
        }

        public void Clear()
        {
            lock (this.padLock)
            {
                this.Targets.Clear();

                var gfx = this.overlayWindow.Graphics;
                gfx.BeginScene();
                gfx.ClearScene();
                gfx.EndScene();
            }

            // Automatically cancel background threads if they are no longer needed
            this.AutoEnableDisable();
        }

        private void CloseInputForm()
        {
            lock (this.padLock)
            {
                if (!this.adding) return;
            }

            this.inputForm?.Close();
            this.inputForm?.Dispose();

            lock (this.padLock)
            {
                this.adding = false;
                this.update = true;
            }
        }

        public void Disable()
        {
            if (!this.Enabled) return;

            this.CloseInputForm();

            // Cancel any active threads
            this.overlayCancellationTokenSource?.Cancel();

            lock (this.padLock)
            {
                this.overlayWindow.Hide();
            }

            // Wait for tasks to cancel
            this.overlayTask?.Wait();

            this.Enabled = false;
        }

        private void Draw()
        {
            lock (this.padLock)
            {
                if (!this.update) return;

                var gfx = this.overlayWindow.Graphics;

                gfx.BeginScene();
                gfx.ClearScene();

                foreach (var target in this.Targets) DrawTarget(gfx, this.screen, target);

                if (this.adding)
                {
                    DrawTarget(gfx, this.screen, this.newTarget);
                    this.DrawCoordinates(gfx);
                }
                else if (this.crosshair)
                {
                    DrawCross(gfx, this.screen, this.mousePosition, this.GetPaletteColor(0));
                }

                gfx.EndScene();

                this.update = false;
            }
        }

        private void DrawCoordinates(Direct2DRenderer gfx)
        {
            var coordinates = new Point(this.newTarget.Position.X, this.newTarget.Position.Y);

            if (this.Targets.Count > 0)
            {
                // Use relative coordinates
                var target = this.Targets[this.Targets.Count - 1];
                coordinates.X -= target.Position.X;
                coordinates.Y -= target.Position.Y;
                if (target.Type == OverlayType.Grid)
                {
                    // Grid relative
                    coordinates.X %= target.Pitch;
                    coordinates.Y %= target.Pitch;
                    if (coordinates.X < 0) coordinates.X += target.Pitch;
                    if (coordinates.Y < 0) coordinates.Y += target.Pitch;
                }
            }

            var text = string.Empty;
            if (this.newTarget.Type == OverlayType.Guidelines)
                text = $"({coordinates.X}, {coordinates.Y}, {this.newTarget.Stroke})";
            else if (this.newTarget.Type == OverlayType.Grid)
                text = $"({coordinates.X}, {coordinates.Y}, {this.newTarget.Stroke}, {this.newTarget.Pitch})";

            DrawText(
                gfx,
                this.screen,
                this.newTarget.Position,
                this.newTarget.Color,
                text,
                this.newTarget.Stroke / 2 + 5);
        }

        private static void DrawCross(Direct2DRenderer gfx, Rectangle screen, Point pos, Color color, int stroke = 1)
        {
            var x = pos.X - screen.X;
            var y = pos.Y - screen.Y;

            gfx.DrawLine(x, 0, x, screen.Height, stroke, color);
            gfx.DrawLine(0, y, screen.Width, y, stroke, color);
        }

        private static void DrawGrid(
            Direct2DRenderer gfx,
            Rectangle screen,
            Point pos,
            int pitch,
            Color color,
            int stroke = 1)
        {
            if (pitch < 1) pitch = 1;

            var x = pos.X - screen.X;
            var y = pos.Y - screen.Y;

            for (var i = 0 + x % pitch; i < screen.Width; i += pitch)
                gfx.DrawLine(i, 0, i, screen.Height, stroke, color);
            for (var i = 0 + y % pitch; i < screen.Height; i += pitch)
                gfx.DrawLine(0, i, screen.Width, i, stroke, color);
        }

        private static void DrawTarget(Direct2DRenderer gfx, Rectangle screen, Target target)
        {
            if (target.Type == OverlayType.Guidelines)
                DrawCross(gfx, screen, target.Position, target.Color, target.Stroke);
            else if (target.Type == OverlayType.Grid)
                DrawGrid(gfx, screen, target.Position, target.Pitch, target.Color, target.Stroke);
        }

        private static void DrawText(
            Direct2DRenderer gfx,
            Rectangle screen,
            Point pos,
            Color color,
            string text,
            int offset = 5)
        {
            var x = pos.X - screen.X + offset;
            var y = pos.Y - screen.Y + offset;

            gfx.DrawText(x, y, text, color);
        }

        public void Enable()
        {
            // This method enables the overlay system and spawns
            if (this.Enabled) return;

            // Spawn overlay thread
            this.overlayCancellationTokenSource = new CancellationTokenSource();

            this.overlayTask = new Task(this.UpdateOverlay, this.overlayCancellationTokenSource.Token);
            this.overlayTask.Start();

            lock (this.padLock)
            {
                this.overlayWindow.Show();
            }

            this.Enabled = true;
        }

        private void FormOnKeyDown(object sender, KeyEventArgs e)
        {
            var offset = e.Shift ? 20 : e.Alt ? 10 : e.Control ? 5 : 1;
            var paletteModifier = e.Shift ? 30 : e.Alt ? 20 : e.Control ? 10 : 0;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.CancelAdd();
                    e.Handled = true;
                    break;

                case Keys.Back:
                    this.ResetNewTarget();
                    e.Handled = true;
                    break;

                case Keys.Enter:
                    this.AddTarget();
                    e.Handled = true;
                    break;

                case Keys.Tab:
                    this.ChangeNewTargetType();
                    e.Handled = true;
                    break;

                case Keys.Space:
                    this.ChangeNewTargetColor(this.IterateCurrentPaletteNumber());
                    e.Handled = true;
                    break;

                case Keys.Add:
                    this.ChangeNewTargetStrokeOrPitch(offset);
                    e.Handled = true;
                    break;

                case Keys.Subtract:
                    this.ChangeNewTargetStrokeOrPitch(-offset);
                    e.Handled = true;
                    break;

                case Keys.Left:
                    this.ChangeNewTargetPosition(-offset, 0);
                    break;

                case Keys.Right:
                    this.ChangeNewTargetPosition(offset, 0);
                    break;
                case Keys.Up:
                    this.ChangeNewTargetPosition(0, -offset);
                    break;
                case Keys.Down:
                    this.ChangeNewTargetPosition(0, offset);
                    break;

                case Keys.D1:
                case Keys.NumPad1:
                    this.ChangeNewTargetColor(paletteModifier + 0);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    this.ChangeNewTargetColor(paletteModifier + 1);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    this.ChangeNewTargetColor(paletteModifier + 2);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    this.ChangeNewTargetColor(paletteModifier + 3);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    this.ChangeNewTargetColor(paletteModifier + 4);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    this.ChangeNewTargetColor(paletteModifier + 5);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    this.ChangeNewTargetColor(paletteModifier + 6);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    this.ChangeNewTargetColor(paletteModifier + 7);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    this.ChangeNewTargetColor(paletteModifier + 8);
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                    this.ChangeNewTargetColor(paletteModifier + 9);
                    break;
            }
        }

        private void FormOnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) this.AddTarget();
            else if (e.Button == MouseButtons.Right) this.CancelAdd();
            else if (e.Button == MouseButtons.Middle)
                this.ChangeNewTargetColor(this.IterateCurrentPaletteNumber(false));
        }

        private void FormOnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0) return;

            var isGrid = false;
            lock (this.padLock)
            {
                isGrid = this.newTarget.Type == OverlayType.Grid;
            }

            var multiplier = !isGrid && Keyboard.KeyPressed(VirtualKeyStates.VK_SHIFT)
                                 ? 20
                                 : Keyboard.KeyPressed(VirtualKeyStates.VK_MENU)
                                     ? 10
                                     : Keyboard.KeyPressed(VirtualKeyStates.VK_CONTROL)
                                         ? 5
                                         : 1;
            var offset = e.Delta * multiplier / 120;

            if (isGrid && Keyboard.KeyPressed(VirtualKeyStates.VK_SHIFT)) this.ChangeNewTargetStroke(offset);
            else this.ChangeNewTargetStrokeOrPitch(offset);
        }

        private Color GetPaletteColor(int number)
        {
            var range = number / 10;
            int alpha;
            switch (range)
            {
                default:
                case 0:
                    alpha = 220;
                    break;
                case 1:
                    alpha = 255;
                    break;
                case 2:
                    alpha = 200;
                    break;
                case 3:
                    alpha = 180;
                    break;
                case 4:
                    alpha = 140;
                    break;
                case 5:
                    alpha = 100;
                    break;
                case 6:
                    alpha = 60;
                    break;
                case 7:
                    alpha = 20;
                    break;
                case 8:
                    alpha = 5;
                    break;
            }
            switch (number % 10)

            {
                default:
                case 0: return Color.FromArgb(alpha, 255, 0, 0);
                case 1: return Color.FromArgb(alpha, 0, 255, 0);
                case 2: return Color.FromArgb(alpha, 0, 0, 255);
                case 3: return Color.FromArgb(alpha, 255, 255, 0);
                case 4: return Color.FromArgb(alpha, 255, 0, 255);
                case 5: return Color.FromArgb(alpha, 0, 255, 255);
                case 6: return Color.FromArgb(alpha, 0, 0, 0);
                case 7: return Color.FromArgb(alpha, 255, 255, 255);
                case 8: return Color.FromArgb(alpha, 127, 127, 127);
                case 9: return Color.FromArgb(alpha, 200, 200, 200);
            }
        }

        private void InputFormOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.AddTarget();
                this.CancelAdd();
            }
        }

        private int IterateCurrentPaletteNumber(bool onlyStepAlpha = true)
        {
            if (onlyStepAlpha)
            {
                this.currentPaletteNumber += 10;
                if (this.currentPaletteNumber > 89) this.currentPaletteNumber %= 10;
            }
            else
            {
                this.currentPaletteNumber++;
                if (this.currentPaletteNumber > 89) this.currentPaletteNumber = 0;
            }

            return this.currentPaletteNumber;
        }

        private static int Limit(int min, int max, int value)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void ResetNewTarget(bool resetColor = false)
        {
            lock (this.padLock)
            {
                this.ResetNewTarget(this.newTarget.Type, resetColor);
            }
        }

        private void ResetNewTarget(OverlayType type, bool resetColor = false)
        {
            lock (this.padLock)
            {
                this.newTarget.Stroke = 1;
                this.newTarget.Pitch = 50;
                this.newTarget.Type = type;
                if (resetColor) this.newTarget.Color = this.GetPaletteColor(0);
                if (this.adding) this.update = true;
            }
        }

        private void ResetNewTargetPosition()
        {
            lock (this.padLock)
            {
                try
                {
                    this.newTarget.Position = Mouse.GetCursorPosition();
                    if (this.adding) this.update = true;
                }
                catch (InvalidOperationException)
                {
                    // Failed to ger mouse position
                }
            }
        }

        private void RunInputForm()
        {
            lock (this.padLock)
            {
                if (this.adding) return;
                this.adding = true;
            }

            this.inputForm = new InvisibleForm();
            this.inputForm.KeyDown += this.FormOnKeyDown;
            this.inputForm.MouseClick += this.FormOnMouseClick;
            this.inputForm.MouseDoubleClick += this.InputFormOnMouseDoubleClick;
            this.inputForm.MouseWheel += this.FormOnMouseWheel;

            this.inputForm.Show();
            this.inputForm.Activate();
            this.inputForm.BringToFront();
            User32.SetForegroundWindow(this.inputForm.Handle.ToInt32());
            this.inputForm.Focus();
        }

        private void UpdateOverlay()
        {
            lock (this.padLock)
            {
                this.update = true;
            }

            while (true)
            {
                this.UpdateScreenSize();
                this.CheckMousePosition();

                this.Draw();

                this.overlayCancellationTokenSource.Token.WaitHandle.WaitOne(20);

                if (this.overlayCancellationTokenSource.IsCancellationRequested) return;
            }
        }

        private void UpdateScreenSize()
        {
            var screen = SystemInformation.VirtualScreen;
            lock (this.padLock)
            {
                if (screen.Equals(this.screen)) return;
                this.overlayWindow.SetBounds(screen.X, screen.Y, screen.Width, screen.Height);
                this.screen = screen;
                this.update = true;
            }
        }

        private enum OverlayType
        {
            Guidelines,

            Grid
        }

        private class Target
        {
            public Color Color { get; set; }

            public int Pitch { get; set; }

            public Point Position { get; set; }

            public int Stroke { get; set; }

            public OverlayType Type { get; set; }
        }
    }
}