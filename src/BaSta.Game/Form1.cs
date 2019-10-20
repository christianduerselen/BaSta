using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BaSta.Game.Properties;

namespace BaSta.Game
{
  public class Form1 : Form
  {
    private string _program_version = string.Empty;
    private string _servername = string.Empty;
    private string _databasename = "Game";
    private string _username = "sa";
    private string _password = "Funkwerk_ITK_2008";
    private List<KindOfGame> _kinds_of_game = new List<KindOfGame>();
    private List<Team> _teams = new List<Team>();
    private List<Player> _players = new List<Player>();
    private List<Player> _coaches = new List<Player>();
    private List<Game> _games = new List<Game>();
    private List<GamePlayer> _home_gameplayers = new List<GamePlayer>();
    private List<GamePlayer> _guest_gameplayers = new List<GamePlayer>();
    private List<GamePlayer> _home_coaches = new List<GamePlayer>();
    private List<GamePlayer> _guest_coaches = new List<GamePlayer>();
    private List<GamePlayer> _referees = new List<GamePlayer>();
    private int _dummy_counter = 10;
    private IContainer components;
    private ComboBox cmbGameKinds;
    private Button btnGameParameter;
    private Label label5;
    private Label label4;
    private Label label3;
    private PictureBox pbTeamLogo;
    private Button btnDeleteTeam;
    private Button btnEditTeam;
    private Button btnNewTeam;
    private ComboBox cmbTeams;
    private ListBox lbAvailablePlayers;
    private Label label6;
    private Button btnRemovePlayerFromGame;
    private Button btnSelectPlayerIntoGame;
    private ListBox lbActPlayers;
    private Panel pnlSettings;
    private Button btnSaveParameter;
    private TextBox txtDatabasePassword;
    private TextBox txtDatabaseName;
    private Label label2;
    private Label label1;
    private Button btnSetGuestTeam;
    private Button btnSetHomeTeam;
    private Label label8;
    private Label label7;
    private Button btnStartGame;
    private DateTimePicker dateTimePicker1;
    private Button btnDeleteGame;
    private Button btnNewGame;
    private Label lblGuestTeamName;
    private Label lblHomeTeamName;
    private ListBox lbGuestTeamPlayers;
    private ListBox lbHomeTeamPlayers;
    private ComboBox cmbAvailableGames;
    private Label label9;
    private CheckBox cbSortOrderGames;
    private Label lblAlreadyStarted;
    private Label lblAlreadyStartedCaption;
    private NumericUpDown nudConsolePort;
    private Label lblRemovePlayerFromGame;
    private Label lblPlayerAlreadyActive;
    private Label lblMxPlayerReached;
    private Label lblServerName;
    private Label labelSetGameParameterPlease;
    private Label LabelCaution;
    private NumericUpDown nudUDP_Port;
    private Label label10;
    private CheckBox cbIsReservePlayer;
    private Button btnReferees;
    private Button btnClearAllActivePlayers;
    private CheckBox cbLanAvailable;
    private TextBox txtServerName;
    private Label label12;
    private Label label11;
    private System.Windows.Forms.Timer timer1;
    private NumericUpDown nudHornLength;
    private Label label14;
    private Label label13;
    private NumericUpDown nudSC_HornLength;
    private Label label16;
    private Label label15;
    private CheckBox cbEdgeLightSettingsVisible;
    private Label lblProgramVersion;
    private Label lblProgramVersion1;
    private DataBaseFunctions _dbfunc;
    private GameParameter[] _game_parameters;
    private Game _temp_game;
    private Form _act_game_form;
    private int _max_player;

    protected override void Dispose(bool disposing)
    {
      try
      {
        if (disposing)
        {
          if (components != null)
            components.Dispose();
        }
      }
      catch
      {
      }
      try
      {
        base.Dispose(disposing);
      }
      catch
      {
      }
    }

    private void InitializeComponent()
    {
      components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      cmbGameKinds = new ComboBox();
      btnGameParameter = new Button();
      label5 = new Label();
      label4 = new Label();
      label3 = new Label();
      pbTeamLogo = new PictureBox();
      btnDeleteTeam = new Button();
      btnEditTeam = new Button();
      btnNewTeam = new Button();
      cmbTeams = new ComboBox();
      lbAvailablePlayers = new ListBox();
      label6 = new Label();
      btnRemovePlayerFromGame = new Button();
      btnSelectPlayerIntoGame = new Button();
      lbActPlayers = new ListBox();
      pnlSettings = new Panel();
      lblProgramVersion = new Label();
      cbEdgeLightSettingsVisible = new CheckBox();
      label14 = new Label();
      label13 = new Label();
      nudSC_HornLength = new NumericUpDown();
      nudHornLength = new NumericUpDown();
      txtServerName = new TextBox();
      label12 = new Label();
      label11 = new Label();
      cbLanAvailable = new CheckBox();
      nudUDP_Port = new NumericUpDown();
      nudConsolePort = new NumericUpDown();
      btnSaveParameter = new Button();
      label10 = new Label();
      label16 = new Label();
      label15 = new Label();
      txtDatabasePassword = new TextBox();
      txtDatabaseName = new TextBox();
      label2 = new Label();
      label1 = new Label();
      btnSetGuestTeam = new Button();
      btnSetHomeTeam = new Button();
      label8 = new Label();
      label7 = new Label();
      btnStartGame = new Button();
      dateTimePicker1 = new DateTimePicker();
      btnDeleteGame = new Button();
      btnNewGame = new Button();
      lblGuestTeamName = new Label();
      lblHomeTeamName = new Label();
      lbGuestTeamPlayers = new ListBox();
      lbHomeTeamPlayers = new ListBox();
      cmbAvailableGames = new ComboBox();
      label9 = new Label();
      cbSortOrderGames = new CheckBox();
      lblAlreadyStarted = new Label();
      lblAlreadyStartedCaption = new Label();
      lblRemovePlayerFromGame = new Label();
      lblPlayerAlreadyActive = new Label();
      lblMxPlayerReached = new Label();
      lblServerName = new Label();
      labelSetGameParameterPlease = new Label();
      LabelCaution = new Label();
      cbIsReservePlayer = new CheckBox();
      btnReferees = new Button();
      btnClearAllActivePlayers = new Button();
      timer1 = new System.Windows.Forms.Timer(components);
      lblProgramVersion1 = new Label();
      ((ISupportInitialize) pbTeamLogo).BeginInit();
      pnlSettings.SuspendLayout();
      nudSC_HornLength.BeginInit();
      nudHornLength.BeginInit();
      nudUDP_Port.BeginInit();
      nudConsolePort.BeginInit();
      SuspendLayout();
      componentResourceManager.ApplyResources((object) cmbGameKinds, "cmbGameKinds");
      cmbGameKinds.BackColor = Color.FromArgb(64, 64, 64);
      cmbGameKinds.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbGameKinds.ForeColor = Color.White;
      cmbGameKinds.FormattingEnabled = true;
      cmbGameKinds.Name = "cmbGameKinds";
      cmbGameKinds.SelectedIndexChanged += new EventHandler(cmbGameKinds_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) btnGameParameter, "btnGameParameter");
      btnGameParameter.Name = "btnGameParameter";
      btnGameParameter.UseVisualStyleBackColor = true;
      btnGameParameter.Click += new EventHandler(btnGameParameter_Click);
      componentResourceManager.ApplyResources((object) label5, "label5");
      label5.ForeColor = Color.White;
      label5.Name = "label5";
      componentResourceManager.ApplyResources((object) label4, "label4");
      label4.ForeColor = Color.White;
      label4.Name = "label4";
      componentResourceManager.ApplyResources((object) label3, "label3");
      label3.ForeColor = Color.White;
      label3.Name = "label3";
      componentResourceManager.ApplyResources((object) pbTeamLogo, "pbTeamLogo");
      pbTeamLogo.Name = "pbTeamLogo";
      pbTeamLogo.TabStop = false;
      componentResourceManager.ApplyResources((object) btnDeleteTeam, "btnDeleteTeam");
      btnDeleteTeam.Name = "btnDeleteTeam";
      btnDeleteTeam.UseVisualStyleBackColor = true;
      btnDeleteTeam.Click += new EventHandler(btnDeleteTeam_Click);
      componentResourceManager.ApplyResources((object) btnEditTeam, "btnEditTeam");
      btnEditTeam.Name = "btnEditTeam";
      btnEditTeam.UseVisualStyleBackColor = true;
      btnEditTeam.Click += new EventHandler(btnEditTeam_Click);
      componentResourceManager.ApplyResources((object) btnNewTeam, "btnNewTeam");
      btnNewTeam.Name = "btnNewTeam";
      btnNewTeam.UseVisualStyleBackColor = true;
      btnNewTeam.Click += new EventHandler(btnNewTeam_Click);
      componentResourceManager.ApplyResources((object) cmbTeams, "cmbTeams");
      cmbTeams.BackColor = Color.FromArgb(64, 64, 64);
      cmbTeams.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbTeams.ForeColor = Color.White;
      cmbTeams.FormattingEnabled = true;
      cmbTeams.Name = "cmbTeams";
      cmbTeams.SelectedIndexChanged += new EventHandler(cmbTeams_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) lbAvailablePlayers, "lbAvailablePlayers");
      lbAvailablePlayers.BackColor = Color.Gray;
      lbAvailablePlayers.ForeColor = Color.White;
      lbAvailablePlayers.FormattingEnabled = true;
      lbAvailablePlayers.Name = "lbAvailablePlayers";
      componentResourceManager.ApplyResources((object) label6, "label6");
      label6.ForeColor = Color.White;
      label6.Name = "label6";
      componentResourceManager.ApplyResources((object) btnRemovePlayerFromGame, "btnRemovePlayerFromGame");
      btnRemovePlayerFromGame.Name = "btnRemovePlayerFromGame";
      btnRemovePlayerFromGame.UseVisualStyleBackColor = true;
      btnRemovePlayerFromGame.Click += new EventHandler(btnRemovePlayerFromGame_Click);
      componentResourceManager.ApplyResources((object) btnSelectPlayerIntoGame, "btnSelectPlayerIntoGame");
      btnSelectPlayerIntoGame.Name = "btnSelectPlayerIntoGame";
      btnSelectPlayerIntoGame.UseVisualStyleBackColor = true;
      btnSelectPlayerIntoGame.Click += new EventHandler(btnSelectPlayerIntoGame_Click);
      componentResourceManager.ApplyResources((object) lbActPlayers, "lbActPlayers");
      lbActPlayers.BackColor = Color.Gray;
      lbActPlayers.ForeColor = Color.White;
      lbActPlayers.FormattingEnabled = true;
      lbActPlayers.Name = "lbActPlayers";
      componentResourceManager.ApplyResources((object) pnlSettings, "pnlSettings");
      pnlSettings.BackColor = Color.FromArgb(64, 64, 64);
      pnlSettings.Controls.Add((Control) lblProgramVersion);
      pnlSettings.Controls.Add((Control) cbEdgeLightSettingsVisible);
      pnlSettings.Controls.Add((Control) label14);
      pnlSettings.Controls.Add((Control) label13);
      pnlSettings.Controls.Add((Control) nudSC_HornLength);
      pnlSettings.Controls.Add((Control) nudHornLength);
      pnlSettings.Controls.Add((Control) txtServerName);
      pnlSettings.Controls.Add((Control) label12);
      pnlSettings.Controls.Add((Control) label11);
      pnlSettings.Controls.Add((Control) cbLanAvailable);
      pnlSettings.Controls.Add((Control) nudUDP_Port);
      pnlSettings.Controls.Add((Control) nudConsolePort);
      pnlSettings.Controls.Add((Control) btnSaveParameter);
      pnlSettings.Controls.Add((Control) label10);
      pnlSettings.Controls.Add((Control) label16);
      pnlSettings.Controls.Add((Control) label15);
      pnlSettings.Name = "pnlSettings";
      componentResourceManager.ApplyResources((object) lblProgramVersion, "lblProgramVersion");
      lblProgramVersion.ForeColor = Color.Silver;
      lblProgramVersion.Name = "lblProgramVersion";
      componentResourceManager.ApplyResources((object) cbEdgeLightSettingsVisible, "cbEdgeLightSettingsVisible");
      cbEdgeLightSettingsVisible.Checked = Settings.Default.BasketballEdgeLightSettingsVisible;
      cbEdgeLightSettingsVisible.DataBindings.Add(new Binding("Checked", (object) Settings.Default, "BasketballEdgeLightSettingsVisible", true, DataSourceUpdateMode.OnPropertyChanged));
      cbEdgeLightSettingsVisible.ForeColor = Color.White;
      cbEdgeLightSettingsVisible.Name = "cbEdgeLightSettingsVisible";
      cbEdgeLightSettingsVisible.UseVisualStyleBackColor = true;
      cbEdgeLightSettingsVisible.Click += new EventHandler(cbEdgeLightSettingsVisible_Click);
      componentResourceManager.ApplyResources((object) label14, "label14");
      label14.ForeColor = Color.White;
      label14.Name = "label14";
      componentResourceManager.ApplyResources((object) label13, "label13");
      label13.ForeColor = Color.White;
      label13.Name = "label13";
      componentResourceManager.ApplyResources((object) nudSC_HornLength, "nudSC_HornLength");
      nudSC_HornLength.Maximum = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      nudSC_HornLength.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      nudSC_HornLength.Name = "nudSC_HornLength";
      nudSC_HornLength.ReadOnly = true;
      nudSC_HornLength.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) nudHornLength, "nudHornLength");
      nudHornLength.Maximum = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      nudHornLength.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      nudHornLength.Name = "nudHornLength";
      nudHornLength.ReadOnly = true;
      nudHornLength.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) txtServerName, "txtServerName");
      txtServerName.Name = "txtServerName";
      componentResourceManager.ApplyResources((object) label12, "label12");
      label12.ForeColor = Color.White;
      label12.Name = "label12";
      componentResourceManager.ApplyResources((object) label11, "label11");
      label11.ForeColor = Color.White;
      label11.Name = "label11";
      componentResourceManager.ApplyResources((object) cbLanAvailable, "cbLanAvailable");
      cbLanAvailable.ForeColor = Color.White;
      cbLanAvailable.Name = "cbLanAvailable";
      cbLanAvailable.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) nudUDP_Port, "nudUDP_Port");
      nudUDP_Port.Maximum = new Decimal(new int[4]
      {
        10000,
        0,
        0,
        0
      });
      nudUDP_Port.Name = "nudUDP_Port";
      componentResourceManager.ApplyResources((object) nudConsolePort, "nudConsolePort");
      nudConsolePort.Maximum = new Decimal(new int[4]
      {
        32,
        0,
        0,
        0
      });
      nudConsolePort.Name = "nudConsolePort";
      nudConsolePort.ReadOnly = true;
      componentResourceManager.ApplyResources((object) btnSaveParameter, "btnSaveParameter");
      btnSaveParameter.Name = "btnSaveParameter";
      btnSaveParameter.UseVisualStyleBackColor = true;
      btnSaveParameter.Click += new EventHandler(btnSaveParameter_Click);
      componentResourceManager.ApplyResources((object) label10, "label10");
      label10.ForeColor = Color.White;
      label10.Name = "label10";
      componentResourceManager.ApplyResources((object) label16, "label16");
      label16.ForeColor = Color.White;
      label16.Name = "label16";
      componentResourceManager.ApplyResources((object) label15, "label15");
      label15.ForeColor = Color.White;
      label15.Name = "label15";
      componentResourceManager.ApplyResources((object) txtDatabasePassword, "txtDatabasePassword");
      txtDatabasePassword.DataBindings.Add(new Binding("Text", (object) Settings.Default, "DatabasePassword", true, DataSourceUpdateMode.OnPropertyChanged));
      txtDatabasePassword.Name = "txtDatabasePassword";
      txtDatabasePassword.ReadOnly = true;
      txtDatabasePassword.Text = Settings.Default.DatabasePassword;
      componentResourceManager.ApplyResources((object) txtDatabaseName, "txtDatabaseName");
      txtDatabaseName.DataBindings.Add(new Binding("Text", (object) Settings.Default, "DatabaseName", true, DataSourceUpdateMode.OnPropertyChanged));
      txtDatabaseName.Name = "txtDatabaseName";
      txtDatabaseName.ReadOnly = true;
      txtDatabaseName.Text = Settings.Default.DatabaseName;
      componentResourceManager.ApplyResources((object) label2, "label2");
      label2.Name = "label2";
      componentResourceManager.ApplyResources((object) label1, "label1");
      label1.Name = "label1";
      componentResourceManager.ApplyResources((object) btnSetGuestTeam, "btnSetGuestTeam");
      btnSetGuestTeam.Name = "btnSetGuestTeam";
      btnSetGuestTeam.UseVisualStyleBackColor = true;
      btnSetGuestTeam.Click += new EventHandler(btnSetTeam_Click);
      componentResourceManager.ApplyResources((object) btnSetHomeTeam, "btnSetHomeTeam");
      btnSetHomeTeam.Name = "btnSetHomeTeam";
      btnSetHomeTeam.UseVisualStyleBackColor = true;
      btnSetHomeTeam.Click += new EventHandler(btnSetTeam_Click);
      componentResourceManager.ApplyResources((object) label8, "label8");
      label8.ForeColor = Color.White;
      label8.Name = "label8";
      componentResourceManager.ApplyResources((object) label7, "label7");
      label7.ForeColor = Color.White;
      label7.Name = "label7";
      componentResourceManager.ApplyResources((object) btnStartGame, "btnStartGame");
      btnStartGame.Name = "btnStartGame";
      btnStartGame.UseVisualStyleBackColor = true;
      btnStartGame.Click += new EventHandler(btnStartGame_Click);
      componentResourceManager.ApplyResources((object) dateTimePicker1, "dateTimePicker1");
      dateTimePicker1.Name = "dateTimePicker1";
      dateTimePicker1.CloseUp += new EventHandler(dateTimePicker1_CloseUp);
      componentResourceManager.ApplyResources((object) btnDeleteGame, "btnDeleteGame");
      btnDeleteGame.Name = "btnDeleteGame";
      btnDeleteGame.UseVisualStyleBackColor = true;
      btnDeleteGame.Click += new EventHandler(btnDeleteGame_Click);
      componentResourceManager.ApplyResources((object) btnNewGame, "btnNewGame");
      btnNewGame.Name = "btnNewGame";
      btnNewGame.UseVisualStyleBackColor = true;
      btnNewGame.Click += new EventHandler(btnNewGame_Click);
      componentResourceManager.ApplyResources((object) lblGuestTeamName, "lblGuestTeamName");
      lblGuestTeamName.BackColor = Color.FromArgb(64, 64, 64);
      lblGuestTeamName.BorderStyle = BorderStyle.FixedSingle;
      lblGuestTeamName.ForeColor = Color.White;
      lblGuestTeamName.Name = "lblGuestTeamName";
      componentResourceManager.ApplyResources((object) lblHomeTeamName, "lblHomeTeamName");
      lblHomeTeamName.BackColor = Color.FromArgb(64, 64, 64);
      lblHomeTeamName.BorderStyle = BorderStyle.FixedSingle;
      lblHomeTeamName.ForeColor = Color.White;
      lblHomeTeamName.Name = "lblHomeTeamName";
      componentResourceManager.ApplyResources((object) lbGuestTeamPlayers, "lbGuestTeamPlayers");
      lbGuestTeamPlayers.BackColor = Color.Gray;
      lbGuestTeamPlayers.ForeColor = Color.White;
      lbGuestTeamPlayers.FormattingEnabled = true;
      lbGuestTeamPlayers.Name = "lbGuestTeamPlayers";
      componentResourceManager.ApplyResources((object) lbHomeTeamPlayers, "lbHomeTeamPlayers");
      lbHomeTeamPlayers.BackColor = Color.Gray;
      lbHomeTeamPlayers.ForeColor = Color.White;
      lbHomeTeamPlayers.FormattingEnabled = true;
      lbHomeTeamPlayers.Name = "lbHomeTeamPlayers";
      componentResourceManager.ApplyResources((object) cmbAvailableGames, "cmbAvailableGames");
      cmbAvailableGames.BackColor = Color.FromArgb(64, 64, 64);
      cmbAvailableGames.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbAvailableGames.ForeColor = Color.White;
      cmbAvailableGames.FormattingEnabled = true;
      cmbAvailableGames.Name = "cmbAvailableGames";
      cmbAvailableGames.SelectedIndexChanged += new EventHandler(cmbAvailableGames_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) label9, "label9");
      label9.ForeColor = Color.White;
      label9.Name = "label9";
      componentResourceManager.ApplyResources((object) cbSortOrderGames, "cbSortOrderGames");
      cbSortOrderGames.Checked = Settings.Default.AvailableGamesSortByIncreasingDate;
      cbSortOrderGames.DataBindings.Add(new Binding("Checked", (object) Settings.Default, "AvailableGamesSortByIncreasingDate", true, DataSourceUpdateMode.OnPropertyChanged));
      cbSortOrderGames.ForeColor = Color.White;
      cbSortOrderGames.Name = "cbSortOrderGames";
      cbSortOrderGames.UseVisualStyleBackColor = true;
      cbSortOrderGames.Click += new EventHandler(cbSortOrderGames_Click);
      componentResourceManager.ApplyResources((object) lblAlreadyStarted, "lblAlreadyStarted");
      lblAlreadyStarted.Name = "lblAlreadyStarted";
      componentResourceManager.ApplyResources((object) lblAlreadyStartedCaption, "lblAlreadyStartedCaption");
      lblAlreadyStartedCaption.Name = "lblAlreadyStartedCaption";
      componentResourceManager.ApplyResources((object) lblRemovePlayerFromGame, "lblRemovePlayerFromGame");
      lblRemovePlayerFromGame.Name = "lblRemovePlayerFromGame";
      componentResourceManager.ApplyResources((object) lblPlayerAlreadyActive, "lblPlayerAlreadyActive");
      lblPlayerAlreadyActive.Name = "lblPlayerAlreadyActive";
      componentResourceManager.ApplyResources((object) lblMxPlayerReached, "lblMxPlayerReached");
      lblMxPlayerReached.Name = "lblMxPlayerReached";
      componentResourceManager.ApplyResources((object) lblServerName, "lblServerName");
      lblServerName.ForeColor = SystemColors.ControlDarkDark;
      lblServerName.Name = "lblServerName";
      componentResourceManager.ApplyResources((object) labelSetGameParameterPlease, "labelSetGameParameterPlease");
      labelSetGameParameterPlease.Name = "labelSetGameParameterPlease";
      componentResourceManager.ApplyResources((object) LabelCaution, "LabelCaution");
      LabelCaution.Name = "LabelCaution";
      componentResourceManager.ApplyResources((object) cbIsReservePlayer, "cbIsReservePlayer");
      cbIsReservePlayer.ForeColor = Color.White;
      cbIsReservePlayer.Name = "cbIsReservePlayer";
      cbIsReservePlayer.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) btnReferees, "btnReferees");
      btnReferees.Name = "btnReferees";
      btnReferees.UseVisualStyleBackColor = true;
      btnReferees.Click += new EventHandler(btnReferees_Click);
      componentResourceManager.ApplyResources((object) btnClearAllActivePlayers, "btnClearAllActivePlayers");
      btnClearAllActivePlayers.Name = "btnClearAllActivePlayers";
      btnClearAllActivePlayers.UseVisualStyleBackColor = true;
      btnClearAllActivePlayers.Click += new EventHandler(btnClearAllActivePlayers_Click);
      timer1.Enabled = true;
      timer1.Tick += new EventHandler(timer1_Tick);
      componentResourceManager.ApplyResources((object) lblProgramVersion1, "lblProgramVersion1");
      lblProgramVersion1.ForeColor = Color.Silver;
      lblProgramVersion1.Name = "lblProgramVersion1";
      componentResourceManager.ApplyResources((object) this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.FromArgb(42, 0, 0);
      Controls.Add((Control) lblProgramVersion1);
      Controls.Add((Control) pnlSettings);
      Controls.Add((Control) btnClearAllActivePlayers);
      Controls.Add((Control) btnReferees);
      Controls.Add((Control) cbIsReservePlayer);
      Controls.Add((Control) labelSetGameParameterPlease);
      Controls.Add((Control) LabelCaution);
      Controls.Add((Control) cmbAvailableGames);
      Controls.Add((Control) lblServerName);
      Controls.Add((Control) lblRemovePlayerFromGame);
      Controls.Add((Control) lblPlayerAlreadyActive);
      Controls.Add((Control) lblMxPlayerReached);
      Controls.Add((Control) txtDatabasePassword);
      Controls.Add((Control) lblAlreadyStartedCaption);
      Controls.Add((Control) txtDatabaseName);
      Controls.Add((Control) label2);
      Controls.Add((Control) lblAlreadyStarted);
      Controls.Add((Control) label1);
      Controls.Add((Control) cbSortOrderGames);
      Controls.Add((Control) label9);
      Controls.Add((Control) btnStartGame);
      Controls.Add((Control) dateTimePicker1);
      Controls.Add((Control) btnDeleteGame);
      Controls.Add((Control) btnNewGame);
      Controls.Add((Control) lblGuestTeamName);
      Controls.Add((Control) lblHomeTeamName);
      Controls.Add((Control) lbGuestTeamPlayers);
      Controls.Add((Control) lbHomeTeamPlayers);
      Controls.Add((Control) btnSetGuestTeam);
      Controls.Add((Control) btnSetHomeTeam);
      Controls.Add((Control) label6);
      Controls.Add((Control) btnRemovePlayerFromGame);
      Controls.Add((Control) btnSelectPlayerIntoGame);
      Controls.Add((Control) lbActPlayers);
      Controls.Add((Control) label5);
      Controls.Add((Control) label4);
      Controls.Add((Control) label3);
      Controls.Add((Control) pbTeamLogo);
      Controls.Add((Control) btnDeleteTeam);
      Controls.Add((Control) btnEditTeam);
      Controls.Add((Control) btnNewTeam);
      Controls.Add((Control) cmbTeams);
      Controls.Add((Control) lbAvailablePlayers);
      Controls.Add((Control) btnGameParameter);
      Controls.Add((Control) cmbGameKinds);
      Controls.Add((Control) label8);
      Controls.Add((Control) label7);
      DataBindings.Add(new Binding("Location", (object) Settings.Default, "StartLocation", true, DataSourceUpdateMode.OnPropertyChanged));
      KeyPreview = true;
      Location = Settings.Default.StartLocation;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = nameof (Form1);
      FormClosing += new FormClosingEventHandler(Form1_FormClosing);
      Load += new EventHandler(Form1_Load);
      KeyDown += new KeyEventHandler(Form1_KeyDown);
      ((ISupportInitialize) pbTeamLogo).EndInit();
      pnlSettings.ResumeLayout(false);
      pnlSettings.PerformLayout();
      nudSC_HornLength.EndInit();
      nudHornLength.EndInit();
      nudUDP_Port.EndInit();
      nudConsolePort.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    public void disposeFormAsync(Form target)
    {
      if (target.InvokeRequired)
      {
        target.Invoke((Delegate) new Form1.disposeFormAsyncDelegate(disposeFormAsync), (object) target);
      }
      else
      {
        target.Dispose();
        target = (Form) null;
      }
    }

    public void HideForm()
    {
      Hide();
    }

    public Form1()
    {
      InitializeComponent();
      try
      {
        Settings.Default.Reload();
      }
      catch
      {
        Settings.Default.Reset();
        Settings.Default.Reload();
      }
      _program_version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      _program_version = "20120313";
      _program_version = "$Version: 03.24 $ ($VersDate: 2016-JAN-21 $, $VersTime: 13:38 $)";
      _program_version = _program_version.Replace(" $", "");
      _program_version = _program_version.Replace("$", "");
      _program_version = _program_version.Replace("Version: ", "");
      _program_version = _program_version.Replace("VersDate: ", "");
      _program_version = _program_version.Replace("VersTime: ", "");
      lblProgramVersion.Text = "Programmversion " + _program_version;
      lblProgramVersion1.Text = lblProgramVersion.Text;
      _program_version = "$Version: 03.24 $";
      _program_version = _program_version.Replace(" $", "");
      _program_version = _program_version.Replace("$", "");
      _program_version = _program_version.Replace("Version: ", "");
      Text = Text + "    (Version " + _program_version + ")";
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
      string str = Application.StartupPath + "\\Sport.cfg";
      if (e.Control && e.Alt && e.Shift)
      {
        switch (e.KeyCode)
        {
          case Keys.A:
            if (System.IO.File.Exists(str))
              System.IO.File.Delete(str);
            int num1 = (int) MessageBox.Show("Program has to be restarted!");
            Close();
            break;
          case Keys.E:
            ProcessStartInfo processStartInfo1 = new ProcessStartInfo("notepad.exe", Application.StartupPath + "\\Parameter.cfg");
            Process process1 = new Process();
            process1.StartInfo = processStartInfo1;
            process1.Start();
            process1.WaitForExit();
            int num2 = (int) MessageBox.Show("Program has to be restarted!");
            Close();
            break;
          case Keys.R:
            Settings.Default.Reset();
            int num3 = (int) MessageBox.Show("Program has to be restarted!", "Default settings saved!");
            Close();
            break;
          case Keys.S:
            nudConsolePort.Value = (Decimal) Settings.Default.ConsolePort;
            nudUDP_Port.Value = (Decimal) Settings.Default.UDP_Port;
            nudHornLength.Value = (Decimal) (Settings.Default.HornLength / 1000);
            nudSC_HornLength.Value = (Decimal) (Settings.Default.ShotclockHornLength / 1000);
            txtServerName.Text = Settings.Default.ServerName;
            cbLanAvailable.Checked = Settings.Default.LAN;
            pnlSettings.Visible = true;
            break;
          case Keys.V:
            if (System.IO.File.Exists(str))
              System.IO.File.Delete(str);
            StreamWriter streamWriter = new StreamWriter(str);
            streamWriter.WriteLine(cmbGameKinds.Text.Trim());
            streamWriter.Close();
            ProcessStartInfo processStartInfo2 = new ProcessStartInfo("notepad.exe", str);
            Process process2 = new Process();
            process2.StartInfo = processStartInfo2;
            process2.Start();
            process2.WaitForExit();
            int num4 = (int) MessageBox.Show("Program has to be restarted!");
            Close();
            break;
        }
      }
      else
      {
        if (e.KeyCode != Keys.Return)
          return;
        pnlSettings.Visible = false;
        Settings.Default.DatabaseName = txtDatabaseName.Text.Trim();
        Settings.Default.DatabasePassword = txtDatabasePassword.Text.Trim();
        Settings.Default.Save();
      }
    }

    private void cmbGameKinds_SelectedIndexChanged(object sender, EventArgs e)
    {
      Settings.Default.LastGameKindIndex = cmbGameKinds.SelectedIndex;
      Settings.Default.Save();
      cbIsReservePlayer.Visible = _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName.Trim() == "Soccer";
      btnReferees.Visible = _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName.Trim() == "Soccer";
      _game_parameters = _dbfunc.GameParameter(_kinds_of_game[cmbGameKinds.SelectedIndex].ID, 1);
      if (_game_parameters == null)
      {
        int num = (int) MessageBox.Show(labelSetGameParameterPlease.Text + cmbGameKinds.Text, LabelCaution.Text);
        btnGameParameter_Click((object) this, (EventArgs) null);
      }
      Dictionary<string, PeriodParameter> dictionary = _dbfunc.PeriodParameterDictionary(_kinds_of_game[cmbGameKinds.SelectedIndex].ID, 1);
      _max_player = dictionary == null ? 15 : (!dictionary.ContainsKey("MaxPlayer") ? 15 : Convert.ToInt32(dictionary["MaxPlayer"].ParameterIntValue));
      if (_kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName == "Tennis" || _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName == "Badminton")
      {
        Height = 200;
        label5.Visible = false;
        label6.Visible = false;
        lbAvailablePlayers.Visible = false;
        lbGuestTeamPlayers.Visible = false;
        lbHomeTeamPlayers.Visible = false;
        lbActPlayers.Visible = false;
        btnClearAllActivePlayers.Visible = false;
      }
      else
      {
        Height = 456;
        label5.Visible = true;
        label6.Visible = true;
        lbAvailablePlayers.Visible = true;
        lbGuestTeamPlayers.Visible = true;
        lbHomeTeamPlayers.Visible = true;
        lbActPlayers.Visible = true;
        btnClearAllActivePlayers.Visible = true;
      }
      cmbAvailableGames.Items.Clear();
      lblHomeTeamName.Text = string.Empty;
      lblGuestTeamName.Text = string.Empty;
      lbHomeTeamPlayers.Items.Clear();
      lbGuestTeamPlayers.Items.Clear();
      _temp_game = (Game) null;
      _players = (List<Player>) null;
      _home_gameplayers = (List<GamePlayer>) null;
      _guest_gameplayers = (List<GamePlayer>) null;
      pbTeamLogo.Image = (Image) null;
      cmbTeams.Items.Clear();
      dateTimePicker1.Value = DateTime.Now;
      lbAvailablePlayers.Items.Clear();
      lbActPlayers.Items.Clear();
      _teams = _dbfunc.ReadTeams(_kinds_of_game[cmbGameKinds.SelectedIndex].ID);
      cmbTeams.Items.Clear();
      if (_teams != null)
      {
        for (int index = 0; index < _teams.Count; ++index)
        {
          try
          {
            cmbTeams.Items.Add((object) _teams[index].TeamName.Trim());
          }
          catch
          {
          }
        }
        if (_teams.Count > 0)
          cmbTeams.SelectedIndex = 0;
      }
      _refresh_available_games();
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      Settings.Default.LAN = cbLanAvailable.Checked;
      Settings.Default.LastGameKindIndex = cmbGameKinds.SelectedIndex;
      Settings.Default.Save();
    }

    private void btnGameParameter_Click(object sender, EventArgs e)
    {
      try
      {
        switch (_kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName.Trim())
        {
          case "Basketball":
            int num1 = (int) new SettingsBasketball(_servername, _databasename, _username, _password, _kinds_of_game[cmbGameKinds.SelectedIndex].ID).ShowDialog();
            break;
        }
        cmbGameKinds_SelectedIndexChanged((object) this, (EventArgs) null);
      }
      catch
      {
      }
    }

    private void btnNewTeam_Click(object sender, EventArgs e)
    {
      int num = (int) new TeamEditor(_servername, _databasename, _username, _password, _kinds_of_game[cmbGameKinds.SelectedIndex], (Team) null, false).ShowDialog();
      cmbGameKinds_SelectedIndexChanged((object) this, (EventArgs) null);
      for (int index = 0; index < cmbTeams.Items.Count; ++index)
      {
        if (_teams[index].TeamID == Settings.Default.LastTeamID)
          cmbTeams.SelectedIndex = index;
      }
      Settings.Default.LastTeamID = -1;
      Settings.Default.Save();
    }

    private void btnEditTeam_Click(object sender, EventArgs e)
    {
      if (cmbTeams.SelectedIndex <= -1)
        return;
      bool flag = _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName.Trim() == "Icehockey";
      int num = (int) new TeamEditor(_servername, _databasename, _username, _password, _kinds_of_game[cmbGameKinds.SelectedIndex], _teams[cmbTeams.SelectedIndex], false).ShowDialog();
      _teams = _dbfunc.ReadTeams(_kinds_of_game[cmbGameKinds.SelectedIndex].ID);
      cmbTeams.Items.Clear();
      if (_teams != null)
      {
        for (int index = 0; index < _teams.Count; ++index)
          cmbTeams.Items.Add((object) _teams[index].TeamName.Trim());
        if (_teams.Count > 0)
          cmbTeams.SelectedIndex = 0;
      }
      for (int index = 0; index < cmbTeams.Items.Count; ++index)
      {
        if (_teams[index].TeamID == Settings.Default.LastTeamID)
          cmbTeams.SelectedIndex = index;
      }
      Settings.Default.LastTeamID = -1;
      Settings.Default.Save();
    }

    private void btnDeleteTeam_Click(object sender, EventArgs e)
    {
      if (cmbTeams.SelectedIndex <= -1)
        return;
      if (MessageBox.Show("Die Mannschaft " + cmbTeams.Text.Trim() + " wird gelöscht!", "Achtung:", MessageBoxButtons.OKCancel) == DialogResult.OK)
        _dbfunc.DeleteTeam(_teams[cmbTeams.SelectedIndex].TeamID);
      cmbGameKinds_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void cmbTeams_SelectedIndexChanged(object sender, EventArgs e)
    {
      cbIsReservePlayer.Checked = false;
      if (cmbTeams.SelectedIndex > -1)
      {
        pbTeamLogo.Image = (Image) null;
        if (_teams[cmbTeams.SelectedIndex].TeamLogo != null)
        {
          try
          {
            MemoryStream memoryStream = new MemoryStream(_teams[cmbTeams.SelectedIndex].TeamLogo, 0, _teams[cmbTeams.SelectedIndex].TeamLogo.Length);
            pbTeamLogo.Image = (Image) new Bitmap((Stream) memoryStream);
            memoryStream.Close();
          }
          catch
          {
            pbTeamLogo.Image = (Image) null;
          }
        }
        string gameKindName = _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName;
        _players = _dbfunc.ReadPlayers(_teams[cmbTeams.SelectedIndex].TeamID, false);
        if (gameKindName == "Tennis")
          _players = (List<Player>) null;
        if (gameKindName == "Badminton")
          _players = (List<Player>) null;
        lbAvailablePlayers.Items.Clear();
        lbActPlayers.Items.Clear();
        if (_players == null)
          return;
        for (int index = 0; index < _players.Count; ++index)
        {
          if (_players[index].PlayerNumber > -1)
            lbAvailablePlayers.Items.Add((object) (_players[index].PlayerNumber.ToString().PadLeft(2) + ", " + _players[index].PlayerName.Trim()));
          if (_players[index].PlayerNumber > -1 && _players[index].PlayerIsActive)
            lbActPlayers.Items.Add((object) (_players[index].PlayerNumber.ToString().PadLeft(2) + ", " + _players[index].PlayerName.Trim()));
        }
      }
      else
        pbTeamLogo.Image = (Image) null;
    }

    private void btnSelectPlayerIntoGame_Click(object sender, EventArgs e)
    {
      if (lbAvailablePlayers.SelectedIndex <= -1)
        return;
      if (!_players[lbAvailablePlayers.SelectedIndex].PlayerIsActive && lbActPlayers.Items.Count < _max_player)
      {
        _players[lbAvailablePlayers.SelectedIndex].PlayerIsActive = true;
        _players[lbAvailablePlayers.SelectedIndex].PlayerIsReservePlayer = cbIsReservePlayer.Checked;
        lbActPlayers.Items.Clear();
        for (int index = 0; index < _players.Count; ++index)
        {
          if (_players[index].PlayerIsActive)
            lbActPlayers.Items.Add((object) (_players[index].PlayerNumber.ToString().PadLeft(2) + ", " + _players[index].PlayerName));
        }
        _dbfunc.SavePlayers(_players);
        if (lbAvailablePlayers.SelectedIndex < lbAvailablePlayers.Items.Count - 1)
          ++lbAvailablePlayers.SelectedIndex;
        else
          lbAvailablePlayers.SelectedIndex = 0;
      }
      else if (_players[lbAvailablePlayers.SelectedIndex].PlayerIsActive)
      {
        int num1 = (int) MessageBox.Show(lblPlayerAlreadyActive.Text);
      }
      else
      {
        if (lbActPlayers.Items.Count < _max_player)
          return;
        int num2 = (int) MessageBox.Show(lblRemovePlayerFromGame.Text, lblMxPlayerReached.Text);
      }
    }

    private void btnRemovePlayerFromGame_Click(object sender, EventArgs e)
    {
      if (lbActPlayers.SelectedIndex <= -1)
        return;
      int int32 = Convert.ToInt32(lbActPlayers.SelectedItem.ToString().Substring(0, 2).Trim());
      for (int index = 0; index < _players.Count; ++index)
      {
        if (_players[index].PlayerNumber == int32)
          _players[index].PlayerIsActive = false;
      }
      lbActPlayers.Items.Clear();
      for (int index = 0; index < lbAvailablePlayers.Items.Count; ++index)
      {
        if (_players[index].PlayerIsActive)
          lbActPlayers.Items.Add((object) (_players[index].PlayerNumber.ToString().PadLeft(2) + ", " + _players[index].PlayerName));
      }
      _dbfunc.SavePlayers(_players);
    }

    private void btnClearAllActivePlayers_Click(object sender, EventArgs e)
    {
      cbIsReservePlayer.Checked = false;
      for (int index1 = 0; index1 < lbActPlayers.Items.Count; ++index1)
      {
        lbActPlayers.SelectedIndex = index1;
        for (int index2 = 0; index2 < _players.Count; ++index2)
          _players[index2].PlayerIsActive = false;
        _dbfunc.SavePlayers(_players);
        cmbTeams_SelectedIndexChanged((object) this, (EventArgs) null);
      }
    }

    private void btnSaveParameter_Click(object sender, EventArgs e)
    {
      if (txtServerName.Text.Trim() != string.Empty)
        Settings.Default.ServerName = txtServerName.Text.Trim();
      lblServerName.Text = txtServerName.Text.Trim();
      Settings.Default.LAN = cbLanAvailable.Checked;
      Settings.Default.UDP_Port = Convert.ToInt32(nudUDP_Port.Value);
      Settings.Default.DatabaseName = txtDatabaseName.Text.Trim();
      Settings.Default.DatabasePassword = txtDatabasePassword.Text.Trim();
      Settings.Default.ConsolePort = Convert.ToInt32(nudConsolePort.Value);
      Settings.Default.HornLength = Convert.ToInt32(nudHornLength.Value) * 1000;
      Settings.Default.ShotclockHornLength = Convert.ToInt32(nudSC_HornLength.Value) * 1000;
      Settings.Default.Save();
      _servername = Settings.Default.ServerName;
      _databasename = Settings.Default.DatabaseName;
      _password = Settings.Default.DatabasePassword;
      MsgLookForServer msgLookForServer = new MsgLookForServer(_servername);
      msgLookForServer.Show();
      msgLookForServer.Update();
      if (ConnectDatabase())
      {
        pnlSettings.Visible = false;
        _refresh_kind_of_games();
      }
      msgLookForServer.Close();
    }

    private bool ConnectDatabase(string DatabaseName)
    {
      bool flag;
      if (!_dbfunc.ConnectDatabase(_servername, DatabaseName, _username))
      {
        int num = (int) MessageBox.Show("Server not connected!");
        flag = false;
      }
      else
        flag = true;
      return flag;
    }

    private bool ConnectDatabase()
    {
      bool flag = true;
      _dbfunc.ServerName = Settings.Default.ServerName;
      if (!_dbfunc.ConnectDatabase())
      {
        int num = (int) MessageBox.Show("Server not connected!");
        flag = false;
      }
      return flag;
    }

    private void btnSetTeam_Click(object sender, EventArgs e)
    {
      if (_temp_game == null)
        _temp_game = new Game();
      if (!_temp_game.Started)
      {
        _temp_game.GameKindID = _kinds_of_game[cmbGameKinds.SelectedIndex].ID;
        _temp_game.DateTime = dateTimePicker1.Value;
        if ((Button) sender == btnSetHomeTeam && _temp_game.GuestTeamName.Trim() != cmbTeams.Text.Trim())
        {
          _temp_game.HomeTeamID = _teams[cmbTeams.SelectedIndex].TeamID;
          _temp_game.HomeTeamName = _teams[cmbTeams.SelectedIndex].TeamName.ToUpper();
          _temp_game.HomeTeamLogo = _dbfunc.ByteArrayToImage(_teams[cmbTeams.SelectedIndex].TeamLogo);
          lblHomeTeamName.Text = _temp_game.HomeTeamName;
          _select_players_into_game(true);
        }
        if ((Button) sender == btnSetGuestTeam && _temp_game.HomeTeamName.Trim() != cmbTeams.Text.Trim())
        {
          _temp_game.GuestTeamID = _teams[cmbTeams.SelectedIndex].TeamID;
          _temp_game.GuestTeamName = _teams[cmbTeams.SelectedIndex].TeamName.ToUpper();
          _temp_game.GuestTeamLogo = _dbfunc.ByteArrayToImage(_teams[cmbTeams.SelectedIndex].TeamLogo);
          lblGuestTeamName.Text = _temp_game.GuestTeamName;
          _select_players_into_game(false);
        }
        _refresh_game_players();
        if (!(lblHomeTeamName.Text.Trim() != string.Empty) || !(lblGuestTeamName.Text.Trim() != string.Empty))
          return;
        List<GamePlayer> homeGameplayers = _home_gameplayers;
        List<GamePlayer> guestGameplayers = _guest_gameplayers;
        _temp_game.ID = _dbfunc.SaveGame(_temp_game);
        if (homeGameplayers != null)
        {
          for (int index = 0; index < homeGameplayers.Count; ++index)
            homeGameplayers[index].GameID = _temp_game.ID;
          _dbfunc.SaveGamePlayers(homeGameplayers);
        }
        if (guestGameplayers != null)
        {
          for (int index = 0; index < guestGameplayers.Count; ++index)
            guestGameplayers[index].GameID = _temp_game.ID;
          _dbfunc.SaveGamePlayers(guestGameplayers);
        }
        _refresh_available_games();
      }
      else
      {
        int num = (int) MessageBox.Show(lblAlreadyStarted.Text, lblAlreadyStartedCaption.Text);
      }
    }

    private void _select_players_into_game(bool IsHomeTeam)
    {
      if (_temp_game == null)
        return;
      if (!_temp_game.Started)
      {
        if (IsHomeTeam)
        {
          _dbfunc.DeleteGamePlayers(_temp_game.ID, _temp_game.HomeTeamID);
          if (lbActPlayers.Items.Count <= 0)
            return;
          _home_gameplayers.Clear();
          for (int index = 0; index < _players.Count; ++index)
          {
            if (_players[index].PlayerIsActive)
              _home_gameplayers.Add(new GamePlayer(_temp_game.ID, _temp_game.HomeTeamID, _players[index].PlayerNumber, _players[index].PlayerName, 0, 0, _players[index].PlayerIsReservePlayer, 0, _players[index].PlayerIsGoalkeeper, _players[index].PlayerIsCoach));
          }
        }
        else
        {
          _dbfunc.DeleteGamePlayers(_temp_game.ID, _temp_game.GuestTeamID);
          if (lbActPlayers.Items.Count <= 0)
            return;
          _guest_gameplayers.Clear();
          for (int index = 0; index < _players.Count; ++index)
          {
            if (_players[index].PlayerIsActive)
              _guest_gameplayers.Add(new GamePlayer(_temp_game.ID, _temp_game.GuestTeamID, _players[index].PlayerNumber, _players[index].PlayerName, 0, 0, _players[index].PlayerIsReservePlayer, 0, _players[index].PlayerIsGoalkeeper, _players[index].PlayerIsCoach));
          }
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Game was already started!");
      }
    }

    private void _refresh_game_players()
    {
      lbHomeTeamPlayers.Items.Clear();
      if (_home_gameplayers != null)
      {
        for (int index = 0; index < _home_gameplayers.Count; ++index)
        {
          GamePlayer homeGameplayer = _home_gameplayers[index];
          lbHomeTeamPlayers.Items.Add((object) (homeGameplayer.PlayerNumber.ToString().PadLeft(2) + ", " + homeGameplayer.Name));
        }
      }
      lbGuestTeamPlayers.Items.Clear();
      if (_guest_gameplayers == null)
        return;
      for (int index = 0; index < _guest_gameplayers.Count; ++index)
      {
        GamePlayer guestGameplayer = _guest_gameplayers[index];
        lbGuestTeamPlayers.Items.Add((object) (guestGameplayer.PlayerNumber.ToString().PadLeft(2) + ", " + guestGameplayer.Name));
      }
    }

    private void _refresh_available_games()
    {
      if (_game_parameters == null)
        return;
      int num = -1;
      if (_temp_game != null)
        num = _temp_game.ID;
      _games = _dbfunc.ReadGames(_kinds_of_game[cmbGameKinds.SelectedIndex].ID, cbSortOrderGames.Checked);
      cmbAvailableGames.Items.Clear();
      if (_games != null)
      {
        for (int index = 0; index < _games.Count; ++index)
          cmbAvailableGames.Items.Add((object) (_games[index].HomeTeamName.Trim() + " - " + _games[index].GuestTeamName.Trim()));
      }
      if (cmbAvailableGames.Items.Count > 0)
      {
        cmbAvailableGames.SelectedIndex = cmbAvailableGames.Items.Count - 1;
        for (int index = 0; index < _games.Count; ++index)
        {
          if (_games[index].ID == num)
            cmbAvailableGames.SelectedIndex = index;
        }
      }
      else
      {
        cmbAvailableGames.SelectedIndex = -1;
        btnNewGame_Click((object) this, (EventArgs) null);
      }
    }

    private void cmbAvailableGames_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = true;
      if (cmbAvailableGames.SelectedIndex > -1)
      {
        _temp_game = _games[cmbAvailableGames.SelectedIndex];
        lblHomeTeamName.Text = _temp_game.HomeTeamName.Trim();
        lblGuestTeamName.Text = _temp_game.GuestTeamName.Trim();
        dateTimePicker1.Value = _temp_game.DateTime;
        string gameKindName = _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName;
        if (gameKindName == "Tennis")
          flag = false;
        if (gameKindName == "Badminton")
          flag = false;
        if (flag)
        {
          _home_gameplayers = _dbfunc.GamePlayers(_temp_game.ID, _temp_game.HomeTeamID);
          _guest_gameplayers = _dbfunc.GamePlayers(_temp_game.ID, _temp_game.GuestTeamID);
        }
        else
        {
          _home_gameplayers = new List<GamePlayer>();
          _guest_gameplayers = new List<GamePlayer>();
        }
        _refresh_game_players();
        btnStartGame.Enabled = true;
      }
      else
      {
        lblHomeTeamName.Text = string.Empty;
        lblGuestTeamName.Text = string.Empty;
        dateTimePicker1.Value = DateTime.Now;
        _home_gameplayers.Clear();
        _guest_gameplayers.Clear();
        _refresh_game_players();
        btnStartGame.Enabled = false;
      }
    }

    private void btnNewGame_Click(object sender, EventArgs e)
    {
      dateTimePicker1.Value = DateTime.Now;
      _temp_game = new Game();
      if (_home_gameplayers == null)
        _home_gameplayers = new List<GamePlayer>();
      if (_guest_gameplayers == null)
        _guest_gameplayers = new List<GamePlayer>();
      _home_gameplayers.Clear();
      _guest_gameplayers.Clear();
      lblHomeTeamName.Text = string.Empty;
      lblGuestTeamName.Text = string.Empty;
      lbHomeTeamPlayers.Items.Clear();
      lbGuestTeamPlayers.Items.Clear();
      cmbAvailableGames.SelectedIndex = -1;
    }

    private void btnDeleteGame_Click(object sender, EventArgs e)
    {
      if (_temp_game == null)
        return;
      if (MessageBox.Show(_temp_game.HomeTeamName.Trim() + " vs. " + _temp_game.GuestTeamName.Trim() + "(" + _temp_game.DateTime.ToShortDateString() + ") will be deleted!", "Caution:", MessageBoxButtons.OKCancel) != DialogResult.OK)
        return;
      _dbfunc.DeleteGame(_temp_game);
      _temp_game = (Game) null;
      _refresh_available_games();
      if (cmbAvailableGames.SelectedIndex != -1)
        return;
      dateTimePicker1.Value = DateTime.Now;
      lblHomeTeamName.Text = string.Empty;
      lblGuestTeamName.Text = string.Empty;
      lbHomeTeamPlayers.Items.Clear();
      lbGuestTeamPlayers.Items.Clear();
      _home_gameplayers.Clear();
      _guest_gameplayers.Clear();
    }

    private void btnStartGame_Click(object sender, EventArgs e)
    {
      if (cmbAvailableGames.SelectedIndex <= -1)
        return;
      Settings.Default.Save();
      Hide();
      switch (_kinds_of_game[cmbGameKinds.SelectedIndex].GameKindName.Trim())
      {
        case "Basketball":
          _act_game_form = (Form) new Basketball(_program_version, _dbfunc, _games[cmbAvailableGames.SelectedIndex], _home_gameplayers, _guest_gameplayers);
          _act_game_form.Text = _kinds_of_game[cmbGameKinds.SelectedIndex].GameKindNameAlias;
          int num1 = (int) _act_game_form.ShowDialog();
          cmbAvailableGames_SelectedIndexChanged((object) this, (EventArgs) null);
          break;
      }
      _act_game_form = (Form) null;
      Show();
    }

    private void cbSortOrderGames_Click(object sender, EventArgs e)
    {
      _refresh_available_games();
    }

    private void dateTimePicker1_CloseUp(object sender, EventArgs e)
    {
      if (_temp_game == null)
        return;
      _temp_game.DateTime = dateTimePicker1.Value;
      _dbfunc.SaveGame(_temp_game);
      _refresh_available_games();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      if (System.IO.File.Exists(Application.StartupPath + "\\Parameter.cfg"))
      {
        StreamReader streamReader = new StreamReader(Application.StartupPath + "\\Parameter.cfg");
        for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
        {
          string upper = str.ToUpper();
          if (upper.StartsWith("SERVERNAME="))
            Settings.Default.ServerName = upper.Substring(11).Trim();
          if (upper.StartsWith("LAN="))
          {
            Settings.Default.LAN = upper.EndsWith("ON");
            Settings.Default.Save();
          }
        }
        streamReader.Close();
        Settings.Default.Save();
      }
      cbLanAvailable.Checked = Settings.Default.LAN;
      _servername = Settings.Default.ServerName;
      lblServerName.Text = _servername.Trim();
      _databasename = Settings.Default.DatabaseName;
      _password = Settings.Default.DatabasePassword;
      MsgLookForServer msgLookForServer = new MsgLookForServer(_servername);
      msgLookForServer.Show();
      msgLookForServer.Update();
      _dbfunc = new DataBaseFunctions(_servername, _databasename, _username, _password);
      if (ConnectDatabase("Game"))
      {
        _dbfunc = new DataBaseFunctions(_servername, _databasename, _username, _password);
        _dbfunc.ConnectDatabase();
        _refresh_kind_of_games();
      }
      msgLookForServer.Close();
      BringToFront();
    }

    private void _refresh_kind_of_games()
    {
      _dbfunc.FillKindOfGames();
      cmbGameKinds.Items.Clear();
      _kinds_of_game = _dbfunc.ReadKindOfGames();
      for (int index = 0; index < _kinds_of_game.Count; ++index)
        cmbGameKinds.Items.Add((object) _kinds_of_game[index].GameKindNameAlias.Trim());
      if (System.IO.File.Exists(Application.StartupPath + "\\Sport.cfg"))
      {
        ArrayList arrayList = new ArrayList();
        StreamReader streamReader = new StreamReader(Application.StartupPath + "\\Sport.cfg");
        for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
          arrayList.Add((object) str);
        streamReader.Close();
        int count = cmbGameKinds.Items.Count;
        string[] strArray = new string[count];
        for (int index = 0; index < cmbGameKinds.Items.Count; ++index)
          strArray[index] = cmbGameKinds.Items[index].ToString();
        for (int index1 = 0; index1 < count; ++index1)
        {
          if (!arrayList.Contains((object) strArray[index1]))
          {
            for (int index2 = 0; index2 < _kinds_of_game.Count; ++index2)
            {
              if (_kinds_of_game[index2].GameKindNameAlias.Trim() == strArray[index1])
              {
                _kinds_of_game.RemoveAt(index2);
                index2 = _kinds_of_game.Count;
              }
            }
            cmbGameKinds.Items.Remove((object) strArray[index1]);
          }
        }
        cmbGameKinds.Enabled = arrayList.Count > 1;
      }
      if (Settings.Default.LastGameKindIndex < cmbGameKinds.Items.Count)
        cmbGameKinds.SelectedIndex = Settings.Default.LastGameKindIndex;
      else if (_kinds_of_game.Count > 0)
        cmbGameKinds.SelectedIndex = 0;
      else
        cmbGameKinds.SelectedIndex = -1;
      if (cmbGameKinds.Items.Count <= 0 || cmbGameKinds.SelectedIndex >= 0)
        return;
      cmbGameKinds.SelectedIndex = 0;
    }

    private void btnReferees_Click(object sender, EventArgs e)
    {
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      if (_dummy_counter > 0)
      {
        --_dummy_counter;
      }
      else
      {
        _dummy_counter = 10;
        try
        {
          if (Convert.ToBoolean(_act_game_form.Tag))
            return;
          _dbfunc.KeepConnectionAlive();
        }
        catch
        {
          _act_game_form.Tag = (object) 0;
        }
      }
    }

    private void cbEdgeLightSettingsVisible_Click(object sender, EventArgs e)
    {
      if (!cbEdgeLightSettingsVisible.Checked)
        return;
      int num = (int) MessageBox.Show("Diese Einstellungen sind nur bei Basketball für Korbanzeigen 'Stramatel' wirksam!");
    }

    private delegate void disposeFormAsyncDelegate(Form target);
  }
}
