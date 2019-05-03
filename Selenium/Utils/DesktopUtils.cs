using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Selenium.Utils
{
    public static class DesktopUtils
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Method to bring any desktop window to front. Window shouldn't be minimized.
        /// </summary>
        /// <param name="className">Field isn't mandatory, null can be pushed.</param>
        /// <param name="windowName">Field is mandatory.</param>
        /// Example for 'className' - 'CalcFrame' for Calculator, 'Chrome_WidgetWin_1' for Chrome, etc
        /// Example for 'windowName' - 'Calculator' for Calculator, 'Google - Google Chrome' for Chrome (Open https://www.google.com)
        /// <returns>Boolean result</returns>
        public static bool BringApplicationToFront(string windowName, string className = null)
        {
            bool isSuccess = false;
            try
            {
                var window = FindWindow(className, windowName);

                if (window != IntPtr.Zero)
                {
                    isSuccess = SetForegroundWindow(window);
                }
                return isSuccess;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get current executing path
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
