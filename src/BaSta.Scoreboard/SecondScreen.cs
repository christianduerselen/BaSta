using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BaSta.Scoreboard.Properties;
using Timer = System.Windows.Forms.Timer;

namespace BaSta.Scoreboard
{
    public class SecondScreen : Form
    {
        private bool _my_effect_ready = true;
        private string[] _effect_names = Enum.GetNames(typeof(Effects.FadingEffect));
        private string _layout_pre_name = string.Empty;
        private string _loaded_layout_name = string.Empty;
        private string _old_data = string.Empty;
        private string _filename = string.Empty;
        private string _servername = string.Empty;
        private string _databasename = string.Empty;
        private string _password = string.Empty;
        private string _username = "sa";
        private int _teamID_home = -1;
        private int _teamID_guest = -1;
        private Dictionary<string, ScoreboardLabel> _fields = new Dictionary<string, ScoreboardLabel>();
        private Point _graphic_location = new Point(0, 0);
        private Size _graphic_size = new Size(1, 1);
        private string _teamname_home = "HEIM";
        private string _teamname_guest = "GAST";
        private Effects.IEffect _my_effect;
        private bool _layout_loaded;
        private bool _loading;
        private DataBaseFunctions _dbfunc;
        private bool _database_connected;
        private Team _home_team;
        private Team _guest_team;
        private Panel _background_pnl;
        private PictureBox pictureBox1;
        private Mci _mci;
        private Timer _mci_timer;
        private int _ext_effect_index;
        private bool _graphic_in_window;
        private Image _actual_picture;
        private bool _ext_graphic_in_window;
        private bool _allow_lan_graphic;
        private bool _show_video;
        
        public PictureBox Picturebox
        {
            get
            {
                return pictureBox1;
            }
        }

        public bool GraphicInWindow
        {
            set
            {
                _graphic_in_window = value;
            }
        }

        public Image ActualPicture
        {
            get
            {
                return _actual_picture;
            }
            set
            {
                _actual_picture = value;
            }
        }

        public bool AllowGraphicFromLAN
        {
            set
            {
                _allow_lan_graphic = value;
            }
        }

        public Point GraphicLocation
        {
            get
            {
                return _graphic_location;
            }
            set
            {
                _graphic_location = value;
            }
        }

        public Size GraphicSize
        {
            get
            {
                return _graphic_size;
            }
            set
            {
                _graphic_size = value;
            }
        }

        public bool ShowVideo
        {
            get
            {
                return _show_video;
            }
            set
            {
                _show_video = value;
            }
        }

        public SecondScreen(string InitString)
        {
            _servername = Settings.Default.ServerName;
            _databasename = Settings.Default.DatabaseName;
            _password = Settings.Default.DatabasePassword;
            string[] strArray = InitString.Split('=')[1].Split(',');
            _layout_pre_name = strArray[0];
            int int32_1 = Convert.ToInt32(strArray[1]);
            int int32_2 = Convert.ToInt32(strArray[2]);
            int int32_3 = Convert.ToInt32(strArray[3]);
            int int32_4 = Convert.ToInt32(strArray[4]);
            InitializeComponent();
            Location = new Point(int32_1, int32_2);
            Size = new Size(int32_3, int32_4);
            _graphic_size = Size;
            pictureBox1 = new PictureBox();
            pictureBox1.ErrorImage = (Image)null;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = new Size(0, 0);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BackColor = Color.Black;
            addPictureBoxAsync((Form)this, pictureBox1);
            hidePictureBoxAsync(pictureBox1);
        }

        public void SetData(string Data)
        {
            if (_loading)
                return;

            if (Data.StartsWith("*"))
            {
                string[] strArray1 = Data.Split('|');
                if ((!_layout_loaded || _layout_pre_name + strArray1[0].Substring(1).Trim() != _loaded_layout_name.Trim()) && strArray1[0].Substring(1).Trim() != "none")
                {
                    _loading = true;
                    loadLayoutAsync((Form)this, _layout_pre_name + strArray1[0].Substring(1));
                    _loading = false;
                }
                else if (_old_data != Data)
                {
                    _old_data = Data;
                    for (int index = 1; index < strArray1.Length; ++index)
                    {
                        string[] strArray2 = strArray1[index].Split('=');
                        if (!(strArray2[0] == "Horn") && !(strArray2[0] == "SC_Horn"))
                        {
                            if (strArray2[0] == "TeamID_Home")
                            {
                                if (_teamID_home != Convert.ToInt32(strArray2[1]))
                                {
                                    _teamID_home = Convert.ToInt32(strArray2[1]);
                                    if (!_database_connected)
                                        _database_connected = ConnectDatabase();
                                    if (_database_connected)
                                    {
                                        _home_team = _dbfunc.ReadTeam(_teamID_home);
                                        if (_home_team != null && _home_team.TeamLogo != null)
                                        {
                                            try
                                            {
                                                MemoryStream memoryStream = new MemoryStream(_home_team.TeamLogo, 0, _home_team.TeamLogo.Length);
                                                setLabelImageAsync(_background_pnl, "LogoHome", new Bitmap((Stream)memoryStream));
                                                memoryStream.Close();
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    setLabelImageAsync(_background_pnl, "LogoHome", (Bitmap)null);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (strArray2[0] == "TeamID_Guest")
                            {
                                if (_teamID_guest != Convert.ToInt32(strArray2[1]))
                                {
                                    _teamID_guest = Convert.ToInt32(strArray2[1]);
                                    if (!_database_connected)
                                        _database_connected = ConnectDatabase();
                                    if (_database_connected)
                                    {
                                        _guest_team = _dbfunc.ReadTeam(_teamID_guest);
                                        if (_guest_team != null && _guest_team.TeamLogo != null)
                                        {
                                            try
                                            {
                                                MemoryStream memoryStream = new MemoryStream(_guest_team.TeamLogo, 0, _guest_team.TeamLogo.Length);
                                                setLabelImageAsync(_background_pnl, "LogoGuest", new Bitmap((Stream)memoryStream));
                                                memoryStream.Close();
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    setLabelImageAsync(_background_pnl, "LogoGuest", (Bitmap)null);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    setHT_LabelTextAsync(strArray2[0], strArray2[1], strArray2[2] == "1");
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            if (!Data.StartsWith("!"))
                return;
            switch (Data[1])
            {
                case 'E':
                    _ext_effect_index = Convert.ToInt32(Data.Substring(2));
                    break;
                case 'W':
                    _ext_graphic_in_window = false;
                    break;
                case 'w':
                    _ext_graphic_in_window = true;
                    break;
            }
        }

        public void SetBytes(byte[] Data)
        {
            MemoryStream memoryStream = new MemoryStream(Data, 0, Data.Length);
            Bitmap BM = new Bitmap((Stream)memoryStream);
            memoryStream.Close();
            if (BM.Width > 1 && BM.Height > 1)
                ShowReceivedGraphic(BM);
            else
                HideGraphic();
        }

        public void HideGraphic()
        {
            if (_mci_timer != null)
            {
                _mci_timer.Stop();
                _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
            }
            if (_mci != null)
            {
                try
                {
                    _mci.Stop();
                }
                catch
                {
                }
                if (_mci.IsOpen)
                {
                    try
                    {
                        _mci.Close();
                    }
                    catch
                    {
                    }
                }
                try
                {
                    _mci.Dispose();
                }
                catch
                {
                }
                _mci = (Mci)null;
            }
            if (pictureBox1 == null)
                return;
            if (_my_effect != null)
            {
                while (_my_effect.ThreadActive)
                    Application.DoEvents();
            }
            hidePictureBoxAsync(pictureBox1);
        }

        public void ShowGraphic(Bitmap Graphic, bool GraphicInWindow)
        {
            showPictureBoxAsync(pictureBox1, GraphicInWindow);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = (Image)Graphic;
            Update();
        }

        public void ShowGraphic(string FileName, bool GraphicInWindow)
        {
            if (FileName != null)
                _filename = FileName;
            if (!File.Exists(_filename))
                return;
            try
            {
                pictureBox1.Image = Image.FromFile(_filename);
                showPictureBoxAsync(pictureBox1, GraphicInWindow);
                Update();
            }
            catch
            {
                try
                {
                    if (_mci != null)
                    {
                        if (_mci.IsOpen)
                        {
                            try
                            {
                                _mci.Close();
                            }
                            catch
                            {
                            }
                        }
                        try
                        {
                            _mci.Dispose();
                        }
                        catch
                        {
                        }
                        _mci = (Mci)null;
                    }
                    _mci = new Mci();
                    _mci.Open(_filename, (Control)pictureBox1);
                    showPictureBoxAsync(pictureBox1, GraphicInWindow);
                    if (_mci_timer != null)
                    {
                        _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                        _mci_timer.Dispose();
                    }
                    _mci_timer = (Timer)null;
                    _mci_timer = new Timer();
                    int num = 1;
                    try
                    {
                        num = _mci.Length;
                    }
                    catch
                    {
                    }
                    _mci_timer.Interval = num;
                    _mci_timer.Tick += new EventHandler(_mci_timer_Tick);
                    try
                    {
                        _mci.SetRectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    try
                    {
                        _mci.Volume = 0;
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    try
                    {
                        _mci.Play(false);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    try
                    {
                        _mci_timer.Start();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    _actual_picture = (Image)null;
                }
                catch (Exception ex)
                {
                    ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                }
            }
        }

        public void ShowGraphic(string FileName, bool GraphicInWindow, int EffectIndex)
        {
            do
                Thread.Sleep(1);
            while (_loading);

            try
            {
                if (!File.Exists(FileName) || !_my_effect_ready)
                    return;
                Point point = new Point(0, 0);
                Size newSize = Size;
                if (GraphicInWindow)
                    newSize = _graphic_size;
                Bitmap bitmap = new Bitmap(Image.FromFile(FileName), newSize);
                _filename = FileName;
                if (_mci != null)
                {
                    _mci.Close();
                    _mci.Dispose();
                    _mci = (Mci)null;
                }
                if (_mci_timer != null)
                {
                    _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                    _mci_timer.Dispose();
                }
                showPictureBoxAsync(pictureBox1, GraphicInWindow);
                if (_actual_picture == null)
                {
                    _actual_picture = (Image)new Bitmap(newSize.Width, newSize.Height);
                    Graphics.FromImage(_actual_picture).Clear(Color.Black);
                }
                _get_effect(EffectIndex);
                try
                {
                    Image image = Image.FromFile(FileName);
                    FrameDimension dimension = new FrameDimension(image.FrameDimensionsList[0]);
                    if (image.GetFrameCount(dimension) > 1)
                    {
                        _my_effect = (Effects.IEffect)null;
                        _actual_picture = image;
                    }
                }
                catch
                {
                }
                if (_my_effect != null)
                {
                    do
                    {
                        Thread.Sleep(1);
                    }
                    while (!_my_effect_ready);

                    _my_effect_ready = false;
                    _my_effect.Ready += new Effects.ReadyDelegate(_my_effect_Ready);
                    _my_effect.PercentComplete += new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
                    _my_effect.Fade(pictureBox1, _actual_picture, (Image)bitmap);
                    _actual_picture = Image.FromFile(FileName);
                }
                else
                    pictureBox1.Image = Image.FromFile(FileName);
            }
            catch
            {
                try
                {
                    if (_mci != null)
                    {
                        _mci.Close();
                        _mci.Dispose();
                        _mci = (Mci)null;
                    }
                    if (_mci_timer != null)
                    {
                        _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                        _mci_timer.Dispose();
                    }
                    _mci = new Mci();
                    _mci.Open(FileName, (Control)pictureBox1);
                    showPictureBoxAsync(pictureBox1, GraphicInWindow);
                    if (_mci_timer != null)
                    {
                        _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                        _mci_timer.Dispose();
                    }
                    _mci_timer = (Timer)null;
                    _mci_timer = new Timer();
                    int num = 1;
                    try
                    {
                        num = _mci.Length;
                    }
                    catch
                    {
                    }
                    _mci_timer.Interval = num;
                    _mci_timer.Tick += new EventHandler(_mci_timer_Tick);
                    try
                    {
                        _mci.SetRectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    try
                    {
                        _mci.Volume = 0;
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    try
                    {
                        _mci.Play(false);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    try
                    {
                        _mci_timer.Start();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                    _actual_picture = (Image)null;
                }
                catch (Exception ex)
                {
                    ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                }
            }
        }

        public void ShowGraphic(Image Img, bool GraphicInWindow, int EffectIndex)
        {
            do
            {
                Thread.Sleep(1);
            }
            while (_loading);
            if (!_my_effect_ready)
                return;
            Point point = new Point(0, 0);
            Size newSize = Size;
            if (GraphicInWindow)
                newSize = _graphic_size;
            Bitmap bitmap = new Bitmap(Img, newSize);
            if (_mci != null)
            {
                _mci.Close();
                _mci.Dispose();
                _mci = (Mci)null;
            }
            if (_mci_timer != null)
            {
                _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                _mci_timer.Dispose();
            }
            showPictureBoxAsync(pictureBox1, GraphicInWindow);
            if (_actual_picture == null)
            {
                _actual_picture = (Image)new Bitmap(newSize.Width, newSize.Height);
                Graphics.FromImage(_actual_picture).Clear(Color.Black);
            }
            _get_effect(EffectIndex);
            if (_my_effect != null)
            {
                do
                {
                    Thread.Sleep(1);
                }
                while (!_my_effect_ready);

                _my_effect_ready = false;
                _my_effect.Ready += new Effects.ReadyDelegate(_my_effect_Ready);
                _my_effect.PercentComplete += new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
                _my_effect.Fade(pictureBox1, _actual_picture, (Image)bitmap);
                _actual_picture = Img;
            }
            else
                pictureBox1.Image = Img;
        }

        private void _mci_timer_Tick(object sender, EventArgs e)
        {
        }

        private void _get_effect(int EffectIndex)
        {
            switch (_effect_names[EffectIndex])
            {
                case "None":
                    _my_effect = (Effects.IEffect)new Effects.None();
                    break;
                case "Alpha":
                    _my_effect = (Effects.IEffect)new Effects.Alpha();
                    break;
                case "TopDown":
                    _my_effect = (Effects.IEffect)new Effects.Down();
                    break;
                case "BottomUp":
                    _my_effect = (Effects.IEffect)new Effects.Up();
                    break;
                case "RightToLeft":
                    _my_effect = (Effects.IEffect)new Effects.Left();
                    break;
                case "LeftToRight":
                    _my_effect = (Effects.IEffect)new Effects.Right();
                    break;
                case "RectangleUpperLeftToLowerRight":
                    _my_effect = (Effects.IEffect)new Effects.RectangleUpperLeftToLowerRight();
                    break;
                case "RectangleUpperRightToLowerLeft":
                    _my_effect = (Effects.IEffect)new Effects.RectangleUpperRightToLowerLeft();
                    break;
                case "RectangleLowerLeftToUpperRight":
                    _my_effect = (Effects.IEffect)new Effects.RectangleLowerLeftToUpperRight();
                    break;
                case "RectangleLowerRightToUpperLeft":
                    _my_effect = (Effects.IEffect)new Effects.RectangleLowerRightToUpperLeft();
                    break;
                case "DiagonalUpperLeftToLowerRight":
                    _my_effect = (Effects.IEffect)new Effects.DiagonalUpperLeftToLowerRight();
                    break;
                case "DiagonalUpperRightToLowerLeft":
                    _my_effect = (Effects.IEffect)new Effects.DiagonalUpperRightToLowerLeft();
                    break;
                case "DiagonalLowerLeftToUpperRight":
                    _my_effect = (Effects.IEffect)new Effects.DiagonalLowerLeftToUpperRight();
                    break;
                case "DiagonalLowerRightToUpperLeft":
                    _my_effect = (Effects.IEffect)new Effects.DiagonalLowerRightToUpperLeft();
                    break;
                case "EllipticFromInside":
                    _my_effect = (Effects.IEffect)new Effects.EllipticFromInside();
                    break;
                case "EllipticFromOutside":
                    _my_effect = (Effects.IEffect)new Effects.EllipticFromOutside();
                    break;
                case "RectangleFromInside":
                    _my_effect = (Effects.IEffect)new Effects.RectangleFromInside();
                    break;
                case "RectangleFromOutside":
                    _my_effect = (Effects.IEffect)new Effects.RectangleFromOutside();
                    break;
                case "ZoomIn":
                    _my_effect = (Effects.IEffect)new Effects.ZoomIn();
                    break;
                case "ZoomOut":
                    _my_effect = (Effects.IEffect)new Effects.ZoomOut();
                    break;
                case "CurtainDown":
                    _my_effect = (Effects.IEffect)new Effects.CurtainDown();
                    break;
                case "CurtainUp":
                    _my_effect = (Effects.IEffect)new Effects.CurtainUp();
                    break;
                case "CurtainRight":
                    _my_effect = (Effects.IEffect)new Effects.CurtainRight();
                    break;
                case "CurtainLeft":
                    _my_effect = (Effects.IEffect)new Effects.CurtainLeft();
                    break;
                case "SlideDown":
                    _my_effect = (Effects.IEffect)new Effects.SlideDown();
                    break;
                case "SlideUp":
                    _my_effect = (Effects.IEffect)new Effects.SlideUp();
                    break;
                case "SlideRight":
                    _my_effect = (Effects.IEffect)new Effects.SlideRight();
                    break;
                case "SlideLeft":
                    _my_effect = (Effects.IEffect)new Effects.SlideLeft();
                    break;
                case "Mosaic":
                    _my_effect = (Effects.IEffect)new Effects.Mosaic();
                    break;
            }
        }

        private void ShowReceivedGraphic(Bitmap BM)
        {
            if (_mci != null)
            {
                _mci.Close();
                _mci.Dispose();
                _mci = (Mci)null;
            }
            if (_mci_timer != null)
            {
                _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                _mci_timer.Dispose();
            }
            showPictureBoxAsync(pictureBox1, _ext_graphic_in_window);
            if (_actual_picture == null)
            {
                _actual_picture = (Image)new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics.FromImage(_actual_picture).Clear(Color.Black);
            }
            _get_effect(_ext_effect_index);
            if (_my_effect == null)
                return;
            do
            {
                Thread.Sleep(1);
            }
            while (!_my_effect_ready);

            _my_effect_ready = false;
            _my_effect.Ready += new Effects.ReadyDelegate(_my_effect_Ready);
            _my_effect.PercentComplete += new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
            _my_effect.Fade(pictureBox1, _actual_picture, (Image)BM);
            _actual_picture = (Image)BM;
        }

        private void _my_effect_PercentComplete(Effects.IEffect sender, int value)
        {
        }

        private void _my_effect_Ready(Effects.IEffect sender)
        {
            _my_effect_ready = true;
        }

        public void Clear()
        {
            _show_video = false;
            if (pictureBox1 != null)
            {
                if (_my_effect != null)
                {
                    while (_my_effect.ThreadActive)
                        Application.DoEvents();
                }
                hidePictureBoxAsync(pictureBox1);
            }
            clearScoreboardAsync((Form)this, pictureBox1);
        }

        public void loadLayoutAsync(Form target, string layoutname)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new SecondScreen.loadLayoutAsyncDelegate(loadLayoutAsync), (object)target, (object)layoutname);
            }
            else
            {
                if (!(layoutname != _loaded_layout_name))
                    return;
                ArrayList arrayList = new ArrayList();
                HideGraphic();
                target.BackgroundImage = (Image)null;
                _fields.Clear();
                target.Controls.Clear();
                _loaded_layout_name = layoutname;
                ScoreboardLabel scoreboardLabel1 = new ScoreboardLabel();
                string path = Application.StartupPath + "\\" + layoutname + ".csv";
                if (!File.Exists(path))
                    return;
                _background_pnl = new Panel();
                _background_pnl.Location = new Point(0, 0);
                _background_pnl.Size = target.Size;
                target.Controls.Add((Control)_background_pnl);
                if (File.Exists(Application.StartupPath + "//" + layoutname + ".jpg"))
                {
                    Bitmap bitmap = new Bitmap(target.Width, target.Height);
                    Graphics.FromImage((Image)bitmap).DrawImage(Image.FromFile(Application.StartupPath + "//" + layoutname + ".jpg"), new Rectangle(0, 0, target.Width, target.Height));
                    _background_pnl.BackgroundImage = (Image)bitmap;
                }
                _fields.Clear();
                StreamReader streamReader = new StreamReader(path);
                string[] strArray1 = streamReader.ReadLine().Split(';');
                if (strArray1.Length < 7)
                {
                    if (strArray1.Length > 1)
                        Size = new Size(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]));
                    if (strArray1.Length > 5)
                    {
                        _graphic_location = new Point(Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[3]));
                        _graphic_size = new Size(Convert.ToInt32(strArray1[4]), Convert.ToInt32(strArray1[5]));
                    }
                }
                for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                {
                    string[] strArray2 = str.Split(';');
                    ScoreboardLabel scoreboardLabel2 = new ScoreboardLabel();
                    scoreboardLabel2.MyLabel.BackColor = Color.Transparent;
                    scoreboardLabel2.MyLabel.Name = strArray2[0];
                    scoreboardLabel2.MyLabel.Left = Convert.ToInt32(strArray2[1]);
                    scoreboardLabel2.MyLabel.Top = Convert.ToInt32(strArray2[2]);
                    scoreboardLabel2.MyLabel.Width = Convert.ToInt32(strArray2[3]);
                    scoreboardLabel2.MyLabel.Height = Convert.ToInt32(strArray2[4]);
                    FontStyle style = !(strArray2[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
                    scoreboardLabel2.MyLabel.Font = new Font(strArray2[5], Convert.ToSingle(strArray2[6]), style);
                    scoreboardLabel2.MyLabel.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[8]));
                    scoreboardLabel2.MyLabel.BackColor = Color.Transparent;
                    scoreboardLabel2.NormalColor = scoreboardLabel2.MyLabel.ForeColor;
                    scoreboardLabel2.AlternateColor = Color.FromArgb(Convert.ToInt32(strArray2[9]));
                    ContentAlignment contentAlignment;
                    switch (strArray2[10])
                    {
                        case "BottomLeft":
                            contentAlignment = ContentAlignment.BottomLeft;
                            break;
                        case "BottomCenter":
                            contentAlignment = ContentAlignment.BottomCenter;
                            break;
                        case "BottomRight":
                            contentAlignment = ContentAlignment.BottomRight;
                            break;
                        case "TopLeft":
                            contentAlignment = ContentAlignment.TopLeft;
                            break;
                        case "TopCenter":
                            contentAlignment = ContentAlignment.TopCenter;
                            break;
                        case "TopRight":
                            contentAlignment = ContentAlignment.TopRight;
                            break;
                        case "MiddleLeft":
                            contentAlignment = ContentAlignment.MiddleLeft;
                            break;
                        case "MiddleCenter":
                            contentAlignment = ContentAlignment.MiddleCenter;
                            break;
                        case "MiddleRight":
                            contentAlignment = ContentAlignment.MiddleRight;
                            break;
                        default:
                            contentAlignment = ContentAlignment.MiddleLeft;
                            break;
                    }
                    scoreboardLabel2.MyLabel.TextAlign = contentAlignment;
                    scoreboardLabel2.MyLabel.Text = strArray2[11];
                    if (strArray2[0] == "TeamNameHome" && _teamname_home != string.Empty)
                        scoreboardLabel2.MyLabel.Text = _teamname_home;
                    if (strArray2[0] == "TeamNameGuest" && _teamname_guest != string.Empty)
                        scoreboardLabel2.MyLabel.Text = _teamname_guest;
                    _fields.Add(scoreboardLabel2.MyLabel.Name, scoreboardLabel2);
                    if (!scoreboardLabel2.MyLabel.Name.StartsWith("Logo"))
                    {
                        if (_background_pnl != null)
                            _background_pnl.Controls.Add((Control)scoreboardLabel2.MyLabel);
                        else
                            target.Controls.Add((Control)scoreboardLabel2.MyLabel);
                    }
                }
                streamReader.Close();
                _layout_loaded = true;
                addPictureBoxAsync((Form)this, pictureBox1);
            }
        }

        public void addPictureBoxAsync(Form target, PictureBox picturebox)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new SecondScreen.addPictureBoxAsyncDelegate(addPictureBoxAsync), (object)target, (object)picturebox);
            }
            else
            {
                target.Controls.Add((Control)picturebox);
                picturebox.BringToFront();
            }
        }

        public void clearScoreboardAsync(Form target, PictureBox picturebox)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new SecondScreen.clearScoreboardAsyncDelegate(clearScoreboardAsync), (object)target, (object)picturebox);
            }
            else
            {
                if (_mci != null)
                {
                    _mci.Close();
                    _mci.Dispose();
                }
                _mci = (Mci)null;
                if (_mci_timer != null)
                {
                    _mci_timer.Stop();
                    _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
                    _mci_timer.Dispose();
                }
                _mci_timer = (Timer)null;
                try
                {
                    picturebox.Location = new Point(0, 0);
                    picturebox.Size = Size;
                    picturebox.CreateGraphics().Clear(Color.Black);
                    picturebox.Update();
                    _actual_picture = (Image)new Bitmap(Width, Height);
                    Graphics.FromImage(_actual_picture).Clear(Color.Black);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void setHT_LabelTextAsync(string target_name, string msg, bool alternate_color)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((Delegate)new SecondScreen.setHT_LabelTextAsyncDelegate(setHT_LabelTextAsync), (object)target_name, (object)msg, (object)alternate_color);
                }
                else
                {
                    try
                    {
                        if (!_fields.ContainsKey(target_name))
                            return;
                        _fields[target_name].MyLabel.Text = msg;
                        _fields[target_name].MyLabel.ForeColor = alternate_color ? _fields[target_name].AlternateColor : _fields[target_name].NormalColor;
                    }
                    catch (Exception ex)
                    {
                        int length = ex.Message.Length;
                    }
                }
            }
            catch
            {
            }
        }

        public void setLabelImageAsync(Panel target, string controlname, Bitmap img)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new SecondScreen.setLabelImageAsyncDelegate(setLabelImageAsync), (object)target, (object)controlname, (object)img);
            }
            else
            {
                try
                {
                    if (!_fields.ContainsKey(controlname))
                        return;
                    Image image = (Image)new Bitmap(target.Width, target.Height);
                    if (target.BackgroundImage != null)
                        image = target.BackgroundImage;
                    Graphics.FromImage(image).DrawImage((Image)img, new Rectangle(_fields[controlname].MyLabel.Location, _fields[controlname].MyLabel.Size));
                    target.BackgroundImage = image;
                    target.Refresh();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void showLabelAsync(Label textBox, Point location, Size size)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((Delegate)new SecondScreen.showLabelAsyncDelegate(showLabelAsync), (object)textBox, (object)location, (object)size);
            }
            else
            {
                textBox.Location = location;
                textBox.Size = size;
                textBox.BringToFront();
            }
        }

        public void hideLabelAsync(Label textBox)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((Delegate)new SecondScreen.hideLabelAsyncDelegate(hideLabelAsync), (object)textBox);
            }
            else
            {
                textBox.Size = new Size(0, 0);
                textBox.Text = string.Empty;
            }
        }

        public void hidePictureBoxAsync(PictureBox picturebox)
        {
            if (picturebox.InvokeRequired)
            {
                picturebox.Invoke((Delegate)new SecondScreen.hidePictureBoxAsyncDelegate(hidePictureBoxAsync), (object)picturebox);
            }
            else
            {
                picturebox.Size = new Size(0, 0);
                _actual_picture = (Image)null;
                picturebox.Image = _actual_picture;
                picturebox.Update();
                picturebox.Refresh();
            }
        }

        public void showPictureBoxAsync(PictureBox picturebox, bool graphic_in_window)
        {
            if (picturebox.InvokeRequired)
            {
                picturebox.Invoke((Delegate)new SecondScreen.showPictureBoxAsyncDelegate(showPictureBoxAsync), (object)picturebox, (object)graphic_in_window);
            }
            else
            {
                if (graphic_in_window)
                {
                    picturebox.Location = _graphic_location;
                    picturebox.Size = _graphic_size;
                }
                else
                {
                    picturebox.Location = new Point(0, 0);
                    picturebox.Size = Size;
                }
                picturebox.SizeMode = PictureBoxSizeMode.Zoom;
                picturebox.BringToFront();
            }
        }

        private bool ConnectDatabase()
        {
            _dbfunc = new DataBaseFunctions(_servername, _databasename, _username, _password);
            return _dbfunc.ConnectDatabase();
        }

        private void SecondScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_my_effect != null)
            {
                _my_effect.Dispose();
                _my_effect.Ready -= new Effects.ReadyDelegate(_my_effect_Ready);
                _my_effect.PercentComplete -= new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
                _my_effect = (Effects.IEffect)null;
            }
            GC.Collect();
            if (_mci == null)
                return;
            if (_mci.IsOpen)
                _mci.Close();
            _mci.Dispose();
            _mci = (Mci)null;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(292, 266);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            Name = nameof(SecondScreen);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            FormClosing += new FormClosingEventHandler(SecondScreen_FormClosing);
            ResumeLayout(false);
        }

        public delegate void loadLayoutAsyncDelegate(Form target, string layoutname);

        public delegate void addPictureBoxAsyncDelegate(Form target, PictureBox picturebox);

        public delegate void clearScoreboardAsyncDelegate(Form target, PictureBox picturebox);

        public delegate void setHT_LabelTextAsyncDelegate(
          string target_name,
          string msg,
          bool alternate_color);

        public delegate void setLabelImageAsyncDelegate(Panel target, string controlname, Bitmap img);

        public delegate void showLabelAsyncDelegate(Label textBox, Point location, Size size);

        public delegate void hideLabelAsyncDelegate(Label textBox);

        public delegate void hidePictureBoxAsyncDelegate(PictureBox picturebox);

        public delegate void showPictureBoxAsyncDelegate(PictureBox picturebox, bool graphic_in_window);
    }
}