using System;
using System.IO.Ports;
using System.Linq;
using System.Management;

namespace BaSta.Link
{
    public static class SerialPortSelector
    {
        public static string Select(string name)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"Please enter the COM port number for '{name}':");

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                var tList = (from n in portnames
                    join p in ports on n equals p["DeviceID"].ToString()
                    select " [" + n + "] " + p["Caption"]).ToList();

                tList.ForEach(Console.WriteLine);
            }

            Console.Write("Port number: (e.g. '1' for COM1): ");
            Console.ResetColor();
            
            return "COM" + Console.ReadLine();
        }
    }
}