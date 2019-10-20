using System;
using System.Windows.Forms;

namespace BaSta.Layout
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length < 1)
                Application.Run(new Form1(string.Empty));
            else
                Application.Run(new Form1(args[0]));
        }
    }
}