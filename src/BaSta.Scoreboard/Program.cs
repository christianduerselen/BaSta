using System;
using System.Threading;
using System.Windows.Forms;

namespace BaSta.Scoreboard
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
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                if (ex.Message.Length <= 0)
                    return;
                MessageBox.Show(ex.Message);
            }
        }

        private static bool IsOtherInstanceRunning()
        {
            _mutex = new Mutex(false, Application.ProductName + "_MultiStartPrevent");
            return !_mutex.WaitOne(0, true);
        }
    }
}