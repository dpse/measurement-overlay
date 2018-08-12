namespace MeasurementOverlay.Forms
{
    using System;
    using System.Windows.Forms;

    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            this.InitializeComponent();
        }

        public Keys AddGridKeyData
        {
            get => this.hotkeyBoxAddGrid.KeyData;
            set => this.hotkeyBoxAddGrid.KeyData = value;
        }

        public Keys AddGuidelinesKeyData
        {
            get => this.hotkeyBoxAddGuidelines.KeyData;
            set => this.hotkeyBoxAddGuidelines.KeyData = value;
        }

        public bool AutoStart
        {
            get => this.checkBoxAutoStart.Checked;
            set => this.checkBoxAutoStart.Checked = value;
        }

        public Keys ClearAllKeyData
        {
            get => this.hotkeyBoxClearAll.KeyData;
            set => this.hotkeyBoxClearAll.KeyData = value;
        }

        public Keys ToggleCrosshairKeyData
        {
            get => this.hotkeyBoxToggleCrosshair.KeyData;
            set => this.hotkeyBoxToggleCrosshair.KeyData = value;
        }

        public Keys ToggleHideAllKeyData
        {
            get => this.hotkeyBoxToggleHideAll.KeyData;
            set => this.hotkeyBoxToggleHideAll.KeyData = value;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}