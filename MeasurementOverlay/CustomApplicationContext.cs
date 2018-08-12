namespace MeasurementOverlay
{
    using System;
    using System.Windows.Forms;

    using MeasurementOverlay.Forms;
    using MeasurementOverlay.Properties;

    using NHotkey;
    using NHotkey.WindowsForms;

    public class CustomApplicationContext : ApplicationContext
    {
        private readonly MenuItem crosshairMenuItem;

        private readonly MenuItem hideMenuItem;

        private readonly Overlay overlay;

        private readonly NotifyIcon trayIcon;

        public CustomApplicationContext()
        {
            if (Settings.Default.UpgradeSettings)
            {
                Settings.Default.Upgrade();
                Settings.Default.Save();
            }

            this.overlay = new Overlay();

            this.UpdateHotkeys();

            this.hideMenuItem = new MenuItem("&Hide All", this.ToggleHideAll);
            this.crosshairMenuItem = new MenuItem("C&rosshair", this.ToggleCrosshair);

            this.trayIcon = new NotifyIcon
                                {
                                    Icon = Resources.Icon,
                                    ContextMenu =
                                        new ContextMenu(
                                            new[]
                                                {
                                                    new MenuItem("Add Guid&elines", this.AddGuidelines),
                                                    new MenuItem("Add &Grid", this.AddGrid),
                                                    this.crosshairMenuItem, this.hideMenuItem,
                                                    new MenuItem("C&lear All", this.ClearAll),
                                                    new MenuItem("-"),
                                                    new MenuItem("&Settings", this.DisplaySettings),
                                                    new MenuItem("-"), new MenuItem("&Exit", this.Exit)
                                                }),
                                    Visible = true
                                };

            this.trayIcon.DoubleClick += this.AddGuidelines;
        }

        private void AddGrid(object sender, EventArgs e)
        {
            this.overlay.AddGrid();
        }

        private void AddGuidelines(object sender, EventArgs e)
        {
            this.overlay.AddGuideLines();
        }

        private void ClearAll(object sender, EventArgs e)
        {
            this.overlay.Clear();
            if (!this.overlay.Enabled) this.hideMenuItem.Checked = false;
        }

        private void DisplaySettings(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm())
            {
                settingsForm.AddGuidelinesKeyData = Settings.Default.AddGuidelinesHotkey;
                settingsForm.AddGridKeyData = Settings.Default.AddGridHotkey;
                settingsForm.ToggleCrosshairKeyData = Settings.Default.ToggleCrosshairHotkey;
                settingsForm.ToggleHideAllKeyData = Settings.Default.ToggleHideAllHotkey;
                settingsForm.ClearAllKeyData = Settings.Default.ClearAllHotkey;

                settingsForm.AutoStart = AutoStartHandler.AutoStart;

                if (settingsForm.ShowDialog() != DialogResult.OK) return;

                Settings.Default.AddGuidelinesHotkey = settingsForm.AddGuidelinesKeyData;
                Settings.Default.AddGridHotkey = settingsForm.AddGridKeyData;
                Settings.Default.ToggleCrosshairHotkey = settingsForm.ToggleCrosshairKeyData;
                Settings.Default.ToggleHideAllHotkey = settingsForm.ToggleHideAllKeyData;
                Settings.Default.ClearAllHotkey = settingsForm.ClearAllKeyData;

                Settings.Default.Save();

                AutoStartHandler.AutoStart = settingsForm.AutoStart;
            }

            this.UpdateHotkeys();
        }

        private void Exit(object sender, EventArgs e)
        {
            this.overlay.Disable();

            // Hide tray icon, otherwise it will remain shown until user mouses over it
            this.trayIcon.Visible = false;

            Application.Exit();
        }

        private void ToggleCrosshair(object sender, EventArgs e)
        {
            this.crosshairMenuItem.Checked = !this.crosshairMenuItem.Checked;
            this.overlay.Crosshair = this.crosshairMenuItem.Checked;
        }

        private void ToggleHideAll(object sender, EventArgs e)
        {
            this.hideMenuItem.Checked = !this.hideMenuItem.Checked;
            if (this.hideMenuItem.Checked) this.overlay.Disable();
            else this.overlay.Enable();
        }

        private static void UpdateHotkey(string name, Keys keyData, EventHandler<HotkeyEventArgs> handler = null)
        {
            if (keyData == Keys.None) HotkeyManager.Current.Remove(name);
            else
                try
                {
                    HotkeyManager.Current.AddOrReplace(name, keyData, handler);
                }
                catch (HotkeyAlreadyRegisteredException)
                {
                }
        }

        private void UpdateHotkeys()
        {
            UpdateHotkey("AddGuidelines", Settings.Default.AddGuidelinesHotkey, this.AddGuidelines);
            UpdateHotkey("AddGrid", Settings.Default.AddGridHotkey, this.AddGrid);
            UpdateHotkey("ToggleCrosshair", Settings.Default.ToggleCrosshairHotkey, this.ToggleCrosshair);
            UpdateHotkey("ToggleHideAll", Settings.Default.ToggleHideAllHotkey, this.ToggleHideAll);
            UpdateHotkey("ClearAll", Settings.Default.ClearAllHotkey, this.ClearAll);
        }
    }
}