namespace MeasurementOverlay.Forms.Controls
{
    using System.Windows.Forms;

    internal class HotkeyBox : TextBox
    {
        private Keys keyData;

        public Keys KeyCode => this.KeyData & ~Keys.Modifiers;

        public Keys KeyData
        {
            get => this.keyData;
            set
            {
                if (value == this.keyData) return;

                this.keyData = value;
                this.UpdateText();
            }
        }

        public int KeyValue => (int)this.KeyData;

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                this.KeyData = Keys.None;
                this.Clear();
            }
            else
            {
                this.KeyData = e.KeyData;
            }

            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void UpdateText()
        {
            if (this.keyData == Keys.None)
            {
                this.Clear();
            }
            else
            {
                var converter = new KeysConverter();
                this.Text = converter.ConvertToString(this.keyData);
            }
        }
    }
}