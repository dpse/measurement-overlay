namespace MeasurementOverlay
{
    using System.Windows.Forms;

    using Microsoft.Win32;

    internal static class AutoStartHandler
    {
        private const string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        public static bool AutoStart
        {
            get
            {
                bool value;
                using (var rkApp = Registry.CurrentUser.OpenSubKey(key, true))
                {
                    value = rkApp.GetValue(Application.ProductName) != null;
                    rkApp.Close();
                }

                return value;
            }
            set
            {
                using (var rkApp = Registry.CurrentUser.OpenSubKey(key, true))
                {
                    if (rkApp.GetValue(Application.ProductName) == null)
                    {
                        if (value) rkApp.SetValue(Application.ProductName, Application.ExecutablePath);
                    }
                    else
                    {
                        if (!value) rkApp.DeleteValue(Application.ProductName, false);
                    }

                    rkApp.Close();
                }
            }
        }
    }
}