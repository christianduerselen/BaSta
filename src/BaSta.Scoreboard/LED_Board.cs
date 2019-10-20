using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using BaSta.Scoreboard.Properties;

namespace BaSta.Scoreboard
{
    public class LED_Board : Form
    {
        private bool _init = true;
        private LED_Board.DisplayState _state = LED_Board.DisplayState.Score;
        private string _sequence_name = string.Empty;
        private List<SecondScreen> _screens = new List<SecondScreen>();
        private bool _my_effect_ready = true;
        private string[] _effect_names = Enum.GetNames(typeof(Effects.FadingEffect));
        private string _stramatel_console_port = string.Empty;
        private string[] _stramatel_buffer = new string[64];
        private string _wige_console_port = string.Empty;
        private string[] _wige_buffer = new string[64];
        private int _video_volume = 1000;
        private string _filename = string.Empty;
        private Dictionary<string, ScoreboardLabel> _fields = new Dictionary<string, ScoreboardLabel>();
        private Dictionary<string, LED_Board.WIGE_Parameter> _WIGE_field_parameter = new Dictionary<string, LED_Board.WIGE_Parameter>();
        private string _loaded_layout_name = string.Empty;
        private string _manual_layout_name = string.Empty;
        private bool _do_show_score = true;
        private string _servername = string.Empty;
        private string _databasename = string.Empty;
        private string _password = string.Empty;
        private string _username = "sa";
        private int _teamID_home = -1;
        private int _teamID_guest = -1;
        private SoundPlayer _hornblower = new SoundPlayer();
        private SoundPlayer _shotclock_hornblower = new SoundPlayer();
        private int _ext_scb_refresh_interval = 50;
        private Point _layout_graphic_location = Settings.Default.DefaultGraphicWindowLocation;
        private Point _graphic_location = Settings.Default.DefaultGraphicWindowLocation;
        private Size _layout_graphic_size = Settings.Default.DefaultGraphicWindowSize;
        private Size _graphic_size = Settings.Default.DefaultGraphicWindowSize;
        private Point _cfg_graphic_location = new Point();
        private Size _cfg_graphic_size = new Size();
        private bool _insert_game_time = true;
        private Point _gametime_insert_location = new Point(0, 0);
        private string _gametime_insert_font_name = "Arial";
        private int _gametime_insert_font_size = 16;
        private Color _gametime_insert_color = Color.Red;
        private bool _insert_day_time = true;
        private string _day_time = string.Empty;
        private Point _daytime_insert_location = new Point(0, 0);
        private string _daytime_insert_font_name = "Arial";
        private int _daytime_insert_font_size = 16;
        private Color _daytime_insert_color = Color.Red;
        private string _teamname_home = "HEIM";
        private string _teamname_guest = "GAST";
        private string _soccer_background_picture_directory = Application.StartupPath;
        private string _program_version = string.Empty;
        private string _buffer = string.Empty;
        private Stopwatch _sw = new Stopwatch();
        private string _temp_data = string.Empty;
        private string _old_data = string.Empty;
        private string _sequence_path = "c:\\";
        private string _media_path = "c:\\";
        private string _period = string.Empty;
        private string _game_time = string.Empty;
        private Preview _preview;
        private Effects.IEffect _my_effect;
        private Image _actual_picture;
        private int _actual_effect_index;
        private List<IScoreBoard> _ext_scoreboard;
        private SerialPort _stramatel_port;
        private Timer _stramatel_timer;
        private SerialPort _wige_port;
        private Timer _wige_timer;
        private Mci _mci;
        private Timer _mci_timer;
        private UdpReceiver _receiver;
        private bool _layout_loaded;
        private bool _loading;
        private Bitmap _score_background;
        private bool _layout_in_use;
        private DataBaseFunctions _dbfunc;
        private bool _database_connected;
        private Team _home_team;
        private Team _guest_team;
        private bool _show_video;
        private FontStyle _gametime_insert_fontstyle;
        private FontStyle _daytime_insert_fontstyle;
        private Image _homelogo;
        private Image _guestlogo;
        private int _ext_effect_index;
        private bool _graphic_in_window;
        private bool _ext_graphic_in_window;
        private bool _allow_lan_graphic;
        private bool _insert_game_time_old;
        private Sportart _sport;
        private Label SC_Horn;
        private Label Horn;
        private PictureBox pictureBox1;
        private Panel _background_pnl;

        public void RefreshTeams()
        {
            for (int index = 0; index < _ext_scoreboard.Count; ++index)
                _ext_scoreboard[index].RefreshTeams = true;
        }

        public bool RedLight
        {
            set
            {
                if (_ext_scoreboard == null || _ext_scoreboard.Count <= 0)
                    return;
                for (int index = 0; index < _ext_scoreboard.Count; ++index)
                    _ext_scoreboard[index].RedLight = value;
            }
        }

        public void SetRedLight(bool value)
        {
            if (_ext_scoreboard == null || _ext_scoreboard.Count <= 0)
                return;
            for (int index = 0; index < _ext_scoreboard.Count; ++index)
                _ext_scoreboard[index].RedLight = value;
        }

        public LED_Board.DisplayState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public List<SecondScreen> Screens
        {
            get
            {
                return _screens;
            }
            set
            {
                _screens = value;
            }
        }

        public event LED_Board.GraphicAnimationDetectedDelegate GraphicAnimationDetected;

        public event LED_Board.SequenceRequestDelegate SequenceRequested;

        public event LED_Board.MciElapsedDelegate MciElapsed;

        public event LED_Board.PercentCompletedDelegate PercentComplete;

        public event LED_Board.LayoutCompletedDelegate LayoutComplete;

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

        public int ActualEffectIndex
        {
            get
            {
                return _actual_effect_index;
            }
            set
            {
                _actual_effect_index = value;
            }
        }

        public string FileName
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }

        public string Sport
        {
            get
            {
                return _loaded_layout_name;
            }
        }

        public bool LayoutLocked
        {
            set
            {
                _layout_in_use = value;
            }
        }

        public bool DoShowScore
        {
            get
            {
                return _do_show_score;
            }
            set
            {
                _do_show_score = value;
                _background_pnl.Visible = _do_show_score;
                _background_pnl.BackColor = Color.Black;
                _background_pnl.BackgroundImage = (Image)_score_background;
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
                _layout_graphic_location = value;
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
                _layout_graphic_size = value;
            }
        }

        public Point CfgGraphicLocation
        {
            get
            {
                return _cfg_graphic_location;
            }
            set
            {
                _cfg_graphic_location = value;
            }
        }

        public Size CfgGraphicSize
        {
            get
            {
                return _cfg_graphic_size;
            }
            set
            {
                _cfg_graphic_size = value;
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

        public bool InsertGameTime
        {
            get
            {
                return _insert_game_time;
            }
            set
            {
                _insert_game_time = value;
                try
                {
                    if (!File.Exists(_filename) || _insert_game_time || (!pictureBox1.Visible || IsAnimation(_filename)))
                        return;
                    pictureBox1.Image = Image.FromFile(_filename);
                }
                catch
                {
                }
            }
        }

        public Point GameTimeInsertLocation
        {
            get
            {
                return _gametime_insert_location;
            }
            set
            {
                _gametime_insert_location = value;
                try
                {
                    if (!pictureBox1.Visible || IsAnimation(_filename))
                        return;
                    pictureBox1.Image = Image.FromFile(_filename);
                    pictureBox1.Update();
                }
                catch
                {
                }
            }
        }

        public string GameTimeInsertFontName
        {
            get
            {
                return _gametime_insert_font_name;
            }
            set
            {
                _gametime_insert_font_name = value;
            }
        }

        public FontStyle GameTimeInsertFontStyle
        {
            get
            {
                return _gametime_insert_fontstyle;
            }
            set
            {
                _gametime_insert_fontstyle = value;
            }
        }

        public int GameTimeInsertFontSize
        {
            get
            {
                return _gametime_insert_font_size;
            }
            set
            {
                _gametime_insert_font_size = value;
            }
        }

        public Color GameTimeInsertColor
        {
            get
            {
                return _gametime_insert_color;
            }
            set
            {
                _gametime_insert_color = value;
            }
        }

        public bool InsertDayTime
        {
            get
            {
                return _insert_day_time;
            }
            set
            {
                _insert_day_time = value;
                if (!(_filename.Trim() != string.Empty) || _insert_day_time || (!pictureBox1.Visible || IsAnimation(_filename)))
                    return;
                pictureBox1.Image = Image.FromFile(_filename);
            }
        }

        public string DayTime
        {
            set
            {
                _day_time = value;
                if (_day_time.StartsWith("0"))
                    _day_time = " " + _day_time.Substring(1);
                if (!_insert_day_time || pictureBox1 == null || (!pictureBox1.Visible || !(_filename != string.Empty)) || !File.Exists(_filename))
                    return;
                Bitmap bitmap = new Bitmap((Image)new Bitmap(_filename), pictureBox1.Size);
                Graphics graphics1 = Graphics.FromImage((Image)bitmap);
                graphics1.SmoothingMode = SmoothingMode.HighQuality;
                graphics1.DrawString(_day_time.PadLeft(8), new Font(_daytime_insert_font_name, (float)_daytime_insert_font_size, _daytime_insert_fontstyle), (Brush)new SolidBrush(_daytime_insert_color), (PointF)_daytime_insert_location);
                Graphics graphics2 = pictureBox1.CreateGraphics();
                graphics2.DrawImage((Image)bitmap, 0, 0);
                graphics1.Dispose();
                graphics2.Dispose();
            }
        }

        public Point DayTimeInsertLocation
        {
            get
            {
                return _daytime_insert_location;
            }
            set
            {
                _daytime_insert_location = value;
                try
                {
                    if (!pictureBox1.Visible || IsAnimation(_filename))
                        return;
                    pictureBox1.Image = Image.FromFile(_filename);
                    pictureBox1.Update();
                }
                catch
                {
                }
            }
        }

        public string DayTimeInsertFontName
        {
            get
            {
                return _daytime_insert_font_name;
            }
            set
            {
                _daytime_insert_font_name = value;
            }
        }

        public FontStyle DayTimeInsertFontStyle
        {
            get
            {
                return _daytime_insert_fontstyle;
            }
            set
            {
                _daytime_insert_fontstyle = value;
            }
        }

        public int DayTimeInsertFontSize
        {
            get
            {
                return _daytime_insert_font_size;
            }
            set
            {
                _daytime_insert_font_size = value;
            }
        }

        public Color DayTimeInsertColor
        {
            get
            {
                return _daytime_insert_color;
            }
            set
            {
                _daytime_insert_color = value;
            }
        }

        private void createVideoAsync(PictureBox target, string filename)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.createVideoAsyncDelegate(createVideoAsync), (object)target, (object)filename);
            }
            else
            {
                int width = target.Width;
                int height = target.Height;
                _mci = new Mci();
                _mci.Open(filename, (Control)pictureBox1);
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
                if (num > 1 && num < 2000)
                    num *= 500;
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
                    _mci.Volume = 10000;
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
        }

        private void updatePanelAsync(Panel target)
        {
            if (target.InvokeRequired)
                target.Invoke((Delegate)new LED_Board.updatePanelAsyncDelegate(updatePanelAsync), (object)target);
            else
                target.Update();
        }

        private void setLabelTextAsync(Label target, string msg)
        {
            try
            {
                if (target == null)
                    return;
                if (target.InvokeRequired)
                    target.Invoke((Delegate)new LED_Board.setLabelTextAsyncDelegate(setLabelTextAsync), (object)target, (object)msg);
                else
                    target.Text = msg;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void setLabelImageAsync(Panel target, string controlname, Bitmap img)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.setLabelImageAsyncDelegate(setLabelImageAsync), (object)target, (object)controlname, (object)img);
            }
            else
            {
                try
                {
                    if (!_fields.ContainsKey(controlname))
                        return;
                    Image image = (Image)new Bitmap(target.Width, target.Height);
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

        public void setPanelImageAsync(
          Panel target,
          string controlname_home,
          Bitmap img_home,
          string controlname_guest,
          Bitmap img_guest)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.setPanelImageAsyncDelegate(setPanelImageAsync), (object)target, (object)controlname_home, (object)img_home, (object)controlname_guest, (object)img_guest);
            }
            else
            {
                try
                {
                    Image image = (Image)new Bitmap(target.Width, target.Height);
                    Graphics graphics = Graphics.FromImage(image);
                    if (File.Exists(Application.StartupPath + "//" + _loaded_layout_name + ".jpg"))
                        graphics.DrawImage(Image.FromFile(Application.StartupPath + "//" + _loaded_layout_name + ".jpg"), new Rectangle(0, 0, target.Width, target.Height));
                    if (_fields.ContainsKey(controlname_home) && img_home != null)
                        graphics.DrawImage((Image)img_home, new Rectangle(_fields[controlname_home].MyLabel.Location, _fields[controlname_home].MyLabel.Size));
                    if (_fields.ContainsKey(controlname_guest) && img_guest != null)
                        graphics.DrawImage((Image)img_guest, new Rectangle(_fields[controlname_guest].MyLabel.Location, _fields[controlname_guest].MyLabel.Size));
                    target.BackgroundImage = image;
                    target.Refresh();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void addPictureBoxAsync(Form target, PictureBox picturebox)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.addPictureBoxAsyncDelegate(addPictureBoxAsync), (object)target, (object)picturebox);
            }
            else
            {
                target.Controls.Add((Control)picturebox);
                picturebox.BringToFront();
            }
        }

        public void resizePictureBoxAsync(PictureBox picturebox)
        {
            if (picturebox.InvokeRequired)
                picturebox.Invoke((Delegate)new LED_Board.resizePictureBoxAsyncDelegate(resizePictureBoxAsync), (object)picturebox);
            else if (_graphic_in_window)
                picturebox.Size = _graphic_size;
            else
                picturebox.Size = Size;
        }

        public void removeControlAsync(Form target, Control control)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.removeControlAsyncDelegate(removeControlAsync), (object)target, (object)control);
            }
            else
            {
                target.Controls.Remove(control);
                control.Dispose();
                control = (Control)null;
            }
        }

        public PictureBox getPictureboxAsync(PictureBox picturebox)
        {
            if (!picturebox.InvokeRequired)
                return picturebox;
            picturebox.Invoke((Delegate)new LED_Board.getPictureboxAsyncDelegate(getPictureboxAsync), (object)picturebox);
            return (PictureBox)null;
        }

        public void hidePictureBoxAsync(PictureBox picturebox)
        {
            if (picturebox.InvokeRequired)
            {
                picturebox.Invoke((Delegate)new LED_Board.hidePictureBoxAsyncDelegate(hidePictureBoxAsync), (object)picturebox);
            }
            else
            {
                picturebox.Size = new Size(0, 0);
                _actual_picture = (Image)null;
                picturebox.Image = _actual_picture;
            }
        }

        public void showPictureBoxAsync(PictureBox picturebox)
        {
            if (picturebox.InvokeRequired)
            {
                picturebox.Invoke((Delegate)new LED_Board.showPictureBoxAsyncDelegate(showPictureBoxAsync), (object)picturebox);
            }
            else
            {
                _do_show_score = false;
                if (_graphic_in_window)
                {
                    picturebox.Location = _graphic_location;
                    picturebox.Size = _graphic_size;
                }
                else
                {
                    picturebox.Location = new Point(0, 0);
                    picturebox.Size = Size;
                }
                pictureBox1.Visible = true;
                pictureBox1.BringToFront();
            }
        }

        public void hidePanelAsync(Panel panel)
        {
            if (panel == null)
                return;
            if (panel.InvokeRequired)
            {
                panel.Invoke((Delegate)new LED_Board.hidePanelAsyncDelegate(hidePanelAsync), (object)panel);
            }
            else
            {
                panel.Left += Width;
                _do_show_score = false;
            }
        }

        public void showPanelAsync(Panel panel)
        {
            if (panel == null)
                return;
            if (panel.InvokeRequired)
            {
                panel.Invoke((Delegate)new LED_Board.showPanelAsyncDelegate(showPanelAsync), (object)panel);
            }
            else
            {
                panel.Left = 0;
                _do_show_score = true;
                _state = LED_Board.DisplayState.Score;
            }
        }

        public void resizePanelAsync(Panel panel, Point location, Size size)
        {
            if (panel == null)
                return;
            if (panel.InvokeRequired)
            {
                panel.Invoke((Delegate)new LED_Board.resizePanelAsyncDelegate(resizePanelAsync), (object)panel, (object)location, (object)size);
            }
            else
            {
                panel.Location = location;
                panel.Size = size;
            }
        }

        public void showLabelAsync(Label textBox, Point location, Size size)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((Delegate)new LED_Board.showLabelAsyncDelegate(showLabelAsync), (object)textBox, (object)location, (object)size);
            }
            else
            {
                textBox.Location = location;
                textBox.Size = size;
                textBox.Visible = true;
                textBox.BringToFront();
                textBox.Update();
                for (int index = 0; index < _screens.Count; ++index)
                {
                    Bitmap bitmap = new Bitmap(textBox.Width, textBox.Height);
                    Graphics graphics = Graphics.FromImage((Image)bitmap);
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                    graphics.Dispose();
                    Bitmap Graphic = new Bitmap((Image)bitmap, _screens[index].Size);
                    _screens[index].ShowGraphic(Graphic, false);
                }
            }
        }

        public void showTextBoxAsync(TextBox textBox, Point location, Size size)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((Delegate)new LED_Board.showTextBoxAsyncDelegate(showTextBoxAsync), (object)textBox, (object)location, (object)size);
            }
            else
            {
                textBox.Location = location;
                textBox.Size = size;
                textBox.BringToFront();
            }
        }

        public void hideTextBoxAsync(TextBox textBox)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((Delegate)new LED_Board.hideTextBoxAsyncDelegate(hideTextBoxAsync), (object)textBox);
            }
            else
            {
                textBox.Text = string.Empty;
                textBox.Visible = false;
            }
        }

        public void hideLabelAsync(Label textBox)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke((Delegate)new LED_Board.hideLabelAsyncDelegate(hideLabelAsync), (object)textBox);
            }
            else
            {
                textBox.Text = string.Empty;
                textBox.Visible = false;
            }
        }

        public void Clear()
        {
            if (_preview != null)
                _preview.HideGraphicPreviewAsync();
            _show_video = false;
            _do_show_score = false;
            _state = LED_Board.DisplayState.Cleared;
            if (_background_pnl == null)
            {
                _background_pnl = new Panel();
                _background_pnl.Size = Size;
                Controls.Add((Control)_background_pnl);
            }
            _background_pnl.Hide();
            _background_pnl.Visible = false;
            HideGraphic();
            for (int index = 0; index < _screens.Count; ++index)
                _screens[index].Clear();
        }

        private Image BlackImage()
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.Clear(Color.Black);
            graphics.Dispose();
            return (Image)bitmap;
        }

        public void clearScoreboardAsync(Form target, PictureBox picturebox)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.clearScoreboardAsyncDelegate(clearScoreboardAsync), (object)target, (object)picturebox);
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
                    _actual_picture = BlackImage();
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
                    Invoke((Delegate)new LED_Board.setHT_LabelTextAsyncDelegate(setHT_LabelTextAsync), (object)target_name, (object)msg, (object)alternate_color);
                }
                else
                {
                    try
                    {
                        if (!_fields.ContainsKey(target_name))
                            return;
                        _fields[target_name].MyLabel.Text = msg;
                        _fields[target_name].MyLabel.ForeColor = alternate_color ? _fields[target_name].AlternateColor : _fields[target_name].NormalColor;
                        if (!_insert_game_time || pictureBox1 == null || (!(target_name == "GameTime") || !(_filename != string.Empty)) || !File.Exists(_filename))
                            return;
                        Bitmap bitmap = new Bitmap((Image)new Bitmap(_filename), pictureBox1.Size);
                        Graphics graphics1 = Graphics.FromImage((Image)bitmap);
                        graphics1.SmoothingMode = SmoothingMode.HighQuality;
                        graphics1.DrawString(msg.PadLeft(6), new Font(_gametime_insert_font_name, (float)_gametime_insert_font_size, _gametime_insert_fontstyle), (Brush)new SolidBrush(_gametime_insert_color), (PointF)_gametime_insert_location);
                        Graphics graphics2 = pictureBox1.CreateGraphics();
                        graphics2.DrawImage((Image)bitmap, 0, 0);
                        graphics1.Dispose();
                        graphics2.Dispose();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                    }
                }
            }
            catch
            {
            }
        }

        public void setHT_ControlTextAsync(string[] text)
        {
            if (InvokeRequired)
            {
                Invoke((Delegate)new LED_Board.setHT_ControlTextAsyncDelegate(setHT_ControlTextAsync), (object)text);
            }
            else
            {
                for (int index = 1; index < text.Length; ++index)
                {
                    string[] strArray = text[index].Split('=');
                    _fields[strArray[0]].MyLabel.Text = strArray[1];
                }
            }
        }

        public void _loadLayoutAsync(Panel target, string layoutname)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new LED_Board.loadLayoutAsyncDelegate(_loadLayoutAsync), (object)target, (object)layoutname);
            }
            else
            {
                if (!(layoutname != _loaded_layout_name))
                    return;
                _WIGE_field_parameter.Clear();
                Clear();
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
                _fields.Clear();
                StreamReader streamReader = new StreamReader(path);
                string[] strArray1 = streamReader.ReadLine().Split(';');
                if (strArray1.Length < 7)
                {
                    if (strArray1.Length > 1)
                    {
                        Settings.Default.SCB_Size = new Size(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]));
                        Settings.Default.Save();
                        _write_cfg_line("SCB_WINDOW_SIZE", strArray1[0] + "," + strArray1[1]);
                    }
                    Size = Settings.Default.SCB_Size;
                    BlackImage().Save(Application.StartupPath + "\\Black.jpg", ImageFormat.Jpeg);
                    if (strArray1.Length > 5)
                    {
                        Settings.Default.DefaultGraphicWindowLocation = new Point(Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[3]));
                        Settings.Default.DefaultGraphicWindowSize = new Size(Convert.ToInt32(strArray1[4]), Convert.ToInt32(strArray1[5]));
                        Settings.Default.Save();
                    }
                    _graphic_location = Settings.Default.DefaultGraphicWindowLocation;
                    _layout_graphic_location = _graphic_location;
                    _graphic_size = Settings.Default.DefaultGraphicWindowSize;
                    _layout_graphic_size = _graphic_size;
                }
                try
                {
                    resizePanelAsync(target, new Point(0, 0), Settings.Default.SCB_Size);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (File.Exists(Application.StartupPath + "//" + layoutname + ".jpg"))
                {
                    _score_background = new Bitmap(target.Width, target.Height);
                    Graphics.FromImage((Image)_score_background).DrawImage(Image.FromFile(Application.StartupPath + "//" + layoutname + ".jpg"), new Rectangle(0, 0, target.Width, target.Height));
                    target.BackgroundImage = (Image)_score_background;
                    target.Update();
                }
                else
                    target.BackgroundImage = (Image)null;
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
                    if (strArray2[0] == "LogoHome" && _homelogo != null)
                        scoreboardLabel2.MyLabel.Image = (Image)new Bitmap(_homelogo, scoreboardLabel2.MyLabel.Size);
                    if (strArray2[0] == "LogoGuest" && _guestlogo != null)
                        scoreboardLabel2.MyLabel.Image = (Image)new Bitmap(_guestlogo, scoreboardLabel2.MyLabel.Size);
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
                    if (strArray2.Length > 12)
                    {
                        try
                        {
                            LED_Board.WIGE_Parameter wigeParameter = new LED_Board.WIGE_Parameter(scoreboardLabel2.MyLabel.Name, strArray2[12] + ";" + strArray2[13] + ";" + strArray2[14], scoreboardLabel2.MyLabel.Text.Length);
                            _WIGE_field_parameter.Add(wigeParameter.IdentString, wigeParameter);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        if (strArray2[0] == "TeamNameHome" && _teamname_home != string.Empty)
                            scoreboardLabel2.MyLabel.Text = _teamname_home;
                        if (strArray2[0] == "TeamNameGuest" && _teamname_guest != string.Empty)
                            scoreboardLabel2.MyLabel.Text = _teamname_guest;
                    }
                    _fields.Add(scoreboardLabel2.MyLabel.Name, scoreboardLabel2);
                    if (!scoreboardLabel2.MyLabel.Name.StartsWith("Logo") && target != null)
                        target.Controls.Add((Control)scoreboardLabel2.MyLabel);
                }
                streamReader.Close();
                _layout_loaded = true;
                ShowScore();
                LayoutComplete();
            }
        }

        public void CancelCurrentEffect()
        {
            if (_my_effect != null)
                _my_effect.Dispose();
            _my_effect = (Effects.IEffect)null;
            _my_effect_ready = true;
        }

        public void SetKindOfGame(int Index)
        {
            Sportart sportart = Sportart.Basketball;
            switch (Index)
            {
                case 0:
                    sportart = Sportart.Basketball;
                    _loadLayoutAsync(_background_pnl, "Basketball");
                    break;
                case 1:
                    sportart = Sportart.Handball;
                    _loadLayoutAsync(_background_pnl, "Handball");
                    break;
                case 2:
                    sportart = Sportart.Volleyball;
                    _loadLayoutAsync(_background_pnl, "Volleyball");
                    break;
                case 3:
                    sportart = Sportart.Football;
                    _loadLayoutAsync(_background_pnl, "Football");
                    break;
                case 4:
                    sportart = Sportart.Waterpolo;
                    _loadLayoutAsync(_background_pnl, "Waterpolo");
                    break;
                case 5:
                    sportart = Sportart.Soccer;
                    _loadLayoutAsync(_background_pnl, "Soccer");
                    break;
                case 6:
                    sportart = Sportart.Tennis;
                    _loadLayoutAsync(_background_pnl, "Tennis");
                    break;
                case 7:
                    sportart = Sportart.Icehockey;
                    _loadLayoutAsync(_background_pnl, "Icehockey");
                    break;
                case 8:
                    sportart = Sportart.Hockey;
                    _loadLayoutAsync(_background_pnl, "Hockey");
                    break;
                case 9:
                    sportart = Sportart.Badminton;
                    _loadLayoutAsync(_background_pnl, "Badminton");
                    break;
            }
            _sport = sportart;
            if (_ext_scoreboard == null || _ext_scoreboard.Count <= 0)
                return;
            for (int index = 0; index < _ext_scoreboard.Count; ++index)
                _ext_scoreboard[index].Sport = sportart;
        }

        public void SetKindOfGame(string KindOfGame)
        {
            Sportart sportart = Sportart.Basketball;
            switch (KindOfGame)
            {
                case "Basketball":
                    sportart = Sportart.Basketball;
                    _loadLayoutAsync(_background_pnl, "Basketball");
                    break;
                case "Handball":
                    sportart = Sportart.Handball;
                    _loadLayoutAsync(_background_pnl, "Handball");
                    break;
                case "Volleyball":
                    sportart = Sportart.Volleyball;
                    _loadLayoutAsync(_background_pnl, "Volleyball");
                    break;
                case "Football":
                    sportart = Sportart.Football;
                    _loadLayoutAsync(_background_pnl, "Football");
                    break;
                case "Wasserball":
                    sportart = Sportart.Waterpolo;
                    _loadLayoutAsync(_background_pnl, "Waterpolo");
                    break;
                case "Fussball":
                    sportart = Sportart.Soccer;
                    _loadLayoutAsync(_background_pnl, "Soccer");
                    break;
                case "Hallenfussball":
                    sportart = Sportart.Indoorsoccer;
                    _loadLayoutAsync(_background_pnl, "IndoorSoccer");
                    break;
                case "Tennis":
                    sportart = Sportart.Tennis;
                    _loadLayoutAsync(_background_pnl, "Tennis");
                    break;
                case "Eishockey":
                    sportart = Sportart.Icehockey;
                    _loadLayoutAsync(_background_pnl, "Icehockey");
                    break;
                case "Hockey":
                    sportart = Sportart.Hockey;
                    _loadLayoutAsync(_background_pnl, "Hockey");
                    break;
                case "Badminton":
                    sportart = Sportart.Badminton;
                    _loadLayoutAsync(_background_pnl, "Badminton");
                    break;
            }
            _sport = sportart;
            if (_ext_scoreboard == null || _ext_scoreboard.Count <= 0)
                return;
            for (int index = 0; index < _ext_scoreboard.Count; ++index)
                _ext_scoreboard[index].Sport = sportart;
        }

        public PictureBox Picturebox
        {
            get
            {
                return pictureBox1;
            }
        }

        public void ReloadLayout()
        {
            _temp_data = string.Empty;
            _layout_loaded = false;
            _loaded_layout_name = string.Empty;
            _layout_in_use = false;
        }

        public void ConnectAnotherServer()
        {
            _servername = string.Empty;
            _database_connected = ConnectDatabase();
        }

        public LED_Board(string ProgramVersion, string SoccerBackgroundPictureDirectory)
        {
            _program_version = ProgramVersion;
            int ReceivePort = 8000;
            _soccer_background_picture_directory = SoccerBackgroundPictureDirectory;
            _graphic_size = Size;
            InitializeComponent();
            _graphic_size = Size;
            _gametime_insert_font_name = Settings.Default.GameTimeInsertFont.Name;
            _gametime_insert_font_size = (int)Settings.Default.GameTimeInsertFont.Size;
            _gametime_insert_fontstyle = Settings.Default.GameTimeInsertFont.Style;
            _gametime_insert_color = Settings.Default.GameTimeInsertColor;
            _gametime_insert_location = Settings.Default.GameTimeInsertLocation;
            _insert_game_time = Settings.Default.DoInsertgameTime;
            string str1 = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "claxon.wav");
            if (File.Exists(str1))
                _hornblower = new SoundPlayer(str1);
            string str2 = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "sc_claxon.wav");
            if (File.Exists(str2))
                _shotclock_hornblower = new SoundPlayer(str2);
            _read_cfg();
            _servername = Settings.Default.ServerName;
            _databasename = Settings.Default.DatabaseName;
            _password = Settings.Default.DatabasePassword;
            if (!File.Exists(Application.StartupPath + "\\Scoreboards.cfg"))
                _write_cfg_line("SERVERNAME", _servername);
            StreamReader streamReader1 = new StreamReader(Application.StartupPath + "\\Scoreboards.cfg");
            for (string InitString = streamReader1.ReadLine(); InitString != null; InitString = streamReader1.ReadLine())
            {
                if (InitString.StartsWith("SERVERNAME="))
                {
                    Settings.Default.ServerName = InitString.Substring(11).Trim();
                    Settings.Default.Save();
                }
                if (InitString.StartsWith("PREVIEW_WINDOW_LOCATION"))
                {
                    string[] strArray = InitString.Split('=')[1].Split(',');
                    if (_preview == null)
                    {
                        _preview = new Preview();
                        _preview.Show();
                    }
                    _preview.Location = new Point(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]));
                }
                if (InitString.StartsWith("PREVIEW_WINDOW_SIZE"))
                {
                    string[] strArray = InitString.Split('=')[1].Split(',');
                    if (_preview == null)
                    {
                        _preview = new Preview();
                        _preview.Show();
                    }
                    _preview.Size = new Size(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]));
                }
                if (InitString.StartsWith("SCREEN="))
                    _screens.Add(new SecondScreen(InitString));
                if (InitString.StartsWith("WIGE_MST2002"))
                {
                    _wige_console_port = InitString.Split('=')[1];
                    _wige_port = new SerialPort(_wige_console_port, 9600, Parity.None, 8, StopBits.One);
                    _wige_port.Handshake = Handshake.None;
                    try
                    {
                        _wige_port.Open();
                    }
                    catch
                    {
                    }
                    if (_wige_port.IsOpen)
                    {
                        _wige_port.DtrEnable = false;
                        _wige_port.RtsEnable = false;
                        _wige_port.DataReceived += new SerialDataReceivedEventHandler(_wige_port_DataReceived);
                        _wige_timer = new Timer();
                        _wige_timer.Interval = 25;
                        _wige_timer.Start();
                    }
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    if (!Directory.Exists(folderPath + "\\Spieldaten"))
                        Directory.CreateDirectory(folderPath + "\\Spieldaten");
                    if (Directory.Exists(folderPath + "\\Spieldaten"))
                    {
                        _homelogo = (Image)null;
                        if (File.Exists(folderPath + "\\Spieldaten\\Heim.jpg"))
                            _homelogo = Image.FromFile(folderPath + "\\Spieldaten\\Heim.jpg");
                        _guestlogo = (Image)null;
                        if (File.Exists(folderPath + "\\Spieldaten\\Gast.jpg"))
                            _guestlogo = Image.FromFile(folderPath + "\\Spieldaten\\Gast.jpg");
                        if (File.Exists(folderPath + "\\Spieldaten\\Mannschaftsnamen.txt"))
                        {
                            StreamReader streamReader2 = new StreamReader(folderPath + "\\Spieldaten\\Mannschaftsnamen.txt");
                            for (string str3 = streamReader2.ReadLine(); str3 != null; str3 = streamReader2.ReadLine())
                            {
                                string[] strArray = str3.Split('=');
                                strArray[0] = strArray[0].ToUpper();
                                if (strArray[0] == "HEIM")
                                    _teamname_home = strArray[1].Trim();
                                if (strArray[0] == "GAST")
                                    _teamname_guest = strArray[1].Trim();
                            }
                            streamReader2.Close();
                        }
                    }
                }
                if (InitString.StartsWith("STRAMATEL-TV-KONSOLE"))
                {
                    _stramatel_console_port = InitString.Split('=')[1];
                    _stramatel_port = new SerialPort(_stramatel_console_port, 19200, Parity.None, 8, StopBits.One);
                    _stramatel_port.Handshake = Handshake.None;
                    try
                    {
                        _stramatel_port.Open();
                    }
                    catch
                    {
                    }
                    if (_stramatel_port.IsOpen)
                    {
                        _stramatel_port.DtrEnable = false;
                        _stramatel_port.RtsEnable = false;
                        _stramatel_port.DataReceived += new SerialDataReceivedEventHandler(_stramatel_port_DataReceived);
                        _stramatel_timer = new Timer();
                        _stramatel_timer.Interval = 25;
                        _stramatel_timer.Start();
                    }
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    if (!Directory.Exists(folderPath + "\\Spieldaten"))
                        Directory.CreateDirectory(folderPath + "\\Spieldaten");
                    if (!File.Exists(folderPath + "\\Spieldaten\\Mannschaftsnamen.txt"))
                    {
                        StreamWriter streamWriter = new StreamWriter(folderPath + "\\Spieldaten\\Mannschaftsnamen.txt");
                        streamWriter.WriteLine("HEIM=BERLIN ADLER");
                        streamWriter.WriteLine("GAST=GÄSTE");
                        streamWriter.Close();
                    }
                    if (Directory.Exists(folderPath + "\\Spieldaten"))
                    {
                        _homelogo = (Image)null;
                        if (File.Exists(folderPath + "\\Spieldaten\\Heim.jpg"))
                            _homelogo = Image.FromFile(folderPath + "\\Spieldaten\\Heim.jpg");
                        _guestlogo = (Image)null;
                        if (File.Exists(folderPath + "\\Spieldaten\\Gast.jpg"))
                            _guestlogo = Image.FromFile(folderPath + "\\Spieldaten\\Gast.jpg");
                        if (File.Exists(folderPath + "\\Spieldaten\\Mannschaftsnamen.txt"))
                        {
                            StreamReader streamReader2 = new StreamReader(folderPath + "\\Spieldaten\\Mannschaftsnamen.txt");
                            for (string str3 = streamReader2.ReadLine(); str3 != null; str3 = streamReader2.ReadLine())
                            {
                                string[] strArray = str3.Split('=');
                                strArray[0] = strArray[0].ToUpper();
                                if (strArray[0] == "HEIM")
                                    _teamname_home = strArray[1].Trim();
                                if (strArray[0] == "GAST")
                                    _teamname_guest = strArray[1].Trim();
                            }
                            streamReader2.Close();
                        }
                    }
                }
                if (InitString.StartsWith("UDP_PORT"))
                    ReceivePort = Convert.ToInt32(InitString.Split('=')[1]);
            }
            streamReader1.Close();
            _servername = Settings.Default.ServerName;
            _databasename = Settings.Default.DatabaseName;
            _password = Settings.Default.DatabasePassword;
            if (_stramatel_console_port == string.Empty)
            {
                _receiver = new UdpReceiver(ReceivePort);
                _receiver.DataReceived += _receiver_DataReceived;
                _receiver.BytesReceived += _receiver_BytesReceived;
            }
            for (int index = 0; index < _screens.Count; ++index)
                _screens[index].Show();
        }

        public void _write_cfg_line(string description, string new_value)
        {
            bool flag = false;
            List<string> stringList = new List<string>();
            string empty = string.Empty;
            if (File.Exists(Application.StartupPath + "\\Scoreboards.cfg"))
            {
                StreamReader streamReader = new StreamReader(Application.StartupPath + "\\Scoreboards.cfg");
                for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                {
                    if (!str.StartsWith(description))
                    {
                        stringList.Add(str);
                    }
                    else
                    {
                        if (!flag)
                            stringList.Add(description + "=" + new_value);
                        flag = true;
                    }
                }
                streamReader.Close();
            }
            if (!flag)
            {
                string str = description + "=" + new_value;
                stringList.Add(str);
            }
            StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\Scoreboards.cfg", false);
            for (int index = 0; index < stringList.Count; ++index)
                streamWriter.WriteLine(stringList[index]);
            streamWriter.Close();
        }

        public void _read_cfg()
        {
            if (!File.Exists(Application.StartupPath + "\\Scoreboards.cfg"))
                return;
            int num1 = 0;
            StreamReader streamReader1 = new StreamReader(Application.StartupPath + "\\Scoreboards.cfg");
            for (string str = streamReader1.ReadLine(); str != null; str = streamReader1.ReadLine())
            {
                if (str.StartsWith("SCB_TOPMOST"))
                    TopMost = str.EndsWith("ON");
                if (str.StartsWith("EXT_SCB_"))
                    ++num1;
            }
            streamReader1.Close();
            if (num1 <= 0)
                return;
            _ext_scoreboard = new List<IScoreBoard>();
            int num2 = -1;
            StreamReader streamReader2 = new StreamReader(Application.StartupPath + "\\Scoreboards.cfg");
            for (string str = streamReader2.ReadLine(); str != null; str = streamReader2.ReadLine())
            {
                if (!str.StartsWith(";") && !str.StartsWith("/"))
                {
                    string[] strArray = str.Split('=');
                    if (str.StartsWith("VIDEO_VOLUME"))
                        _video_volume = Convert.ToInt32(strArray[1]);
                    if (str.StartsWith("REFRESH_INTERVAL"))
                        _ext_scb_refresh_interval = Convert.ToInt32(strArray[1]);
                    if (str.StartsWith("EXT_SCB_"))
                    {
                        ++num2;
                        switch (strArray[0].ToUpper())
                        {
                            case "EXT_SCB_FAVERO":
                                _ext_scoreboard.Add((IScoreBoard)new FaveroSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_RG96":
                                _ext_scoreboard.Add((IScoreBoard)new RG96_ShotclockSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_STRAMATEL":
                                _ext_scoreboard.Add((IScoreBoard)new StramatelSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval, 1));
                                continue;
                            case "EXT_SCB_STRAMATEL1":
                                _ext_scoreboard.Add((IScoreBoard)new StramatelSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval, 1));
                                continue;
                            case "EXT_SCB_STRAMATEL2":
                                _ext_scoreboard.Add((IScoreBoard)new StramatelSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval, 2));
                                continue;
                            case "EXT_SCB_STRAMATELWATER":
                                _ext_scoreboard.Add((IScoreBoard)new StramatelWaterSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval, 2));
                                continue;
                            case "EXT_SCB_STRAMATEL_ICEHOCKEY":
                                _ext_scoreboard.Add((IScoreBoard)new StramatelIcehockeySender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_MTVISUAL":
                                _ext_scoreboard.Add((IScoreBoard)new MTVisualSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_HORN":
                                _ext_scoreboard.Add((IScoreBoard)new ExtHorn(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_DEBUG":
                                _ext_scoreboard.Add((IScoreBoard)new DebugSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_WIGE":
                                _ext_scoreboard.Add((IScoreBoard)new WIGESender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_SCHAUF":
                                _ext_scoreboard.Add((IScoreBoard)new SchaufSender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            case "EXT_SCB_NAUNET98":
                                _ext_scoreboard.Add((IScoreBoard)new Nautronic_98_Sender(Convert.ToInt32(strArray[1].Substring(3)), _ext_scb_refresh_interval));
                                continue;
                            default:
                                continue;
                        }
                    }
                }
            }
            streamReader2.Close();
        }

        public bool GraphicInWindow
        {
            set
            {
                _graphic_in_window = value;
            }
        }

        public bool AllowGraphicFromLAN
        {
            set
            {
                _allow_lan_graphic = value;
                for (int index = 0; index < _screens.Count; ++index)
                    _screens[index].AllowGraphicFromLAN = value;
            }
        }

        private void _receiver_BytesReceived(UdpReceiver sender, byte[] Data)
        {
            if (!_allow_lan_graphic)
                return;
            MemoryStream memoryStream = new MemoryStream(Data, 0, Data.Length);
            Bitmap bitmap = new Bitmap((Stream)memoryStream);
            memoryStream.Close();
            if (bitmap.Width > 1)
            {
                if (bitmap.Height > 1)
                {
                    try
                    {
                        ShowGraphic(Application.StartupPath + "\\Black.jpg", _ext_graphic_in_window, 0, true);
                        ShowGraphic((Image)bitmap, _ext_graphic_in_window, _ext_effect_index);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            }
            SequenceRequested(string.Empty, _graphic_in_window);
            _show_video = false;
            HideGraphic();
            ShowScore();
            _actual_picture = (Image)new Bitmap(Size.Width, Size.Height);
            _filename = string.Empty;
            pictureBox1.Image = _actual_picture;
            _do_show_score = true;
            _state = LED_Board.DisplayState.Score;
            for (int index = 0; index < _screens.Count; ++index)
            {
                _screens[index].ShowVideo = false;
                _screens[index].HideGraphic();
                _screens[index].ActualPicture = (Image)new Bitmap(_screens[index].Size.Width, _screens[index].Size.Height);
                _screens[index].Picturebox.Image = _screens[index].ActualPicture;
            }
        }

        private void _stramatel_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_stramatel_port.IsOpen)
                return;
            if (!_sw.IsRunning)
                _sw.Start();
            int bytesToRead = _stramatel_port.BytesToRead;
            for (int index = 0; index < bytesToRead; ++index)
            {
                int num = _stramatel_port.ReadByte();
                if (num == 248)
                    _buffer = string.Empty + (object)(char)num;
                else
                    _buffer += (string)(object)(char)num;
                if (num == 13)
                {
                    _sw.Stop();
                    _sw.Reset();
                    _sw.Start();
                    if (_buffer.StartsWith(string.Empty + (object)'ø') && (_sport == Sportart.Football && _buffer.Length == 24 || _buffer.Length == 54) && _loaded_layout_name != string.Empty)
                        EvaluateStramatelData(_buffer);
                    _buffer = string.Empty;
                    _stramatel_port.DtrEnable = false;
                    _stramatel_port.RtsEnable = false;
                }
            }
        }

        private void _wige_port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!_wige_port.IsOpen)
                return;
            int bytesToRead = _wige_port.BytesToRead;
            for (int index = 0; index < bytesToRead; ++index)
            {
                int num = _wige_port.ReadByte();
                if (num == 2)
                    _buffer = string.Empty;
                _buffer += (string)(object)(char)num;
                if (num == 3)
                {
                    if (_buffer[0] == '\x0002' && _loaded_layout_name != string.Empty)
                        EvaluateWigeData(_buffer);
                    _wige_port.DtrEnable = false;
                    _wige_port.RtsEnable = false;
                }
            }
        }

        private bool ConnectDatabase()
        {
            _dbfunc = new DataBaseFunctions(_servername, _databasename, _username, _password);
            return _dbfunc.ConnectDatabase();
        }

        public static bool IsVersionNumberLowerOrEqual(
          string versionToCompare,
          string versionToCompareWith)
        {
            string[] separator = new string[1] { "." };
            string[] strArray1 = versionToCompare.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string[] strArray2 = versionToCompareWith.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int totalWidth = 5;
            for (int index = 0; index < strArray1.Length; ++index)
                strArray1[index] = int.Parse(strArray1[index]).ToString().PadRight(totalWidth, '0');
            for (int index = 0; index < strArray2.Length; ++index)
                strArray2[index] = int.Parse(strArray2[index]).ToString().PadRight(totalWidth, '0');
            for (int index = 0; index < strArray1.Length; ++index)
            {
                int num1 = int.Parse(strArray2[index]);
                int num2 = int.Parse(strArray1[index]);
                if (num1 > num2)
                    return true;
                if (num1 < num2)
                    return false;
            }
            return true;
        }

        private void _receiver_DataReceived(UdpReceiver sender, string Data)
        {
            if (_layout_in_use || _loading)
                return;
            _temp_data += Data;
            _temp_data = !_temp_data.Contains("*") ? (!_temp_data.Contains("!") ? string.Empty : _temp_data.Substring(_temp_data.IndexOf('!'))) : _temp_data.Substring(_temp_data.IndexOf('*'));
            if (_temp_data.StartsWith("*"))
            {
                string[] strArray1 = _temp_data.Split('|');
                strArray1[0] = !(_manual_layout_name != string.Empty) ? strArray1[0].Substring(1).Trim() : _manual_layout_name;
                if ((!_layout_loaded || strArray1[0] != _loaded_layout_name.Trim()) && strArray1[1] != "none")
                {
                    _loading = true;
                    _homelogo = (Image)null;
                    _guestlogo = (Image)null;
                    _loadLayoutAsync(_background_pnl, strArray1[0]);
                    Sportart sportart = Sportart.Basketball;
                    switch (strArray1[0])
                    {
                        case "Basketball":
                            sportart = Sportart.Basketball;
                            break;
                        case "Handball":
                            sportart = Sportart.Handball;
                            break;
                        case "Volleyball":
                            sportart = Sportart.Volleyball;
                            break;
                        case "Waterpolo":
                            sportart = Sportart.Waterpolo;
                            break;
                        case "Soccer":
                            sportart = Sportart.Soccer;
                            break;
                        case "IndoorSoccer":
                            sportart = Sportart.Indoorsoccer;
                            break;
                        case "Tennis":
                            sportart = Sportart.Tennis;
                            break;
                        case "Icehockey":
                            sportart = Sportart.Icehockey;
                            break;
                        case "Hockey":
                            sportart = Sportart.Hockey;
                            break;
                        case "Football":
                            sportart = Sportart.Football;
                            break;
                    }
                    _sport = sportart;
                    if (_ext_scoreboard != null && _ext_scoreboard.Count > 0)
                    {
                        for (int index = 0; index < _ext_scoreboard.Count; ++index)
                            _ext_scoreboard[index].Sport = sportart;
                    }
                    _loading = false;
                }
                else
                {
                    updatePanelAsync(_background_pnl);
                    for (int index1 = 1; index1 < strArray1.Length; ++index1)
                    {
                        string[] strArray2 = strArray1[index1].Split('=');
                        if (strArray2[0] == "Version" && _program_version != strArray2[1])
                        {
                            _program_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                            if (!LED_Board.IsVersionNumberLowerOrEqual(strArray2[1], _program_version))
                            {
                                int num = (int)MessageBox.Show("Unsuitable program versions!", "GameControl and Scoreboard");
                                _program_version = strArray2[1];
                            }
                        }
                        else
                        {
                            if (_ext_scoreboard != null && _ext_scoreboard.Count > 0)
                            {
                                for (int index2 = 0; index2 < _ext_scoreboard.Count; ++index2)
                                {
                                    if (strArray2[0] == "GameTime")
                                    {
                                        try
                                        {
                                            _ext_scoreboard[index2].TimeRunning = strArray2[2] != "1";
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    _ext_scoreboard[index2].SetFieldValue(strArray2[0], strArray2[1]);
                                }
                            }
                            if (strArray2[0] == "Horn")
                            {
                                try
                                {
                                    setLabelTextAsync(Horn, strArray2[1]);
                                }
                                catch
                                {
                                }
                            }
                            else if (strArray2[0] == "SC_Horn")
                                setLabelTextAsync(SC_Horn, strArray2[1]);
                            else if (strArray2[0] == "TeamID_Home")
                            {
                                if (_teamID_home != Convert.ToInt32(strArray2[1]))
                                {
                                    _teamID_home = Convert.ToInt32(strArray2[1]);
                                    if (!_database_connected)
                                        _database_connected = ConnectDatabase();
                                    if (_database_connected)
                                    {
                                        _home_team = _dbfunc.ReadTeam(_teamID_home);
                                        _show_team_logos(_home_team, _guest_team);
                                    }
                                    if (_ext_scoreboard != null)
                                    {
                                        for (int index2 = 0; index2 < _ext_scoreboard.Count; ++index2)
                                        {
                                            if (_home_team != null)
                                                _ext_scoreboard[index2].SetFieldValue("TeamNameHome", _home_team.TeamName);
                                            if (_guest_team != null)
                                                _ext_scoreboard[index2].SetFieldValue("TeamNameGuest", _guest_team.TeamName);
                                            if (_home_team != null && _guest_team != null)
                                                _ext_scoreboard[index2].StartSending = true;
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
                                        _show_team_logos(_home_team, _guest_team);
                                    }
                                    if (_ext_scoreboard != null)
                                    {
                                        for (int index2 = 0; index2 < _ext_scoreboard.Count; ++index2)
                                        {
                                            if (_home_team != null)
                                                _ext_scoreboard[index2].SetFieldValue("TeamNameHome", _home_team.TeamName);
                                            if (_guest_team != null)
                                                _ext_scoreboard[index2].SetFieldValue("TeamNameGuest", _guest_team.TeamName);
                                            if (_home_team != null && _guest_team != null)
                                                _ext_scoreboard[index2].StartSending = true;
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
                                if (strArray2[0] == "GameTime")
                                {
                                    try
                                    {
                                        setHT_LabelTextAsync("GameTimeCopy", strArray2[1], strArray2[2] == "1");
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                for (int index = 0; index < _screens.Count; ++index)
                    _screens[index].SetData(_temp_data);
                _temp_data = string.Empty;
            }
            else
            {
                if (!_temp_data.StartsWith("!"))
                    return;
                switch (_temp_data[1])
                {
                    case 'C':
                        string path = Application.StartupPath + "\\Media\\" + _temp_data.Substring(2);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            break;
                        }
                        break;
                    case 'E':
                        _ext_effect_index = Convert.ToInt32(_temp_data.Substring(2));
                        break;
                    case 'G':
                        if (_temp_data.Substring(2) == "1")
                        {
                            _graphic_location = _cfg_graphic_location;
                            _graphic_size = _cfg_graphic_size;
                        }
                        if (_temp_data.Substring(2) == "0")
                        {
                            _graphic_location = _layout_graphic_location;
                            _graphic_size = _layout_graphic_size;
                            break;
                        }
                        break;
                    case 'I':
                        if (_allow_lan_graphic)
                        {
                            ShowGraphic(Application.StartupPath + "\\Media\\" + _temp_data.Substring(3) + ".jpg", _ext_graphic_in_window, _ext_effect_index, true);
                            break;
                        }
                        break;
                    case 'M':
                        _make_simple_image(_temp_data);
                        break;
                    case 'R':
                        _make_complex_image(_temp_data);
                        break;
                    case 'S':
                        if (!_database_connected)
                            _database_connected = ConnectDatabase();
                        if (_database_connected)
                        {
                            _make_sequence(_temp_data);
                            break;
                        }
                        break;
                    case 'V':
                        if (_temp_data.Substring(2) != _program_version)
                        {
                            int num = (int)MessageBox.Show("Unsuitable program versions!", "GameControl and Scoreboard");
                            break;
                        }
                        break;
                    case 'W':
                        _ext_graphic_in_window = false;
                        break;
                    case 'w':
                        _ext_graphic_in_window = true;
                        break;
                }
                _temp_data = string.Empty;
            }
        }

        private void _show_team_logos(Team HomeTeam, Team GuestTeam)
        {
            if (HomeTeam == null || GuestTeam == null)
                return;
            Bitmap img_home;
            if (HomeTeam.TeamLogo.Length > 0)
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream(HomeTeam.TeamLogo, 0, HomeTeam.TeamLogo.Length);
                    img_home = new Bitmap((Stream)memoryStream);
                    memoryStream.Close();
                }
                catch
                {
                    img_home = (Bitmap)null;
                }
            }
            else
                img_home = (Bitmap)null;
            Bitmap img_guest;
            if (GuestTeam.TeamLogo.Length > 0)
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream(GuestTeam.TeamLogo, 0, GuestTeam.TeamLogo.Length);
                    img_guest = new Bitmap((Stream)memoryStream);
                    memoryStream.Close();
                }
                catch
                {
                    img_guest = (Bitmap)null;
                }
            }
            else
                img_guest = (Bitmap)null;
            setPanelImageAsync(_background_pnl, "LogoHome", img_home, "LogoGuest", img_guest);
        }

        public string SequencePath
        {
            set
            {
                _sequence_path = value;
            }
        }

        public string MediaPath
        {
            set
            {
                _media_path = value;
            }
        }

        private void _make_sequence(string Data)
        {
            string[] strArray = Data.Split('|');
            string str1 = strArray[1];
            string str2 = strArray[2];
            string empty1 = string.Empty;
            string empty2;
            try
            {
                empty2 = strArray[3];
            }
            catch
            {
                empty2 = string.Empty;
            }
            int gameID;
            try
            {
                gameID = Convert.ToInt32(strArray[4]);
            }
            catch
            {
                gameID = -1;
            }
            int teamID;
            try
            {
                teamID = Convert.ToInt32(strArray[5]);
            }
            catch
            {
                teamID = -1;
            }
            bool is_home_team = strArray[6] == "1";
            bool is_reserve_player_only = strArray[7] == "1";
            bool is_coach = strArray[8] == "1";
            bool is_referees = strArray[9] == "1";
            int playernumber;
            try
            {
                playernumber = Convert.ToInt32(strArray[10]);
            }
            catch
            {
                playernumber = -1;
            }
            string empty3 = string.Empty;
            string empty4;
            try
            {
                empty4 = strArray[11];
            }
            catch
            {
                empty4 = string.Empty;
            }
            string empty5 = string.Empty;
            string empty6;
            try
            {
                empty6 = strArray[12];
            }
            catch
            {
                empty6 = string.Empty;
            }
            string empty7 = string.Empty;
            string empty8;
            try
            {
                empty8 = strArray[13];
            }
            catch
            {
                empty8 = string.Empty;
            }
            string empty9 = string.Empty;
            string empty10;
            try
            {
                empty10 = strArray[14];
            }
            catch
            {
                empty10 = string.Empty;
            }
            bool flag = strArray[15] == "1";
            Bitmap bitmap;
            if ((playernumber == 0 || is_referees) && str2 != "Corners")
            {
                bitmap = _load_layout_liste(str2, empty2, gameID, teamID, is_home_team, is_reserve_player_only, is_coach, is_referees);
            }
            else
            {
                bitmap = !(str2 == "Corners") ? _load_layout_einzel(str2, teamID, playernumber, empty4, empty6, empty8, empty10) : _load_layout_corners(str2, teamID, playernumber, empty4, empty6, empty8, empty10);
                if (str2 == "ChangeOutGuest")
                    bitmap.Save(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\ChangeInGuest.jpg", ImageFormat.Jpeg);
            }
            bitmap?.Save(_media_path + "\\" + str2 + ".jpg", ImageFormat.Jpeg);
            if (flag)
                return;
            if (File.Exists(_sequence_path + "\\" + str2 + ".seq"))
                SequenceRequested(str2, _ext_graphic_in_window);
            else
                ShowGraphic(_media_path + "\\" + str2 + ".jpg", _ext_graphic_in_window, _ext_effect_index, true);
        }

        private void _make_simple_image(string data)
        {
            string[] strArray1 = data.Split('|');
            string path = Application.StartupPath + "\\" + strArray1[1] + "_Layouts\\" + strArray1[2] + ".csv";
            string str1 = Application.StartupPath + "\\" + strArray1[1] + "_BackgroundImages\\" + strArray1[2] + ".jpg";
            string str2 = Application.StartupPath + "\\Media\\" + strArray1[2] + ".jpg";
            bool flag1 = false;
            if (strArray1[3] == "1")
                flag1 = true;
            List<string> stringList = new List<string>();
            bool flag2 = false;
            if (strArray1[2] == "ResultTable")
            {
                if (strArray1[4] == "1")
                    flag2 = true;
                for (int index = 5; index < strArray1.Length; ++index)
                    stringList.Add(strArray1[index]);
            }
            else
            {
                for (int index = 4; index < strArray1.Length; ++index)
                    stringList.Add(strArray1[index]);
            }
            if (stringList.Count > 0 && stringList[0].Trim() != string.Empty)
            {
                if (!File.Exists(path))
                    return;
                StreamReader streamReader = new StreamReader(path);
                string[] strArray2 = streamReader.ReadLine().Split(';');
                Label label = new Label();
                if (strArray2.Length >= 7)
                    return;
                Bitmap bitmap = new Bitmap(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
                Graphics graphic = Graphics.FromImage((Image)bitmap);
                if (File.Exists(str1))
                    graphic.DrawImage(Image.FromFile(str1), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                for (string str3 = streamReader.ReadLine(); str3 != null; str3 = streamReader.ReadLine())
                {
                    string[] strArray3 = str3.Split(';');
                    label.BackColor = Color.Transparent;
                    label.Name = strArray3[0];
                    label.Left = Convert.ToInt32(strArray3[1]);
                    label.Top = Convert.ToInt32(strArray3[2]);
                    label.Width = Convert.ToInt32(strArray3[3]);
                    label.Height = Convert.ToInt32(strArray3[4]);
                    FontStyle style = !(strArray3[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
                    label.Font = new Font(strArray3[5], Convert.ToSingle(strArray3[6]), style);
                    label.ForeColor = Color.FromArgb(Convert.ToInt32(strArray3[8]));
                    if (flag2)
                        label.ForeColor = Color.FromArgb(Convert.ToInt32(strArray3[9]));
                    StringAlignment alignment;
                    StringAlignment linealignment;
                    switch (strArray3[10])
                    {
                        case "BottomLeft":
                            alignment = StringAlignment.Far;
                            linealignment = StringAlignment.Near;
                            break;
                        case "BottomCenter":
                            alignment = StringAlignment.Far;
                            linealignment = StringAlignment.Center;
                            break;
                        case "BottomRight":
                            alignment = StringAlignment.Far;
                            linealignment = StringAlignment.Far;
                            break;
                        case "TopLeft":
                            alignment = StringAlignment.Near;
                            linealignment = StringAlignment.Near;
                            break;
                        case "TopCenter":
                            alignment = StringAlignment.Near;
                            linealignment = StringAlignment.Center;
                            break;
                        case "TopRight":
                            alignment = StringAlignment.Near;
                            linealignment = StringAlignment.Far;
                            break;
                        case "MiddleLeft":
                            alignment = StringAlignment.Center;
                            linealignment = StringAlignment.Near;
                            break;
                        case "MiddleCenter":
                            alignment = StringAlignment.Center;
                            linealignment = StringAlignment.Center;
                            break;
                        case "MiddleRight":
                            alignment = StringAlignment.Center;
                            linealignment = StringAlignment.Far;
                            break;
                        default:
                            alignment = StringAlignment.Center;
                            linealignment = StringAlignment.Center;
                            break;
                    }
                    label.Text = strArray3[11];
                    string empty = string.Empty;
                    string text;
                    try
                    {
                        text = !path.EndsWith("ResultTable.csv") ? stringList[Convert.ToInt32(label.Name.Substring(4)) - 1] : stringList[Convert.ToInt32(label.Name.Substring(4, 1)) - 1];
                    }
                    catch
                    {
                        text = string.Empty;
                    }
                    if (text != string.Empty)
                    {
                        switch (label.Name.Substring(0, 4))
                        {
                            case "Titl":
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text, label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                            case "Line":
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text, label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                            case "Team":
                                string str4 = " -";
                                if (text.Trim() == string.Empty)
                                    str4 = "  ";
                                if (strArray3[0].Substring(5, 1) == "1")
                                    DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text.Substring(0, 25).Trim() + str4, label.Font, label.ForeColor, alignment, linealignment);
                                if (strArray3[0].Substring(5, 1) == "2")
                                {
                                    DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text.Substring(28, 25).Trim(), label.Font, label.ForeColor, alignment, linealignment);
                                    continue;
                                }
                                continue;
                            case "Rslt":
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text.Substring(52).Trim(), label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                            case "Rank":
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text.Substring(0, 3), label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                            case "Name":
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text.Substring(4, 25).Trim(), label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                            case "Pnt.":
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), text.Substring(34).Trim(), label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                            case "Goal":
                                if (text.Substring(30, 3).Trim() != string.Empty)
                                {
                                    DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), "(" + text.Substring(30, 3).Trim() + ")", label.Font, label.ForeColor, alignment, linealignment);
                                    continue;
                                }
                                continue;
                            default:
                                DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), label.Text, label.Font, label.ForeColor, alignment, linealignment);
                                continue;
                        }
                    }
                }
                streamReader.Close();
                try
                {
                    bitmap.Save(str2, ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (!flag1)
                    return;
                ShowGraphic((Image)bitmap, false, _actual_effect_index);
            }
            else
            {
                if (!File.Exists(str2))
                    return;
                File.Delete(str2);
            }
        }

        private void _make_complex_image(string data)
        {
            string[] strArray1 = data.Split('|');
            string path = Application.StartupPath + "\\" + strArray1[1] + "_Layouts\\" + strArray1[2] + ".csv";
            string str1 = Application.StartupPath + "\\" + strArray1[1] + "_BackgroundImages\\" + strArray1[2] + ".jpg";
            string filename = Application.StartupPath + "\\Media\\" + strArray1[2] + ".jpg";
            bool flag1 = false;
            if (strArray1[3] == "1")
                flag1 = true;
            bool flag2 = false;
            if (strArray1[4] == "1")
                flag2 = true;
            if (!File.Exists(path))
                return;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int index = 5; index < strArray1.Length; ++index)
            {
                string[] strArray2 = strArray1[index].Split('=');
                dictionary.Add(strArray2[0], strArray2[1]);
            }
            StreamReader streamReader = new StreamReader(path);
            string[] strArray3 = streamReader.ReadLine().Split(';');
            if (strArray3.Length >= 7)
                return;
            Bitmap bitmap = new Bitmap(Convert.ToInt32(strArray3[0]), Convert.ToInt32(strArray3[1]));
            Graphics graphic = Graphics.FromImage((Image)bitmap);
            if (File.Exists(str1))
                graphic.DrawImage(Image.FromFile(str1), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            Label label = new Label();
            for (string str2 = streamReader.ReadLine(); str2 != null; str2 = streamReader.ReadLine())
            {
                string[] strArray2 = str2.Split(';');
                label.BackColor = Color.Transparent;
                label.Name = strArray2[0];
                label.Left = Convert.ToInt32(strArray2[1]);
                label.Top = Convert.ToInt32(strArray2[2]);
                label.Width = Convert.ToInt32(strArray2[3]);
                label.Height = Convert.ToInt32(strArray2[4]);
                FontStyle style = !(strArray2[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
                label.Font = new Font(strArray2[5], Convert.ToSingle(strArray2[6]), style);
                label.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[8]));
                if (flag2)
                    label.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[9]));
                StringAlignment alignment;
                StringAlignment linealignment;
                switch (strArray2[10])
                {
                    case "BottomLeft":
                        alignment = StringAlignment.Far;
                        linealignment = StringAlignment.Near;
                        break;
                    case "BottomCenter":
                        alignment = StringAlignment.Far;
                        linealignment = StringAlignment.Center;
                        break;
                    case "BottomRight":
                        alignment = StringAlignment.Far;
                        linealignment = StringAlignment.Far;
                        break;
                    case "TopLeft":
                        alignment = StringAlignment.Near;
                        linealignment = StringAlignment.Near;
                        break;
                    case "TopCenter":
                        alignment = StringAlignment.Near;
                        linealignment = StringAlignment.Center;
                        break;
                    case "TopRight":
                        alignment = StringAlignment.Near;
                        linealignment = StringAlignment.Far;
                        break;
                    case "MiddleLeft":
                        alignment = StringAlignment.Center;
                        linealignment = StringAlignment.Near;
                        break;
                    case "MiddleCenter":
                        alignment = StringAlignment.Center;
                        linealignment = StringAlignment.Center;
                        break;
                    case "MiddleRight":
                        alignment = StringAlignment.Center;
                        linealignment = StringAlignment.Far;
                        break;
                    default:
                        alignment = StringAlignment.Center;
                        linealignment = StringAlignment.Center;
                        break;
                }
                label.Image = (Image)null;
                if (dictionary.ContainsKey(label.Name))
                {
                    if (label.Name.Contains("Logo"))
                    {
                        int int32 = Convert.ToInt32(dictionary[label.Name]);
                        if (_dbfunc == null)
                            ConnectDatabase();
                        Team team = _dbfunc.ReadTeam(int32);
                        MemoryStream memoryStream = new MemoryStream(team.TeamLogo, 0, team.TeamLogo.Length);
                        label.Image = (Image)new Bitmap((Stream)memoryStream);
                        memoryStream.Close();
                    }
                    else
                        label.Text = dictionary[label.Name];
                }
                else
                    label.Text = strArray2[11];
                if (label.Image != null)
                    graphic.DrawImage(label.Image, new Rectangle(label.Location, label.Size));
                else
                    DrawText(graphic, new Rectangle(label.Left, label.Top, label.Width, label.Height), label.Text, label.Font, label.ForeColor, alignment, linealignment);
            }
            streamReader.Close();
            try
            {
                bitmap.Save(filename, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (!flag1)
                return;
            new SoundPlayer(Application.StartupPath + "\\gong.wav").Play();
            ShowGraphic((Image)bitmap, false, 0);
        }

        private Bitmap _load_layout_liste(
          string layoutname,
          string list_title,
          int gameID,
          int teamID,
          bool is_home_team,
          bool is_reserve_player_only,
          bool is_coach,
          bool is_referees)
        {
            List<GamePlayer> gamePlayerList1 = new List<GamePlayer>();
            List<GamePlayer> gamePlayerList2 = new List<GamePlayer>();
            List<Player> playerList1 = new List<Player>();
            List<GamePlayer> gamePlayerList3 = new List<GamePlayer>();
            string path = Application.StartupPath + "\\" + _sport.ToString() + "_Layouts\\" + layoutname + ".csv";
            Bitmap bitmap = (Bitmap)null;
            if (File.Exists(path))
            {
                int TeamID = 0;
                ListBox listBox = new ListBox();
                listBox.Sorted = false;
                if (is_coach)
                {
                    List<Player> playerList2 = _dbfunc.ReadCoaches(teamID);
                    for (int index = 0; index < playerList2.Count; ++index)
                    {
                        TeamID = playerList2[index].PlayerTeamID;
                        listBox.Items.Add((object)(playerList2[index].PlayerNumber.ToString().PadLeft(2) + "0" + playerList2[index].PlayerName.Trim()));
                    }
                }
                else if (is_referees)
                {
                    List<GamePlayer> gamePlayerList4 = _dbfunc.GamePlayers(gameID, teamID, is_coach);
                    for (int index = 0; index < gamePlayerList4.Count; ++index)
                    {
                        TeamID = gamePlayerList4[index].TeamID;
                        listBox.Items.Add((object)(gamePlayerList4[index].PlayerNumber.ToString().PadLeft(2) + "0" + gamePlayerList4[index].Name.Trim()));
                    }
                }
                else if (is_home_team)
                {
                    List<GamePlayer> gamePlayerList4 = _dbfunc.GamePlayers(gameID, teamID, is_coach);
                    for (int index = 0; index < gamePlayerList4.Count; ++index)
                    {
                        TeamID = gamePlayerList4[index].TeamID;
                        if (gamePlayerList4[index].IsReservePlayer == is_reserve_player_only)
                            listBox.Items.Add((object)(gamePlayerList4[index].PlayerNumber.ToString().PadLeft(2) + gamePlayerList4[index].GamePlayerCards.ToString() + gamePlayerList4[index].Name.Trim()));
                    }
                }
                else
                {
                    List<GamePlayer> gamePlayerList4 = _dbfunc.GamePlayers(gameID, teamID, is_coach);
                    for (int index = 0; index < gamePlayerList4.Count; ++index)
                    {
                        TeamID = gamePlayerList4[index].TeamID;
                        if (gamePlayerList4[index].IsReservePlayer == is_reserve_player_only)
                            listBox.Items.Add((object)(gamePlayerList4[index].PlayerNumber.ToString().PadLeft(2) + gamePlayerList4[index].GamePlayerCards.ToString() + gamePlayerList4[index].Name.Trim()));
                    }
                }
                StreamReader streamReader = new StreamReader(path);
                string[] strArray1 = streamReader.ReadLine().Split(';');
                ScoreboardLabel scoreboardLabel = new ScoreboardLabel();
                if (strArray1.Length < 7)
                {
                    bitmap = new Bitmap(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]));
                    Graphics graphic = Graphics.FromImage((Image)bitmap);
                    if (File.Exists(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\" + layoutname + ".jpg"))
                        graphic.DrawImage(Image.FromFile(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\" + layoutname + ".jpg"), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                    {
                        string[] strArray2 = str.Split(';');
                        scoreboardLabel.MyLabel.BackColor = Color.Transparent;
                        scoreboardLabel.MyLabel.Left = Convert.ToInt32(strArray2[1]);
                        scoreboardLabel.MyLabel.Top = Convert.ToInt32(strArray2[2]);
                        scoreboardLabel.MyLabel.Width = Convert.ToInt32(strArray2[3]);
                        scoreboardLabel.MyLabel.Height = Convert.ToInt32(strArray2[4]);
                        FontStyle style = !(strArray2[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
                        scoreboardLabel.MyLabel.Font = new Font(strArray2[5], Convert.ToSingle(strArray2[6]), style);
                        scoreboardLabel.MyLabel.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[8]));
                        scoreboardLabel.NormalColor = scoreboardLabel.MyLabel.ForeColor;
                        scoreboardLabel.AlternateColor = Color.FromArgb(Convert.ToInt32(strArray2[9]));
                        StringAlignment alignment;
                        StringAlignment linealignment;
                        switch (strArray2[10])
                        {
                            case "BottomLeft":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Near;
                                break;
                            case "BottomCenter":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Center;
                                break;
                            case "BottomRight":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Far;
                                break;
                            case "TopLeft":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Near;
                                break;
                            case "TopCenter":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Center;
                                break;
                            case "TopRight":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Far;
                                break;
                            case "MiddleLeft":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Near;
                                break;
                            case "MiddleCenter":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Center;
                                break;
                            case "MiddleRight":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Far;
                                break;
                            default:
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Center;
                                break;
                        }
                        scoreboardLabel.MyLabel.Text = strArray2[11];
                        switch (strArray2[0])
                        {
                            case "ListTitle":
                                DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), list_title, scoreboardLabel.MyLabel.Font, scoreboardLabel.MyLabel.ForeColor, alignment, linealignment);
                                break;
                            case "TeamName":
                                Team team1 = _dbfunc.ReadTeam(TeamID);
                                if (team1 != null)
                                {
                                    DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), team1.TeamName, scoreboardLabel.MyLabel.Font, scoreboardLabel.MyLabel.ForeColor, alignment, linealignment);
                                    break;
                                }
                                break;
                            case "TeamLogo":
                                Team team2 = _dbfunc.ReadTeam(TeamID);
                                if (team2 != null)
                                {
                                    MemoryStream memoryStream = new MemoryStream(team2.TeamLogo, 0, team2.TeamLogo.Length);
                                    Bitmap original = new Bitmap((Stream)memoryStream);
                                    memoryStream.Close();
                                    if (original != null)
                                    {
                                        graphic.DrawImage((Image)original, _scale_picture(original, scoreboardLabel.MyLabel));
                                        break;
                                    }
                                    break;
                                }
                                break;
                            default:
                                if (strArray2[0].StartsWith("PlayerNo"))
                                {
                                    int int32 = Convert.ToInt32(strArray2[0].Substring(8));
                                    if (listBox.Items.Count > int32)
                                    {
                                        DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), listBox.Items[int32].ToString().Substring(0, 2), scoreboardLabel.MyLabel.Font, scoreboardLabel.MyLabel.ForeColor, alignment, linealignment);
                                        break;
                                    }
                                    break;
                                }
                                if (strArray2[0].StartsWith("PlayerName"))
                                {
                                    int int32 = Convert.ToInt32(strArray2[0].Substring(10));
                                    if (listBox.Items.Count > int32)
                                    {
                                        DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), listBox.Items[int32].ToString().Substring(3), scoreboardLabel.MyLabel.Font, scoreboardLabel.MyLabel.ForeColor, alignment, linealignment);
                                        break;
                                    }
                                    break;
                                }
                                if (strArray2[0].StartsWith("Cards"))
                                {
                                    int int32 = Convert.ToInt32(strArray2[0].Substring(5));
                                    if (listBox.Items.Count > int32)
                                    {
                                        int num = 0;
                                        try
                                        {
                                            num = Convert.ToInt32(listBox.Items[int32].ToString().Substring(2, 1));
                                        }
                                        catch
                                        {
                                        }
                                        if (num == 1)
                                            DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), "█", scoreboardLabel.MyLabel.Font, scoreboardLabel.NormalColor, alignment, linealignment);
                                        if (num > 1)
                                        {
                                            DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), "█", scoreboardLabel.MyLabel.Font, scoreboardLabel.AlternateColor, alignment, linealignment);
                                            break;
                                        }
                                        break;
                                    }
                                    break;
                                }
                                DrawText(graphic, new Rectangle(scoreboardLabel.MyLabel.Left, scoreboardLabel.MyLabel.Top, scoreboardLabel.MyLabel.Width, scoreboardLabel.MyLabel.Height), scoreboardLabel.MyLabel.Text, scoreboardLabel.MyLabel.Font, scoreboardLabel.NormalColor, alignment, linealignment);
                                break;
                        }
                    }
                    streamReader.Close();
                }
            }
            return bitmap;
        }

        private Bitmap _load_layout_einzel(
          string layoutname,
          int teamID,
          int playernumber,
          string playername,
          string act_game_minute,
          string score_home,
          string score_guest)
        {
            string path = Application.StartupPath + "\\" + _sport.ToString() + "_Layouts\\" + layoutname + ".csv";
            Bitmap bitmap = (Bitmap)null;
            if (File.Exists(path))
            {
                Bitmap original1 = (Bitmap)null;
                Team team1 = _dbfunc.ReadTeam(teamID);
                if (team1 != null && team1.TeamLogo != null)
                {
                    try
                    {
                        MemoryStream memoryStream = new MemoryStream(team1.TeamLogo, 0, team1.TeamLogo.Length);
                        original1 = new Bitmap((Stream)memoryStream);
                        memoryStream.Close();
                    }
                    catch
                    {
                        original1 = new Bitmap(Width, Height);
                    }
                }
                StreamReader streamReader = new StreamReader(path);
                string[] strArray1 = streamReader.ReadLine().Split(';');
                Label feld = new Label();
                if (strArray1.Length < 7)
                {
                    bitmap = new Bitmap(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]));
                    Graphics graphic = Graphics.FromImage((Image)bitmap);
                    if (File.Exists(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\" + layoutname + ".jpg"))
                        graphic.DrawImage(Image.FromFile(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\" + layoutname + ".jpg"), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                    for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                    {
                        string[] strArray2 = str.Split(';');
                        feld.BackColor = Color.Transparent;
                        feld.Left = Convert.ToInt32(strArray2[1]);
                        feld.Top = Convert.ToInt32(strArray2[2]);
                        feld.Width = Convert.ToInt32(strArray2[3]);
                        feld.Height = Convert.ToInt32(strArray2[4]);
                        FontStyle style = !(strArray2[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
                        feld.Font = new Font(strArray2[5], Convert.ToSingle(strArray2[6]), style);
                        feld.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[8]));
                        StringAlignment alignment;
                        StringAlignment linealignment;
                        switch (strArray2[10])
                        {
                            case "BottomLeft":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Near;
                                break;
                            case "BottomCenter":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Center;
                                break;
                            case "BottomRight":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Far;
                                break;
                            case "TopLeft":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Near;
                                break;
                            case "TopCenter":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Center;
                                break;
                            case "TopRight":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Far;
                                break;
                            case "MiddleLeft":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Near;
                                break;
                            case "MiddleCenter":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Center;
                                break;
                            case "MiddleRight":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Far;
                                break;
                            default:
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Center;
                                break;
                        }
                        feld.Text = strArray2[11];
                        switch (strArray2[0])
                        {
                            case "ScoreHome":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), score_home, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "ScoreGuest":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), score_guest, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "GameTime":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), act_game_minute + ".Minute", feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "PlayerNumber":
                                if (playernumber > -1)
                                {
                                    DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), playernumber.ToString(), feld.Font, feld.ForeColor, alignment, linealignment);
                                    break;
                                }
                                break;
                            case "PlayerName":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), playername, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "PlayerImage":
                                Bitmap original2 = playernumber >= 0 ? _dbfunc.GamePlayerImage(teamID, playernumber) : _dbfunc.GamePlayerImage(teamID, playernumber, playername);
                                if (original2 != null)
                                {
                                    graphic.DrawImage((Image)original2, _scale_picture(original2, feld));
                                    break;
                                }
                                break;
                            case "TeamName":
                                Team team2 = _dbfunc.ReadTeam(teamID);
                                if (team2 != null)
                                {
                                    DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), team2.TeamName, feld.Font, feld.ForeColor, alignment, linealignment);
                                    break;
                                }
                                break;
                            case "TeamLogo":
                                if (original1 != null)
                                {
                                    graphic.DrawImage((Image)original1, _scale_picture(original1, feld));
                                    break;
                                }
                                break;
                            default:
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), feld.Text, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                        }
                    }
                    streamReader.Close();
                }
            }
            return bitmap;
        }

        private Bitmap _load_layout_corners(
          string layoutname,
          int teamID,
          int playernumber,
          string playername,
          string act_game_minute,
          string score_home,
          string score_guest)
        {
            string path = Application.StartupPath + "\\" + _sport.ToString() + "_Layouts\\" + layoutname + ".csv";
            Bitmap bitmap1 = (Bitmap)null;
            if (File.Exists(path))
            {
                Bitmap original = (Bitmap)null;
                Team team1 = _dbfunc.ReadTeam(teamID);
                if (team1 != null && team1.TeamLogo != null)
                {
                    try
                    {
                        MemoryStream memoryStream = new MemoryStream(team1.TeamLogo, 0, team1.TeamLogo.Length);
                        original = new Bitmap((Stream)memoryStream);
                        memoryStream.Close();
                    }
                    catch
                    {
                        original = new Bitmap(Width, Height);
                    }
                }
                Bitmap bitmap2 = (Bitmap)null;
                Team team2 = _dbfunc.ReadTeam(playernumber);
                if (team2 != null && team2.TeamLogo != null)
                {
                    try
                    {
                        MemoryStream memoryStream = new MemoryStream(team2.TeamLogo, 0, team2.TeamLogo.Length);
                        bitmap2 = new Bitmap((Stream)memoryStream);
                        memoryStream.Close();
                    }
                    catch
                    {
                        bitmap2 = new Bitmap(Width, Height);
                    }
                }
                StreamReader streamReader = new StreamReader(path);
                string[] strArray1 = streamReader.ReadLine().Split(';');
                Label feld = new Label();
                if (strArray1.Length < 7)
                {
                    bitmap1 = new Bitmap(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]));
                    Graphics graphic = Graphics.FromImage((Image)bitmap1);
                    if (File.Exists(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\" + layoutname + ".jpg"))
                        graphic.DrawImage(Image.FromFile(Application.StartupPath + "\\" + _sport.ToString() + "_BackgroundImages\\" + layoutname + ".jpg"), new Rectangle(0, 0, bitmap1.Width, bitmap1.Height));
                    for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                    {
                        string[] strArray2 = str.Split(';');
                        feld.BackColor = Color.Transparent;
                        feld.Left = Convert.ToInt32(strArray2[1]);
                        feld.Top = Convert.ToInt32(strArray2[2]);
                        feld.Width = Convert.ToInt32(strArray2[3]);
                        feld.Height = Convert.ToInt32(strArray2[4]);
                        FontStyle style = !(strArray2[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
                        feld.Font = new Font(strArray2[5], Convert.ToSingle(strArray2[6]), style);
                        feld.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[8]));
                        StringAlignment alignment;
                        StringAlignment linealignment;
                        switch (strArray2[10])
                        {
                            case "BottomLeft":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Near;
                                break;
                            case "BottomCenter":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Center;
                                break;
                            case "BottomRight":
                                alignment = StringAlignment.Far;
                                linealignment = StringAlignment.Far;
                                break;
                            case "TopLeft":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Near;
                                break;
                            case "TopCenter":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Center;
                                break;
                            case "TopRight":
                                alignment = StringAlignment.Near;
                                linealignment = StringAlignment.Far;
                                break;
                            case "MiddleLeft":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Near;
                                break;
                            case "MiddleCenter":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Center;
                                break;
                            case "MiddleRight":
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Far;
                                break;
                            default:
                                alignment = StringAlignment.Center;
                                linealignment = StringAlignment.Center;
                                break;
                        }
                        feld.Text = strArray2[11];
                        switch (strArray2[0])
                        {
                            case "CornersHome":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), score_home, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "CornersGuest":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), score_guest, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "TeamNameHome":
                                if (team1 != null)
                                {
                                    DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), team1.TeamName, feld.Font, feld.ForeColor, alignment, linealignment);
                                    break;
                                }
                                break;
                            case "TeamLogoHome":
                                if (original != null)
                                {
                                    graphic.DrawImage((Image)original, _scale_picture(original, feld));
                                    break;
                                }
                                break;
                            case "TeamNameGuest":
                                if (team2 != null)
                                {
                                    DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), team2.TeamName, feld.Font, feld.ForeColor, alignment, linealignment);
                                    break;
                                }
                                break;
                            case "TeamLogoGuest":
                                if (bitmap2 != null)
                                {
                                    graphic.DrawImage((Image)bitmap2, _scale_picture(original, feld));
                                    break;
                                }
                                break;
                            default:
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), feld.Text, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                        }
                    }
                    streamReader.Close();
                }
            }
            return bitmap1;
        }

        private Rectangle _scale_picture(Bitmap original, Label feld)
        {
            Rectangle rectangle = new Rectangle(0, 0, original.Width, original.Height);
            try
            {
                if (original != null)
                {
                    if (feld != null)
                    {
                        if (original.Height > 0)
                        {
                            if (original.Width > 0)
                            {
                                if (feld.Height > 0)
                                {
                                    if (feld.Width > 0)
                                    {
                                        int num = original.Width * feld.Height / original.Height;
                                        rectangle.X = feld.Left;
                                        rectangle.Y = feld.Top;
                                        if (num >= feld.Width)
                                        {
                                            rectangle.Width = feld.Width;
                                            rectangle.Height = original.Height * feld.Width / original.Width;
                                            rectangle.Y = feld.Top + (feld.Height - rectangle.Height) / 2;
                                        }
                                        else
                                        {
                                            rectangle.Height = feld.Height;
                                            rectangle.Width = original.Width * feld.Height / original.Height;
                                            rectangle.X = feld.Left + (feld.Width - rectangle.Width) / 2;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("_scale_picture : " + ex.Message);
            }
            return rectangle;
        }

        public void DrawText(
          Graphics graphic,
          Rectangle rect,
          string text,
          Font font,
          Color color,
          StringAlignment alignment,
          StringAlignment linealignment)
        {
            try
            {
                SolidBrush solidBrush = new SolidBrush(color);
                StringFormat format = new StringFormat();
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                format.Alignment = linealignment;
                format.LineAlignment = linealignment;
                graphic.DrawString(text.Trim(), font, (Brush)solidBrush, (RectangleF)rect, format);
            }
            catch
            {
            }
        }

        private void LED_Board_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int index = 0; index < _screens.Count; ++index)
                _screens[index].Close();
            if (_my_effect != null)
            {
                _my_effect.Dispose();
                _my_effect.Ready -= new Effects.ReadyDelegate(_my_effect_Ready);
                _my_effect.PercentComplete -= new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
                _my_effect = (Effects.IEffect)null;
            }
            GC.Collect();
            if (_mci != null)
            {
                if (_mci.IsOpen)
                    _mci.Close();
                _mci.Dispose();
                _mci = (Mci)null;
            }
            Settings.Default.GameTimeInsertLocation = _gametime_insert_location;
            Settings.Default.GameTimeInsertColor = _gametime_insert_color;
            Settings.Default.GameTimeInsertFont = new Font(_gametime_insert_font_name, (float)_gametime_insert_font_size, _gametime_insert_fontstyle);
            Settings.Default.Save();
            if (_ext_scoreboard == null || _ext_scoreboard.Count <= 0)
                return;
            for (int index = 0; index < _ext_scoreboard.Count; ++index)
                _ext_scoreboard[index].Dispose();
        }

        private void Horn_TextChanged(object sender, EventArgs e)
        {
            if ((Label)sender == Horn)
            {
                if (Horn.Text == "On")
                    _hornblower.Play();
                else
                    _hornblower.Stop();
                _fields.ContainsKey("GameTime");
                if (_ext_scoreboard == null)
                    return;
                for (int index = 0; index < _ext_scoreboard.Count; ++index)
                {
                    _ext_scoreboard[index].Horn = Horn.Text == "On";
                    if (_fields.ContainsKey("GameTime"))
                    {
                        if (Horn.Text == "On" && (_fields["GameTime"].MyLabel.Text.Trim() == "0.0" || _fields["GameTime"].MyLabel.Text.Trim() == "0.1"))
                        {
                            _ext_scoreboard[index].ShotclockHorn = true;
                            _ext_scoreboard[index].RedLight = true;
                        }
                        else
                        {
                            _ext_scoreboard[index].ShotclockHorn = false;
                            _ext_scoreboard[index].RedLight = false;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(SC_Horn.Text);
                if (SC_Horn.Text == "On")
                    _shotclock_hornblower.Play();
                else
                    _shotclock_hornblower.Stop();
                if (_ext_scoreboard == null)
                    return;
                for (int index = 0; index < _ext_scoreboard.Count; ++index)
                    _ext_scoreboard[index].ShotclockHorn = SC_Horn.Text == "On";
            }
        }

        private void _get_effect(int EffectIndex)
        {
            if (_my_effect != null)
                _my_effect.Dispose();
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
            _actual_effect_index = EffectIndex;
        }

        public void ShowTempGraphic()
        {
            ShowGraphic(Application.StartupPath + "\\TempPicture.jpg", _ext_graphic_in_window, _ext_effect_index, true);
        }

        private bool IsAnimation(string FileName)
        {
            string upper = FileName.ToUpper();
            if (!upper.EndsWith(".MPG") && !upper.EndsWith(".MPEG") && (!upper.EndsWith(".WMV") && !upper.EndsWith(".AVI")) && (!upper.EndsWith(".MOV") && !upper.EndsWith(".VOB") && !upper.EndsWith(".MOV")))
                return upper.EndsWith(".MP4");
            return true;
        }

        public void ShowGraphic(
          string FileName,
          bool GraphicInWindow,
          int EffectIndex,
          bool CreateActImageFromScore)
        {
            _graphic_in_window = GraphicInWindow;
            try
            {
                resizePictureBoxAsync(pictureBox1);
                if (File.Exists(FileName))
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
                    showPictureBoxAsync(pictureBox1);
                    if (!IsAnimation(FileName))
                    {
                        _actual_picture = _state != LED_Board.DisplayState.Score || !CreateActImageFromScore ? pictureBox1.Image : (Image)_make_score_screenshot();
                        if (_actual_picture == null)
                        {
                            if (File.Exists(_filename))
                            {
                                try
                                {
                                    _actual_picture = Image.FromFile(_filename);
                                }
                                catch
                                {
                                    _actual_picture = BlackImage();
                                }
                            }
                            else
                                _actual_picture = BlackImage();
                        }
                        _filename = FileName;
                        StreamReader streamReader = new StreamReader(_filename);
                        Bitmap bitmap1 = new Bitmap(streamReader.BaseStream);
                        streamReader.Close();
                        Bitmap bitmap2 = new Bitmap((Image)bitmap1, new Size(1024, 768));
                        if (_my_effect != null)
                            _my_effect.Dispose();
                        _my_effect = (Effects.IEffect)null;
                        _get_effect(EffectIndex);
                        try
                        {
                            if (!_filename.EndsWith(".gif"))
                            {
                                if (!_filename.EndsWith(".GIF"))
                                    goto label_20;
                            }
                            Image image = Image.FromFile(_filename);
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
                        label_20:
                        if (_my_effect != null)
                        {
                            _insert_game_time_old = _insert_game_time;
                            _insert_game_time = false;
                            _my_effect_ready = false;
                            _my_effect.Ready += new Effects.ReadyDelegate(_my_effect_Ready);
                            _my_effect.PercentComplete += new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
                            _my_effect.Fade(pictureBox1, _actual_picture, (Image)bitmap2);
                            _actual_picture = (Image)bitmap2;
                        }
                        else
                            pictureBox1.Image = Image.FromFile(_filename);
                    }
                    else
                    {
                        GraphicAnimationDetected();
                        try
                        {
                            _filename = string.Empty;
                            _actual_picture = BlackImage();
                            pictureBox1.Image = _actual_picture;
                            createVideoAsync(pictureBox1, FileName);
                        }
                        catch (Exception ex)
                        {
                            hidePictureBoxAsync(pictureBox1);
                            ErrorLogger.addToLog(ex.StackTrace, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace.ToString());
            }
            for (int index = 0; index < _screens.Count; ++index)
                _screens[index].ShowGraphic(FileName, GraphicInWindow, EffectIndex);
            if (_preview == null)
                return;
            _preview.ShowGraphicPreviewAsync(FileName);
        }

        public void RefreshLogos()
        {
            _loaded_layout_name = string.Empty;
        }

        public void ShowScore()
        {
            showPanelAsync(_background_pnl);
            _show_team_logos(_home_team, _guest_team);
            if (_ext_scoreboard != null && _init)
            {
                _init = false;
                if (_ext_scoreboard.Count > 0)
                {
                    for (int index = 0; index < _ext_scoreboard.Count; ++index)
                    {
                        _ext_scoreboard[index].DoRefreshHomeTeam = true;
                        _ext_scoreboard[index].DoRefreshGuestTeam = true;
                    }
                }
            }
            if (_screens == null || _screens.Count <= 0)
                return;
            for (int index = 0; index < _screens.Count; ++index)
            {
                _screens[index].Clear();
                _screens[index].HideGraphic();
            }
        }

        public void HideScore()
        {
            hidePanelAsync(_background_pnl);
        }

        private void _video_Ending(object sender, EventArgs e)
        {
            if (MciElapsed == null)
                return;
            MciElapsed();
        }

        public void ShowGraphic(Image Img, bool GraphicInWindow, int EffectIndex)
        {
            _graphic_in_window = GraphicInWindow;
            resizePictureBoxAsync(pictureBox1);
            while (_loading)
                Application.DoEvents();
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
                if (_actual_picture == null)
                    _actual_picture = pictureBox1.Image;
                if (_actual_picture == null)
                {
                    if (File.Exists(_filename))
                    {
                        try
                        {
                            _actual_picture = Image.FromFile(_filename);
                        }
                        catch
                        {
                            _actual_picture = BlackImage();
                        }
                    }
                    else
                        _actual_picture = BlackImage();
                }
                _filename = string.Empty;
                showPictureBoxAsync(pictureBox1);
                if (_state == LED_Board.DisplayState.Score)
                    _actual_picture = (Image)_make_score_screenshot();
                _get_effect(EffectIndex);
                if (_my_effect != null)
                {
                    while (!_my_effect_ready)
                        Application.DoEvents();
                    _my_effect_ready = false;
                    _my_effect.Ready += new Effects.ReadyDelegate(_my_effect_Ready);
                    _my_effect.PercentComplete += new Effects.PercentCompletedDelegate(_my_effect_PercentComplete);
                    _my_effect.Fade(pictureBox1, _actual_picture, Img);
                    _actual_picture = Img;
                }
            }
            catch
            {
            }
            for (int index = 0; index < _screens.Count; ++index)
                _screens[index].ShowGraphic(Img, GraphicInWindow, _actual_effect_index);
        }

        private void _my_effect_PercentComplete(Effects.IEffect sender, int value)
        {
            PercentComplete(value);
        }

        private void _my_effect_Ready(Effects.IEffect sender)
        {
            _insert_game_time = _insert_game_time_old;
            if (_my_effect == null)
                return;
            _my_effect.Dispose();
            _my_effect_ready = true;
        }

        public void ResizeGraphic(bool GraphicInWindow)
        {
            ShowGraphic(_filename, GraphicInWindow);
        }

        public void ShowGraphic(string FileName, bool GraphicInWindow)
        {
            if (FileName != null)
                _filename = FileName;
            if (!File.Exists(_filename))
                return;
            _graphic_in_window = GraphicInWindow;
            for (int index = 0; index < _screens.Count; ++index)
                _screens[index].ShowGraphic(_filename, _graphic_in_window);
            try
            {
                showPictureBoxAsync(pictureBox1);
                pictureBox1.Image = Image.FromFile(_filename);
                Update();
            }
            catch
            {
                try
                {
                    showPictureBoxAsync(pictureBox1);
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
                    if (_mci != null)
                        return;
                    _mci = new Mci();
                    _mci.Open(_filename, (Control)pictureBox1);
                    showPictureBoxAsync(pictureBox1);
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
                        _mci.Volume = 10000;
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
            hidePictureBoxAsync(pictureBox1);
        }

        private void _mci_timer_Tick(object sender, EventArgs e)
        {
            _mci_timer.Stop();
            _mci_timer.Tick -= new EventHandler(_mci_timer_Tick);
            if (_mci == null)
                return;
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
            MciElapsed();
        }

        private void EvaluateStramatelData(string Buffer)
        {
            string str = Buffer;
            Buffer = string.Empty;
            if (str.Length > 54)
                str = str.Substring(0, 54);
            if ((byte)str[1] != (byte)102 && (byte)str[1] != (byte)98 && (byte)str[1] != (byte)119)
            {
                if ((byte)str[1] != (byte)54)
                    _game_time = (byte)str[7] == (byte)32 ? ((byte)str[4] != (byte)48 ? str.Substring(4, 2) + "." + str.Substring(6, 2) : " " + str.Substring(5, 1) + "." + str.Substring(6, 2)) : str.Substring(4, 2) + ":" + str.Substring(6, 2);
                if (_sport == Sportart.Volleyball)
                    setHT_LabelTextAsync("TimeoutTime", _game_time, (byte)str[20] == (byte)49);
                else
                    setHT_LabelTextAsync("GameTime", _game_time, (byte)str[20] == (byte)49);
            }
            if ((byte)str[1] != (byte)51 && (byte)str[1] != (byte)53 && ((byte)str[1] != (byte)54 && (byte)str[1] != (byte)57) && (byte)str[1] != (byte)58)
            {
                if ((byte)str[1] != (byte)108)
                    goto label_13;
            }
            if (_sport == Sportart.Football)
                setHT_LabelTextAsync("Period", str.Substring(17, 1), false);
            else
                setHT_LabelTextAsync("Period", str.Substring(14, 1), false);
            label_13:
            try
            {
                switch ((byte)str[1])
                {
                    case 51:
                        try
                        {
                            setLabelTextAsync(Horn, str[19] == '1' ? "On" : "Off");
                            if ((byte)str[3] == (byte)49)
                            {
                                setHT_LabelTextAsync("ServiceHome", "o", false);
                                setHT_LabelTextAsync("ServiceGuest", string.Empty, false);
                            }
                            else if ((byte)str[3] == (byte)50)
                            {
                                setHT_LabelTextAsync("ServiceHome", string.Empty, false);
                                setHT_LabelTextAsync("ServiceGuest", "o", false);
                            }
                            else
                            {
                                setHT_LabelTextAsync("ServiceHome", string.Empty, false);
                                setHT_LabelTextAsync("ServiceGuest", string.Empty, false);
                            }
                            setHT_LabelTextAsync("ScoreHome", str.Substring(8, 3), false);
                            setHT_LabelTextAsync("ScoreGuest", str.Substring(11, 3), false);
                            setHT_LabelTextAsync("TeamFoulsHome", str.Substring(15, 1), (byte)str[15] > (byte)52);
                            setHT_LabelTextAsync("TeamFoulsGuest", str.Substring(16, 1), (byte)str[16] > (byte)52);
                            if (str.Substring(17, 1) != " ")
                                setHT_LabelTextAsync("TimeOutsHome", "".PadLeft(Convert.ToInt32(str.Substring(17, 1)), 'o'), false);
                            else
                                setHT_LabelTextAsync("TimeOutsHome", string.Empty, false);
                            if (str.Substring(18, 1) != " ")
                                setHT_LabelTextAsync("TimeOutsGuest", "".PadLeft(Convert.ToInt32(str.Substring(18, 1)), 'o'), false);
                            else
                                setHT_LabelTextAsync("TimeOutsGuest", string.Empty, false);
                            for (int index = 0; index < 12; ++index)
                            {
                                setHT_LabelTextAsync("FoulsH" + (index + 1).ToString(), string.Empty + (object)(char)((uint)str[22 + index] | 48U), str[22 + index] > '4');
                                setHT_LabelTextAsync("FoulsG" + (index + 1).ToString(), string.Empty + (object)(char)((uint)str[34 + index] | 48U), str[34 + index] > '4');
                            }
                            setHT_LabelTextAsync("TimeoutTimeHome", str.Substring(46, 2), false);
                            setHT_LabelTextAsync("ShotTime", str.Substring(48, 2), false);
                            setLabelTextAsync(SC_Horn, str[50] == '1' ? "On" : "Off");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    case 53:
                        if (_sport == Sportart.Football)
                        {
                            if (str.Substring(14, 1) != " ")
                            {
                                if (Convert.ToInt32(str.Substring(14, 1)) > 0)
                                    setHT_LabelTextAsync("TimeOutsHome", "".PadLeft(Convert.ToInt32(str.Substring(14, 1)), 'o'), false);
                                else
                                    setHT_LabelTextAsync("TimeOutsHome", "", false);
                            }
                            else
                                setHT_LabelTextAsync("TimeOutsHome", "", false);
                            if (str.Substring(15, 1) != " ")
                            {
                                if (Convert.ToInt32(str.Substring(15, 1)) > 0)
                                    setHT_LabelTextAsync("TimeOutsGuest", "".PadLeft(Convert.ToInt32(str.Substring(15, 1)), 'o'), false);
                                else
                                    setHT_LabelTextAsync("TimeOutsGuest", "", false);
                            }
                            else
                                setHT_LabelTextAsync("TimeOutsGuest", "", false);
                            setHT_LabelTextAsync("ScoreHome", str.Substring(9, 2), false);
                            setHT_LabelTextAsync("ScoreGuest", str.Substring(12, 2), false);
                            setHT_LabelTextAsync("ShotTime", str.Substring(8, 1) + str.Substring(11, 1), false);
                            setHT_LabelTextAsync("ToGo", str.Substring(2, 2), false);
                            setHT_LabelTextAsync("Down", str.Substring(16, 1), false);
                            if (str.Substring(18, 1) == "0")
                            {
                                setHT_LabelTextAsync("ServiceHome", "o", false);
                                setHT_LabelTextAsync("ServiceGuest", "", false);
                            }
                            if (!(str.Substring(18, 1) == "1"))
                                break;
                            setHT_LabelTextAsync("ServiceHome", "", false);
                            setHT_LabelTextAsync("ServiceGuest", "o", false);
                            break;
                        }
                        setLabelTextAsync(Horn, str[19] == '1' ? "On" : "Off");
                        if (str.Substring(17, 1) != " ")
                            setHT_LabelTextAsync("TimeOutsHome", "".PadLeft(Convert.ToInt32(str.Substring(17, 1)), 'o'), false);
                        else
                            setHT_LabelTextAsync("TimeOutsHome", "", false);
                        if (str.Substring(18, 1) != " ")
                            setHT_LabelTextAsync("TimeOutsGuest", "".PadLeft(Convert.ToInt32(str.Substring(18, 1)), 'o'), false);
                        else
                            setHT_LabelTextAsync("TimeOutsGuest", "", false);
                        setHT_LabelTextAsync("ScoreHome", str.Substring(8, 3), false);
                        setHT_LabelTextAsync("ScoreGuest", str.Substring(11, 3), false);
                        if (str.Substring(22, 1) != " ")
                            setHT_LabelTextAsync("PenaltyTimeH1", str.Substring(22, 1) + ":" + str.Substring(23, 2), false);
                        else
                            setHT_LabelTextAsync("PenaltyTimeH1", str.Substring(23, 2), false);
                        if (str.Substring(25, 1) != " ")
                            setHT_LabelTextAsync("PenaltyTimeH2", str.Substring(25, 1) + ":" + str.Substring(26, 2), false);
                        else
                            setHT_LabelTextAsync("PenaltyTimeH2", str.Substring(26, 2), false);
                        if (str.Substring(28, 1) != " ")
                            setHT_LabelTextAsync("PenaltyTimeH3", str.Substring(28, 1) + ":" + str.Substring(29, 2), false);
                        else
                            setHT_LabelTextAsync("PenaltyTimeH3", str.Substring(29, 2), false);
                        if (str.Substring(35, 1) != " ")
                            setHT_LabelTextAsync("PenaltyTimeG1", str.Substring(35, 1) + ":" + str.Substring(36, 2), false);
                        else
                            setHT_LabelTextAsync("PenaltyTimeG1", str.Substring(36, 2), false);
                        if (str.Substring(38, 1) != " ")
                            setHT_LabelTextAsync("PenaltyTimeG2", str.Substring(38, 1) + ":" + str.Substring(39, 2), false);
                        else
                            setHT_LabelTextAsync("PenaltyTimeG2", str.Substring(39, 2), false);
                        if (str.Substring(41, 1) != " ")
                        {
                            setHT_LabelTextAsync("PenaltyTimeG3", str.Substring(41, 1) + ":" + str.Substring(42, 2), false);
                            break;
                        }
                        setHT_LabelTextAsync("PenaltyTimeG3", str.Substring(42, 2), false);
                        break;
                    case 54:
                        setLabelTextAsync(Horn, str[19] == '1' ? "On" : "Off");
                        setHT_LabelTextAsync("SetsWonHome", str.Substring(15, 1), false);
                        setHT_LabelTextAsync("SetsWonGuest", str.Substring(16, 1), false);
                        setHT_LabelTextAsync("ScoreHome", str.Substring(9, 2), false);
                        setHT_LabelTextAsync("ScoreGuest", str.Substring(12, 2), false);
                        if (str.Substring(17, 1) != " ")
                            setHT_LabelTextAsync("TimeoutsHome", "".PadLeft(Convert.ToInt32(str.Substring(17, 1)), 'o'), false);
                        else
                            setHT_LabelTextAsync("TimeoutsHome", string.Empty, false);
                        if (str.Substring(18, 1) != " ")
                            setHT_LabelTextAsync("TimeoutsGuest", "".PadLeft(Convert.ToInt32(str.Substring(18, 1)), 'o'), false);
                        else
                            setHT_LabelTextAsync("TimeoutsGuest", string.Empty, false);
                        for (int index = 0; index < 4; ++index)
                        {
                            setHT_LabelTextAsync("SetScoreHome" + (index + 1).ToString(), str.Substring(24 + 4 * index, 2), false);
                            setHT_LabelTextAsync("SetScoreGuest" + (index + 1).ToString(), str.Substring(26 + 4 * index, 2), false);
                        }
                        break;
                    case 55:
                        try
                        {
                            setLabelTextAsync(Horn, (byte)str[19] == (byte)49 ? "On" : "Off");
                            for (int index = 0; index < 13; ++index)
                            {
                                if (str.Substring(22 + index * 2, 2).Trim() != string.Empty)
                                    setHT_LabelTextAsync("PointsG" + (index + 1).ToString(), str.Substring(22 + index * 2, 2), false);
                                else
                                    setHT_LabelTextAsync("PointsG" + (index + 1).ToString(), "0", false);
                            }
                            setHT_LabelTextAsync("PointsG14=" + str.Substring(11, 2), "0", false);
                            setHT_LabelTextAsync("ShotTime", str.Substring(48, 2), false);
                            setLabelTextAsync(SC_Horn, str[50] == '1' ? "On" : "Off");
                            break;
                        }
                        catch
                        {
                            break;
                        }
                    case 56:
                        try
                        {
                            setLabelTextAsync(Horn, str[19] == '1' ? "On" : "Off");
                            for (int index = 0; index < 13; ++index)
                            {
                                if (str.Substring(22 + index * 2, 2).Trim() != string.Empty)
                                    setHT_LabelTextAsync("PointsH" + (index + 1).ToString(), str.Substring(22 + index * 2, 2), false);
                                else
                                    setHT_LabelTextAsync("PointsH" + (index + 1).ToString(), "0", false);
                            }
                            setHT_LabelTextAsync("PointsH14=" + str.Substring(11, 2), "0", false);
                            setHT_LabelTextAsync("ShotTime", str.Substring(48, 2), false);
                            setLabelTextAsync(SC_Horn, str[50] == '1' ? "On" : "Off");
                            break;
                        }
                        catch
                        {
                            break;
                        }
                    case 98:
                        try
                        {
                            if (str[4] == ' ' && str[5] == ' ')
                            {
                                setHT_LabelTextAsync("TeamNameHome", str.Substring(6, 9), false);
                                setHT_LabelTextAsync("TeamNameGuest", str.Substring(18, 9), false);
                                break;
                            }
                            setHT_LabelTextAsync("NoG" + Convert.ToInt16(str.Substring(4, 2)).ToString(), str.Substring(16, 2), false);
                            setHT_LabelTextAsync("NameG" + Convert.ToInt16(str.Substring(4, 2)).ToString(), str.Substring(6, 10).Trim(), false);
                            break;
                        }
                        catch
                        {
                            break;
                        }
                    case 119:
                        try
                        {
                            setHT_LabelTextAsync("NoH" + Convert.ToInt16(str.Substring(4, 2)).ToString(), str.Substring(16, 2), false);
                            setHT_LabelTextAsync("NameH" + Convert.ToInt16(str.Substring(4, 2)).ToString(), str.Substring(6, 10).Trim(), false);
                            break;
                        }
                        catch
                        {
                            break;
                        }
                }
            }
            catch
            {
            }
        }

        private void EvaluateWigeData(string Buffer)
        {
            string key1 = Buffer.Substring(2, 6);
            if (key1.EndsWith("62"))
                key1 = Buffer.Substring(2, 4) + "00";
            if ((_loaded_layout_name == "Basketball" || _loaded_layout_name == "Volleyball") && Buffer[5] == 'B')
                key1 = Buffer.Substring(2, 4) + "04";
            int startIndex = 8;
            string empty = string.Empty;
            if (Buffer.Length <= 1 || !_WIGE_field_parameter.ContainsKey(key1))
                return;
            LED_Board.WIGE_Parameter wigeParameter = _WIGE_field_parameter[key1];
            int length1 = wigeParameter.Length;
            switch (wigeParameter.Type)
            {
                case LED_Board.WIGE_FieldType.ASCII:
                    while (Buffer.Length > startIndex + length1)
                    {
                        if (_WIGE_field_parameter.ContainsKey(key1))
                        {
                            if (key1.StartsWith("03") || key1.StartsWith("04") || wigeParameter.Name == "ActFoulPlayer")
                                Console.WriteLine(Buffer);
                            string msg = Buffer.Substring(startIndex, length1);
                            if (wigeParameter.Name == "GameTime")
                            {
                                msg = msg[3] != ' ' ? msg.Substring(0, 2) + ":" + msg.Substring(2, 2) : msg.Substring(0, 2) + "." + msg.Substring(2, 1) + " ";
                                if (msg[0] == '0')
                                    msg = string.Empty + (object)' ' + msg.Substring(1);
                            }
                            else if (!(wigeParameter.Name == "ActFoulPlayer"))
                            {
                                if (wigeParameter.Name.StartsWith("PenaltyTime"))
                                {
                                    if (!msg.StartsWith(" "))
                                        msg = msg.Substring(0, 1) + ":" + msg.Substring(1);
                                }
                                else if (wigeParameter.Name == "TimeoutTime")
                                {
                                    if (msg.StartsWith("  "))
                                    {
                                        msg = Convert.ToInt32(msg).ToString();
                                        if (msg == "0")
                                            msg = string.Empty;
                                    }
                                    else
                                        msg = msg.Substring(0, 2) + ":" + msg.Substring(2);
                                }
                            }
                            bool alternate_color = wigeParameter.Name.StartsWith("TeamFouls") && msg == " 5";
                            setHT_LabelTextAsync(wigeParameter.Name, msg, alternate_color);
                        }
                        startIndex += length1;
                        key1 = Buffer.Substring(2, 4) + (wigeParameter.CharacterAdress + wigeParameter.Length).ToString("00");
                        if (_WIGE_field_parameter.ContainsKey(key1))
                        {
                            wigeParameter = _WIGE_field_parameter[key1];
                            length1 = wigeParameter.Length;
                        }
                    }
                    break;
                case LED_Board.WIGE_FieldType.Bin:
                    for (int index = 4; index < 16; ++index)
                    {
                        string key2 = Buffer.Substring(2, 4) + index.ToString("00");
                        if (_WIGE_field_parameter.ContainsKey(key2))
                        {
                            wigeParameter = _WIGE_field_parameter[key2];
                            int length2 = wigeParameter.Length;
                        }
                        if (_WIGE_field_parameter.ContainsKey(key2))
                        {
                            string msg = string.Empty;
                            switch ((byte)Buffer[7 + index])
                            {
                                case 33:
                                    msg = "o";
                                    break;
                                case 35:
                                    msg = "oo";
                                    break;
                                case 36:
                                    msg = "o";
                                    break;
                                case 39:
                                    msg = "ooo";
                                    break;
                                case 40:
                                    msg = "o";
                                    break;
                                case 47:
                                    msg = "oooo";
                                    break;
                                case 95:
                                    msg = "ooooo";
                                    break;
                            }
                            bool alternate_color = false;
                            if (msg == "ooooo")
                                alternate_color = true;
                            setHT_LabelTextAsync(wigeParameter.Name, msg, alternate_color);
                        }
                    }
                    break;
                case LED_Board.WIGE_FieldType.Dot:
                    byte num1 = (byte)((uint)Buffer[9] & 15U);
                    if (Buffer[9] > '9')
                        num1 = (byte)((uint)Buffer[9] - 55U);
                    byte num2 = (byte)((uint)Buffer[8] & 15U);
                    if (Buffer[8] > '9')
                        num2 = (byte)((uint)Buffer[8] - 55U);
                    byte num3 = (byte)((uint)(byte)((uint)num2 << 4) | (uint)num1);
                    int num4 = 0;
                    while (num4 < 8 && _WIGE_field_parameter.ContainsKey(key1))
                    {
                        string msg = string.Empty;
                        for (int index = 0; index < length1; ++index)
                        {
                            ++num4;
                            msg = ((int)num3 & 1) <= 0 ? msg + " " : msg + "o";
                            num3 >>= 1;
                        }
                        if (wigeParameter.Name == "ServiceGuest" && msg.Trim() == string.Empty)
                            Console.WriteLine();
                        setLabelTextAsync(_fields[wigeParameter.Name].MyLabel, msg);
                        key1 = Buffer.Substring(2, 4) + num4.ToString("00");
                        if (_WIGE_field_parameter.ContainsKey(key1))
                        {
                            wigeParameter = _WIGE_field_parameter[key1];
                            length1 = wigeParameter.Length;
                        }
                    }
                    break;
            }
        }

        private void setLabel(string target_name, string msg, bool alternate_color)
        {
            if (!_fields.ContainsKey(target_name))
                return;
            _fields[target_name].MyLabel.Text = msg;
            _fields[target_name].MyLabel.ForeColor = alternate_color ? _fields[target_name].AlternateColor : _fields[target_name].NormalColor;
        }

        private Bitmap _make_score_screenshot()
        {
            Bitmap bitmap;
            Point point;
            if (_graphic_in_window)
            {
                bitmap = new Bitmap(_graphic_size.Width, _graphic_size.Height);
                point = new Point(Left + _graphic_location.X, Top + _graphic_location.Y);
            }
            else
            {
                bitmap = new Bitmap(Width, Height);
                point = new Point(Left, Top);
            }
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.CopyFromScreen(point.X, point.Y, 0, 0, bitmap.Size);
            graphics.Dispose();
            if (pictureBox1 != null)
                pictureBox1.Image = (Image)bitmap;
            return bitmap;
        }

        public void ShowTextAsScreenshot(
          TextBox Source,
          Point SourceLocation,
          string BackgroundFileName,
          bool ShowInWindow,
          int EffectIndex)
        {
            Bitmap bitmap1 = new Bitmap(Source.Width - 8, Source.Height - 8);
            Graphics graphics = Graphics.FromImage((Image)bitmap1);
            if (File.Exists(BackgroundFileName))
                graphics.DrawImage(Image.FromFile(BackgroundFileName), new Point(0, 0));
            graphics.CopyFromScreen(SourceLocation.X, SourceLocation.Y, 0, 0, bitmap1.Size);
            graphics.Dispose();
            Bitmap bitmap2 = new Bitmap((Image)bitmap1, Size);
            _actual_effect_index = EffectIndex <= -1 ? 0 : EffectIndex;
            ShowGraphic((Image)bitmap2, ShowInWindow, _actual_effect_index);
        }

        private void LED_Board_Resize(object sender, EventArgs e)
        {
        }

        private void LED_Board_Load(object sender, EventArgs e)
        {
            Clear();
        }

        private void InitializeComponent()
        {
            SC_Horn = new Label();
            Horn = new Label();
            pictureBox1 = new PictureBox();
            _background_pnl = new Panel();
            ((ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            SC_Horn.AutoSize = true;
            SC_Horn.ForeColor = Color.FromArgb(0, 192, 0);
            SC_Horn.Location = new Point(12, 244);
            SC_Horn.Name = "SC_Horn";
            SC_Horn.Size = new Size(21, 13);
            SC_Horn.TabIndex = 5;
            SC_Horn.Text = "Off";
            SC_Horn.Visible = false;
            SC_Horn.TextChanged += new EventHandler(Horn_TextChanged);
            Horn.AutoSize = true;
            Horn.ForeColor = Color.FromArgb(0, 192, 0);
            Horn.Location = new Point(358, 244);
            Horn.Name = "Horn";
            Horn.Size = new Size(21, 13);
            Horn.TabIndex = 4;
            Horn.Text = "Off";
            Horn.Visible = false;
            Horn.TextChanged += new EventHandler(Horn_TextChanged);
            pictureBox1.BackColor = Color.Black;
            pictureBox1.ErrorImage = (Image)null;
            pictureBox1.Location = new Point(12, 13);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(231, 199);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            _background_pnl.Location = new Point(2, 3);
            _background_pnl.Name = "_background_pnl";
            _background_pnl.Size = new Size(232, 209);
            _background_pnl.TabIndex = 7;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(391, 333);
            Controls.Add((Control)SC_Horn);
            Controls.Add((Control)Horn);
            Controls.Add((Control)_background_pnl);
            Controls.Add((Control)pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Name = nameof(LED_Board);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
            Load += new EventHandler(LED_Board_Load);
            FormClosing += new FormClosingEventHandler(LED_Board_FormClosing);
            Resize += new EventHandler(LED_Board_Resize);
            ((ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public Panel BackgroundPanel
        {
            get
            {
                return _background_pnl;
            }
            set
            {
                _background_pnl = value;
            }
        }

        public enum WIGE_FieldType
        {
            ASCII = 65, // 0x00000041
            Bin = 66, // 0x00000042
            Dot = 68, // 0x00000044
        }

        public class WIGE_Parameter
        {
            private string _name = string.Empty;
            private int _device_adress = -1;
            private int _character_adress = -1;
            private LED_Board.WIGE_FieldType _type = LED_Board.WIGE_FieldType.ASCII;
            private string _identstring = string.Empty;
            private int _length;

            public string Name
            {
                get
                {
                    return _name;
                }
            }

            public int DeviceAdress
            {
                get
                {
                    return _device_adress;
                }
            }

            public int CharacterAdress
            {
                get
                {
                    return _character_adress;
                }
            }

            public LED_Board.WIGE_FieldType Type
            {
                get
                {
                    return _type;
                }
            }

            public int Length
            {
                get
                {
                    return _length;
                }
            }

            public string IdentString
            {
                get
                {
                    return _identstring;
                }
            }

            public WIGE_Parameter(string Name, string ParameterString, int Length)
            {
                _name = Name;
                _length = Length;
                string[] strArray = ParameterString.Split(';');
                _device_adress = Convert.ToInt32(strArray[0]);
                _character_adress = Convert.ToInt32(strArray[1]);
                switch (strArray[2])
                {
                    case "A":
                        _type = LED_Board.WIGE_FieldType.ASCII;
                        break;
                    case "D":
                        _type = LED_Board.WIGE_FieldType.Dot;
                        break;
                    case "B":
                        _type = LED_Board.WIGE_FieldType.Bin;
                        break;
                }
                _identstring = _device_adress.ToString("00") + "I" + strArray[2] + _character_adress.ToString("00");
            }
        }

        public enum DisplayState
        {
            Cleared,
            Score,
        }

        public delegate void GraphicAnimationDetectedDelegate();

        public delegate void SequenceRequestDelegate(string SequenceName, bool InWindow);

        public delegate void MciElapsedDelegate();

        public delegate void PercentCompletedDelegate(int value);

        public delegate void LayoutCompletedDelegate();

        private delegate void createVideoAsyncDelegate(PictureBox target, string filename);

        private delegate void updatePanelAsyncDelegate(Panel target);

        private delegate void setLabelTextAsyncDelegate(Label target, string msg);

        public delegate void setLabelImageAsyncDelegate(Panel target, string controlname, Bitmap img);

        public delegate void setPanelImageAsyncDelegate(
          Panel target,
          string controlname_home,
          Bitmap img_home,
          string controlname_guest,
          Bitmap img_guest);

        public delegate void addPictureBoxAsyncDelegate(Form target, PictureBox picturebox);

        public delegate void resizePictureBoxAsyncDelegate(PictureBox picturebox);

        public delegate void removeControlAsyncDelegate(Form target, Control control);

        public delegate PictureBox getPictureboxAsyncDelegate(PictureBox picturebox);

        public delegate void hidePictureBoxAsyncDelegate(PictureBox picturebox);

        public delegate void showPictureBoxAsyncDelegate(PictureBox picturebox);

        public delegate void hidePanelAsyncDelegate(Panel panel);

        public delegate void showPanelAsyncDelegate(Panel panel);

        public delegate void resizePanelAsyncDelegate(Panel panel, Point location, Size size);

        public delegate void showLabelAsyncDelegate(Label textBox, Point location, Size size);

        public delegate void showTextBoxAsyncDelegate(TextBox textBox, Point location, Size size);

        public delegate void hideTextBoxAsyncDelegate(TextBox textBox);

        public delegate void hideLabelAsyncDelegate(Label textBox);

        public delegate void clearScoreboardAsyncDelegate(Form target, PictureBox picturebox);

        public delegate void setHT_LabelTextAsyncDelegate(string target_name, string msg, bool alternate_color);

        public delegate void setHT_ControlTextAsyncDelegate(string[] text);

        public delegate void loadLayoutAsyncDelegate(Panel target, string layoutname);
    }
}
