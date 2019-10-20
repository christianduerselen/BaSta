using System;
using System.Threading;
using System.Windows.Forms;

namespace BaSta.Game
{
    internal static class Program
    {
        private static Mutex _mutex;

        [STAThread]
        private static void Main()
        {
            if (IsOtherInstanceRunning())
            {
                MessageBox.Show(@"Dieses Programm kann nicht mehrfach ausgeführt werden.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    Application.Run(new Form1());
                }
                catch
                {
                    // Ignored
                }
            }
        }

        public static bool IsOtherInstanceRunning()
        {
            _mutex = new Mutex(false, Application.ProductName + "_MultiStartPrevent");
            return !_mutex.WaitOne(0, true);
        }
    }
}