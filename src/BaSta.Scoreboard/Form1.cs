using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using BaSta.Scoreboard.Properties;

namespace BaSta.Scoreboard
{
    public class Form1 : Form
    {
        public static int UseBasketHorn = 1;
        private string _program_version = "20130829";
        private int _remote_port = 8100;
        private string _remote_directory = string.Empty;
        private Rectangle _my_rect = new Rectangle(0, 0, 1, 1);
        private InvokeFunctions _invokes = new InvokeFunctions();
        private List<PictureBox> _pictureboxes = new List<PictureBox>();
        private List<string> _file_list = new List<string>();
        private List<string> _pictures = new List<string>();
        private Size _default_preview_size = new Size(48, 36);
        private Point _default_preview_startpoint = new Point(16, 16);
        private int _default_preview_dist = 10;
        private Point _in_location = new Point(0, 0);
        private Point _preview_position = new Point(10, 50);
        private Size _small_preview_size = new Size(60, 45);
        private Size _large_preview_size = new Size(96, 72);
        private string _sequence_path = Application.StartupPath + "\\Sequences";
        private string _media_path = Application.StartupPath + "\\Media";
        private string _picture_path = string.Empty;
        private string _animation_path = string.Empty;
        private string _ppt_path = string.Empty;
        private List<string> _animation_filenames = new List<string>();
        private List<Sequence> _sequences = new List<Sequence>();
        private List<string> _ppt_filenames = new List<string>();
        private bool _lock_secrets = true;
        private Bitmap _first_video_frame = new Bitmap(512, 324);
        private SoundPlayer _gong = new SoundPlayer();
        private SoundPlayer _extra_sound = new SoundPlayer();
        private string _soccer_background_picture_directory = Application.StartupPath + "\\SoccerBackgroundImages";
        private string _background_image_name_2save = string.Empty;
        private string _sport = string.Empty;
        private UdpReceiver _remote_receiver;
        private UDP_Sender _remote_sender;
        private int _max_previews_x;
        private int _max_previews_y;
        private LED_Board _scb;
        private Size _graphic_size;
        private Point _graphic_location;
        private Size _cfg_graphic_size;
        private Point _cfg_graphic_location;
        private Sequence _temp_sequence;
        private int _sequencepart_pointer;
        private System.Windows.Forms.Timer _sequence_timer;
        private long _sequence_length;
        private bool _endless;
        private bool _singlestep;
        private int _default_sequence_effect_index;
        private Mci _animation_preview_mci;
        private OpenFileDialog _new_horn_signal;
        private int _snapshot_counter;
        private IContainer components;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button btnClearSCB;
        private Button btnShowScore;
        private Button btnShowVideo;
        private RadioButton rbSmallGraphic;
        private RadioButton rbFullGraphic;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem tsmNewSequence;
        private ToolStripMenuItem tsmPlaySequence;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmDeleteSequence;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem tsmAddNewPictureToSequence;
        private ToolStripMenuItem tsmMoveUpMediaInSequence;
        private ToolStripMenuItem tsmMoveDownMediaInSequence;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem tsmDeleteMediaFromSequence;
        private ToolStripMenuItem tsmAddAnimation;
        private OpenFileDialog ofdPictures;
        private OpenFileDialog ofdAnimations;
        private ToolStripMenuItem tmsPlaySequenceEndless;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private GroupBox groupBox1;
        private ListBox lbAnimations;
        private ListBox lbSequenceContent;
        private Panel pnlVideoPreview;
        private Label label1;
        private TextBox txtInputNewSequenceName;
        private PictureBox pbAnimationPreview;
        private Button btnSelectPicturePath;
        private Button btnSelectAnimationPath;
        private Button btnShowSingleStep;
        private FolderBrowserDialog folderBrowserDialog2;
        private Button btnShowAnimation;
        private Label label2;
        private Label label3;
        private ComboBox cmbEffect;
        private ProgressBar progressBar1;
        private Label label4;
        private ComboBox cmbSelectKindOfGame;
        private ComboBox cmbVideoSource;
        private Label label5;
        private Button btnFreeText;
        private ContextMenuStrip contextMenuStrip3;
        private ToolStripMenuItem stretchVideoToDisplaySizeToolStripMenuItem;
        private CheckBox cbAllowGraphicFromLAN;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button btnShowPPT;
        private Button btnSelectPPT_Path;
        private ListBox lbAvailablePPTs;
        private Button btnGong;
        private ContextMenuStrip contextMenuStrip4;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem backgoundImagesToolStripMenuItem;
        private ToolStripMenuItem soccerToolStripMenuItem1;
        private ToolStripMenuItem soccerToolStripMenuItem;
        private ToolStripMenuItem changeInToolStripMenuItem;
        private ToolStripMenuItem changeOutToolStripMenuItem;
        private ToolStripMenuItem redCardToolStripMenuItem;
        private ToolStripMenuItem yellowCardToolStripMenuItem;
        private ToolStripMenuItem goalHomeToolStripMenuItem;
        private ToolStripMenuItem scoreToolStripMenuItem;
        private ToolStripMenuItem introRefereesToolStripMenuItem;
        private ToolStripMenuItem introSinglePlayerToolStripMenuItem;
        private ToolStripMenuItem introGuestteamToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private OpenFileDialog openFileDialog1;
        private Button btnRefreshPPT_List;
        private ToolStripMenuItem introCoachesHomeTeamToolStripMenuItem;
        private ToolStripMenuItem introCoachesGuestTeamToolStripMenuItem;
        private Button btnVideoSnapShot;
        private GroupBox groupBox2;
        private ToolStripMenuItem goalGuestToolStripMenuItem;
        private OpenFileDialog openFileDialog2;
        private ToolStripMenuItem yellowredCardToolStripMenuItem;
        private ToolStripMenuItem insertScoreDisplayToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem flashtableToolStripMenuItem;
        private ToolStripMenuItem resultTableToolStripMenuItem;
        private ToolStripMenuItem resultToolStripMenuItem;
        private ComboBox cmbAvailableSequences;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem bildAusMedienverzeichnisHinzufügenToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem gametimeInsertParameterToolStripMenuItem;
        private ToolStripMenuItem fontToolStripMenuItem;
        private ToolStripMenuItem colorToolStripMenuItem;
        private ToolStripMenuItem locationToolStripMenuItem;
        private FontDialog fontDialog1;
        private ColorDialog colorDialog1;
        private ToolStripMenuItem insertGametimeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private Panel gbTimeInsertPosition;
        private NumericUpDown nudTimeInsertPositionX;
        private Button btnTimeInsertPositionSave;
        private Label label6;
        private Button btnTimeInsertPositionCancel;
        private NumericUpDown nudTimeInsertPositionY;
        private Label label7;
        private Label label8;
        private ToolStripMenuItem cornersToolStripMenuItem;
        private Panel gbDaytimeInsertPosition;
        private Label label9;
        private NumericUpDown nudDayTimeInsertPositionX;
        private Button btnDaytimeInsertPositionSave;
        private Button btnDaytimeInsertPositionCancel;
        private NumericUpDown nudDayTimeInsertPositionY;
        private Label label10;
        private Label label11;
        private ToolStripMenuItem daytimeInsertParameterToolStripMenuItem;
        private ToolStripMenuItem insertDaytimeToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem fontDayTimeToolStripMenuItem;
        private ToolStripMenuItem colorDayTimeToolStripMenuItem;
        private ToolStripMenuItem locationDayTimeToolStripMenuItem;
        private Label lblDayTime;
        private System.Windows.Forms.Timer timerDayTime;
        private ToolTip toolTip1;
        private ToolStripMenuItem scorerToolStripMenuItem;
        private ToolStripMenuItem introHomeReserveToolStripMenuItem;
        private ToolStripMenuItem introGuestReserveToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem2;
        private Panel panel1;
        private ContextMenuStrip contextMenuStrip5;
        private ToolStripMenuItem insertAnimationToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip6;
        private ToolStripMenuItem insertPictureToolStripMenuItem3;
        private Label lblProgramVersion;
        private Label lblSelectedAnimationName;

        public void showGraphicAsync(
          LED_Board target,
          string filename,
          bool in_window,
          int effect_index,
          bool act_graphic_from_score)
        {
            if (target.InvokeRequired)
                target.Invoke((Delegate)new Form1.showGraphicAsyncDelegate(showGraphicAsync), (object)target, (object)filename, (object)in_window, (object)effect_index, (object)act_graphic_from_score);
            else
                target.ShowGraphic(filename, in_window, effect_index, act_graphic_from_score);
        }

        public void startSequenceAsync(Form target, string sequencename, bool inwindow)
        {
            if (target.InvokeRequired)
            {
                target.Invoke((Delegate)new Form1.startSequenceAsyncDelegate(startSequenceAsync), (object)target, (object)sequencename, (object)inwindow);
            }
            else
            {
                rbSmallGraphic.Checked = inwindow;
                int index1 = -1;
                for (int index2 = 0; index2 < cmbAvailableSequences.Items.Count; ++index2)
                {
                    if (cmbAvailableSequences.Items[index2].ToString() == sequencename)
                        index1 = index2;
                }
                if (index1 > -1)
                {
                    _endless = false;
                    _singlestep = false;
                    _temp_sequence = _sequences[index1];
                    _sequencepart_pointer = 0;
                    _send_remote_command("SHOW_SEQUENCE=" + cmbAvailableSequences.SelectedIndex.ToString());
                    _start_sequencepart();
                }
                else
                    btnShowScore_Click((object)this, (EventArgs)null);
            }
        }

        public string SequencePath
        {
            get
            {
                return _sequence_path;
            }
        }

        public Form1()
        {
            InitializeComponent();
            UseBasketHorn = 1;
            lblProgramVersion.Text = "Programmversion " + _program_version;
            _program_version = "$Version: 03.15 $ ($VersDate: 2016-APR-11 $, $VersTime: 15:32 $)";
            _program_version = _program_version.Replace(" $", "");
            _program_version = _program_version.Replace("$", "");
            _program_version = _program_version.Replace("Version: ", "");
            _program_version = _program_version.Replace("VersDate: ", "");
            _program_version = _program_version.Replace("VersTime: ", "");
            lblProgramVersion.Text = "Programmversion " + _program_version;
            _picture_path = Settings.Default.SelectedPicturePath;
            _animation_path = Settings.Default.SelectedAnimationPath;
            _ppt_path = Settings.Default.SelectedPPT_Path;
            insertGametimeToolStripMenuItem.Checked = Settings.Default.DoInsertgameTime;
            _graphic_size = new Size(357, 206);
            _graphic_location = new Point(78, 130);
            Size size1 = Settings.Default.SCB_Size;
            Point point1 = new Point(0, 0);
            _first_video_frame = new Bitmap(size1.Width, size1.Height);
            Size size2 = new Size(1024, 768);
            Point point2 = new Point(1024, 0);
            Show();
            Update();
            if (cmbSelectKindOfGame.Visible)
                cmbSelectKindOfGame.SelectedIndex = Settings.Default.LastSelectedKindOfGameIndex;
            _graphic_size = size1;
            _graphic_location = new Point(0, 0);
            if (!File.Exists(Application.StartupPath + "\\Scoreboards.cfg"))
                _reset_scoreboards_cfg(Application.StartupPath + "\\Scoreboards.cfg");
            if (File.Exists(Application.StartupPath + "\\Scoreboards.cfg"))
            {
                StreamReader streamReader = new StreamReader(Application.StartupPath + "\\Scoreboards.cfg");
                for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                {
                    if (str.StartsWith("STRAMATEL-TV-KONSOLE") || str.StartsWith("WIGE_MST2002"))
                    {
                        cmbSelectKindOfGame.Visible = true;
                        try
                        {
                            cmbSelectKindOfGame.SelectedIndex = Settings.Default.LastSelectedKindOfGameIndex;
                        }
                        catch
                        {
                            cmbSelectKindOfGame.SelectedIndex = 0;
                        }
                    }
                    if (!str.StartsWith(";") && !str.StartsWith("/") && !str.StartsWith("EXT_SCB_"))
                    {
                        string[] strArray1 = str.Split('=');
                        switch (strArray1[0].ToUpper())
                        {
                            case "SCB_WINDOW_SIZE":
                                try
                                {
                                    string[] strArray2 = strArray1[1].Split(',');
                                    size1 = new Size(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
                                    Settings.Default.SCB_Size = size1;
                                    Settings.Default.Save();
                                    _graphic_size = size1;
                                    Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            case "SCB_WINDOW_LOCATION":
                                try
                                {
                                    string[] strArray2 = strArray1[1].Split(',');
                                    point1 = new Point(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            case "GRAPHIC_WINDOW_SIZE":
                                try
                                {
                                    string[] strArray2 = strArray1[1].Split(',');
                                    _cfg_graphic_size = new Size(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            case "GRAPHIC_WINDOW_LOCATION":
                                try
                                {
                                    string[] strArray2 = strArray1[1].Split(',');
                                    _cfg_graphic_location = new Point(Convert.ToInt32(strArray2[0]), Convert.ToInt32(strArray2[1]));
                                    continue;
                                }
                                catch
                                {
                                    continue;
                                }
                            case "GRAPHIC_OVER_LAN":
                                cbAllowGraphicFromLAN.Visible = strArray1[1] == "ON";
                                continue;
                            case "SOCCER_BACKROUND_SETTINGS":
                                backgoundImagesToolStripMenuItem.Visible = strArray1[1] == "ON";
                                continue;
                            case "REMOTE_SEND_PORT":
                                if (_remote_receiver == null)
                                {
                                    _remote_port = Convert.ToInt32(strArray1[1]);
                                    _remote_sender = new UDP_Sender(string.Empty, _remote_port);
                                    if (!_remote_sender.IsConnected)
                                    {
                                        int num = (int)MessageBox.Show("Remote interface not connected!");
                                        continue;
                                    }
                                    continue;
                                }
                                continue;
                            case "REMOTE_DIRECTORY":
                                if (_remote_sender != null && _remote_sender.IsConnected)
                                {
                                    _remote_directory = strArray1[1];
                                    new Thread(new ThreadStart(_copy_remote_directories)).Start();
                                    continue;
                                }
                                continue;
                            case "REMOTE_RECEIVE_PORT":
                                if (_remote_sender == null)
                                {
                                    _remote_port = Convert.ToInt32(strArray1[1]);
                                    _remote_receiver = new UdpReceiver(_remote_port);
                                    _remote_receiver.DataReceived += _remote_receiver_DataReceived;
                                }
                                continue;
                            case "USE_BASKETHORN":
                                int num1;
                                try
                                {
                                    num1 = Convert.ToInt32(strArray1[1]);
                                }
                                catch
                                {
                                    num1 = 1;
                                }
                                Form1.UseBasketHorn = num1;
                                continue;
                            default:
                                continue;
                        }
                    }
                }
                streamReader.Close();
            }
            _scb = new LED_Board(_program_version, _soccer_background_picture_directory);
            if (_scb != null)
            {
                _scb.Size = size1;
                _scb.Location = point1;
                _scb.GraphicSize = _graphic_size;
                _scb.GraphicLocation = _graphic_location;
                _scb.PercentComplete += new LED_Board.PercentCompletedDelegate(_scb_PercentComplete);
                _scb.SequenceRequested += new LED_Board.SequenceRequestDelegate(_scb_SequenceRequested);
                _scb.GraphicAnimationDetected += new LED_Board.GraphicAnimationDetectedDelegate(_scb_GraphicAnimationDetected);
                _scb.Show();
                _scb.Update();
                _scb.MciElapsed += new LED_Board.MciElapsedDelegate(_scb_MciElapsed);
                _scb.LayoutComplete += new LED_Board.LayoutCompletedDelegate(_scb_LayoutComplete);
                _scb.AllowGraphicFromLAN = Settings.Default.AllowGraphicFromLAN;
                _scb.GraphicInWindow = !Settings.Default.ShowGraphicInFullscreen;
                _scb.SequencePath = _sequence_path;
                _scb.MediaPath = _media_path;
                _scb.CfgGraphicLocation = _cfg_graphic_location;
                _scb.CfgGraphicSize = _cfg_graphic_size;
                _scb.DayTimeInsertColor = Settings.Default.DayTimeInsertColor;
                _scb.DayTimeInsertFontName = Settings.Default.DayTimeInsertFont.Name;
                _scb.DayTimeInsertFontSize = (int)Settings.Default.DayTimeInsertFont.Size;
                _scb.DayTimeInsertFontStyle = Settings.Default.DayTimeInsertFont.Style;
                _scb.DayTimeInsertLocation = Settings.Default.DayTimeInsertLocation;
                _scb.InsertDayTime = Settings.Default.DoInsertDayTime;
                insertDaytimeToolStripMenuItem.Checked = Settings.Default.DoInsertDayTime;
                _scb.GameTimeInsertColor = Settings.Default.GameTimeInsertColor;
                _scb.GameTimeInsertFontName = Settings.Default.GameTimeInsertFont.Name;
                _scb.GameTimeInsertFontSize = (int)Settings.Default.GameTimeInsertFont.Size;
                _scb.GameTimeInsertFontStyle = Settings.Default.GameTimeInsertFont.Style;
                _scb.GameTimeInsertLocation = Settings.Default.GameTimeInsertLocation;
                _scb.InsertGameTime = Settings.Default.DoInsertgameTime;
                insertGametimeToolStripMenuItem.Checked = Settings.Default.DoInsertgameTime;
                _scb.resizePictureBoxAsync(_scb.Picturebox);
            }
            if (cbAllowGraphicFromLAN.Checked)
                cbAllowGraphicFromLAN.ForeColor = Color.Red;
            else
                cbAllowGraphicFromLAN.ForeColor = Color.White;
            Focus();
            cmbEffect.SelectedIndex = Settings.Default.LastSelectedEffect;
            if (cmbSelectKindOfGame.Text.Trim() != string.Empty)
                cmbSelectKindOfGame_SelectedIndexChanged((object)this, (EventArgs)null);
            if (!File.Exists(Application.StartupPath + "\\gong.wav"))
                return;
            _gong = new SoundPlayer(Application.StartupPath + "\\gong.wav");
        }

        private void _scb_GraphicAnimationDetected()
        {
            lblSelectedAnimationName.ForeColor = Color.Lime;
        }

        private void _scb_LayoutComplete()
        {
            btnShowScore_Click((object)this, (EventArgs)null);
        }

        private void _scb_SequenceRequested(string SequenceName, bool InWindow)
        {
            startSequenceAsync((Form)this, SequenceName, InWindow);
        }

        private Bitmap _load_layout_einzel(
          DataBaseFunctions dbFunc,
          string layoutname,
          int teamID,
          int playernumber,
          string playername,
          string act_game_minute,
          string score_home,
          string score_guest)
        {
            string path = Application.StartupPath + "\\" + layoutname + ".csv";
            Bitmap bitmap = (Bitmap)null;
            if (File.Exists(path))
            {
                Bitmap original1 = (Bitmap)null;
                Team team1 = dbFunc.ReadTeam(teamID);
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
                    }
                }
                StreamReader streamReader = new StreamReader(path);
                string[] strArray1 = streamReader.ReadLine().Split(';');
                Label feld = new Label();
                if (strArray1.Length < 7)
                {
                    bitmap = new Bitmap(Convert.ToInt32(strArray1[4]), Convert.ToInt32(strArray1[5]));
                    Graphics graphic = Graphics.FromImage((Image)bitmap);
                    if (File.Exists(Application.StartupPath + "//" + layoutname + ".jpg"))
                        graphic.DrawImage(Image.FromFile(Application.StartupPath + "//" + layoutname + ".jpg"), new Rectangle(0, 0, bitmap.Width, bitmap.Height));
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
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), playernumber.ToString(), feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "PlayerName":
                                DrawText(graphic, new Rectangle(feld.Left, feld.Top, feld.Width, feld.Height), playername, feld.Font, feld.ForeColor, alignment, linealignment);
                                break;
                            case "PlayerImage":
                                Bitmap original2 = dbFunc.GamePlayerImage(teamID, playernumber);
                                if (original2 != null)
                                {
                                    graphic.DrawImage((Image)original2, _scale_picture(original2, feld));
                                    break;
                                }
                                break;
                            case "TeamName":
                                Team team2 = dbFunc.ReadTeam(teamID);
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

        private Rectangle _scale_picture(Bitmap original, Label feld)
        {
            Rectangle rectangle = new Rectangle();
            if (original != null && feld != null)
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

        private void _copy_remote_directories()
        {
            if (!Directory.Exists(_remote_directory))
                return;
            _send_remote_command("CLEAR_SCB");
            _send_remote_command("CLEAR_PREVIEWS");
            _send_remote_command("CLEAR_SEQUENCES");
            _send_remote_command("CLEAR_ANIMATIONS");
            Thread.Sleep(1000);
            _copy_directory("Media", _remote_directory + "\\Media");
            _copy_directory("Sequences", _remote_directory + "\\Sequences");
            _copy_directory(Settings.Default.SelectedPicturePath, _remote_directory + "\\Pictures");
            _copy_directory(Settings.Default.SelectedAnimationPath, _remote_directory + "\\Animations");
            Thread.Sleep(1000);
            _send_remote_command("REMOTE_REQUESTED");
            _send_remote_command("EFFECT_SELECTED=" + cmbEffect.SelectedIndex.ToString());
            _send_remote_command("FULL_GRAPHIC=" + rbFullGraphic.Checked.ToString());
        }

        private void _copy_directory(string _source_directory, string _target_directory)
        {
            if ((int)_target_directory[_target_directory.Length - 1] != (int)Path.DirectorySeparatorChar)
                _target_directory += (string)(object)Path.DirectorySeparatorChar;
            if (!Directory.Exists(_target_directory))
                Directory.CreateDirectory(_target_directory);
            foreach (string fileSystemEntry in Directory.GetFileSystemEntries(_source_directory))
            {
                try
                {
                    if (!File.Exists(_target_directory + Path.GetFileName(fileSystemEntry)))
                        File.Copy(fileSystemEntry, _target_directory + Path.GetFileName(fileSystemEntry), true);
                }
                catch
                {
                }
            }
        }

        private void _remote_receiver_DataReceived(UdpReceiver sender, string Data)
        {
            string[] strArray = Data.Split('=');
            switch (strArray[0])
            {
                case "#####":
                    _remote_receiver.DataReceived -= _remote_receiver_DataReceived;
                    Form1_FormClosing(this, null);
                    Process.Start("shutdown.exe", "-s -t 00");
                    Close();
                    break;
                case "CLEAR_PREVIEWS":
                    _clear_previews();
                    break;
                case "CLEAR_ANIMATIONS":
                    _clear_animations();
                    break;
                case "CLEAR_SEQUENCES":
                    _clear_sequeces();
                    break;
                case "REMOTE_REQUESTED":
                    _picture_path = "c:\\Remote_SCB_Files\\Pictures";
                    if (!Directory.Exists(_picture_path))
                        Directory.CreateDirectory(_picture_path);
                    _fill_previews();
                    _animation_path = "c:\\Remote_SCB_Files\\Animations";
                    if (!Directory.Exists(_animation_path))
                        Directory.CreateDirectory(_animation_path);
                    _fill_animations();
                    _media_path = "c:\\Remote_SCB_Files\\Media";
                    if (!Directory.Exists(_media_path))
                        Directory.CreateDirectory(_media_path);
                    _sequence_path = "c:\\Remote_SCB_Files\\Sequences";
                    if (!Directory.Exists(_sequence_path))
                        Directory.CreateDirectory(_sequence_path);
                    _fill_sequences();
                    break;
                case "REMOTE_CLOSED":
                    _dispose_animation_preview();
                    _picture_path = Settings.Default.SelectedPicturePath;
                    if (!Directory.Exists(_picture_path))
                        Directory.CreateDirectory(_picture_path);
                    _fill_previews();
                    _animation_path = Settings.Default.SelectedAnimationPath;
                    if (!Directory.Exists(_animation_path))
                        Directory.CreateDirectory(_animation_path);
                    _fill_animations();
                    _media_path = Application.StartupPath + "\\Media";
                    if (!Directory.Exists(_media_path))
                        Directory.CreateDirectory(_media_path);
                    _sequence_path = Application.StartupPath + "\\Sequences";
                    if (!Directory.Exists(_sequence_path))
                        Directory.CreateDirectory(_sequence_path);
                    _fill_sequences();
                    cmbEffect.SelectedIndex = Settings.Default.LastSelectedEffect;
                    if (Settings.Default.ShowGraphicInFullscreen)
                    {
                        rbFullGraphic.Checked = true;
                        rbSmallGraphic.Checked = false;
                        break;
                    }
                    rbFullGraphic.Checked = false;
                    rbSmallGraphic.Checked = true;
                    break;
                case "EFFECT_SELECTED":
                    cmbEffect.SelectedIndex = Convert.ToInt32(strArray[1]);
                    break;
                case "FULL_GRAPHIC":
                    if (strArray[1].ToUpper() == "TRUE")
                    {
                        rbFullGraphic.Checked = true;
                        rbSmallGraphic.Checked = false;
                    }
                    else
                    {
                        rbFullGraphic.Checked = false;
                        rbSmallGraphic.Checked = true;
                    }
                    rbFullGraphic_Click((object)this, (EventArgs)null);
                    break;
                case "SHOW_PICTURE":
                    try
                    {
                        _temp_Click((object)groupBox1.Controls[strArray[1]], (EventArgs)null);
                        break;
                    }
                    catch
                    {
                        break;
                    }
                case "ANIMATION_SELECTED":
                    _invokes.setListboxIndexAsync(lbAnimations, Convert.ToInt32(strArray[1]));
                    break;
                case "SHOW_ANIMATION":
                    _dispose_animation_preview();
                    btnShowAnimation_Click_1((object)this, (EventArgs)null);
                    break;
                case "SHOW_SEQUENCE":
                    cmbAvailableSequences.SelectedIndex = Convert.ToInt32(strArray[1]);
                    tsmPlaySequence_Click((object)this, (EventArgs)null);
                    break;
                case "SHOW_SEQUENCE_ENDLESS":
                    cmbAvailableSequences.SelectedIndex = Convert.ToInt32(strArray[1]);
                    tmsPlaySequenceEndless_Click((object)this, (EventArgs)null);
                    break;
                case "SHOW_SEQUENCE_STEP":
                    cmbAvailableSequences.SelectedIndex = Convert.ToInt32(strArray[1]);
                    lbSequenceContent.SelectedIndex = Convert.ToInt32(strArray[2]);
                    btnShowSingleStep_Click((object)this, (EventArgs)null);
                    break;
                case "SHOW_SCORE":
                    btnShowScore_Click((object)this, (EventArgs)null);
                    break;
                case "SHOW_VIDEO":
                    btnShowVideo_Click((object)this, (EventArgs)null);
                    break;
                case "CLEAR_SCB":
                    btnClearSCB_Click((object)this, (EventArgs)null);
                    break;
            }
        }

        private void _scb_MciElapsed()
        {
            if (_sequence_timer != null)
            {
                _sequence_timer.Stop();
            }
            else
            {
                _sequence_timer = new System.Windows.Forms.Timer();
                _sequence_timer.Tick += new EventHandler(_sequence_timer_Tick);
            }
            _sequence_timer.Interval = 100;
            _sequence_length = 1L;
            _sequence_timer.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_scb != null)
            {
                _scb.CancelCurrentEffect();
                Settings.Default.DoInsertgameTime = _scb.InsertGameTime;
                Settings.Default.DayTimeInsertColor = _scb.DayTimeInsertColor;
                Settings.Default.DayTimeInsertFont = new Font(_scb.DayTimeInsertFontName, (float)_scb.DayTimeInsertFontSize, _scb.DayTimeInsertFontStyle);
                Settings.Default.DayTimeInsertLocation = _scb.DayTimeInsertLocation;
                Settings.Default.DoInsertDayTime = _scb.InsertDayTime;
                Settings.Default.GameTimeInsertColor = _scb.GameTimeInsertColor;
                Settings.Default.GameTimeInsertFont = new Font(_scb.GameTimeInsertFontName, (float)_scb.GameTimeInsertFontSize, _scb.GameTimeInsertFontStyle);
                Settings.Default.GameTimeInsertLocation = _scb.GameTimeInsertLocation;
            }
            _send_remote_command("REMOTE_CLOSED");
            Settings.Default.LastSelectedSequenceIndex = cmbAvailableSequences.SelectedIndex;
            Settings.Default.LastSelectedTabIndex = tabControl1.SelectedIndex;
            Settings.Default.LastSelectedEffect = cmbEffect.SelectedIndex;
            Settings.Default.Save();
            _dispose_animation_preview();
            if (_scb == null)
                return;
            _scb.Close();
        }

        private void _reset_scoreboards_cfg(string cfg_name)
        {
            StreamWriter streamWriter = new StreamWriter(cfg_name, false);
            streamWriter.WriteLine("//Kommentarzeilen NICHT entfernen oder verändern!");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Um Zeilen zu aktivieren führendes Semikolon entfernen");
            streamWriter.WriteLine("//Auf Verfügbarkeit der Schnittstellen achten.");
            streamWriter.WriteLine("//Um Zeilen zu deaktivieren Semikolon am Zeilenbeginn einfügen");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Position und Größe des Tafel-Fensters");
            streamWriter.WriteLine("SCB_WINDOW_LOCATION=0,0");
            streamWriter.WriteLine("SCB_WINDOW_SIZE=336,168");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Position und Größe des Grafik-Fensters");
            streamWriter.WriteLine("GRAPHIC_WINDOW_LOCATION=0,26");
            streamWriter.WriteLine("GRAPHIC_WINDOW_SIZE=336,142");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Position und Größe des oder der Zusatz-Fenster");
            streamWriter.WriteLine("//SCREEN=FullScreen,336,0,1024,768");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("SCB_TOPMOST=ON");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Zulassen der Bildübertragung, ON oder OFF");
            streamWriter.WriteLine("GRAPHIC_OVER_LAN=OFF");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Lautstärke für die Wiedergabe von Animationen, 0...1000");
            streamWriter.WriteLine("VIDEO_VOLUME=1000");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Fussballhintergrundeinstellungen sichtbar, ON oder OFF");
            streamWriter.WriteLine("SOCCER_BACKROUND_SETTINGS=OFF");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Port der LAN-Verbindung, default ist 8000");
            streamWriter.WriteLine("UDP_PORT=8000");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Externer Controller");
            streamWriter.WriteLine(";STRAMATEL-TV-KONSOLE=COM1");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Serielle Tafeln");
            streamWriter.WriteLine(";REFRESH_INTERVAL=50");
            streamWriter.WriteLine(";EXT_SCB_STRAMATEL=COM1");
            streamWriter.WriteLine(";EXT_SCB_STRAMATELWATER=COM1");
            streamWriter.WriteLine(";EXT_SCB_OMEGASATURN=COM1");
            streamWriter.WriteLine(";EXT_SCB_FAVERO=COM1");
            streamWriter.WriteLine(";EXT_SCB_VELOMAX=COM1");
            streamWriter.WriteLine(";EXT_SCB_MTVISUAL=COM1");
            streamWriter.WriteLine(";EXT_SCB_DEBUG=COM1");
            streamWriter.WriteLine(";EXT_SCB_HORN=COM1");
            streamWriter.WriteLine(";EXT_SCB_WIGE=COM1");
            streamWriter.WriteLine(";EXT_SCB_SCHAUF=COM1");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Für Basketball: Horn am Korb benutzen (1), oder aus (0)");
            streamWriter.WriteLine(";USE_BASKETHORN=1");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.WriteLine("//Remote-Einstellungen");
            streamWriter.WriteLine(";REMOTE_SEND_PORT=8100");
            streamWriter.WriteLine(";REMOTE_DIRECTORY=");
            streamWriter.WriteLine("REMOTE_RECEIVE_PORT=8100");
            streamWriter.WriteLine("//-----------------------------------------------------");
            streamWriter.Close();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                txtInputNewSequenceName.BackColor = Color.White;
                if (txtInputNewSequenceName.Focused)
                {
                    string str = txtInputNewSequenceName.Text.Trim();
                    if (!File.Exists(_sequence_path + "\\" + txtInputNewSequenceName.Text.Trim() + ".seq"))
                    {
                        _sequences.Add(new Sequence(_sequence_path, _sequence_path + "\\" + txtInputNewSequenceName.Text.Trim() + ".seq", _media_path));
                        txtInputNewSequenceName.Text = string.Empty;
                        _fill_sequences();
                        for (int index = 0; index < cmbAvailableSequences.Items.Count; ++index)
                        {
                            if (cmbAvailableSequences.Items[index].ToString().Trim() == str)
                                cmbAvailableSequences.SelectedIndex = index;
                        }
                        cmbAvailableSequences.Focus();
                    }
                    else
                    {
                        int num = (int)MessageBox.Show("Eine Sequenz mit diesem Namen existiert schon!");
                        txtInputNewSequenceName.Text = string.Empty;
                        txtInputNewSequenceName.Focus();
                    }
                }
            }
            if (e.Control && e.Alt && e.Shift)
            {
                string str1 = Application.StartupPath + "\\Sport.cfg";
                string str2 = Application.StartupPath + "\\Scoreboards.cfg";
                switch (e.KeyCode)
                {
                    case Keys.B:
                        string str3 = Application.StartupPath + "\\" + _scb.Sport + ".jpg";
                        if (!File.Exists(str3))
                        {
                            Bitmap bitmap = new Bitmap(_scb.Width, _scb.Height);
                            Graphics.FromImage((Image)bitmap).Clear(Color.Black);
                            bitmap.Save(str3, ImageFormat.Jpeg);
                        }
                        if (File.Exists(str3))
                        {
                            _scb.LayoutLocked = true;
                            ProcessStartInfo processStartInfo = new ProcessStartInfo("mspaint.exe", str3);
                            Process process = new Process();
                            process.StartInfo = processStartInfo;
                            process.Start();
                            process.WaitForExit();
                            _scb.ReloadLayout();
                            break;
                        }
                        break;
                    case Keys.C:
                        _scb.CancelCurrentEffect();
                        break;
                    case Keys.E:
                        string str4 = Application.StartupPath + "\\Temp.cfg";
                        bool flag = false;
                        if (!File.Exists(str2))
                        {
                            flag = true;
                            _reset_scoreboards_cfg(str2);
                        }
                        File.Copy(str2, str4, true);
                        ProcessStartInfo processStartInfo1 = new ProcessStartInfo("notepad.exe", str2);
                        Process process1 = new Process();
                        process1.StartInfo = processStartInfo1;
                        process1.Start();
                        process1.WaitForExit();
                        if (File.ReadAllText(str4) != File.ReadAllText(str2) || flag)
                        {
                            int num = (int)MessageBox.Show("Restart program!", "Configurationfile changed!");
                            File.Delete(str4);
                            Close();
                            break;
                        }
                        File.Delete(str4);
                        break;
                    case Keys.H:
                        if (_new_horn_signal == null)
                        {
                            _new_horn_signal = new OpenFileDialog();
                            _new_horn_signal.InitialDirectory = "c:\\";
                            _new_horn_signal.Filter = "WAV-Dateien (*.wav)|*.wav";
                        }
                        DialogResult dialogResult = MessageBox.Show("Select new hornsound ?", "Signal to change", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                            _new_horn_signal.Title = "Horn-Signal";
                        else
                            _new_horn_signal.Title = "Shotclock-Signal";
                        int num1 = (int)_new_horn_signal.ShowDialog();
                        if (_new_horn_signal.FileName.EndsWith(".wav"))
                        {
                            if (dialogResult == DialogResult.Yes)
                            {
                                if (File.Exists(Application.StartupPath + "\\claxon.wav"))
                                {
                                    File.Copy(_new_horn_signal.FileName, Application.StartupPath + "\\claxon.wav", true);
                                    break;
                                }
                                break;
                            }
                            if (File.Exists(Application.StartupPath + "\\sc_claxon.wav"))
                            {
                                File.Copy(_new_horn_signal.FileName, Application.StartupPath + "\\_sc_claxon.wav", true);
                                break;
                            }
                            break;
                        }
                        break;
                    case Keys.L:
                        if (File.Exists(Application.StartupPath + "\\Layouteditor 2008.exe"))
                        {
                            _scb.LayoutLocked = true;
                            ProcessStartInfo processStartInfo2 = new ProcessStartInfo(Application.StartupPath + "\\Layouteditor 2008.exe", _scb.Sport);
                            Process process2 = new Process();
                            process2.StartInfo = processStartInfo2;
                            process2.Start();
                            process2.WaitForExit();
                            _scb.ReloadLayout();
                            break;
                        }
                        break;
                    case Keys.N:
                        if (_scb != null)
                        {
                            _scb.ConnectAnotherServer();
                            break;
                        }
                        break;
                    case Keys.P:
                        Location = new Point(0, 0);
                        break;
                    case Keys.Q:
                        _scb.RefreshLogos();
                        break;
                    case Keys.R:
                        if (MessageBox.Show("Configuration will be setted to default values!", "Caution:", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            _reset_scoreboards_cfg(str2);
                            break;
                        }
                        break;
                    case Keys.T:
                        _scb.RefreshTeams();
                        break;
                    case Keys.X:
                        _scb.SetRedLight(false);
                        break;
                    case Keys.Y:
                        _scb.SetRedLight(true);
                        break;
                    case Keys.OemOpenBrackets:
                        _lock_secrets = false;
                        int num2 = (int)MessageBox.Show("Please press '?' once again to get hidden informations!");
                        break;
                }
            }
            if (e.Control || e.Alt || e.Shift)
                return;
            Point timeInsertLocation = _scb.GameTimeInsertLocation;
            bool flag1 = false;
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    btnClearSCB_Click((object)this, (EventArgs)null);
                    break;
                case Keys.F1:
                    Process.Start("AcroRd32.exe", Application.StartupPath + "\\Anwenderdokumentation Tafelregie.pdf");
                    break;
                case Keys.OemOpenBrackets:
                    if (!_lock_secrets)
                    {
                        _lock_secrets = true;
                        Process.Start("AcroRd32.exe", Application.StartupPath + "\\Einstellungen.pdf");
                        break;
                    }
                    break;
            }
            if (!flag1 || !(label1.Text != string.Empty))
                return;
            _scb.ShowGraphic(label1.Text, rbSmallGraphic.Checked);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Settings.Default.SelectedPicturePath))
            {
                Settings.Default.SelectedPicturePath = "c:\\";
                Settings.Default.Save();
            }
            folderBrowserDialog1.SelectedPath = Settings.Default.SelectedPicturePath;
            _init_picture_previews();
            _fill_previews();
            _fill_animations();
            _fill_sequences();
            _cleanup_mediafiles();
            _fill_ppts();
            rbFullGraphic.Checked = Settings.Default.ShowGraphicInFullscreen;
            tabControl1.SelectedIndex = Settings.Default.LastSelectedTabIndex;
        }

        private void _scb_PercentComplete(int value)
        {
            switch (value)
            {
                case 2:
                    _invokes.setControlVisibleAsync((Control)progressBar1, true);
                    break;
                case 100:
                    _invokes.setControlVisibleAsync((Control)progressBar1, false);
                    return;
            }
            _invokes.setProgressBarValueAsync(progressBar1, value);
        }

        private void btnClearSCB_Click(object sender, EventArgs e)
        {
            try
            {
                if (progressBar1.Visible)
                    return;
                if (_sequence_timer != null)
                {
                    _sequence_timer.Stop();
                    _sequence_timer.Tick -= new EventHandler(_sequence_timer_Tick);
                }
                _sequence_timer = (System.Windows.Forms.Timer)null;
                _singlestep = false;
                if (_scb == null)
                    return;
                _scb.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " = " + ex.StackTrace.ToString());
            }
        }

        private void btnShowScore_Click(object sender, EventArgs e)
        {
            try
            {
                if (_sequence_timer != null)
                {
                    _sequence_timer.Stop();
                    _sequence_timer.Tick -= new EventHandler(_sequence_timer_Tick);
                }
                _sequence_timer = (System.Windows.Forms.Timer)null;
                _send_remote_command("SHOW_SCORE");
                _scb.ShowVideo = false;
                _scb.HideGraphic();
                _scb.Clear();
                _scb.ActualPicture = (Image)new Bitmap(1, 1);
                _scb.FileName = string.Empty;
                _scb.Picturebox.Image = _scb.ActualPicture;
                _scb.DoShowScore = true;
                _scb.State = LED_Board.DisplayState.Score;
                _scb.ShowScore();
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnShowScore : " + ex.Message);
            }
        }

        private void _init_picture_previews()
        {
            _max_previews_x = (groupBox1.Width - _default_preview_startpoint.X - _default_preview_startpoint.X) / (_default_preview_size.Width + _default_preview_dist) - 1;
            _max_previews_y = (groupBox1.Height - _default_preview_startpoint.Y - _default_preview_startpoint.Y) / (_default_preview_size.Height + _default_preview_dist);
            for (int index1 = 0; index1 < _max_previews_y; ++index1)
            {
                int num = _default_preview_startpoint.Y + (_default_preview_dist + _default_preview_size.Height) * index1;
                for (int index2 = 0; index2 < _max_previews_x; ++index2)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Name = "pb" + index2.ToString() + index1.ToString();
                    pictureBox.Size = _default_preview_size;
                    pictureBox.Left = _default_preview_startpoint.X + (_default_preview_dist + _default_preview_size.Width) * index2;
                    pictureBox.Top = num;
                    pictureBox.BackColor = Color.Transparent;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Click += new EventHandler(_temp_Click);
                    groupBox1.Controls.Add((Control)pictureBox);
                }
            }
        }

        private void _temp_Click(object sender, EventArgs e)
        {
            try
            {
                btnClearSCB.Focus();
                PictureBox pictureBox = (PictureBox)sender;
                if (pictureBox.Image == null || progressBar1.Visible)
                    return;
                if (_sequence_timer != null)
                {
                    _sequence_timer.Stop();
                    _sequence_timer.Tick -= new EventHandler(_sequence_timer_Tick);
                }
                _sequence_timer = (System.Windows.Forms.Timer)null;
                _scb.ShowVideo = false;
                _scb.ShowGraphic(pictureBox.Tag.ToString(), rbSmallGraphic.Checked, cmbEffect.SelectedIndex, true);
                _send_remote_command("SHOW_PICTURE=" + pictureBox.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("_temp_ShowGrapghic : " + ex.Message);
            }
        }

        private void _dir_search_pictures(string sDir, ref List<string> _file_list)
        {
            _file_list.Clear();
            try
            {
                foreach (string file in Directory.GetFiles(sDir, "*.jpg"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.bmp"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.tif"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.gif"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnSetGraphicFolder_Click(object sender, EventArgs e)
        {
            int num = (int)folderBrowserDialog1.ShowDialog();
            Settings.Default.SelectedPicturePath = folderBrowserDialog1.SelectedPath;
            Settings.Default.Save();
            _picture_path = Settings.Default.SelectedPicturePath;
            _fill_previews();
        }

        private void _clear_previews()
        {
            foreach (Control control in (ArrangedElementCollection)groupBox1.Controls)
            {
                if (control.GetType() == typeof(PictureBox))
                    ((PictureBox)control).Image = (Image)null;
            }
            _pictures.Clear();
        }

        private void _fill_previews()
        {
            int index1 = 0;
            Bitmap bitmap = (Bitmap)null;
            string empty = string.Empty;
            foreach (Control control in (ArrangedElementCollection)groupBox1.Controls)
            {
                if (control.GetType() == typeof(PictureBox))
                    ((PictureBox)control).Image = (Image)null;
            }
            _pictures.Clear();
            _dir_search_pictures(_picture_path, ref _pictures);
            for (int index2 = 0; index2 < _max_previews_y; ++index2)
            {
                for (int index3 = 0; index3 < _max_previews_x; ++index3)
                {
                    if (index1 < _pictures.Count)
                    {
                        if (_pictures[index1].StartsWith("Snapshot"))
                        {
                            string str = _pictures[index1].Substring(8, _pictures[index1].Length - 12);
                            if (_snapshot_counter < Convert.ToInt32(str))
                                _snapshot_counter = Convert.ToInt32(str);
                        }
                        string str1 = _picture_path + "\\" + _pictures[index1];
                        try
                        {
                            bitmap = new Bitmap(Image.FromFile(str1), groupBox1.Controls["pb" + index3.ToString() + index2.ToString()].Size);
                        }
                        catch
                        {
                            int num = (int)MessageBox.Show(str1);
                        }
                      ((PictureBox)groupBox1.Controls["pb" + index3.ToString() + index2.ToString()]).Image = (Image)bitmap;
                        groupBox1.Controls["pb" + index3.ToString() + index2.ToString()].Tag = (object)str1;
                        PictureBox control = (PictureBox)groupBox1.Controls["pb" + index3.ToString() + index2.ToString()];
                        control.BringToFront();
                        string str2 = control.Tag.ToString();
                        string caption = str2.Substring(str2.LastIndexOf("\\") + 1);
                        toolTip1.SetToolTip((Control)control, caption);
                    }
                    ++index1;
                }
            }
        }

        private void btnSelectPicturePath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Settings.Default.SelectedPicturePath;
            int num = (int)folderBrowserDialog1.ShowDialog();
            Settings.Default.SelectedPicturePath = folderBrowserDialog1.SelectedPath;
            Settings.Default.Save();
            _picture_path = Settings.Default.SelectedPicturePath;
            _fill_previews();
        }

        private void _clear_sequeces()
        {
            _sequences.Clear();
            cmbAvailableSequences.Items.Clear();
            lbSequenceContent.Items.Clear();
        }

        private void _fill_sequences()
        {
            if (!Directory.Exists(_sequence_path))
                Directory.CreateDirectory(_sequence_path);
            _sequences.Clear();
            foreach (string file in Directory.GetFiles(_sequence_path, "*.seq"))
                _sequences.Add(new Sequence(_sequence_path, file, _media_path));
            cmbAvailableSequences.Items.Clear();
            lbSequenceContent.Items.Clear();
            for (int index = 0; index < _sequences.Count; ++index)
                cmbAvailableSequences.Items.Add((object)_sequences[index].Name);
            try
            {
                cmbAvailableSequences.SelectedIndex = Settings.Default.LastSelectedSequenceIndex;
            }
            catch
            {
                cmbAvailableSequences.SelectedIndex = -1;
            }
        }

        private void tsmNewSequence_Click(object sender, EventArgs e)
        {
            txtInputNewSequenceName.BackColor = Color.Gold;
            txtInputNewSequenceName.Text = string.Empty;
            txtInputNewSequenceName.Focus();
        }

        private void tsmPlaySequence_Click(object sender, EventArgs e)
        {
            if (cmbAvailableSequences.SelectedIndex <= -1)
                return;
            _endless = false;
            _singlestep = false;
            _temp_sequence = _sequences[cmbAvailableSequences.SelectedIndex];
            _sequencepart_pointer = 0;
            _send_remote_command("SHOW_SEQUENCE=" + cmbAvailableSequences.SelectedIndex.ToString());
            _start_sequencepart();
        }

        private void tmsPlaySequenceEndless_Click(object sender, EventArgs e)
        {
            if (cmbAvailableSequences.SelectedIndex <= -1)
                return;
            _endless = true;
            _singlestep = false;
            _temp_sequence = _sequences[cmbAvailableSequences.SelectedIndex];
            _sequencepart_pointer = 0;
            _send_remote_command("SHOW_SEQUENCE_ENDLESS=" + cmbAvailableSequences.SelectedIndex.ToString());
            _start_sequencepart();
        }

        private void _start_sequencepart()
        {
            Sequence.SequencePart part = _temp_sequence.Parts[_sequencepart_pointer];
            string filename = part.Filename;
            if (!File.Exists(filename) && !filename.EndsWith("Score"))
                return;
            _scb.ShowVideo = false;
            if (_extra_sound != null)
                _extra_sound.Stop();
            try
            {
                _sequence_length = part.Length / 100L;
                int effectIndex = part.EffectIndex;
                if (part.PlayAcusticSignal && File.Exists(filename) && File.Exists(Application.StartupPath + "\\gong.wav"))
                    _gong.Play();
                if (!filename.EndsWith("Score"))
                {
                    if (File.Exists(filename))
                    {
                        if (part.ExtraSoundFileName.Trim() != string.Empty)
                        {
                            _extra_sound.SoundLocation = Application.StartupPath + "\\" + part.ExtraSoundFileName;
                            _extra_sound.Load();
                            _extra_sound.Play();
                        }
                        _scb.ShowGraphic(filename, rbSmallGraphic.Checked, effectIndex, true);
                    }
                }
                else
                {
                    _scb.Clear();
                    btnShowScore_Click((object)this, (EventArgs)null);
                }
                if ((part.IsAnimation || !File.Exists(filename)) && !part.IsScoreDisplay)
                    return;
                if (_sequence_timer == null)
                {
                    _sequence_timer = new System.Windows.Forms.Timer();
                    _sequence_timer.Interval = 100;
                    _sequence_timer.Tick += new EventHandler(_sequence_timer_Tick);
                }
                if (_singlestep)
                    return;
                _sequence_timer.Start();
            }
            catch
            {
            }
        }

        private void _stop_sequence()
        {
            if (_sequence_timer != null)
                _sequence_timer.Stop();
            if (_extra_sound != null)
                _extra_sound.Stop();
            if (_scb.State == LED_Board.DisplayState.Score)
                btnShowScore_Click((object)this, (EventArgs)null);
            else
                _scb.Clear();
        }

        private void _sequence_timer_Tick(object sender, EventArgs e)
        {
            if (_temp_sequence != null)
            {
                --_sequence_length;
                if (_sequence_length >= 1L)
                    return;
                if (!_singlestep)
                {
                    ++_sequencepart_pointer;
                    if (_sequencepart_pointer > _temp_sequence.Parts.Count - 1)
                    {
                        if (_endless)
                        {
                            _sequencepart_pointer = 0;
                            _start_sequencepart();
                        }
                        else
                            _stop_sequence();
                    }
                    else
                        _start_sequencepart();
                }
                else
                    _stop_sequence();
            }
            else
            {
                if (_scb.State != LED_Board.DisplayState.Score)
                    return;
                btnShowScore_Click((object)this, (EventArgs)null);
            }
        }

        private void tsmDeleteSequence_Click(object sender, EventArgs e)
        {
            string fileName = _sequences[cmbAvailableSequences.SelectedIndex].FileName;
            if (MessageBox.Show("Die Sequenz " + cmbAvailableSequences.Text.Trim() + " wird gelöscht!", "Achtung:", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;
            if (File.Exists(fileName))
                File.Delete(fileName);
            _fill_sequences();
        }

        private void btnNewSequenceNameCancel_Click(object sender, EventArgs e)
        {
            txtInputNewSequenceName.Text = string.Empty;
            pnlVideoPreview.Visible = true;
        }

        private void btnSequenceNameOk_Click(object sender, EventArgs e)
        {
            if (!File.Exists(_sequence_path + "\\" + txtInputNewSequenceName.Text.Trim() + ".seq"))
                _sequences.Add(new Sequence(_sequence_path, _sequence_path + "\\" + txtInputNewSequenceName.Text.Trim() + ".seq", _media_path));
            _fill_sequences();
        }

        private void tsmAddNewPictureToSequence_Click(object sender, EventArgs e)
        {
            if (cmbAvailableSequences.Text.Trim() == string.Empty)
            {
                int num = (int)MessageBox.Show("Bitte der neuen Sequenz einen Namen geben und mit 'Enter' speichern!");
                txtInputNewSequenceName.Focus();
            }
            else
            {
                ofdPictures.InitialDirectory = _picture_path;
                int num = (int)ofdPictures.ShowDialog();
            }
        }

        private void tsmAddAnimation_Click(object sender, EventArgs e)
        {
            if (cmbAvailableSequences.Text.Trim() == string.Empty)
            {
                int num = (int)MessageBox.Show("Bitte der neuen Sequenz einen Namen geben und mit 'Enter' speichern!");
                txtInputNewSequenceName.Focus();
            }
            else
            {
                ofdAnimations.InitialDirectory = Settings.Default.SelectedAnimationPath;
                int num = (int)ofdAnimations.ShowDialog();
            }
        }

        private void ofdPictures_FileOk(object sender, CancelEventArgs e)
        {
            _add_sequencepart(ofdPictures.FileName, false, false);
        }

        private void ofdAnimations_FileOk(object sender, CancelEventArgs e)
        {
            _add_sequencepart(ofdAnimations.FileName, true, false);
        }

        private void _add_sequencepart(string FileName, bool IsAnimation, bool IsScoreDisplay)
        {
            string FileName1 = string.Empty;
            if (!IsScoreDisplay)
            {
                if (!Directory.Exists(Application.StartupPath + _media_path))
                    Directory.CreateDirectory(_media_path);
                string str = _media_path + FileName.Substring(FileName.LastIndexOf("\\"), FileName.Length - FileName.LastIndexOf("\\"));
                if (!File.Exists(str))
                    File.Copy(FileName, str, true);
                FileName1 = _media_path + FileName.Substring(FileName.LastIndexOf("\\"), FileName.Length - FileName.LastIndexOf("\\"));
            }
            if (IsAnimation)
            {
                DialogResult dialogResult1 = MessageBox.Show("Gong zu Beginn abspielen?", "Akustisches Signal", MessageBoxButtons.YesNo);
                DialogResult dialogResult2 = MessageBox.Show("Extra-Klangdatei abspielen?", "Zusätzlich:", MessageBoxButtons.YesNo);
                string ExtraSoundFilename = string.Empty;
                if (dialogResult2 == DialogResult.Yes)
                {
                    int num = (int)openFileDialog2.ShowDialog();
                    ExtraSoundFilename = openFileDialog2.FileName.Trim().Substring(openFileDialog2.FileName.LastIndexOf("\\") + 1);
                    File.Copy(openFileDialog2.FileName, Application.StartupPath + "\\" + ExtraSoundFilename, true);
                }
                if (!_sequences[cmbAvailableSequences.SelectedIndex].AddAnimation(FileName1, dialogResult1 == DialogResult.Yes, ExtraSoundFilename))
                {
                    int num1 = (int)MessageBox.Show("Can't add " + FileName);
                }
                else
                    _sequences[cmbAvailableSequences.SelectedIndex].Save(_sequence_path);
            }
            else
            {
                int DefaultLength = 3;
                InputDisplayLength inputDisplayLength = new InputDisplayLength(FileName, _default_sequence_effect_index, DefaultLength);
                int num1 = (int)inputDisplayLength.ShowDialog();
                int displayLength = inputDisplayLength.DisplayLength;
                _default_sequence_effect_index = inputDisplayLength.EffectIndex;
                if (!IsScoreDisplay)
                {
                    if (!_sequences[cmbAvailableSequences.SelectedIndex].AddPicture(FileName1, (long)(inputDisplayLength.DisplayLength * 1000), inputDisplayLength.EffectIndex, inputDisplayLength.PlayAcusticSignal, inputDisplayLength.ExtraSoundFileName))
                    {
                        int num2 = (int)MessageBox.Show("Can't add " + FileName);
                    }
                    else
                        _sequences[cmbAvailableSequences.SelectedIndex].Save(_sequence_path);
                }
                else
                {
                    _sequences[cmbAvailableSequences.SelectedIndex].AddScoreDisplay((long)(inputDisplayLength.DisplayLength * 1000), inputDisplayLength.PlayAcusticSignal, inputDisplayLength.ExtraSoundFileName);
                    _sequences[cmbAvailableSequences.SelectedIndex].Save(_sequence_path);
                }
                inputDisplayLength.Dispose();
            }
            int selectedIndex = cmbAvailableSequences.SelectedIndex;
            _fill_sequences();
            cmbAvailableSequences.SelectedIndex = selectedIndex;
        }

        private void btnEditSequencePartOK_Click(object sender, EventArgs e)
        {
        }

        private void btnEditSequencePartCancel_Click(object sender, EventArgs e)
        {
            _fill_sequences();
        }

        private void tsmMoveUpMediaInSequence_Click(object sender, EventArgs e)
        {
            _sequences[cmbAvailableSequences.SelectedIndex].MovePart(lbSequenceContent.SelectedIndex, -1);
            _sequences[cmbAvailableSequences.SelectedIndex].Save(_sequence_path);
            int selectedIndex = cmbAvailableSequences.SelectedIndex;
            _fill_sequences();
            cmbAvailableSequences.SelectedIndex = selectedIndex;
        }

        private void tsmMoveDownMediaInSequence_Click(object sender, EventArgs e)
        {
            _sequences[cmbAvailableSequences.SelectedIndex].MovePart(lbSequenceContent.SelectedIndex, 1);
            _sequences[cmbAvailableSequences.SelectedIndex].Save(_sequence_path);
            int selectedIndex = cmbAvailableSequences.SelectedIndex;
            _fill_sequences();
            cmbAvailableSequences.SelectedIndex = selectedIndex;
        }

        private void tsmDeleteMediaFromSequence_Click(object sender, EventArgs e)
        {
            _sequences[cmbAvailableSequences.SelectedIndex].DeletePart(lbSequenceContent.SelectedIndex);
            _sequences[cmbAvailableSequences.SelectedIndex].Save(_sequence_path);
            int selectedIndex = cmbAvailableSequences.SelectedIndex;
            _fill_sequences();
            cmbAvailableSequences.SelectedIndex = selectedIndex;
            lbSequenceContent.SelectedIndex = -1;
        }

        private void _cleanup_mediafiles()
        {
            if (!Directory.Exists(_media_path))
                return;
            DirectoryInfo directoryInfo = new DirectoryInfo(_media_path);
            ArrayList arrayList = new ArrayList();
            foreach (FileInfo file in directoryInfo.GetFiles())
                arrayList.Add((object)file.FullName);
            foreach (Sequence sequence in _sequences)
            {
                foreach (Sequence.SequencePart part in sequence.Parts)
                {
                    if (arrayList.Contains((object)part.Filename))
                        arrayList.Remove((object)part.Filename);
                }
            }
            foreach (string path in arrayList)
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
            }
        }

        private void btnShowSingleStep_Click(object sender, EventArgs e)
        {
            if (lbSequenceContent.Items.Count <= 0)
                return;
            _scb.ShowVideo = false;
            if (_sequence_timer != null)
            {
                _sequence_timer.Stop();
                _sequence_timer.Tick -= new EventHandler(_sequence_timer_Tick);
            }
            _sequence_timer = (System.Windows.Forms.Timer)null;
            if (!_singlestep && _sequencepart_pointer == 0)
                --_sequencepart_pointer;
            ++_sequencepart_pointer;
            if (_sequencepart_pointer >= lbSequenceContent.Items.Count || _sequencepart_pointer < 0)
                _sequencepart_pointer = 0;
            lbSequenceContent.SelectedIndex = _sequencepart_pointer;
            _send_remote_command("SHOW_SEQUENCE_STEP=" + cmbAvailableSequences.SelectedIndex.ToString() + "=" + lbSequenceContent.SelectedIndex.ToString());
            _singlestep = true;
            _temp_sequence = _sequences[cmbAvailableSequences.SelectedIndex];
            _start_sequencepart();
        }

        private void _dispose_animation_preview()
        {
            if (_animation_preview_mci != null)
            {
                if (_animation_preview_mci.IsOpen)
                    _animation_preview_mci.Close();
                _animation_preview_mci.Dispose();
            }
            _animation_preview_mci = (Mci)null;
        }

        private void _clear_animations()
        {
            _dispose_animation_preview();
            lbAnimations.Items.Clear();
        }

        private void _fill_animations()
        {
            _dispose_animation_preview();
            lbAnimations.Items.Clear();
            _dir_search_animations(_animation_path, ref _animation_filenames);
            if (_animation_filenames.Count <= 0)
                return;
            for (int index = 0; index < _animation_filenames.Count; ++index)
                lbAnimations.Items.Add((object)_animation_filenames[index].ToString());
        }

        private void _dir_search_animations(string sDir, ref List<string> _file_list)
        {
            _file_list.Clear();
            try
            {
                foreach (string file in Directory.GetFiles(sDir, "*.mpg"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.mpeg"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.avi"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.mp4"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.wmv"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                foreach (string file in Directory.GetFiles(sDir, "*.mov"))
                {
                    string str = file.Substring(file.LastIndexOf('\\') + 1);
                    _file_list.Add(str);
                }
                _file_list.Sort();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnSelectAnimationPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Settings.Default.SelectedAnimationPath;
            int num = (int)folderBrowserDialog1.ShowDialog();
            Settings.Default.SelectedAnimationPath = folderBrowserDialog1.SelectedPath;
            Settings.Default.Save();
            _animation_path = Settings.Default.SelectedAnimationPath;
            _fill_animations();
        }

        private void btnShowVideo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_sequence_timer != null)
                {
                    _sequence_timer.Stop();
                    _sequence_timer.Tick -= new EventHandler(_sequence_timer_Tick);
                }
                _sequence_timer = (System.Windows.Forms.Timer)null;
                try
                {
                    if (_first_video_frame != null)
                    {
                        _scb.ShowGraphic((Image)_first_video_frame, rbSmallGraphic.Checked, cmbEffect.SelectedIndex);
                        int num = 32000;
                        while (progressBar1.Visible)
                        {
                            if (num > 0)
                            {
                                Application.DoEvents();
                                --num;
                            }
                            else
                                break;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("_first_video_frame");
                }
                try
                {
                    _send_remote_command("SHOW_VIDEO");
                }
                catch
                {
                    Console.WriteLine("Command=SHOW_VIDEO");
                }
                _scb.ShowVideo = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnShowVideo : " + ex.Message);
            }
        }

        private void btnVideoSnapShot_Click(object sender, EventArgs e)
        {
            ++_snapshot_counter;
            _first_video_frame.Save(_picture_path + "\\Snapshot" + _snapshot_counter.ToString() + ".jpg", ImageFormat.Jpeg);
            _fill_previews();
        }

        private void lbAnimations_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lbAnimations.SelectedIndex <= -1)
                return;
            _send_remote_command("ANIMATION_SELECTED=" + lbAnimations.SelectedIndex.ToString());
            _dispose_animation_preview();
            _animation_preview_mci = new Mci();
            _animation_preview_mci.Open(_animation_path + "\\" + _animation_filenames[lbAnimations.SelectedIndex].ToString(), (Control)pbAnimationPreview);
            try
            {
                _animation_preview_mci.SetRectangle(0, 0, pbAnimationPreview.Width, pbAnimationPreview.Height);
            }
            catch
            {
            }
            try
            {
                _animation_preview_mci.Volume = 0;
            }
            catch
            {
            }
            try
            {
                _animation_preview_mci.Play(true);
            }
            catch
            {
            }
        }

        private void lbAnimations_Leave(object sender, EventArgs e)
        {
            _dispose_animation_preview();
        }

        private void btnShowAnimation_Click_1(object sender, EventArgs e)
        {
            if (lbAnimations.SelectedIndex <= -1)
                return;
            if (_sequence_timer != null)
            {
                _sequence_timer.Stop();
                _sequence_timer.Tick -= new EventHandler(_sequence_timer_Tick);
            }
            _sequence_timer = (System.Windows.Forms.Timer)null;
            _scb.ShowVideo = false;
            lblSelectedAnimationName.Text = _animation_path + "\\" + lbAnimations.SelectedItem.ToString();
            lblSelectedAnimationName.ForeColor = Color.Red;
            _scb.ShowGraphic(_animation_path + "\\" + lbAnimations.SelectedItem.ToString(), rbSmallGraphic.Checked, 0, false);
            _send_remote_command("SHOW_ANIMATION=" + lbAnimations.SelectedIndex.ToString());
        }

        private void cmbSelectKindOfGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_scb == null)
                return;
            if (cmbSelectKindOfGame.Visible && cmbSelectKindOfGame.SelectedIndex > -1)
                _scb.SetKindOfGame(cmbSelectKindOfGame.Text);
            Settings.Default.LastSelectedKindOfGameIndex = cmbSelectKindOfGame.SelectedIndex;
            Settings.Default.Save();
        }

        private void rbSmallGraphic_MouseUp(object sender, MouseEventArgs e)
        {
            _do_change_graphic_size(true);
        }

        private void rbFullGraphic_Click(object sender, EventArgs e)
        {
            _do_change_graphic_size(false);
        }

        private void _do_change_graphic_size(bool _is_small)
        {
            Settings.Default.ShowGraphicInFullscreen = rbFullGraphic.Checked;
            Settings.Default.Save();
            _send_remote_command("FULL_GRAPHIC=" + rbFullGraphic.Checked.ToString());
            _scb.GraphicInWindow = _is_small;
            _scb.FileName = string.Empty;
            _scb.ActualPicture = (Image)null;
        }

        private void cmbEffect_SelectedIndexChanged(object sender, EventArgs e)
        {
            _scb.ActualEffectIndex = cmbEffect.SelectedIndex;
            _send_remote_command("EFFECT_SELECTED=" + cmbEffect.SelectedIndex.ToString());
        }

        private void _send_remote_command(string Command)
        {
            if (_remote_sender == null || !_remote_sender.IsConnected)
                return;
            _remote_sender.SendMSG(Command);
        }

        private void cmbEffect_DropDownClosed(object sender, EventArgs e)
        {
            Settings.Default.LastSelectedEffect = cmbEffect.SelectedIndex;
            Settings.Default.Save();
        }

        private void btnFreeText_Click(object sender, EventArgs e)
        {
            int num = (int)new TextEditor(_scb, rbSmallGraphic.Checked, cmbEffect.SelectedIndex).ShowDialog();
            _fill_previews();
        }

        private void stretchVideoToDisplaySizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stretchVideoToDisplaySizeToolStripMenuItem.Checked = !stretchVideoToDisplaySizeToolStripMenuItem.Checked;
            Settings.Default.StretchVideoToDisplaySize = stretchVideoToDisplaySizeToolStripMenuItem.Checked;
            Settings.Default.Save();
            bool showVideo = _scb.ShowVideo;
            btnClearSCB_Click((object)this, (EventArgs)null);
            if (!showVideo)
                return;
            btnShowVideo_Click((object)this, (EventArgs)null);
        }

        private void cbAllowGraphicFromLAN_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAllowGraphicFromLAN.Checked)
                cbAllowGraphicFromLAN.ForeColor = Color.Red;
            else
                cbAllowGraphicFromLAN.ForeColor = Color.White;
            _scb.AllowGraphicFromLAN = cbAllowGraphicFromLAN.Checked;
        }

        private void btnSelectPPT_Path_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Settings.Default.SelectedPPT_Path;
            int num = (int)folderBrowserDialog1.ShowDialog();
            Settings.Default.SelectedPPT_Path = folderBrowserDialog1.SelectedPath;
            Settings.Default.Save();
            _ppt_path = Settings.Default.SelectedPPT_Path;
            _fill_ppts();
        }

        private void _fill_ppts()
        {
            lbAvailablePPTs.Items.Clear();
            _dir_search_ppts(_ppt_path, ref _ppt_filenames);
            if (_ppt_filenames.Count <= 0)
                return;
            for (int index = 0; index < _ppt_filenames.Count; ++index)
                lbAvailablePPTs.Items.Add((object)_ppt_filenames[index].ToString());
        }

        private void _dir_search_ppts(string sDir, ref List<string> _file_list)
        {
            _ppt_filenames.Clear();
            try
            {
                foreach (string file in Directory.GetFiles(sDir, "*.ppt"))
                    _ppt_filenames.Add(file.Substring(file.LastIndexOf('\\') + 1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void lbAvailablePPTs_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnShowPPT_Click(object sender, EventArgs e)
        {
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                _fill_sequences();
            if (tabControl1.SelectedIndex != 1)
                return;
            _fill_ppts();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _fill_ppts();
        }

        private void btnGong_MouseClick(object sender, MouseEventArgs e)
        {
            if (!File.Exists(Application.StartupPath + "\\gong.wav"))
                return;
            _gong.Play();
        }

        private void soccerBackgroundImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sport = "Soccer";
            _background_image_name_2save = ((ToolStripItem)sender).Tag.ToString();
            int num = (int)openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (!Directory.Exists(_soccer_background_picture_directory))
                Directory.CreateDirectory(Application.StartupPath + "\\" + _scb.Sport.ToString() + "_BackgroundImages");
            StreamReader streamReader = new StreamReader(openFileDialog1.FileName);
            Bitmap bitmap1 = new Bitmap(streamReader.BaseStream);
            streamReader.Close();
            Bitmap bitmap2 = new Bitmap(_scb.Width, _scb.Height);
            Graphics graphics = Graphics.FromImage((Image)bitmap2);
            graphics.DrawImage((Image)bitmap1, 0, 0, bitmap2.Width, bitmap2.Height);
            graphics.Dispose();
            bitmap2.Save(Application.StartupPath + "\\" + _sport + "_BackgroundImages\\" + _background_image_name_2save, ImageFormat.Jpeg);
            if (_background_image_name_2save.StartsWith(_sport))
                bitmap2.Save(Application.StartupPath + "\\" + _background_image_name_2save, ImageFormat.Jpeg);
            _sport = string.Empty;
        }

        private void btnRefreshPPT_List_Click(object sender, EventArgs e)
        {
            _fill_ppts();
        }

        private void insertScoreDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _add_sequencepart("Score", false, true);
        }

        private void cmbAvailableSequences_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSequenceContent.Items.Clear();
            if (cmbAvailableSequences.SelectedIndex <= -1)
                return;
            for (int index = 0; index < _sequences[cmbAvailableSequences.SelectedIndex].Parts.Count; ++index)
                lbSequenceContent.Items.Add((object)(_sequences[cmbAvailableSequences.SelectedIndex].Parts[index].Name.PadRight(28, '.') + (_sequences[cmbAvailableSequences.SelectedIndex].Parts[index].Length / 1000L).ToString().PadLeft(3) + "s"));
        }

        private void bildAusMedienverzeichnisHinzufügenToolStripMenuItem_Click(
          object sender,
          EventArgs e)
        {
            ofdPictures.InitialDirectory = Application.StartupPath + "\\Media";
            int num = (int)ofdPictures.ShowDialog();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = new Font(_scb.GameTimeInsertFontName, (float)_scb.GameTimeInsertFontSize, _scb.GameTimeInsertFontStyle);
            int num = (int)fontDialog1.ShowDialog();
            _scb.GameTimeInsertFontName = fontDialog1.Font.Name;
            _scb.GameTimeInsertFontSize = (int)fontDialog1.Font.Size;
            _scb.GameTimeInsertFontStyle = fontDialog1.Font.Style;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = _scb.GameTimeInsertColor;
            int num = (int)colorDialog1.ShowDialog();
            _scb.GameTimeInsertColor = colorDialog1.Color;
        }

        private void insertGametimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertGametimeToolStripMenuItem.Checked = !insertGametimeToolStripMenuItem.Checked;
            if (insertGametimeToolStripMenuItem.Checked)
            {
                insertDaytimeToolStripMenuItem.Checked = false;
                Settings.Default.DoInsertDayTime = false;
                _scb.InsertDayTime = insertDaytimeToolStripMenuItem.Checked;
            }
            Settings.Default.DoInsertgameTime = insertGametimeToolStripMenuItem.Checked;
            Settings.Default.Save();
            _scb.InsertGameTime = insertGametimeToolStripMenuItem.Checked;
        }

        private void locationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbTimeInsertPosition.Visible = true;
            gbTimeInsertPosition.BringToFront();
            gbTimeInsertPosition.Refresh();
            try
            {
                nudTimeInsertPositionX.Value = (Decimal)Settings.Default.GameTimeInsertLocation.X;
                nudTimeInsertPositionY.Value = (Decimal)Settings.Default.GameTimeInsertLocation.Y;
            }
            catch
            {
            }
        }

        private void btnTimeInsertPositionCancel_Click(object sender, EventArgs e)
        {
            gbTimeInsertPosition.Visible = false;
            _scb.GameTimeInsertLocation = Settings.Default.GameTimeInsertLocation;
        }

        private void btnTimeInsertPositionSave_Click(object sender, EventArgs e)
        {
            gbTimeInsertPosition.Visible = false;
            Settings.Default.GameTimeInsertLocation = _scb.GameTimeInsertLocation;
            Settings.Default.Save();
        }

        private void nudTimeInsertPositionX_ValueChanged(object sender, EventArgs e)
        {
            _scb.GameTimeInsertLocation = new Point(Convert.ToInt32(nudTimeInsertPositionX.Value), Convert.ToInt32(nudTimeInsertPositionY.Value));
        }

        private void insertDaytimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insertDaytimeToolStripMenuItem.Checked = !insertDaytimeToolStripMenuItem.Checked;
            if (insertDaytimeToolStripMenuItem.Checked)
            {
                insertGametimeToolStripMenuItem.Checked = false;
                Settings.Default.DoInsertgameTime = false;
                _scb.InsertGameTime = false;
            }
            Settings.Default.DoInsertDayTime = insertGametimeToolStripMenuItem.Checked;
            Settings.Default.Save();
            _scb.InsertDayTime = insertDaytimeToolStripMenuItem.Checked;
        }

        private void fontDayTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = new Font(_scb.DayTimeInsertFontName, (float)_scb.DayTimeInsertFontSize, _scb.DayTimeInsertFontStyle);
            int num = (int)fontDialog1.ShowDialog();
            _scb.DayTimeInsertFontName = fontDialog1.Font.Name;
            _scb.DayTimeInsertFontSize = (int)fontDialog1.Font.Size;
            _scb.DayTimeInsertFontStyle = fontDialog1.Font.Style;
        }

        private void colorDayTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = _scb.DayTimeInsertColor;
            int num = (int)colorDialog1.ShowDialog();
            _scb.DayTimeInsertColor = colorDialog1.Color;
        }

        private void locationDayTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gbDaytimeInsertPosition.Visible = true;
            gbDaytimeInsertPosition.BringToFront();
            gbDaytimeInsertPosition.Refresh();
            try
            {
                nudDayTimeInsertPositionX.Value = (Decimal)Settings.Default.DayTimeInsertLocation.X;
                nudDayTimeInsertPositionY.Value = (Decimal)Settings.Default.DayTimeInsertLocation.Y;
            }
            catch
            {
            }
        }

        private void btnDaytimeInsertPositionSave_Click(object sender, EventArgs e)
        {
            gbDaytimeInsertPosition.Visible = false;
            Settings.Default.DayTimeInsertLocation = _scb.DayTimeInsertLocation;
            Settings.Default.Save();
        }

        private void btnDaytimeInsertPositionCancel_Click(object sender, EventArgs e)
        {
            gbDaytimeInsertPosition.Visible = false;
            _scb.DayTimeInsertLocation = Settings.Default.DayTimeInsertLocation;
        }

        private void nudDayTimeInsertPositionX_ValueChanged(object sender, EventArgs e)
        {
            _scb.DayTimeInsertLocation = new Point(Convert.ToInt32(nudDayTimeInsertPositionX.Value), Convert.ToInt32(nudDayTimeInsertPositionY.Value));
        }

        private void nudDayTimeInsertPositionY_ValueChanged(object sender, EventArgs e)
        {
            _scb.DayTimeInsertLocation = new Point(Convert.ToInt32(nudDayTimeInsertPositionX.Value), Convert.ToInt32(nudDayTimeInsertPositionY.Value));
        }

        private void timerDayTime_Tick(object sender, EventArgs e)
        {
            lblDayTime.Text = DateTime.Now.ToLongTimeString();
            if (progressBar1.Visible)
                return;
            _scb.DayTime = lblDayTime.Text;
        }

        private void lblDayTime_TextChanged(object sender, EventArgs e)
        {
            StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\Dummy.txt", false);
            streamWriter.WriteLine("ABC");
            streamWriter.Flush();
            streamWriter.Close();
        }

        private void goalGuestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sport = "Soccer";
            _background_image_name_2save = ((ToolStripItem)sender).Tag.ToString();
            int num = (int)openFileDialog1.ShowDialog();
        }

        private void lbAnimations_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;
            e.Effect = DragDropEffects.Copy;
        }

        private void lbAnimations_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (data.Length > 0)
                Cursor.Current = Cursors.WaitCursor;
            for (int index = 0; index < data.Length; ++index)
            {
                string sourceFileName = data[index];
                string str = sourceFileName.Substring(sourceFileName.LastIndexOf('\\') + 1);
                if (data[index].ToLower().EndsWith(".avi") || data[index].ToLower().EndsWith(".mpg") || (data[index].ToLower().EndsWith(".mpeg") || data[index].ToLower().EndsWith(".mp4")) || (data[index].ToLower().EndsWith(".wmv") || data[index].ToLower().EndsWith(".vob") || data[index].ToLower().EndsWith(".mov")))
                {
                    DialogResult dialogResult = DialogResult.OK;
                    if (File.Exists(_animation_path + "\\" + str))
                        dialogResult = MessageBox.Show("Datei " + str + " existiert im Arbeitsverzeichnis schon! Überschreiben ?", "Achtung:", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        try
                        {
                            if (sourceFileName != _animation_path + "\\" + str)
                                File.Copy(sourceFileName, _animation_path + "\\" + str, true);
                        }
                        catch
                        {
                            int num = (int)MessageBox.Show("Datei ist in Benutzung und konnte nicht überschrieben werden!");
                        }
                    }
                    _fill_animations();
                    lbAnimations.SelectedItem = (object)str;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    int num1 = (int)MessageBox.Show("Datei " + str + " wurde nicht kopiert!", "Unzulässiges Dateiformat");
                }
            }
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;
            e.Effect = DragDropEffects.Copy;
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (data.Length > 0)
                Cursor.Current = Cursors.WaitCursor;
            for (int index = 0; index < data.Length; ++index)
            {
                string sourceFileName = data[index];
                string str = sourceFileName.Substring(sourceFileName.LastIndexOf('\\') + 1);
                if (data[index].ToLower().EndsWith(".jpg") || data[index].ToString().ToLower().EndsWith(".bmp") || (data[index].ToLower().EndsWith(".tif") || data[index].ToLower().EndsWith(".gif")))
                {
                    DialogResult dialogResult = DialogResult.OK;
                    if (File.Exists(_picture_path + "\\" + str))
                        dialogResult = MessageBox.Show("Datei " + str + " existiert im Arbeitsverzeichnis schon! Überschreiben ?", "Achtung:", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        try
                        {
                            if (sourceFileName != _picture_path + "\\" + str)
                                File.Copy(sourceFileName, _picture_path + "\\" + str, true);
                        }
                        catch
                        {
                            int num = (int)MessageBox.Show("Datei ist in Benutzung und konnte nicht überschrieben werden!");
                        }
                    }
                    _fill_previews();
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    int num1 = (int)MessageBox.Show("Datei " + str + " wurde nicht kopiert!", "Unzulässiges Dateiformat");
                }
            }
        }

        private void contextMenuStrip5_Opening(object sender, CancelEventArgs e)
        {
            insertAnimationToolStripMenuItem.Enabled = false;
            StringCollection fileDropList = Clipboard.GetFileDropList();
            if (fileDropList.Count <= 0)
                return;
            for (int index = 0; index < fileDropList.Count; ++index)
            {
                if (fileDropList[index].ToLower().EndsWith(".avi") || fileDropList[index].ToLower().EndsWith(".mpg") || (fileDropList[index].ToLower().EndsWith(".mpeg") || fileDropList[index].ToLower().EndsWith(".mp4")) || (fileDropList[index].ToLower().EndsWith(".vob") || fileDropList[index].ToLower().EndsWith(".wmv") || fileDropList[index].ToLower().EndsWith(".mov")))
                    insertAnimationToolStripMenuItem.Enabled = true;
            }
        }

        private void contextMenuStrip6_Opening(object sender, CancelEventArgs e)
        {
            insertPictureToolStripMenuItem3.Enabled = false;
            StringCollection fileDropList = Clipboard.GetFileDropList();
            if (fileDropList.Count <= 0)
                return;
            for (int index = 0; index < fileDropList.Count; ++index)
            {
                if (fileDropList[index].ToLower().EndsWith(".bmp") || fileDropList[index].ToLower().EndsWith(".jpg") || (fileDropList[index].ToLower().EndsWith(".jpeg") || fileDropList[index].ToLower().EndsWith(".tif")) || fileDropList[index].ToLower().EndsWith(".gif"))
                    insertPictureToolStripMenuItem3.Enabled = true;
            }
        }

        private void insertAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            StringCollection fileDropList = Clipboard.GetFileDropList();
            if (fileDropList.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int index = 0; index < fileDropList.Count; ++index)
                {
                    string sourceFileName = fileDropList[index];
                    str = sourceFileName.Substring(sourceFileName.LastIndexOf('\\') + 1);
                    if (File.Exists(_animation_path + "\\" + str))
                    {
                        if (MessageBox.Show("Datei " + str + " existiert im Arbeitsverzeichnis schon! Überschreiben ?", "Achtung:", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            try
                            {
                                if (sourceFileName != _animation_path + "\\" + str)
                                    File.Copy(sourceFileName, _animation_path + "\\" + str, true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                int num = (int)MessageBox.Show("Datei ist in Benutzung und konnte nicht überschrieben werden!");
                            }
                        }
                    }
                    else if (sourceFileName != _animation_path + "\\" + str)
                        File.Copy(sourceFileName, _animation_path + "\\" + str);
                }
            }
            _fill_animations();
            lbAnimations.SelectedItem = (object)str;
            Cursor.Current = Cursors.Default;
        }

        private void insertPictureToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string empty = string.Empty;
            StringCollection fileDropList = Clipboard.GetFileDropList();
            if (fileDropList.Count > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                for (int index = 0; index < fileDropList.Count; ++index)
                {
                    string sourceFileName = fileDropList[index];
                    string str = sourceFileName.Substring(sourceFileName.LastIndexOf('\\') + 1);
                    if (File.Exists(_picture_path + "\\" + str))
                    {
                        if (MessageBox.Show("Datei " + str + " existiert im Arbeitsverzeichnis schon! Überschreiben ?", "Achtung:", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            try
                            {
                                if (sourceFileName != _picture_path + "\\" + str)
                                    File.Copy(sourceFileName, _picture_path + "\\" + str, true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                int num = (int)MessageBox.Show("Datei ist in Benutzung und konnte nicht überschrieben werden!");
                            }
                        }
                    }
                    else if (sourceFileName != _picture_path + "\\" + str)
                        File.Copy(sourceFileName, _picture_path + "\\" + str);
                }
            }
            _fill_previews();
            Cursor.Current = Cursors.Default;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
            folderBrowserDialog1 = new FolderBrowserDialog();
            btnClearSCB = new Button();
            btnShowScore = new Button();
            btnShowVideo = new Button();
            rbSmallGraphic = new RadioButton();
            rbFullGraphic = new RadioButton();
            contextMenuStrip1 = new ContextMenuStrip(components);
            tsmNewSequence = new ToolStripMenuItem();
            tsmPlaySequence = new ToolStripMenuItem();
            tmsPlaySequenceEndless = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmDeleteSequence = new ToolStripMenuItem();
            contextMenuStrip2 = new ContextMenuStrip(components);
            tsmAddNewPictureToSequence = new ToolStripMenuItem();
            tsmAddAnimation = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            insertScoreDisplayToolStripMenuItem = new ToolStripMenuItem();
            bildAusMedienverzeichnisHinzufügenToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            tsmMoveUpMediaInSequence = new ToolStripMenuItem();
            tsmMoveDownMediaInSequence = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            tsmDeleteMediaFromSequence = new ToolStripMenuItem();
            ofdPictures = new OpenFileDialog();
            ofdAnimations = new OpenFileDialog();
            groupBox4 = new GroupBox();
            lblSelectedAnimationName = new Label();
            btnShowAnimation = new Button();
            btnSelectAnimationPath = new Button();
            pbAnimationPreview = new PictureBox();
            lbAnimations = new ListBox();
            contextMenuStrip5 = new ContextMenuStrip(components);
            insertAnimationToolStripMenuItem = new ToolStripMenuItem();
            groupBox3 = new GroupBox();
            btnVideoSnapShot = new Button();
            cmbVideoSource = new ComboBox();
            pnlVideoPreview = new Panel();
            contextMenuStrip3 = new ContextMenuStrip(components);
            stretchVideoToDisplaySizeToolStripMenuItem = new ToolStripMenuItem();
            label5 = new Label();
            label3 = new Label();
            btnShowSingleStep = new Button();
            txtInputNewSequenceName = new TextBox();
            lbSequenceContent = new ListBox();
            groupBox1 = new GroupBox();
            btnGong = new Button();
            progressBar1 = new ProgressBar();
            label2 = new Label();
            cmbEffect = new ComboBox();
            btnSelectPicturePath = new Button();
            panel1 = new Panel();
            contextMenuStrip6 = new ContextMenuStrip(components);
            insertPictureToolStripMenuItem3 = new ToolStripMenuItem();
            gbDaytimeInsertPosition = new Panel();
            label9 = new Label();
            nudDayTimeInsertPositionX = new NumericUpDown();
            btnDaytimeInsertPositionSave = new Button();
            btnDaytimeInsertPositionCancel = new Button();
            nudDayTimeInsertPositionY = new NumericUpDown();
            label10 = new Label();
            label11 = new Label();
            gbTimeInsertPosition = new Panel();
            label8 = new Label();
            nudTimeInsertPositionX = new NumericUpDown();
            btnTimeInsertPositionSave = new Button();
            btnTimeInsertPositionCancel = new Button();
            nudTimeInsertPositionY = new NumericUpDown();
            label7 = new Label();
            label6 = new Label();
            label4 = new Label();
            label1 = new Label();
            folderBrowserDialog2 = new FolderBrowserDialog();
            cmbSelectKindOfGame = new ComboBox();
            btnFreeText = new Button();
            cbAllowGraphicFromLAN = new CheckBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            btnRefreshPPT_List = new Button();
            btnShowPPT = new Button();
            btnSelectPPT_Path = new Button();
            lbAvailablePPTs = new ListBox();
            contextMenuStrip4 = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            gametimeInsertParameterToolStripMenuItem = new ToolStripMenuItem();
            insertGametimeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            fontToolStripMenuItem = new ToolStripMenuItem();
            colorToolStripMenuItem = new ToolStripMenuItem();
            locationToolStripMenuItem = new ToolStripMenuItem();
            daytimeInsertParameterToolStripMenuItem = new ToolStripMenuItem();
            insertDaytimeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            fontDayTimeToolStripMenuItem = new ToolStripMenuItem();
            colorDayTimeToolStripMenuItem = new ToolStripMenuItem();
            locationDayTimeToolStripMenuItem = new ToolStripMenuItem();
            backgoundImagesToolStripMenuItem = new ToolStripMenuItem();
            soccerToolStripMenuItem1 = new ToolStripMenuItem();
            soccerToolStripMenuItem = new ToolStripMenuItem();
            introGuestteamToolStripMenuItem = new ToolStripMenuItem();
            introHomeReserveToolStripMenuItem = new ToolStripMenuItem();
            introGuestReserveToolStripMenuItem = new ToolStripMenuItem();
            introSinglePlayerToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            introCoachesHomeTeamToolStripMenuItem = new ToolStripMenuItem();
            introCoachesGuestTeamToolStripMenuItem = new ToolStripMenuItem();
            introRefereesToolStripMenuItem = new ToolStripMenuItem();
            scoreToolStripMenuItem = new ToolStripMenuItem();
            cornersToolStripMenuItem = new ToolStripMenuItem();
            scorerToolStripMenuItem = new ToolStripMenuItem();
            goalHomeToolStripMenuItem = new ToolStripMenuItem();
            goalGuestToolStripMenuItem = new ToolStripMenuItem();
            changeOutToolStripMenuItem = new ToolStripMenuItem();
            changeInToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            yellowCardToolStripMenuItem = new ToolStripMenuItem();
            redCardToolStripMenuItem = new ToolStripMenuItem();
            yellowredCardToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            flashtableToolStripMenuItem = new ToolStripMenuItem();
            resultTableToolStripMenuItem = new ToolStripMenuItem();
            resultToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            groupBox2 = new GroupBox();
            cmbAvailableSequences = new ComboBox();
            openFileDialog2 = new OpenFileDialog();
            fontDialog1 = new FontDialog();
            colorDialog1 = new ColorDialog();
            lblDayTime = new Label();
            timerDayTime = new System.Windows.Forms.Timer(components);
            toolTip1 = new ToolTip(components);
            lblProgramVersion = new Label();
            contextMenuStrip1.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            groupBox4.SuspendLayout();
            ((ISupportInitialize)pbAnimationPreview).BeginInit();
            contextMenuStrip5.SuspendLayout();
            groupBox3.SuspendLayout();
            contextMenuStrip3.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            contextMenuStrip6.SuspendLayout();
            gbDaytimeInsertPosition.SuspendLayout();
            nudDayTimeInsertPositionX.BeginInit();
            nudDayTimeInsertPositionY.BeginInit();
            gbTimeInsertPosition.SuspendLayout();
            nudTimeInsertPositionX.BeginInit();
            nudTimeInsertPositionY.BeginInit();
            tabControl1.SuspendLayout();
            tabPage2.SuspendLayout();
            contextMenuStrip4.SuspendLayout();
            menuStrip1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            componentResourceManager.ApplyResources((object)folderBrowserDialog1, "folderBrowserDialog1");
            componentResourceManager.ApplyResources((object)btnClearSCB, "btnClearSCB");
            btnClearSCB.ForeColor = SystemColors.ControlText;
            btnClearSCB.Name = "btnClearSCB";
            toolTip1.SetToolTip((Control)btnClearSCB, componentResourceManager.GetString("btnClearSCB.ToolTip"));
            btnClearSCB.UseVisualStyleBackColor = true;
            btnClearSCB.Click += new EventHandler(btnClearSCB_Click);
            componentResourceManager.ApplyResources((object)btnShowScore, "btnShowScore");
            btnShowScore.ForeColor = SystemColors.ControlText;
            btnShowScore.Name = "btnShowScore";
            toolTip1.SetToolTip((Control)btnShowScore, componentResourceManager.GetString("btnShowScore.ToolTip"));
            btnShowScore.UseVisualStyleBackColor = true;
            btnShowScore.Click += new EventHandler(btnShowScore_Click);
            componentResourceManager.ApplyResources((object)btnShowVideo, "btnShowVideo");
            btnShowVideo.ForeColor = SystemColors.ControlText;
            btnShowVideo.Name = "btnShowVideo";
            toolTip1.SetToolTip((Control)btnShowVideo, componentResourceManager.GetString("btnShowVideo.ToolTip"));
            btnShowVideo.UseVisualStyleBackColor = true;
            btnShowVideo.Click += new EventHandler(btnShowVideo_Click);
            componentResourceManager.ApplyResources((object)rbSmallGraphic, "rbSmallGraphic");
            rbSmallGraphic.Checked = true;
            rbSmallGraphic.ForeColor = Color.White;
            rbSmallGraphic.Name = "rbSmallGraphic";
            rbSmallGraphic.TabStop = true;
            toolTip1.SetToolTip((Control)rbSmallGraphic, componentResourceManager.GetString("rbSmallGraphic.ToolTip"));
            rbSmallGraphic.UseVisualStyleBackColor = true;
            rbSmallGraphic.Click += new EventHandler(rbFullGraphic_Click);
            rbSmallGraphic.MouseUp += new MouseEventHandler(rbSmallGraphic_MouseUp);
            componentResourceManager.ApplyResources((object)rbFullGraphic, "rbFullGraphic");
            rbFullGraphic.ForeColor = Color.White;
            rbFullGraphic.Name = "rbFullGraphic";
            toolTip1.SetToolTip((Control)rbFullGraphic, componentResourceManager.GetString("rbFullGraphic.ToolTip"));
            rbFullGraphic.UseVisualStyleBackColor = true;
            rbFullGraphic.Click += new EventHandler(rbFullGraphic_Click);
            rbFullGraphic.MouseUp += new MouseEventHandler(rbSmallGraphic_MouseUp);
            componentResourceManager.ApplyResources((object)contextMenuStrip1, "contextMenuStrip1");
            contextMenuStrip1.Items.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) tsmNewSequence,
        (ToolStripItem) tsmPlaySequence,
        (ToolStripItem) tmsPlaySequenceEndless,
        (ToolStripItem) toolStripSeparator1,
        (ToolStripItem) tsmDeleteSequence
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            toolTip1.SetToolTip((Control)contextMenuStrip1, componentResourceManager.GetString("contextMenuStrip1.ToolTip"));
            componentResourceManager.ApplyResources((object)tsmNewSequence, "tsmNewSequence");
            tsmNewSequence.Name = "tsmNewSequence";
            tsmNewSequence.Click += new EventHandler(tsmNewSequence_Click);
            componentResourceManager.ApplyResources((object)tsmPlaySequence, "tsmPlaySequence");
            tsmPlaySequence.Name = "tsmPlaySequence";
            tsmPlaySequence.Click += new EventHandler(tsmPlaySequence_Click);
            componentResourceManager.ApplyResources((object)tmsPlaySequenceEndless, "tmsPlaySequenceEndless");
            tmsPlaySequenceEndless.Name = "tmsPlaySequenceEndless";
            tmsPlaySequenceEndless.Click += new EventHandler(tmsPlaySequenceEndless_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator1, "toolStripSeparator1");
            toolStripSeparator1.Name = "toolStripSeparator1";
            componentResourceManager.ApplyResources((object)tsmDeleteSequence, "tsmDeleteSequence");
            tsmDeleteSequence.Name = "tsmDeleteSequence";
            tsmDeleteSequence.Click += new EventHandler(tsmDeleteSequence_Click);
            componentResourceManager.ApplyResources((object)contextMenuStrip2, "contextMenuStrip2");
            contextMenuStrip2.Items.AddRange(new ToolStripItem[10]
            {
        (ToolStripItem) tsmAddNewPictureToSequence,
        (ToolStripItem) tsmAddAnimation,
        (ToolStripItem) toolStripSeparator4,
        (ToolStripItem) insertScoreDisplayToolStripMenuItem,
        (ToolStripItem) bildAusMedienverzeichnisHinzufügenToolStripMenuItem,
        (ToolStripItem) toolStripSeparator5,
        (ToolStripItem) tsmMoveUpMediaInSequence,
        (ToolStripItem) tsmMoveDownMediaInSequence,
        (ToolStripItem) toolStripSeparator2,
        (ToolStripItem) tsmDeleteMediaFromSequence
            });
            contextMenuStrip2.Name = "contextMenuStrip2";
            toolTip1.SetToolTip((Control)contextMenuStrip2, componentResourceManager.GetString("contextMenuStrip2.ToolTip"));
            componentResourceManager.ApplyResources((object)tsmAddNewPictureToSequence, "tsmAddNewPictureToSequence");
            tsmAddNewPictureToSequence.Name = "tsmAddNewPictureToSequence";
            tsmAddNewPictureToSequence.Click += new EventHandler(tsmAddNewPictureToSequence_Click);
            componentResourceManager.ApplyResources((object)tsmAddAnimation, "tsmAddAnimation");
            tsmAddAnimation.Name = "tsmAddAnimation";
            tsmAddAnimation.Click += new EventHandler(tsmAddAnimation_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator4, "toolStripSeparator4");
            toolStripSeparator4.Name = "toolStripSeparator4";
            componentResourceManager.ApplyResources((object)insertScoreDisplayToolStripMenuItem, "insertScoreDisplayToolStripMenuItem");
            insertScoreDisplayToolStripMenuItem.Name = "insertScoreDisplayToolStripMenuItem";
            insertScoreDisplayToolStripMenuItem.Click += new EventHandler(insertScoreDisplayToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)bildAusMedienverzeichnisHinzufügenToolStripMenuItem, "bildAusMedienverzeichnisHinzufügenToolStripMenuItem");
            bildAusMedienverzeichnisHinzufügenToolStripMenuItem.Name = "bildAusMedienverzeichnisHinzufügenToolStripMenuItem";
            bildAusMedienverzeichnisHinzufügenToolStripMenuItem.Click += new EventHandler(bildAusMedienverzeichnisHinzufügenToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator5, "toolStripSeparator5");
            toolStripSeparator5.Name = "toolStripSeparator5";
            componentResourceManager.ApplyResources((object)tsmMoveUpMediaInSequence, "tsmMoveUpMediaInSequence");
            tsmMoveUpMediaInSequence.Name = "tsmMoveUpMediaInSequence";
            tsmMoveUpMediaInSequence.Click += new EventHandler(tsmMoveUpMediaInSequence_Click);
            componentResourceManager.ApplyResources((object)tsmMoveDownMediaInSequence, "tsmMoveDownMediaInSequence");
            tsmMoveDownMediaInSequence.Name = "tsmMoveDownMediaInSequence";
            tsmMoveDownMediaInSequence.Click += new EventHandler(tsmMoveDownMediaInSequence_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator2, "toolStripSeparator2");
            toolStripSeparator2.Name = "toolStripSeparator2";
            componentResourceManager.ApplyResources((object)tsmDeleteMediaFromSequence, "tsmDeleteMediaFromSequence");
            tsmDeleteMediaFromSequence.Name = "tsmDeleteMediaFromSequence";
            tsmDeleteMediaFromSequence.Click += new EventHandler(tsmDeleteMediaFromSequence_Click);
            componentResourceManager.ApplyResources((object)ofdPictures, "ofdPictures");
            ofdPictures.InitialDirectory = "c:\\";
            ofdPictures.FileOk += new CancelEventHandler(ofdPictures_FileOk);
            componentResourceManager.ApplyResources((object)ofdAnimations, "ofdAnimations");
            ofdAnimations.InitialDirectory = "c:\\";
            ofdAnimations.FileOk += new CancelEventHandler(ofdAnimations_FileOk);
            componentResourceManager.ApplyResources((object)groupBox4, "groupBox4");
            groupBox4.Controls.Add((Control)lblSelectedAnimationName);
            groupBox4.Controls.Add((Control)btnShowAnimation);
            groupBox4.Controls.Add((Control)btnSelectAnimationPath);
            groupBox4.Controls.Add((Control)pbAnimationPreview);
            groupBox4.Controls.Add((Control)lbAnimations);
            groupBox4.ForeColor = Color.White;
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            toolTip1.SetToolTip((Control)groupBox4, componentResourceManager.GetString("groupBox4.ToolTip"));
            componentResourceManager.ApplyResources((object)lblSelectedAnimationName, "lblSelectedAnimationName");
            lblSelectedAnimationName.Name = "lblSelectedAnimationName";
            toolTip1.SetToolTip((Control)lblSelectedAnimationName, componentResourceManager.GetString("lblSelectedAnimationName.ToolTip"));
            componentResourceManager.ApplyResources((object)btnShowAnimation, "btnShowAnimation");
            btnShowAnimation.ForeColor = SystemColors.ControlText;
            btnShowAnimation.Name = "btnShowAnimation";
            toolTip1.SetToolTip((Control)btnShowAnimation, componentResourceManager.GetString("btnShowAnimation.ToolTip"));
            btnShowAnimation.UseVisualStyleBackColor = true;
            btnShowAnimation.Click += new EventHandler(btnShowAnimation_Click_1);
            componentResourceManager.ApplyResources((object)btnSelectAnimationPath, "btnSelectAnimationPath");
            btnSelectAnimationPath.ForeColor = SystemColors.ControlText;
            btnSelectAnimationPath.Name = "btnSelectAnimationPath";
            toolTip1.SetToolTip((Control)btnSelectAnimationPath, componentResourceManager.GetString("btnSelectAnimationPath.ToolTip"));
            btnSelectAnimationPath.UseVisualStyleBackColor = true;
            btnSelectAnimationPath.Click += new EventHandler(btnSelectAnimationPath_Click);
            componentResourceManager.ApplyResources((object)pbAnimationPreview, "pbAnimationPreview");
            pbAnimationPreview.BorderStyle = BorderStyle.FixedSingle;
            pbAnimationPreview.Name = "pbAnimationPreview";
            pbAnimationPreview.TabStop = false;
            toolTip1.SetToolTip((Control)pbAnimationPreview, componentResourceManager.GetString("pbAnimationPreview.ToolTip"));
            componentResourceManager.ApplyResources((object)lbAnimations, "lbAnimations");
            lbAnimations.AllowDrop = true;
            lbAnimations.ContextMenuStrip = contextMenuStrip5;
            lbAnimations.FormattingEnabled = true;
            lbAnimations.Name = "lbAnimations";
            toolTip1.SetToolTip((Control)lbAnimations, componentResourceManager.GetString("lbAnimations.ToolTip"));
            lbAnimations.SelectedIndexChanged += new EventHandler(lbAnimations_SelectedIndexChanged_1);
            lbAnimations.DragDrop += new DragEventHandler(lbAnimations_DragDrop);
            lbAnimations.DragEnter += new DragEventHandler(lbAnimations_DragEnter);
            lbAnimations.Leave += new EventHandler(lbAnimations_Leave);
            componentResourceManager.ApplyResources((object)contextMenuStrip5, "contextMenuStrip5");
            contextMenuStrip5.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) insertAnimationToolStripMenuItem
            });
            contextMenuStrip5.Name = "contextMenuStrip5";
            toolTip1.SetToolTip((Control)contextMenuStrip5, componentResourceManager.GetString("contextMenuStrip5.ToolTip"));
            contextMenuStrip5.Opening += new CancelEventHandler(contextMenuStrip5_Opening);
            componentResourceManager.ApplyResources((object)insertAnimationToolStripMenuItem, "insertAnimationToolStripMenuItem");
            insertAnimationToolStripMenuItem.Name = "insertAnimationToolStripMenuItem";
            insertAnimationToolStripMenuItem.Click += new EventHandler(insertAnimationToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)groupBox3, "groupBox3");
            groupBox3.Controls.Add((Control)btnVideoSnapShot);
            groupBox3.Controls.Add((Control)cmbVideoSource);
            groupBox3.Controls.Add((Control)pnlVideoPreview);
            groupBox3.Controls.Add((Control)label5);
            groupBox3.Controls.Add((Control)btnShowVideo);
            groupBox3.ForeColor = Color.White;
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            toolTip1.SetToolTip((Control)groupBox3, componentResourceManager.GetString("groupBox3.ToolTip"));
            componentResourceManager.ApplyResources((object)btnVideoSnapShot, "btnVideoSnapShot");
            btnVideoSnapShot.Image = null;
            btnVideoSnapShot.Name = "btnVideoSnapShot";
            toolTip1.SetToolTip((Control)btnVideoSnapShot, componentResourceManager.GetString("btnVideoSnapShot.ToolTip"));
            btnVideoSnapShot.UseVisualStyleBackColor = true;
            btnVideoSnapShot.Click += new EventHandler(btnVideoSnapShot_Click);
            componentResourceManager.ApplyResources((object)cmbVideoSource, "cmbVideoSource");
            cmbVideoSource.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVideoSource.FormattingEnabled = true;
            cmbVideoSource.Items.AddRange(new object[1]
            {
        (object) componentResourceManager.GetString("cmbVideoSource.Items")
            });
            cmbVideoSource.Name = "cmbVideoSource";
            toolTip1.SetToolTip((Control)cmbVideoSource, componentResourceManager.GetString("cmbVideoSource.ToolTip"));
            componentResourceManager.ApplyResources((object)pnlVideoPreview, "pnlVideoPreview");
            pnlVideoPreview.BackColor = Color.Maroon;
            pnlVideoPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlVideoPreview.ContextMenuStrip = contextMenuStrip3;
            pnlVideoPreview.Name = "pnlVideoPreview";
            toolTip1.SetToolTip((Control)pnlVideoPreview, componentResourceManager.GetString("pnlVideoPreview.ToolTip"));
            componentResourceManager.ApplyResources((object)contextMenuStrip3, "contextMenuStrip3");
            contextMenuStrip3.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) stretchVideoToDisplaySizeToolStripMenuItem
            });
            contextMenuStrip3.Name = "contextMenuStrip3";
            toolTip1.SetToolTip((Control)contextMenuStrip3, componentResourceManager.GetString("contextMenuStrip3.ToolTip"));
            componentResourceManager.ApplyResources((object)stretchVideoToDisplaySizeToolStripMenuItem, "stretchVideoToDisplaySizeToolStripMenuItem");
            stretchVideoToDisplaySizeToolStripMenuItem.Checked = Settings.Default.StretchVideoToDisplaySize;
            stretchVideoToDisplaySizeToolStripMenuItem.Name = "stretchVideoToDisplaySizeToolStripMenuItem";
            stretchVideoToDisplaySizeToolStripMenuItem.Click += new EventHandler(stretchVideoToDisplaySizeToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)label5, "label5");
            label5.ForeColor = Color.White;
            label5.Name = "label5";
            toolTip1.SetToolTip((Control)label5, componentResourceManager.GetString("label5.ToolTip"));
            componentResourceManager.ApplyResources((object)label3, "label3");
            label3.BackColor = Color.FromArgb(42, 0, 0);
            label3.ForeColor = Color.White;
            label3.Name = "label3";
            toolTip1.SetToolTip((Control)label3, componentResourceManager.GetString("label3.ToolTip"));
            componentResourceManager.ApplyResources((object)btnShowSingleStep, "btnShowSingleStep");
            btnShowSingleStep.BackColor = SystemColors.Control;
            btnShowSingleStep.ForeColor = SystemColors.ControlText;
            btnShowSingleStep.Name = "btnShowSingleStep";
            toolTip1.SetToolTip((Control)btnShowSingleStep, componentResourceManager.GetString("btnShowSingleStep.ToolTip"));
            btnShowSingleStep.UseVisualStyleBackColor = false;
            btnShowSingleStep.Click += new EventHandler(btnShowSingleStep_Click);
            componentResourceManager.ApplyResources((object)txtInputNewSequenceName, "txtInputNewSequenceName");
            txtInputNewSequenceName.BackColor = SystemColors.Control;
            txtInputNewSequenceName.Name = "txtInputNewSequenceName";
            toolTip1.SetToolTip((Control)txtInputNewSequenceName, componentResourceManager.GetString("txtInputNewSequenceName.ToolTip"));
            componentResourceManager.ApplyResources((object)lbSequenceContent, "lbSequenceContent");
            lbSequenceContent.BackColor = SystemColors.Control;
            lbSequenceContent.ContextMenuStrip = contextMenuStrip2;
            lbSequenceContent.FormattingEnabled = true;
            lbSequenceContent.Name = "lbSequenceContent";
            toolTip1.SetToolTip((Control)lbSequenceContent, componentResourceManager.GetString("lbSequenceContent.ToolTip"));
            componentResourceManager.ApplyResources((object)groupBox1, "groupBox1");
            groupBox1.Controls.Add((Control)btnGong);
            groupBox1.Controls.Add((Control)progressBar1);
            groupBox1.Controls.Add((Control)label2);
            groupBox1.Controls.Add((Control)cmbEffect);
            groupBox1.Controls.Add((Control)btnSelectPicturePath);
            groupBox1.Controls.Add((Control)panel1);
            groupBox1.ForeColor = Color.White;
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            toolTip1.SetToolTip((Control)groupBox1, componentResourceManager.GetString("groupBox1.ToolTip"));
            componentResourceManager.ApplyResources((object)btnGong, "btnGong");
            btnGong.ForeColor = SystemColors.ControlText;
            btnGong.Name = "btnGong";
            toolTip1.SetToolTip((Control)btnGong, componentResourceManager.GetString("btnGong.ToolTip"));
            btnGong.UseVisualStyleBackColor = true;
            btnGong.MouseClick += new MouseEventHandler(btnGong_MouseClick);
            componentResourceManager.ApplyResources((object)progressBar1, "progressBar1");
            progressBar1.BackColor = Color.Red;
            progressBar1.ForeColor = Color.Red;
            progressBar1.Name = "progressBar1";
            progressBar1.Step = 1;
            progressBar1.Style = ProgressBarStyle.Marquee;
            toolTip1.SetToolTip((Control)progressBar1, componentResourceManager.GetString("progressBar1.ToolTip"));
            progressBar1.UseWaitCursor = true;
            componentResourceManager.ApplyResources((object)label2, "label2");
            label2.ForeColor = Color.White;
            label2.Name = "label2";
            toolTip1.SetToolTip((Control)label2, componentResourceManager.GetString("label2.ToolTip"));
            componentResourceManager.ApplyResources((object)cmbEffect, "cmbEffect");
            cmbEffect.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEffect.FormattingEnabled = true;
            cmbEffect.Items.AddRange(new object[29]
            {
        (object) componentResourceManager.GetString("cmbEffect.Items"),
        (object) componentResourceManager.GetString("cmbEffect.Items1"),
        (object) componentResourceManager.GetString("cmbEffect.Items2"),
        (object) componentResourceManager.GetString("cmbEffect.Items3"),
        (object) componentResourceManager.GetString("cmbEffect.Items4"),
        (object) componentResourceManager.GetString("cmbEffect.Items5"),
        (object) componentResourceManager.GetString("cmbEffect.Items6"),
        (object) componentResourceManager.GetString("cmbEffect.Items7"),
        (object) componentResourceManager.GetString("cmbEffect.Items8"),
        (object) componentResourceManager.GetString("cmbEffect.Items9"),
        (object) componentResourceManager.GetString("cmbEffect.Items10"),
        (object) componentResourceManager.GetString("cmbEffect.Items11"),
        (object) componentResourceManager.GetString("cmbEffect.Items12"),
        (object) componentResourceManager.GetString("cmbEffect.Items13"),
        (object) componentResourceManager.GetString("cmbEffect.Items14"),
        (object) componentResourceManager.GetString("cmbEffect.Items15"),
        (object) componentResourceManager.GetString("cmbEffect.Items16"),
        (object) componentResourceManager.GetString("cmbEffect.Items17"),
        (object) componentResourceManager.GetString("cmbEffect.Items18"),
        (object) componentResourceManager.GetString("cmbEffect.Items19"),
        (object) componentResourceManager.GetString("cmbEffect.Items20"),
        (object) componentResourceManager.GetString("cmbEffect.Items21"),
        (object) componentResourceManager.GetString("cmbEffect.Items22"),
        (object) componentResourceManager.GetString("cmbEffect.Items23"),
        (object) componentResourceManager.GetString("cmbEffect.Items24"),
        (object) componentResourceManager.GetString("cmbEffect.Items25"),
        (object) componentResourceManager.GetString("cmbEffect.Items26"),
        (object) componentResourceManager.GetString("cmbEffect.Items27"),
        (object) componentResourceManager.GetString("cmbEffect.Items28")
            });
            cmbEffect.Name = "cmbEffect";
            toolTip1.SetToolTip((Control)cmbEffect, componentResourceManager.GetString("cmbEffect.ToolTip"));
            cmbEffect.SelectedIndexChanged += new EventHandler(cmbEffect_SelectedIndexChanged);
            cmbEffect.DropDownClosed += new EventHandler(cmbEffect_DropDownClosed);
            componentResourceManager.ApplyResources((object)btnSelectPicturePath, "btnSelectPicturePath");
            btnSelectPicturePath.ForeColor = SystemColors.ControlText;
            btnSelectPicturePath.Name = "btnSelectPicturePath";
            toolTip1.SetToolTip((Control)btnSelectPicturePath, componentResourceManager.GetString("btnSelectPicturePath.ToolTip"));
            btnSelectPicturePath.UseVisualStyleBackColor = true;
            btnSelectPicturePath.Click += new EventHandler(btnSelectPicturePath_Click);
            componentResourceManager.ApplyResources((object)panel1, "panel1");
            panel1.AllowDrop = true;
            panel1.ContextMenuStrip = contextMenuStrip6;
            panel1.Controls.Add((Control)gbDaytimeInsertPosition);
            panel1.Controls.Add((Control)gbTimeInsertPosition);
            panel1.Controls.Add((Control)label4);
            panel1.Name = "panel1";
            toolTip1.SetToolTip((Control)panel1, componentResourceManager.GetString("panel1.ToolTip"));
            panel1.DragDrop += new DragEventHandler(panel1_DragDrop);
            panel1.DragEnter += new DragEventHandler(panel1_DragEnter);
            componentResourceManager.ApplyResources((object)contextMenuStrip6, "contextMenuStrip6");
            contextMenuStrip6.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) insertPictureToolStripMenuItem3
            });
            contextMenuStrip6.Name = "contextMenuStrip5";
            toolTip1.SetToolTip((Control)contextMenuStrip6, componentResourceManager.GetString("contextMenuStrip6.ToolTip"));
            contextMenuStrip6.Opening += new CancelEventHandler(contextMenuStrip6_Opening);
            componentResourceManager.ApplyResources((object)insertPictureToolStripMenuItem3, "insertPictureToolStripMenuItem3");
            insertPictureToolStripMenuItem3.Name = "insertPictureToolStripMenuItem3";
            insertPictureToolStripMenuItem3.Click += new EventHandler(insertPictureToolStripMenuItem3_Click);
            componentResourceManager.ApplyResources((object)gbDaytimeInsertPosition, "gbDaytimeInsertPosition");
            gbDaytimeInsertPosition.AllowDrop = true;
            gbDaytimeInsertPosition.BackColor = Color.Maroon;
            gbDaytimeInsertPosition.BorderStyle = BorderStyle.FixedSingle;
            gbDaytimeInsertPosition.Controls.Add((Control)label9);
            gbDaytimeInsertPosition.Controls.Add((Control)nudDayTimeInsertPositionX);
            gbDaytimeInsertPosition.Controls.Add((Control)btnDaytimeInsertPositionSave);
            gbDaytimeInsertPosition.Controls.Add((Control)btnDaytimeInsertPositionCancel);
            gbDaytimeInsertPosition.Controls.Add((Control)nudDayTimeInsertPositionY);
            gbDaytimeInsertPosition.Controls.Add((Control)label10);
            gbDaytimeInsertPosition.Controls.Add((Control)label11);
            gbDaytimeInsertPosition.Name = "gbDaytimeInsertPosition";
            toolTip1.SetToolTip((Control)gbDaytimeInsertPosition, componentResourceManager.GetString("gbDaytimeInsertPosition.ToolTip"));
            componentResourceManager.ApplyResources((object)label9, "label9");
            label9.ForeColor = Color.White;
            label9.Name = "label9";
            toolTip1.SetToolTip((Control)label9, componentResourceManager.GetString("label9.ToolTip"));
            componentResourceManager.ApplyResources((object)nudDayTimeInsertPositionX, "nudDayTimeInsertPositionX");
            nudDayTimeInsertPositionX.Maximum = new Decimal(new int[4]
            {
        1000,
        0,
        0,
        0
            });
            nudDayTimeInsertPositionX.Name = "nudDayTimeInsertPositionX";
            toolTip1.SetToolTip((Control)nudDayTimeInsertPositionX, componentResourceManager.GetString("nudDayTimeInsertPositionX.ToolTip"));
            nudDayTimeInsertPositionX.ValueChanged += new EventHandler(nudDayTimeInsertPositionX_ValueChanged);
            componentResourceManager.ApplyResources((object)btnDaytimeInsertPositionSave, "btnDaytimeInsertPositionSave");
            btnDaytimeInsertPositionSave.ForeColor = Color.Black;
            btnDaytimeInsertPositionSave.Name = "btnDaytimeInsertPositionSave";
            toolTip1.SetToolTip((Control)btnDaytimeInsertPositionSave, componentResourceManager.GetString("btnDaytimeInsertPositionSave.ToolTip"));
            btnDaytimeInsertPositionSave.UseVisualStyleBackColor = true;
            btnDaytimeInsertPositionSave.Click += new EventHandler(btnDaytimeInsertPositionSave_Click);
            componentResourceManager.ApplyResources((object)btnDaytimeInsertPositionCancel, "btnDaytimeInsertPositionCancel");
            btnDaytimeInsertPositionCancel.ForeColor = Color.Black;
            btnDaytimeInsertPositionCancel.Name = "btnDaytimeInsertPositionCancel";
            toolTip1.SetToolTip((Control)btnDaytimeInsertPositionCancel, componentResourceManager.GetString("btnDaytimeInsertPositionCancel.ToolTip"));
            btnDaytimeInsertPositionCancel.UseVisualStyleBackColor = true;
            btnDaytimeInsertPositionCancel.Click += new EventHandler(btnDaytimeInsertPositionCancel_Click);
            componentResourceManager.ApplyResources((object)nudDayTimeInsertPositionY, "nudDayTimeInsertPositionY");
            nudDayTimeInsertPositionY.Maximum = new Decimal(new int[4]
            {
        1000,
        0,
        0,
        0
            });
            nudDayTimeInsertPositionY.Name = "nudDayTimeInsertPositionY";
            toolTip1.SetToolTip((Control)nudDayTimeInsertPositionY, componentResourceManager.GetString("nudDayTimeInsertPositionY.ToolTip"));
            nudDayTimeInsertPositionY.ValueChanged += new EventHandler(nudDayTimeInsertPositionY_ValueChanged);
            componentResourceManager.ApplyResources((object)label10, "label10");
            label10.ForeColor = Color.White;
            label10.Name = "label10";
            toolTip1.SetToolTip((Control)label10, componentResourceManager.GetString("label10.ToolTip"));
            componentResourceManager.ApplyResources((object)label11, "label11");
            label11.ForeColor = Color.White;
            label11.Name = "label11";
            toolTip1.SetToolTip((Control)label11, componentResourceManager.GetString("label11.ToolTip"));
            componentResourceManager.ApplyResources((object)gbTimeInsertPosition, "gbTimeInsertPosition");
            gbTimeInsertPosition.AllowDrop = true;
            gbTimeInsertPosition.BackColor = Color.Maroon;
            gbTimeInsertPosition.BorderStyle = BorderStyle.FixedSingle;
            gbTimeInsertPosition.Controls.Add((Control)label8);
            gbTimeInsertPosition.Controls.Add((Control)nudTimeInsertPositionX);
            gbTimeInsertPosition.Controls.Add((Control)btnTimeInsertPositionSave);
            gbTimeInsertPosition.Controls.Add((Control)btnTimeInsertPositionCancel);
            gbTimeInsertPosition.Controls.Add((Control)nudTimeInsertPositionY);
            gbTimeInsertPosition.Controls.Add((Control)label7);
            gbTimeInsertPosition.Controls.Add((Control)label6);
            gbTimeInsertPosition.Name = "gbTimeInsertPosition";
            toolTip1.SetToolTip((Control)gbTimeInsertPosition, componentResourceManager.GetString("gbTimeInsertPosition.ToolTip"));
            componentResourceManager.ApplyResources((object)label8, "label8");
            label8.ForeColor = Color.White;
            label8.Name = "label8";
            toolTip1.SetToolTip((Control)label8, componentResourceManager.GetString("label8.ToolTip"));
            componentResourceManager.ApplyResources((object)nudTimeInsertPositionX, "nudTimeInsertPositionX");
            nudTimeInsertPositionX.Maximum = new Decimal(new int[4]
            {
        1000,
        0,
        0,
        0
            });
            nudTimeInsertPositionX.Name = "nudTimeInsertPositionX";
            toolTip1.SetToolTip((Control)nudTimeInsertPositionX, componentResourceManager.GetString("nudTimeInsertPositionX.ToolTip"));
            nudTimeInsertPositionX.ValueChanged += new EventHandler(nudTimeInsertPositionX_ValueChanged);
            componentResourceManager.ApplyResources((object)btnTimeInsertPositionSave, "btnTimeInsertPositionSave");
            btnTimeInsertPositionSave.ForeColor = Color.Black;
            btnTimeInsertPositionSave.Name = "btnTimeInsertPositionSave";
            toolTip1.SetToolTip((Control)btnTimeInsertPositionSave, componentResourceManager.GetString("btnTimeInsertPositionSave.ToolTip"));
            btnTimeInsertPositionSave.UseVisualStyleBackColor = true;
            btnTimeInsertPositionSave.Click += new EventHandler(btnTimeInsertPositionSave_Click);
            componentResourceManager.ApplyResources((object)btnTimeInsertPositionCancel, "btnTimeInsertPositionCancel");
            btnTimeInsertPositionCancel.ForeColor = Color.Black;
            btnTimeInsertPositionCancel.Name = "btnTimeInsertPositionCancel";
            toolTip1.SetToolTip((Control)btnTimeInsertPositionCancel, componentResourceManager.GetString("btnTimeInsertPositionCancel.ToolTip"));
            btnTimeInsertPositionCancel.UseVisualStyleBackColor = true;
            btnTimeInsertPositionCancel.Click += new EventHandler(btnTimeInsertPositionCancel_Click);
            componentResourceManager.ApplyResources((object)nudTimeInsertPositionY, "nudTimeInsertPositionY");
            nudTimeInsertPositionY.Maximum = new Decimal(new int[4]
            {
        1000,
        0,
        0,
        0
            });
            nudTimeInsertPositionY.Name = "nudTimeInsertPositionY";
            toolTip1.SetToolTip((Control)nudTimeInsertPositionY, componentResourceManager.GetString("nudTimeInsertPositionY.ToolTip"));
            nudTimeInsertPositionY.ValueChanged += new EventHandler(nudTimeInsertPositionX_ValueChanged);
            componentResourceManager.ApplyResources((object)label7, "label7");
            label7.ForeColor = Color.White;
            label7.Name = "label7";
            toolTip1.SetToolTip((Control)label7, componentResourceManager.GetString("label7.ToolTip"));
            componentResourceManager.ApplyResources((object)label6, "label6");
            label6.ForeColor = Color.White;
            label6.Name = "label6";
            toolTip1.SetToolTip((Control)label6, componentResourceManager.GetString("label6.ToolTip"));
            componentResourceManager.ApplyResources((object)label4, "label4");
            label4.ForeColor = Color.White;
            label4.Name = "label4";
            toolTip1.SetToolTip((Control)label4, componentResourceManager.GetString("label4.ToolTip"));
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.BorderStyle = BorderStyle.FixedSingle;
            label1.Name = "label1";
            toolTip1.SetToolTip((Control)label1, componentResourceManager.GetString("label1.ToolTip"));
            componentResourceManager.ApplyResources((object)folderBrowserDialog2, "folderBrowserDialog2");
            componentResourceManager.ApplyResources((object)cmbSelectKindOfGame, "cmbSelectKindOfGame");
            cmbSelectKindOfGame.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectKindOfGame.FormattingEnabled = true;
            cmbSelectKindOfGame.Items.AddRange(new object[7]
            {
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items"),
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items1"),
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items2"),
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items3"),
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items4"),
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items5"),
        (object) componentResourceManager.GetString("cmbSelectKindOfGame.Items6")
            });
            cmbSelectKindOfGame.Name = "cmbSelectKindOfGame";
            toolTip1.SetToolTip((Control)cmbSelectKindOfGame, componentResourceManager.GetString("cmbSelectKindOfGame.ToolTip"));
            cmbSelectKindOfGame.SelectedIndexChanged += new EventHandler(cmbSelectKindOfGame_SelectedIndexChanged);
            componentResourceManager.ApplyResources((object)btnFreeText, "btnFreeText");
            btnFreeText.ForeColor = SystemColors.ControlText;
            btnFreeText.Name = "btnFreeText";
            toolTip1.SetToolTip((Control)btnFreeText, componentResourceManager.GetString("btnFreeText.ToolTip"));
            btnFreeText.UseVisualStyleBackColor = true;
            btnFreeText.Click += new EventHandler(btnFreeText_Click);
            componentResourceManager.ApplyResources((object)cbAllowGraphicFromLAN, "cbAllowGraphicFromLAN");
            cbAllowGraphicFromLAN.Checked = Settings.Default.AllowGraphicFromLAN;
            cbAllowGraphicFromLAN.DataBindings.Add(new Binding("Checked", (object)Settings.Default, "AllowGraphicFromLAN", true, DataSourceUpdateMode.OnPropertyChanged));
            cbAllowGraphicFromLAN.ForeColor = Color.White;
            cbAllowGraphicFromLAN.Name = "cbAllowGraphicFromLAN";
            toolTip1.SetToolTip((Control)cbAllowGraphicFromLAN, componentResourceManager.GetString("cbAllowGraphicFromLAN.ToolTip"));
            cbAllowGraphicFromLAN.UseVisualStyleBackColor = true;
            cbAllowGraphicFromLAN.CheckedChanged += new EventHandler(cbAllowGraphicFromLAN_CheckedChanged);
            componentResourceManager.ApplyResources((object)tabControl1, "tabControl1");
            tabControl1.Controls.Add((Control)tabPage1);
            tabControl1.Controls.Add((Control)tabPage2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            toolTip1.SetToolTip((Control)tabControl1, componentResourceManager.GetString("tabControl1.ToolTip"));
            tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            componentResourceManager.ApplyResources((object)tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            toolTip1.SetToolTip((Control)tabPage1, componentResourceManager.GetString("tabPage1.ToolTip"));
            tabPage1.UseVisualStyleBackColor = true;
            componentResourceManager.ApplyResources((object)tabPage2, "tabPage2");
            tabPage2.Controls.Add((Control)btnRefreshPPT_List);
            tabPage2.Controls.Add((Control)btnShowPPT);
            tabPage2.Controls.Add((Control)btnSelectPPT_Path);
            tabPage2.Controls.Add((Control)lbAvailablePPTs);
            tabPage2.Name = "tabPage2";
            toolTip1.SetToolTip((Control)tabPage2, componentResourceManager.GetString("tabPage2.ToolTip"));
            tabPage2.UseVisualStyleBackColor = true;
            componentResourceManager.ApplyResources((object)btnRefreshPPT_List, "btnRefreshPPT_List");
            btnRefreshPPT_List.Name = "btnRefreshPPT_List";
            toolTip1.SetToolTip((Control)btnRefreshPPT_List, componentResourceManager.GetString("btnRefreshPPT_List.ToolTip"));
            btnRefreshPPT_List.UseVisualStyleBackColor = true;
            btnRefreshPPT_List.Click += new EventHandler(btnRefreshPPT_List_Click);
            componentResourceManager.ApplyResources((object)btnShowPPT, "btnShowPPT");
            btnShowPPT.Name = "btnShowPPT";
            toolTip1.SetToolTip((Control)btnShowPPT, componentResourceManager.GetString("btnShowPPT.ToolTip"));
            btnShowPPT.UseVisualStyleBackColor = true;
            btnShowPPT.Click += new EventHandler(btnShowPPT_Click);
            componentResourceManager.ApplyResources((object)btnSelectPPT_Path, "btnSelectPPT_Path");
            btnSelectPPT_Path.Name = "btnSelectPPT_Path";
            toolTip1.SetToolTip((Control)btnSelectPPT_Path, componentResourceManager.GetString("btnSelectPPT_Path.ToolTip"));
            btnSelectPPT_Path.UseVisualStyleBackColor = true;
            btnSelectPPT_Path.Click += new EventHandler(btnSelectPPT_Path_Click);
            componentResourceManager.ApplyResources((object)lbAvailablePPTs, "lbAvailablePPTs");
            lbAvailablePPTs.BackColor = SystemColors.Control;
            lbAvailablePPTs.ContextMenuStrip = contextMenuStrip4;
            lbAvailablePPTs.FormattingEnabled = true;
            lbAvailablePPTs.Name = "lbAvailablePPTs";
            toolTip1.SetToolTip((Control)lbAvailablePPTs, componentResourceManager.GetString("lbAvailablePPTs.ToolTip"));
            componentResourceManager.ApplyResources((object)contextMenuStrip4, "contextMenuStrip4");
            contextMenuStrip4.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) refreshToolStripMenuItem
            });
            contextMenuStrip4.Name = "contextMenuStrip4";
            toolTip1.SetToolTip((Control)contextMenuStrip4, componentResourceManager.GetString("contextMenuStrip4.ToolTip"));
            componentResourceManager.ApplyResources((object)refreshToolStripMenuItem, "refreshToolStripMenuItem");
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Click += new EventHandler(refreshToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)menuStrip1, "menuStrip1");
            menuStrip1.Items.AddRange(new ToolStripItem[3]
            {
        (ToolStripItem) gametimeInsertParameterToolStripMenuItem,
        (ToolStripItem) daytimeInsertParameterToolStripMenuItem,
        (ToolStripItem) backgoundImagesToolStripMenuItem
            });
            menuStrip1.Name = "menuStrip1";
            toolTip1.SetToolTip((Control)menuStrip1, componentResourceManager.GetString("menuStrip1.ToolTip"));
            componentResourceManager.ApplyResources((object)gametimeInsertParameterToolStripMenuItem, "gametimeInsertParameterToolStripMenuItem");
            gametimeInsertParameterToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) insertGametimeToolStripMenuItem,
        (ToolStripItem) toolStripSeparator6,
        (ToolStripItem) fontToolStripMenuItem,
        (ToolStripItem) colorToolStripMenuItem,
        (ToolStripItem) locationToolStripMenuItem
            });
            gametimeInsertParameterToolStripMenuItem.Name = "gametimeInsertParameterToolStripMenuItem";
            componentResourceManager.ApplyResources((object)insertGametimeToolStripMenuItem, "insertGametimeToolStripMenuItem");
            insertGametimeToolStripMenuItem.Name = "insertGametimeToolStripMenuItem";
            insertGametimeToolStripMenuItem.Click += new EventHandler(insertGametimeToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator6, "toolStripSeparator6");
            toolStripSeparator6.Name = "toolStripSeparator6";
            componentResourceManager.ApplyResources((object)fontToolStripMenuItem, "fontToolStripMenuItem");
            fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            fontToolStripMenuItem.Click += new EventHandler(fontToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)colorToolStripMenuItem, "colorToolStripMenuItem");
            colorToolStripMenuItem.Name = "colorToolStripMenuItem";
            colorToolStripMenuItem.Click += new EventHandler(colorToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)locationToolStripMenuItem, "locationToolStripMenuItem");
            locationToolStripMenuItem.Name = "locationToolStripMenuItem";
            locationToolStripMenuItem.Click += new EventHandler(locationToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)daytimeInsertParameterToolStripMenuItem, "daytimeInsertParameterToolStripMenuItem");
            daytimeInsertParameterToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) insertDaytimeToolStripMenuItem,
        (ToolStripItem) toolStripSeparator7,
        (ToolStripItem) fontDayTimeToolStripMenuItem,
        (ToolStripItem) colorDayTimeToolStripMenuItem,
        (ToolStripItem) locationDayTimeToolStripMenuItem
            });
            daytimeInsertParameterToolStripMenuItem.Name = "daytimeInsertParameterToolStripMenuItem";
            componentResourceManager.ApplyResources((object)insertDaytimeToolStripMenuItem, "insertDaytimeToolStripMenuItem");
            insertDaytimeToolStripMenuItem.Name = "insertDaytimeToolStripMenuItem";
            insertDaytimeToolStripMenuItem.Click += new EventHandler(insertDaytimeToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator7, "toolStripSeparator7");
            toolStripSeparator7.Name = "toolStripSeparator7";
            componentResourceManager.ApplyResources((object)fontDayTimeToolStripMenuItem, "fontDayTimeToolStripMenuItem");
            fontDayTimeToolStripMenuItem.Name = "fontDayTimeToolStripMenuItem";
            fontDayTimeToolStripMenuItem.Click += new EventHandler(fontDayTimeToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)colorDayTimeToolStripMenuItem, "colorDayTimeToolStripMenuItem");
            colorDayTimeToolStripMenuItem.Name = "colorDayTimeToolStripMenuItem";
            colorDayTimeToolStripMenuItem.Click += new EventHandler(colorDayTimeToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)locationDayTimeToolStripMenuItem, "locationDayTimeToolStripMenuItem");
            locationDayTimeToolStripMenuItem.Name = "locationDayTimeToolStripMenuItem";
            locationDayTimeToolStripMenuItem.Click += new EventHandler(locationDayTimeToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)backgoundImagesToolStripMenuItem, "backgoundImagesToolStripMenuItem");
            backgoundImagesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) soccerToolStripMenuItem1
            });
            backgoundImagesToolStripMenuItem.Name = "backgoundImagesToolStripMenuItem";
            componentResourceManager.ApplyResources((object)soccerToolStripMenuItem1, "soccerToolStripMenuItem1");
            soccerToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[24]
            {
        (ToolStripItem) soccerToolStripMenuItem,
        (ToolStripItem) introGuestteamToolStripMenuItem,
        (ToolStripItem) introHomeReserveToolStripMenuItem,
        (ToolStripItem) introGuestReserveToolStripMenuItem,
        (ToolStripItem) introSinglePlayerToolStripMenuItem,
        (ToolStripItem) toolStripMenuItem1,
        (ToolStripItem) introCoachesHomeTeamToolStripMenuItem,
        (ToolStripItem) introCoachesGuestTeamToolStripMenuItem,
        (ToolStripItem) introRefereesToolStripMenuItem,
        (ToolStripItem) scoreToolStripMenuItem,
        (ToolStripItem) cornersToolStripMenuItem,
        (ToolStripItem) scorerToolStripMenuItem,
        (ToolStripItem) goalHomeToolStripMenuItem,
        (ToolStripItem) goalGuestToolStripMenuItem,
        (ToolStripItem) changeOutToolStripMenuItem,
        (ToolStripItem) changeInToolStripMenuItem,
        (ToolStripItem) toolStripMenuItem2,
        (ToolStripItem) yellowCardToolStripMenuItem,
        (ToolStripItem) redCardToolStripMenuItem,
        (ToolStripItem) yellowredCardToolStripMenuItem,
        (ToolStripItem) toolStripSeparator3,
        (ToolStripItem) flashtableToolStripMenuItem,
        (ToolStripItem) resultTableToolStripMenuItem,
        (ToolStripItem) resultToolStripMenuItem
            });
            soccerToolStripMenuItem1.Name = "soccerToolStripMenuItem1";
            componentResourceManager.ApplyResources((object)soccerToolStripMenuItem, "soccerToolStripMenuItem");
            soccerToolStripMenuItem.Name = "soccerToolStripMenuItem";
            soccerToolStripMenuItem.Tag = (object)"IntroHomeTeam.jpg";
            soccerToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introGuestteamToolStripMenuItem, "introGuestteamToolStripMenuItem");
            introGuestteamToolStripMenuItem.Name = "introGuestteamToolStripMenuItem";
            introGuestteamToolStripMenuItem.Tag = (object)"IntroGuestTeam.jpg";
            introGuestteamToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introHomeReserveToolStripMenuItem, "introHomeReserveToolStripMenuItem");
            introHomeReserveToolStripMenuItem.Name = "introHomeReserveToolStripMenuItem";
            introHomeReserveToolStripMenuItem.Tag = (object)"IntroHomeReserve.jpg";
            introHomeReserveToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introGuestReserveToolStripMenuItem, "introGuestReserveToolStripMenuItem");
            introGuestReserveToolStripMenuItem.Name = "introGuestReserveToolStripMenuItem";
            introGuestReserveToolStripMenuItem.Tag = (object)"IntroGuestReserve.jpg";
            introGuestReserveToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introSinglePlayerToolStripMenuItem, "introSinglePlayerToolStripMenuItem");
            introSinglePlayerToolStripMenuItem.Name = "introSinglePlayerToolStripMenuItem";
            introSinglePlayerToolStripMenuItem.Tag = (object)"IntroHomeSingle.jpg";
            introSinglePlayerToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)toolStripMenuItem1, "toolStripMenuItem1");
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Tag = (object)"IntroGuestSingle.jpg";
            toolStripMenuItem1.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introCoachesHomeTeamToolStripMenuItem, "introCoachesHomeTeamToolStripMenuItem");
            introCoachesHomeTeamToolStripMenuItem.Name = "introCoachesHomeTeamToolStripMenuItem";
            introCoachesHomeTeamToolStripMenuItem.Tag = (object)"IntroHomeCoaches.jpg";
            introCoachesHomeTeamToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introCoachesGuestTeamToolStripMenuItem, "introCoachesGuestTeamToolStripMenuItem");
            introCoachesGuestTeamToolStripMenuItem.Name = "introCoachesGuestTeamToolStripMenuItem";
            introCoachesGuestTeamToolStripMenuItem.Tag = (object)"IntroGuestCoaches.jpg";
            introCoachesGuestTeamToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)introRefereesToolStripMenuItem, "introRefereesToolStripMenuItem");
            introRefereesToolStripMenuItem.Name = "introRefereesToolStripMenuItem";
            introRefereesToolStripMenuItem.Tag = (object)"IntroReferees.jpg";
            introRefereesToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)scoreToolStripMenuItem, "scoreToolStripMenuItem");
            scoreToolStripMenuItem.Name = "scoreToolStripMenuItem";
            scoreToolStripMenuItem.Tag = (object)"Soccer.jpg";
            scoreToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)cornersToolStripMenuItem, "cornersToolStripMenuItem");
            cornersToolStripMenuItem.Name = "cornersToolStripMenuItem";
            cornersToolStripMenuItem.Tag = (object)"Corners.jpg";
            cornersToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)scorerToolStripMenuItem, "scorerToolStripMenuItem");
            scorerToolStripMenuItem.Name = "scorerToolStripMenuItem";
            scorerToolStripMenuItem.Tag = (object)"Scorer.jpg";
            scorerToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)goalHomeToolStripMenuItem, "goalHomeToolStripMenuItem");
            goalHomeToolStripMenuItem.Name = "goalHomeToolStripMenuItem";
            goalHomeToolStripMenuItem.Tag = (object)"GoalHome.jpg";
            goalHomeToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)goalGuestToolStripMenuItem, "goalGuestToolStripMenuItem");
            goalGuestToolStripMenuItem.Name = "goalGuestToolStripMenuItem";
            goalGuestToolStripMenuItem.Tag = (object)"GoalGuest.jpg";
            goalGuestToolStripMenuItem.Click += new EventHandler(goalGuestToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)changeOutToolStripMenuItem, "changeOutToolStripMenuItem");
            changeOutToolStripMenuItem.Name = "changeOutToolStripMenuItem";
            changeOutToolStripMenuItem.Tag = (object)"ChangeOut.jpg";
            changeOutToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)changeInToolStripMenuItem, "changeInToolStripMenuItem");
            changeInToolStripMenuItem.Name = "changeInToolStripMenuItem";
            changeInToolStripMenuItem.Tag = (object)"ChangeIn.jpg";
            changeInToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)toolStripMenuItem2, "toolStripMenuItem2");
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Tag = (object)"ChangeOutGuest.jpg";
            toolStripMenuItem2.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)yellowCardToolStripMenuItem, "yellowCardToolStripMenuItem");
            yellowCardToolStripMenuItem.Name = "yellowCardToolStripMenuItem";
            yellowCardToolStripMenuItem.Tag = (object)"Yellow.jpg";
            yellowCardToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)redCardToolStripMenuItem, "redCardToolStripMenuItem");
            redCardToolStripMenuItem.Name = "redCardToolStripMenuItem";
            redCardToolStripMenuItem.Tag = (object)"Red.jpg";
            redCardToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)yellowredCardToolStripMenuItem, "yellowredCardToolStripMenuItem");
            yellowredCardToolStripMenuItem.Name = "yellowredCardToolStripMenuItem";
            yellowredCardToolStripMenuItem.Tag = (object)"YellowRed.jpg";
            yellowredCardToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)toolStripSeparator3, "toolStripSeparator3");
            toolStripSeparator3.Name = "toolStripSeparator3";
            componentResourceManager.ApplyResources((object)flashtableToolStripMenuItem, "flashtableToolStripMenuItem");
            flashtableToolStripMenuItem.Name = "flashtableToolStripMenuItem";
            flashtableToolStripMenuItem.Tag = (object)"FlashTable.jpg";
            flashtableToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)resultTableToolStripMenuItem, "resultTableToolStripMenuItem");
            resultTableToolStripMenuItem.Name = "resultTableToolStripMenuItem";
            resultTableToolStripMenuItem.Tag = (object)"ResultTable.jpg";
            resultTableToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)resultToolStripMenuItem, "resultToolStripMenuItem");
            resultToolStripMenuItem.Name = "resultToolStripMenuItem";
            resultToolStripMenuItem.Tag = (object)"Result.Jpg";
            resultToolStripMenuItem.Click += new EventHandler(soccerBackgroundImagesToolStripMenuItem_Click);
            componentResourceManager.ApplyResources((object)openFileDialog1, "openFileDialog1");
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FileOk += new CancelEventHandler(openFileDialog1_FileOk);
            componentResourceManager.ApplyResources((object)groupBox2, "groupBox2");
            groupBox2.ContextMenuStrip = contextMenuStrip1;
            groupBox2.Controls.Add((Control)cmbAvailableSequences);
            groupBox2.Controls.Add((Control)lbSequenceContent);
            groupBox2.Controls.Add((Control)txtInputNewSequenceName);
            groupBox2.Controls.Add((Control)btnShowSingleStep);
            groupBox2.Controls.Add((Control)label3);
            groupBox2.ForeColor = Color.White;
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            toolTip1.SetToolTip((Control)groupBox2, componentResourceManager.GetString("groupBox2.ToolTip"));
            componentResourceManager.ApplyResources((object)cmbAvailableSequences, "cmbAvailableSequences");
            cmbAvailableSequences.ContextMenuStrip = contextMenuStrip1;
            cmbAvailableSequences.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAvailableSequences.FormattingEnabled = true;
            cmbAvailableSequences.Name = "cmbAvailableSequences";
            toolTip1.SetToolTip((Control)cmbAvailableSequences, componentResourceManager.GetString("cmbAvailableSequences.ToolTip"));
            cmbAvailableSequences.SelectedIndexChanged += new EventHandler(cmbAvailableSequences_SelectedIndexChanged);
            componentResourceManager.ApplyResources((object)openFileDialog2, "openFileDialog2");
            openFileDialog2.InitialDirectory = "c:\\";
            componentResourceManager.ApplyResources((object)lblDayTime, "lblDayTime");
            lblDayTime.ForeColor = Color.White;
            lblDayTime.Name = "lblDayTime";
            toolTip1.SetToolTip((Control)lblDayTime, componentResourceManager.GetString("lblDayTime.ToolTip"));
            lblDayTime.TextChanged += new EventHandler(lblDayTime_TextChanged);
            timerDayTime.Enabled = true;
            timerDayTime.Tick += new EventHandler(timerDayTime_Tick);
            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 100;
            componentResourceManager.ApplyResources((object)lblProgramVersion, "lblProgramVersion");
            lblProgramVersion.ForeColor = Color.White;
            lblProgramVersion.Name = "lblProgramVersion";
            toolTip1.SetToolTip((Control)lblProgramVersion, componentResourceManager.GetString("lblProgramVersion.ToolTip"));
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(42, 0, 0);
            Controls.Add((Control)lblDayTime);
            Controls.Add((Control)menuStrip1);
            Controls.Add((Control)label1);
            Controls.Add((Control)groupBox2);
            Controls.Add((Control)groupBox4);
            Controls.Add((Control)groupBox3);
            Controls.Add((Control)tabControl1);
            Controls.Add((Control)groupBox1);
            Controls.Add((Control)btnShowScore);
            Controls.Add((Control)btnFreeText);
            Controls.Add((Control)btnClearSCB);
            Controls.Add((Control)rbFullGraphic);
            Controls.Add((Control)rbSmallGraphic);
            Controls.Add((Control)cbAllowGraphicFromLAN);
            Controls.Add((Control)cmbSelectKindOfGame);
            Controls.Add((Control)lblProgramVersion);
            DataBindings.Add(new Binding("Location", (object)Settings.Default, "StartLocation", true, DataSourceUpdateMode.OnPropertyChanged));
            ForeColor = SystemColors.Desktop;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Location = Settings.Default.StartLocation;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(Form1);
            toolTip1.SetToolTip((Control)this, componentResourceManager.GetString("$ToolTip"));
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            Load += new EventHandler(Form1_Load);
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            contextMenuStrip1.ResumeLayout(false);
            contextMenuStrip2.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((ISupportInitialize)pbAnimationPreview).EndInit();
            contextMenuStrip5.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            contextMenuStrip3.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            contextMenuStrip6.ResumeLayout(false);
            gbDaytimeInsertPosition.ResumeLayout(false);
            gbDaytimeInsertPosition.PerformLayout();
            nudDayTimeInsertPositionX.EndInit();
            nudDayTimeInsertPositionY.EndInit();
            gbTimeInsertPosition.ResumeLayout(false);
            gbTimeInsertPosition.PerformLayout();
            nudTimeInsertPositionX.EndInit();
            nudTimeInsertPositionY.EndInit();
            tabControl1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            contextMenuStrip4.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        public delegate void showGraphicAsyncDelegate(
          LED_Board target,
          string filename,
          bool in_window,
          int effect_index,
          bool act_graphic_from_score);

        public delegate void startSequenceAsyncDelegate(
          Form target,
          string sequencename,
          bool inwindow);
    }
}
