namespace MeasurementOverlay.Forms
{
    using System.Drawing;
    using System.Windows.Forms;

    using MeasurementOverlay.Properties;

    public class InvisibleForm : Form
    {
        public InvisibleForm()
        {
            this.Icon = Resources.Icon;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.ShowIcon = false;
            this.TopMost = true;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = SystemInformation.VirtualScreen.Size;
            this.Location = SystemInformation.VirtualScreen.Location;
            this.KeyPreview = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            /* Ignore */
        }
    }
}