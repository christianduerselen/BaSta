using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
    public class Mci : IDisposable
    {
        private string _filename = "";
        private string alias;
        private bool isOpen;

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(
            string lpstrCommand,
            StringBuilder lpstrReturnString,
            int uReturnLength,
            IntPtr hwndCallback);

        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciGetErrorString(int dwError, StringBuilder lpstrBuffer, int uLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetShortPathName(
            string lpszLongPath,
            StringBuilder lpszShortPath,
            int cchBuffer);

        public string FileName
        {
            get
            {
                return _filename;
            }
        }

        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        private string GetMciError(int errorCode)
        {
            StringBuilder lpstrBuffer = new StringBuilder((int) byte.MaxValue);
            if (Mci.mciGetErrorString(errorCode, lpstrBuffer, lpstrBuffer.Capacity) == 0)
                return "MCI-Fehler " + (object) errorCode;
            return lpstrBuffer.ToString();
        }

        public void Open(string filename)
        {
            Open(filename, (Control) null);
        }

        public void Open(string filename, Control owner)
        {
            if (IsOpen)
                Close();
            alias = Guid.NewGuid().ToString("N");
            if (!File.Exists(filename))
                throw new FileNotFoundException("Die Datei '" + filename + "' existiert nicht", filename);
            StringBuilder lpszShortPath = new StringBuilder(261);
            if (Mci.GetShortPathName(filename, lpszShortPath, lpszShortPath.Capacity) == 0)
                throw new MciException("Fehler beim Auslesen des kurzen Dateinamens für '" + filename + "': Windows-Fehler " + (object) Marshal.GetLastWin32Error());
            string lpstrCommand = "open " + lpszShortPath.ToString() + " type mpegvideo alias " + alias;
            if (owner != null)
                lpstrCommand = lpstrCommand + " parent " + (object) (int) owner.Handle + " style child";
            Mci.mciSendString(lpstrCommand, (StringBuilder) null, 0, IntPtr.Zero);
            isOpen = true;
            _filename = FileName;
            Mci.mciSendString("set " + alias + " time format ms", (StringBuilder) null, 0, IntPtr.Zero);
        }

        public int Length
        {
            get
            {
                StringBuilder lpstrReturnString = new StringBuilder(261);
                Mci.mciSendString("status " + alias + " length", lpstrReturnString, lpstrReturnString.Capacity, IntPtr.Zero);
                return int.Parse(lpstrReturnString.ToString());
            }
        }

        public void Play(bool repeat)
        {
            Play(0, Length, repeat);
        }

        public void Play(int from, bool repeat)
        {
            Play(from, Length - from, repeat);
        }

        public void Play(int from, int to, bool repeat)
        {
            string lpstrCommand = "play " + alias + " from " + (object) from + " to " + (object) to;
            if (repeat)
                lpstrCommand += " repeat";
            int errorCode = Mci.mciSendString(lpstrCommand, (StringBuilder) null, 0, IntPtr.Zero);
            if (errorCode != 0)
                throw new MciException("Fehler beim Aufruf von 'Play': " + GetMciError(errorCode));
        }

        public int Volume
        {
            get
            {
                StringBuilder lpstrReturnString = new StringBuilder(261);
                int errorCode = Mci.mciSendString("status " + alias + " volume", lpstrReturnString, lpstrReturnString.Capacity, IntPtr.Zero);
                if (errorCode != 0)
                    throw new MciException("Fehler beim Lesen von 'Volume': " + GetMciError(errorCode));
                return int.Parse(lpstrReturnString.ToString());
            }
            set
            {
                try
                {
                    Mci.mciSendString("setaudio " + alias + " volume to " + (object) value, (StringBuilder) null, 0, IntPtr.Zero);
                }
                catch
                {
                }
            }
        }

        public int Position
        {
            get
            {
                StringBuilder lpstrReturnString = new StringBuilder(261);
                int errorCode = Mci.mciSendString("status " + alias + " position", lpstrReturnString, lpstrReturnString.Capacity, IntPtr.Zero);
                if (errorCode != 0)
                    throw new MciException("Fehler beim Lesen von 'Position': " + GetMciError(errorCode));
                return int.Parse(lpstrReturnString.ToString());
            }
            set
            {
                int errorCode1 = Mci.mciSendString("seek " + alias + " to " + (object) value, (StringBuilder) null, 0, IntPtr.Zero);
                if (errorCode1 != 0)
                    throw new MciException("Fehler beim Setzen von 'Position'" + GetMciError(errorCode1));
                int errorCode2 = Mci.mciSendString("play " + alias, (StringBuilder) null, 0, IntPtr.Zero);
                if (errorCode2 != 0)
                    throw new MciException("Fehler beim Aufruf von 'Play': " + GetMciError(errorCode2));
            }
        }

        public int PlaybackSpeed
        {
            get
            {
                StringBuilder lpstrReturnString = new StringBuilder(261);
                int errorCode = Mci.mciSendString("status " + alias + " speed", lpstrReturnString, lpstrReturnString.Capacity, IntPtr.Zero);
                if (errorCode != 0)
                    throw new MciException("Fehler beim Lesen von 'Speed'" + GetMciError(errorCode));
                return int.Parse(lpstrReturnString.ToString());
            }
            set
            {
                int errorCode = Mci.mciSendString("set " + alias + " speed " + (object) value, (StringBuilder) null, 0, IntPtr.Zero);
                if (errorCode != 0)
                    throw new MciException("Fehler beim Setzen von 'Speed': " + GetMciError(errorCode));
            }
        }

        public void SetRectangle(int x, int y, int width, int height)
        {
            Mci.mciSendString("put " + alias + " window at " + (object) x + " " + (object) y + " " + (object) width + " " + (object) height, (StringBuilder) null, 0, IntPtr.Zero);
        }

        public void Stop()
        {
            Mci.mciSendString("stop " + alias, (StringBuilder) null, 0, IntPtr.Zero);
        }

        public void Close()
        {
            if (!isOpen)
                return;
            int errorCode = Mci.mciSendString("close " + alias, (StringBuilder) null, 0, IntPtr.Zero);
            if (errorCode != 0)
                Console.WriteLine("Fehler beim Aufruf von 'Close': " + GetMciError(errorCode).ToString());
            isOpen = false;
        }

        public void Dispose()
        {
            Close();
        }

        ~Mci()
        {
            Close();
        }
    }
}
