using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using BaSta.Game.Properties;

namespace BaSta.Game
{
    public class TeamEditor : Form
    {
        private string _servername = string.Empty;
        private string _databasename = string.Empty;
        private string _user_id = string.Empty;
        private string _password = string.Empty;
        private string _kindOfGame = string.Empty;
        private string _team_logo_name = string.Empty;
        private TextBox txtTeamName;
        private Label label1;
        private ListBox lbAvailablePlayers;
        private Label label2;
        private PictureBox pbTeamLogo;
        private TextBox txtPlayerName;
        private NumericUpDown nudPlayerNumber;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnDeletePlayer;
        private Button btnNewPlayer;
        private OpenFileDialog openFileDialogTeamLogo;
        private Button btnSavePlayer;
        private Button btnImportPlayerList;
        private OpenFileDialog openFileDialog1;
        private PictureBox pbPlayerImage;
        private OpenFileDialog openFileDialog2;
        private Label label6;
        private RadioButton rbIsPlayer;
        private RadioButton rbIsCoach;
        private RadioButton rbIsCoTrainer;
        private CheckBox cbNameToUpper;
        private CheckBox cbIsGoalkeeper;
        private DataBaseFunctions _dbfunc;
        private bool _show_import_button;
        private Team _team;
        private List<Player> _players;
        private Player _act_player;
        private KindOfGame _kind_of_game;
        private bool _no_nud_change;

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(TeamEditor));
            txtTeamName = new TextBox();
            label1 = new Label();
            lbAvailablePlayers = new ListBox();
            label2 = new Label();
            pbTeamLogo = new PictureBox();
            txtPlayerName = new TextBox();
            nudPlayerNumber = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnDeletePlayer = new Button();
            btnNewPlayer = new Button();
            openFileDialogTeamLogo = new OpenFileDialog();
            btnSavePlayer = new Button();
            btnImportPlayerList = new Button();
            openFileDialog1 = new OpenFileDialog();
            pbPlayerImage = new PictureBox();
            openFileDialog2 = new OpenFileDialog();
            label6 = new Label();
            rbIsPlayer = new RadioButton();
            rbIsCoach = new RadioButton();
            rbIsCoTrainer = new RadioButton();
            cbNameToUpper = new CheckBox();
            cbIsGoalkeeper = new CheckBox();
            ((ISupportInitialize)pbTeamLogo).BeginInit();
            nudPlayerNumber.BeginInit();
            ((ISupportInitialize)pbPlayerImage).BeginInit();
            SuspendLayout();
            txtTeamName.AccessibleDescription = (string)null;
            txtTeamName.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)txtTeamName, "txtTeamName");
            txtTeamName.BackgroundImage = (Image)null;
            txtTeamName.Font = (Font)null;
            txtTeamName.Name = "txtTeamName";
            txtTeamName.TextChanged += new EventHandler(txtTeamName_TextChanged);
            txtTeamName.Leave += new EventHandler(txtTeamName_Leave);
            label1.AccessibleDescription = (string)null;
            label1.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.Font = (Font)null;
            label1.Name = "label1";
            lbAvailablePlayers.AccessibleDescription = (string)null;
            lbAvailablePlayers.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)lbAvailablePlayers, "lbAvailablePlayers");
            lbAvailablePlayers.BackgroundImage = (Image)null;
            lbAvailablePlayers.FormattingEnabled = true;
            lbAvailablePlayers.Name = "lbAvailablePlayers";
            lbAvailablePlayers.TabStop = false;
            lbAvailablePlayers.SelectedIndexChanged += new EventHandler(lbAvailablePlayers_SelectedIndexChanged);
            label2.AccessibleDescription = (string)null;
            label2.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label2, "label2");
            label2.Font = (Font)null;
            label2.Name = "label2";
            pbTeamLogo.AccessibleDescription = (string)null;
            pbTeamLogo.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)pbTeamLogo, "pbTeamLogo");
            pbTeamLogo.BackgroundImage = (Image)null;
            pbTeamLogo.BorderStyle = BorderStyle.FixedSingle;
            pbTeamLogo.Font = (Font)null;
            pbTeamLogo.ImageLocation = (string)null;
            pbTeamLogo.Name = "pbTeamLogo";
            pbTeamLogo.TabStop = false;
            pbTeamLogo.Click += new EventHandler(pbTeamLogo_Click);
            txtPlayerName.AccessibleDescription = (string)null;
            txtPlayerName.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)txtPlayerName, "txtPlayerName");
            txtPlayerName.BackgroundImage = (Image)null;
            txtPlayerName.Font = (Font)null;
            txtPlayerName.Name = "txtPlayerName";
            txtPlayerName.TextChanged += new EventHandler(txtPlayerName_TextChanged);
            nudPlayerNumber.AccessibleDescription = (string)null;
            nudPlayerNumber.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudPlayerNumber, "nudPlayerNumber");
            nudPlayerNumber.Font = (Font)null;
            nudPlayerNumber.Minimum = new Decimal(new int[4]
            {
        2,
        0,
        0,
        int.MinValue
            });
            nudPlayerNumber.Name = "nudPlayerNumber";
            nudPlayerNumber.ValueChanged += new EventHandler(nudPlayerNumber_ValueChanged);
            label3.AccessibleDescription = (string)null;
            label3.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label3, "label3");
            label3.Font = (Font)null;
            label3.Name = "label3";
            label4.AccessibleDescription = (string)null;
            label4.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label4, "label4");
            label4.Font = (Font)null;
            label4.Name = "label4";
            label5.AccessibleDescription = (string)null;
            label5.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label5, "label5");
            label5.Font = (Font)null;
            label5.Name = "label5";
            btnDeletePlayer.AccessibleDescription = (string)null;
            btnDeletePlayer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnDeletePlayer, "btnDeletePlayer");
            btnDeletePlayer.BackgroundImage = (Image)null;
            btnDeletePlayer.Font = (Font)null;
            btnDeletePlayer.Name = "btnDeletePlayer";
            btnDeletePlayer.TabStop = false;
            btnDeletePlayer.UseVisualStyleBackColor = true;
            btnDeletePlayer.Click += new EventHandler(btnDeletePlayer_Click);
            btnNewPlayer.AccessibleDescription = (string)null;
            btnNewPlayer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnNewPlayer, "btnNewPlayer");
            btnNewPlayer.BackgroundImage = (Image)null;
            btnNewPlayer.Font = (Font)null;
            btnNewPlayer.Name = "btnNewPlayer";
            btnNewPlayer.TabStop = false;
            btnNewPlayer.UseVisualStyleBackColor = true;
            btnNewPlayer.Click += new EventHandler(btnNewPlayer_Click);
            openFileDialogTeamLogo.DereferenceLinks = false;
            componentResourceManager.ApplyResources((object)openFileDialogTeamLogo, "openFileDialogTeamLogo");
            openFileDialogTeamLogo.InitialDirectory = "c:\\";
            openFileDialogTeamLogo.FileOk += new CancelEventHandler(openFileDialogTeamLogo_FileOk);
            btnSavePlayer.AccessibleDescription = (string)null;
            btnSavePlayer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnSavePlayer, "btnSavePlayer");
            btnSavePlayer.BackgroundImage = (Image)null;
            btnSavePlayer.Font = (Font)null;
            btnSavePlayer.Name = "btnSavePlayer";
            btnSavePlayer.TabStop = false;
            btnSavePlayer.UseVisualStyleBackColor = true;
            btnSavePlayer.Click += new EventHandler(btnSavePlayer_Click);
            btnImportPlayerList.AccessibleDescription = (string)null;
            btnImportPlayerList.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnImportPlayerList, "btnImportPlayerList");
            btnImportPlayerList.BackgroundImage = (Image)null;
            btnImportPlayerList.Font = (Font)null;
            btnImportPlayerList.Name = "btnImportPlayerList";
            btnImportPlayerList.UseVisualStyleBackColor = true;
            btnImportPlayerList.Click += new EventHandler(btnImportPlayerList_Click);
            componentResourceManager.ApplyResources((object)openFileDialog1, "openFileDialog1");
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FileOk += new CancelEventHandler(openFileDialog1_FileOk);
            pbPlayerImage.AccessibleDescription = (string)null;
            pbPlayerImage.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)pbPlayerImage, "pbPlayerImage");
            pbPlayerImage.BackgroundImage = (Image)null;
            pbPlayerImage.BorderStyle = BorderStyle.FixedSingle;
            pbPlayerImage.Font = (Font)null;
            pbPlayerImage.ImageLocation = (string)null;
            pbPlayerImage.Name = "pbPlayerImage";
            pbPlayerImage.TabStop = false;
            pbPlayerImage.Click += new EventHandler(pbPlayerImage_Click);
            openFileDialog2.DereferenceLinks = false;
            componentResourceManager.ApplyResources((object)openFileDialog2, "openFileDialog2");
            openFileDialog2.InitialDirectory = "c:\\";
            openFileDialog2.FileOk += new CancelEventHandler(openFileDialog2_FileOk);
            label6.AccessibleDescription = (string)null;
            label6.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label6, "label6");
            label6.Font = (Font)null;
            label6.Name = "label6";
            rbIsPlayer.AccessibleDescription = (string)null;
            rbIsPlayer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)rbIsPlayer, "rbIsPlayer");
            rbIsPlayer.BackgroundImage = (Image)null;
            rbIsPlayer.Checked = true;
            rbIsPlayer.Font = (Font)null;
            rbIsPlayer.Name = "rbIsPlayer";
            rbIsPlayer.TabStop = true;
            rbIsPlayer.UseVisualStyleBackColor = true;
            rbIsPlayer.Click += new EventHandler(rbIsPlayer_Click);
            rbIsCoach.AccessibleDescription = (string)null;
            rbIsCoach.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)rbIsCoach, "rbIsCoach");
            rbIsCoach.BackgroundImage = (Image)null;
            rbIsCoach.Font = (Font)null;
            rbIsCoach.Name = "rbIsCoach";
            rbIsCoach.UseVisualStyleBackColor = true;
            rbIsCoach.Click += new EventHandler(rbIsCoach_Click);
            rbIsCoTrainer.AccessibleDescription = (string)null;
            rbIsCoTrainer.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)rbIsCoTrainer, "rbIsCoTrainer");
            rbIsCoTrainer.BackgroundImage = (Image)null;
            rbIsCoTrainer.Font = (Font)null;
            rbIsCoTrainer.Name = "rbIsCoTrainer";
            rbIsCoTrainer.UseVisualStyleBackColor = true;
            rbIsCoTrainer.Click += new EventHandler(rbIsCoTrainer_Click);
            cbNameToUpper.AccessibleDescription = (string)null;
            cbNameToUpper.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)cbNameToUpper, "cbNameToUpper");
            cbNameToUpper.BackgroundImage = (Image)null;
            cbNameToUpper.Font = (Font)null;
            cbNameToUpper.Name = "cbNameToUpper";
            cbNameToUpper.UseVisualStyleBackColor = true;
            cbNameToUpper.CheckedChanged += new EventHandler(cbNameToUpper_CheckedChanged);
            cbIsGoalkeeper.AccessibleDescription = (string)null;
            cbIsGoalkeeper.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)cbIsGoalkeeper, "cbIsGoalkeeper");
            cbIsGoalkeeper.BackgroundImage = (Image)null;
            cbIsGoalkeeper.Font = (Font)null;
            cbIsGoalkeeper.Name = "cbIsGoalkeeper";
            cbIsGoalkeeper.UseVisualStyleBackColor = true;
            AccessibleDescription = (string)null;
            AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)null;
            Controls.Add((Control)cbIsGoalkeeper);
            Controls.Add((Control)cbNameToUpper);
            Controls.Add((Control)rbIsCoTrainer);
            Controls.Add((Control)rbIsCoach);
            Controls.Add((Control)rbIsPlayer);
            Controls.Add((Control)pbPlayerImage);
            Controls.Add((Control)btnImportPlayerList);
            Controls.Add((Control)btnSavePlayer);
            Controls.Add((Control)btnDeletePlayer);
            Controls.Add((Control)btnNewPlayer);
            Controls.Add((Control)label5);
            Controls.Add((Control)label4);
            Controls.Add((Control)label3);
            Controls.Add((Control)nudPlayerNumber);
            Controls.Add((Control)txtPlayerName);
            Controls.Add((Control)pbTeamLogo);
            Controls.Add((Control)label2);
            Controls.Add((Control)lbAvailablePlayers);
            Controls.Add((Control)label1);
            Controls.Add((Control)txtTeamName);
            Controls.Add((Control)label6);
            Font = (Font)null;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)null;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(TeamEditor);
            FormClosing += new FormClosingEventHandler(TeamEditor_FormClosing);
            KeyDown += new KeyEventHandler(TeamEditor_KeyDown);
            ((ISupportInitialize)pbTeamLogo).EndInit();
            nudPlayerNumber.EndInit();
            ((ISupportInitialize)pbPlayerImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public TeamEditor(
          string Servername,
          string Databasename,
          string UserID,
          string Password,
          KindOfGame KindOfgame,
          Team Team,
          bool ShowImportButton)
        {
            InitializeComponent();
            cbNameToUpper.Checked = Settings.Default.NamesToUpper;
            _show_import_button = ShowImportButton;
            _kind_of_game = KindOfgame;
            if (KindOfgame.GameKindName.Trim() == "Tennis" || KindOfgame.GameKindName.Trim() == "Badminton")
            {
                Size = new Size((int)byte.MaxValue, 85);
                label1.Text = "Spieler / Doppel";
                Text = "Spieler / Doppel editieren";
            }
            if (KindOfgame.GameKindName != "Soccer")
                cbIsGoalkeeper.Visible = false;
            _open_form(Servername, Databasename, UserID, Password, KindOfgame.ID, Team);
        }

        private void _open_form(
          string Servername,
          string Databasename,
          string UserID,
          string Password,
          int KindOfgameID,
          Team Team)
        {
            if (Directory.Exists(Settings.Default.LastLogoFolder))
            {
                openFileDialogTeamLogo.InitialDirectory = Settings.Default.LastLogoFolder;
            }
            else
            {
                Settings.Default.LastLogoFolder = "c:\\";
                Settings.Default.Save();
            }
            _servername = Servername;
            _databasename = Databasename;
            _password = Password;
            _user_id = UserID;
            _dbfunc = new DataBaseFunctions(_servername, _databasename, _user_id, _password);
            _dbfunc.ConnectDatabase();
            _team = Team;
            if (_team != null)
            {
                txtTeamName.Text = _team.TeamName.Trim();
                if (_team.TeamLogo != null && _team.TeamLogo.Length > 0)
                {
                    MemoryStream memoryStream = new MemoryStream(_team.TeamLogo, 0, _team.TeamLogo.Length);
                    pbTeamLogo.Image = (Image)new Bitmap((Stream)memoryStream);
                    memoryStream.Close();
                }
                _players = _dbfunc.ReadPlayersWithImage(_team.TeamID);
            }
            else
            {
                _team = new Team();
                _players = new List<Player>();
            }
            _team.TeamGameKindID = KindOfgameID;
            ShowPlayers();
            btnImportPlayerList.Visible = _show_import_button;
        }

        private void pbTeamLogo_Click(object sender, EventArgs e)
        {
            if (txtTeamName.Text.Trim() != string.Empty)
            {
                int num1 = (int)openFileDialogTeamLogo.ShowDialog();
            }
            else
            {
                int num2 = (int)MessageBox.Show("Bitte erst den Mannschaftsnamen eingeben!");
            }
        }

        private void ShowPlayers()
        {
            lbAvailablePlayers.Items.Clear();
            for (int index = 0; index < _players.Count; ++index)
            {
                if (_players[index].PlayerNumber > -1)
                    lbAvailablePlayers.Items.Add((object)(_players[index].PlayerNumber.ToString().PadLeft(2) + " , " + _players[index].PlayerName));
                else if (_players[index].PlayerNumber == -1)
                    lbAvailablePlayers.Items.Add((object)("   Trainer : " + _players[index].PlayerName));
                else
                    lbAvailablePlayers.Items.Add((object)("Co-Trainer : " + _players[index].PlayerName));
            }
            lbAvailablePlayers.SelectedIndex = -1;
        }

        private void openFileDialogTeamLogo_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                pbTeamLogo.Image = Image.FromFile(openFileDialogTeamLogo.FileName);
                _team_logo_name = openFileDialogTeamLogo.FileName;
                Bitmap bitmap = new Bitmap(openFileDialogTeamLogo.FileName);
                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save((Stream)memoryStream, ImageFormat.Jpeg);
                _team.TeamLogo = memoryStream.ToArray();
            }
            catch
            {
            }
        }

        private void TeamEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_dbfunc.Connection.State != ConnectionState.Open || !(_team.TeamName.Trim() != string.Empty))
                return;
            int teamID = _dbfunc.SaveTeam(_team);
            if (_team_logo_name != string.Empty)
            {
                try
                {
                    _dbfunc.SaveTeamLogo(teamID, _team_logo_name);
                }
                catch
                {
                }
            }
            string fileName = openFileDialogTeamLogo.FileName;
            if (fileName.Trim() != string.Empty)
                Settings.Default.LastLogoFolder = fileName.Substring(0, fileName.LastIndexOf("\\"));
            Settings.Default.LastTeamID = teamID;
            Settings.Default.Save();
        }

        private void txtTeamName_TextChanged(object sender, EventArgs e)
        {
            if (_team == null)
            {
                if (!(txtTeamName.Text.Trim() != string.Empty))
                    return;
                _team = new Team(0, txtTeamName.Text.Trim());
            }
            else
                _team.TeamName = txtTeamName.Text.Trim();
        }

        private void btnNewPlayer_Click(object sender, EventArgs e)
        {
            ShowPlayers();
            _act_player = new Player();
            nudPlayerNumber.Value = new Decimal(0);
            pbPlayerImage.Image = (Image)null;
            txtPlayerName.Text = string.Empty;
        }

        private void btnDeletePlayer_Click(object sender, EventArgs e)
        {
            if (lbAvailablePlayers.SelectedIndex <= -1 || MessageBox.Show(_act_player.PlayerName + " wird gelöscht!", "Achtung:", MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;
            _dbfunc.DeletePlayer(_act_player);
            _players = _dbfunc.ReadPlayers(_team.TeamID, false);
            ShowPlayers();
            _act_player = new Player();
            nudPlayerNumber.Value = new Decimal(0);
            txtPlayerName.Text = string.Empty;
        }

        private void btnSavePlayer_Click(object sender, EventArgs e)
        {
            if (_act_player == null)
                return;
            _act_player.PlayerTeamID = _team.TeamID;
            _act_player.PlayerIsGoalkeeper = cbIsGoalkeeper.Checked;
            if (_act_player.PlayerImage != null)
                _dbfunc.SavePlayerWithImage(_act_player);
            else
                _dbfunc.SavePlayer(_act_player);
            int num = _act_player.PlayerNumber + 1;
            for (int index = 0; index < lbAvailablePlayers.Items.Count; ++index)
            {
                if (_players[index].PlayerNumber == num)
                    ++num;
            }
            _players = _dbfunc.ReadPlayersWithImage(_team.TeamID);
            ShowPlayers();
            txtPlayerName.Text = string.Empty;
            _act_player = (Player)null;
            int int32 = Convert.ToInt32(nudPlayerNumber.Value);
            bool flag = false;
            while (!flag && int32 <= 99)
            {
                ++int32;
                flag = true;
                for (int index = 0; index < _players.Count; ++index)
                {
                    if (_players[index].PlayerNumber == int32)
                        flag = false;
                }
            }
            nudPlayerNumber.Value = !flag ? new Decimal(0) : (Decimal)int32;
            pbPlayerImage.Image = (Image)null;
            txtPlayerName.Focus();
        }

        private void lbAvailablePlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbAvailablePlayers.SelectedIndex == -1)
            {
                _act_player = (Player)null;
                pbPlayerImage.Image = (Image)null;
                txtPlayerName.Text = string.Empty;
                cbIsGoalkeeper.Checked = false;
            }
            else
                _act_player = _players[lbAvailablePlayers.SelectedIndex];
            if (_act_player == null)
                return;
            _no_nud_change = true;
            nudPlayerNumber.Value = (Decimal)_act_player.PlayerNumber;
            if (_act_player.PlayerNumber > -1)
            {
                nudPlayerNumber.Visible = true;
                if (_kind_of_game.GameKindName == "Soccer")
                    cbIsGoalkeeper.Visible = true;
                cbIsGoalkeeper.Checked = _act_player.PlayerIsGoalkeeper;
            }
            else
            {
                nudPlayerNumber.Visible = false;
                if (_kind_of_game.GameKindName != "Soccer")
                    cbIsGoalkeeper.Visible = false;
                cbIsGoalkeeper.Checked = false;
            }
            if (_act_player.PlayerName == null)
                _act_player.PlayerName = string.Empty;
            txtPlayerName.Text = _act_player.PlayerName.Trim();
            if (_act_player.PlayerImage != null && _act_player.PlayerImage.Length > 0)
            {
                MemoryStream memoryStream = new MemoryStream(_act_player.PlayerImage, 0, _act_player.PlayerImage.Length);
                pbPlayerImage.Image = (Image)new Bitmap((Stream)memoryStream);
                memoryStream.Close();
            }
            else
                pbPlayerImage.Image = (Image)null;
        }

        private void nudPlayerNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!_no_nud_change)
            {
                _act_player = (Player)null;
                for (int index = 0; index < lbAvailablePlayers.Items.Count; ++index)
                {
                    if (_players[index].PlayerNumber == Convert.ToInt32(nudPlayerNumber.Value))
                        lbAvailablePlayers.SelectedIndex = index;
                }
                if (_act_player == null)
                {
                    _act_player = new Player();
                    txtPlayerName.Text = string.Empty;
                    pbPlayerImage.Image = (Image)null;
                }
                _act_player.PlayerNumber = (int)nudPlayerNumber.Value;
                if (_act_player.PlayerNumber > -1)
                {
                    label4.Visible = true;
                    nudPlayerNumber.Visible = true;
                    rbIsCoach.Visible = false;
                    rbIsCoTrainer.Visible = false;
                    rbIsPlayer.Visible = false;
                    cbIsGoalkeeper.Checked = _act_player.PlayerIsGoalkeeper;
                    cbIsGoalkeeper.Visible = true;
                }
                else
                {
                    label4.Visible = false;
                    nudPlayerNumber.Visible = false;
                    rbIsCoach.Visible = true;
                    rbIsCoTrainer.Visible = true;
                    rbIsPlayer.Visible = true;
                    rbIsCoach.Checked = _act_player.PlayerNumber == -1;
                    rbIsCoTrainer.Checked = _act_player.PlayerNumber < -1;
                    cbIsGoalkeeper.Checked = false;
                    cbIsGoalkeeper.Visible = false;
                }
            }
            _no_nud_change = false;
        }

        private void txtPlayerName_TextChanged(object sender, EventArgs e)
        {
            if (_act_player == null)
                _act_player = new Player();
            _act_player.PlayerName = txtPlayerName.Text.Trim();
        }

        private void TeamEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            btnSavePlayer_Click((object)this, (EventArgs)null);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                StreamReader streamReader = new StreamReader(openFileDialog1.FileName, Encoding.Default);
                for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
                {
                    btnNewPlayer_Click((object)this, (EventArgs)null);
                    string[] strArray = str.Split(',');
                    nudPlayerNumber.Value = Convert.ToDecimal(strArray[0].Substring(0, 2).Trim());
                    txtPlayerName.Text = strArray[0].Substring(2).Trim();
                    nudPlayerNumber.Update();
                    txtPlayerName.Update();
                    btnSavePlayer_Click((object)this, (EventArgs)null);
                }
                streamReader.Close();
            }
            catch
            {
                int num = (int)MessageBox.Show("Es wird eine Textdatei mit dem Zeilenformat 'Nr  Name, Vorname ...' erwartet!");
            }
        }

        private void btnImportPlayerList_Click(object sender, EventArgs e)
        {
            int num = (int)openFileDialog1.ShowDialog();
        }

        private void txtTeamName_Leave(object sender, EventArgs e)
        {
            if (_team.TeamID >= 0)
                return;
            _team.TeamID = _dbfunc.SaveTeam(_team);
        }

        private void pbPlayerImage_Click(object sender, EventArgs e)
        {
            int num = (int)openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                pbPlayerImage.Image = Image.FromFile(openFileDialog2.FileName);
                Bitmap bitmap = new Bitmap(openFileDialog2.FileName);
                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save((Stream)memoryStream, ImageFormat.Jpeg);
                _act_player.PlayerImage = memoryStream.ToArray();
            }
            catch
            {
            }
        }

        private void rbIsPlayer_Click(object sender, EventArgs e)
        {
            if (_act_player == null)
                _act_player = new Player();
            if (_act_player.PlayerNumber < 0)
                _act_player.PlayerNumber = 0;
            nudPlayerNumber.Value = (Decimal)_act_player.PlayerNumber;
            cbIsGoalkeeper.Visible = true;
        }

        private void rbIsCoach_Click(object sender, EventArgs e)
        {
            if (_act_player == null)
                _act_player = new Player();
            _act_player.PlayerNumber = -1;
            nudPlayerNumber.Value = (Decimal)_act_player.PlayerNumber;
        }

        private void rbIsCoTrainer_Click(object sender, EventArgs e)
        {
            if (_act_player == null)
                _act_player = new Player();
            _act_player.PlayerNumber = -2;
            nudPlayerNumber.Value = (Decimal)_act_player.PlayerNumber;
        }

        private void cbNameToUpper_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbNameToUpper.Checked)
            {
                txtTeamName.CharacterCasing = CharacterCasing.Normal;
                txtPlayerName.CharacterCasing = CharacterCasing.Normal;
            }
            else
            {
                txtTeamName.CharacterCasing = CharacterCasing.Upper;
                txtTeamName.Text = txtTeamName.Text.ToUpper();
                txtPlayerName.CharacterCasing = CharacterCasing.Upper;
                txtPlayerName.Text = txtPlayerName.Text.ToUpper();
            }
            Settings.Default.NamesToUpper = cbNameToUpper.Checked;
            Settings.Default.Save();
        }
    }
}