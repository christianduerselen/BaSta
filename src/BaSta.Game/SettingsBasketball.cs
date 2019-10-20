using System;
using System.ComponentModel;
using System.Windows.Forms;
using BaSta.Game.Properties;

namespace BaSta.Game
{
    public class SettingsBasketball : Form
    {
        private string _servername = string.Empty;
        private string _databasename = string.Empty;
        private string _user_id = string.Empty;
        private string _password = string.Empty;
        private int _gamekind_id = -1;
        private SettingsFunctions _settings_functions = new SettingsFunctions();
        private Label label1;
        private Label label3;
        private NumericUpDown MaxTimeouts;
        private CheckBox DoResetTimeoutsAtPeriodstart;
        private Label label4;
        private NumericUpDown TimeoutTime;
        private Label label5;
        private Label label6;
        private Label label7;
        private NumericUpDown PeriodTime;
        private Label label8;
        private Label label9;
        private NumericUpDown ShotTimeLong;
        private Label label10;
        private Label label11;
        private NumericUpDown ShotTimeShort;
        private Label label12;
        private NumericUpDown MaxPlayerFouls;
        private Label label13;
        private NumericUpDown MaxTeamfouls;
        private Label label14;
        private Label label15;
        private NumericUpDown BreakTime;
        private NumericUpDown Period;
        private CheckBox DoResetTeamFoulsAtPeriodStart;
        private NumericUpDown MaxPlayer;
        private Label label2;
        private Button btnEnableChangeMaxSets;
        private Label label16;
        private NumericUpDown nudMaxPeriods;
        private DataBaseFunctions _dbfunc;
        private int _act_period;

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SettingsBasketball));
            label1 = new Label();
            label3 = new Label();
            MaxTimeouts = new NumericUpDown();
            DoResetTimeoutsAtPeriodstart = new CheckBox();
            label4 = new Label();
            TimeoutTime = new NumericUpDown();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            PeriodTime = new NumericUpDown();
            label8 = new Label();
            label9 = new Label();
            ShotTimeLong = new NumericUpDown();
            label10 = new Label();
            label11 = new Label();
            ShotTimeShort = new NumericUpDown();
            label12 = new Label();
            MaxPlayerFouls = new NumericUpDown();
            label13 = new Label();
            MaxTeamfouls = new NumericUpDown();
            label14 = new Label();
            label15 = new Label();
            BreakTime = new NumericUpDown();
            Period = new NumericUpDown();
            DoResetTeamFoulsAtPeriodStart = new CheckBox();
            MaxPlayer = new NumericUpDown();
            label2 = new Label();
            btnEnableChangeMaxSets = new Button();
            label16 = new Label();
            nudMaxPeriods = new NumericUpDown();
            MaxTimeouts.BeginInit();
            TimeoutTime.BeginInit();
            PeriodTime.BeginInit();
            ShotTimeLong.BeginInit();
            ShotTimeShort.BeginInit();
            MaxPlayerFouls.BeginInit();
            MaxTeamfouls.BeginInit();
            BreakTime.BeginInit();
            Period.BeginInit();
            MaxPlayer.BeginInit();
            nudMaxPeriods.BeginInit();
            SuspendLayout();
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.Name = "label1";
            componentResourceManager.ApplyResources((object)label3, "label3");
            label3.Name = "label3";
            componentResourceManager.ApplyResources((object)MaxTimeouts, "MaxTimeouts");
            MaxTimeouts.Name = "MaxTimeouts";
            MaxTimeouts.ReadOnly = true;
            MaxTimeouts.Tag = (object)"2";
            MaxTimeouts.Value = new Decimal(new int[4]
            {
        2,
        0,
        0,
        0
            });
            MaxTimeouts.ValueChanged += new EventHandler(Simple_ValueChanged);
            componentResourceManager.ApplyResources((object)DoResetTimeoutsAtPeriodstart, "DoResetTimeoutsAtPeriodstart");
            DoResetTimeoutsAtPeriodstart.Name = "DoResetTimeoutsAtPeriodstart";
            DoResetTimeoutsAtPeriodstart.Tag = (object)"0";
            DoResetTimeoutsAtPeriodstart.UseVisualStyleBackColor = true;
            DoResetTimeoutsAtPeriodstart.Click += new EventHandler(DoResetChanged_Click);
            componentResourceManager.ApplyResources((object)label4, "label4");
            label4.Name = "label4";
            componentResourceManager.ApplyResources((object)TimeoutTime, "TimeoutTime");
            TimeoutTime.Name = "TimeoutTime";
            TimeoutTime.Tag = (object)"60000";
            TimeoutTime.Value = new Decimal(new int[4]
            {
        60,
        0,
        0,
        0
            });
            TimeoutTime.ValueChanged += new EventHandler(Seconds_ValueChanged_1);
            componentResourceManager.ApplyResources((object)label5, "label5");
            label5.Name = "label5";
            componentResourceManager.ApplyResources((object)label6, "label6");
            label6.Name = "label6";
            componentResourceManager.ApplyResources((object)label7, "label7");
            label7.Name = "label7";
            componentResourceManager.ApplyResources((object)PeriodTime, "PeriodTime");
            PeriodTime.Name = "PeriodTime";
            PeriodTime.Tag = (object)"600000";
            PeriodTime.Value = new Decimal(new int[4]
            {
        10,
        0,
        0,
        0
            });
            PeriodTime.ValueChanged += new EventHandler(Minutes_ValueChanged);
            componentResourceManager.ApplyResources((object)label8, "label8");
            label8.Name = "label8";
            componentResourceManager.ApplyResources((object)label9, "label9");
            label9.Name = "label9";
            componentResourceManager.ApplyResources((object)ShotTimeLong, "ShotTimeLong");
            ShotTimeLong.Name = "ShotTimeLong";
            ShotTimeLong.Tag = (object)"24000";
            ShotTimeLong.Value = new Decimal(new int[4]
            {
        24,
        0,
        0,
        0
            });
            ShotTimeLong.ValueChanged += new EventHandler(Seconds_ValueChanged_1);
            componentResourceManager.ApplyResources((object)label10, "label10");
            label10.Name = "label10";
            componentResourceManager.ApplyResources((object)label11, "label11");
            label11.Name = "label11";
            componentResourceManager.ApplyResources((object)ShotTimeShort, "ShotTimeShort");
            ShotTimeShort.Name = "ShotTimeShort";
            ShotTimeShort.Tag = (object)"14000";
            ShotTimeShort.Value = new Decimal(new int[4]
            {
        14,
        0,
        0,
        0
            });
            ShotTimeShort.ValueChanged += new EventHandler(Seconds_ValueChanged_1);
            componentResourceManager.ApplyResources((object)label12, "label12");
            label12.Name = "label12";
            componentResourceManager.ApplyResources((object)MaxPlayerFouls, "MaxPlayerFouls");
            MaxPlayerFouls.Name = "MaxPlayerFouls";
            MaxPlayerFouls.ReadOnly = true;
            MaxPlayerFouls.Tag = (object)"5";
            MaxPlayerFouls.Value = new Decimal(new int[4]
            {
        5,
        0,
        0,
        0
            });
            MaxPlayerFouls.ValueChanged += new EventHandler(Simple_ValueChanged);
            componentResourceManager.ApplyResources((object)label13, "label13");
            label13.Name = "label13";
            componentResourceManager.ApplyResources((object)MaxTeamfouls, "MaxTeamfouls");
            MaxTeamfouls.Name = "MaxTeamfouls";
            MaxTeamfouls.ReadOnly = true;
            MaxTeamfouls.Tag = (object)"5";
            MaxTeamfouls.Value = new Decimal(new int[4]
            {
        5,
        0,
        0,
        0
            });
            MaxTeamfouls.ValueChanged += new EventHandler(Simple_ValueChanged);
            componentResourceManager.ApplyResources((object)label14, "label14");
            label14.Name = "label14";
            componentResourceManager.ApplyResources((object)label15, "label15");
            label15.Name = "label15";
            BreakTime.Increment = new Decimal(new int[4]
            {
        15,
        0,
        0,
        0
            });
            componentResourceManager.ApplyResources((object)BreakTime, "BreakTime");
            BreakTime.Maximum = new Decimal(new int[4]
            {
        60000,
        0,
        0,
        0
            });
            BreakTime.Name = "BreakTime";
            BreakTime.Tag = (object)"120000";
            BreakTime.Value = new Decimal(new int[4]
            {
        120,
        0,
        0,
        0
            });
            BreakTime.ValueChanged += new EventHandler(Seconds_ValueChanged_1);
            componentResourceManager.ApplyResources((object)Period, "Period");
            Period.Minimum = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            Period.Name = "Period";
            Period.ReadOnly = true;
            Period.Tag = (object)"1";
            Period.Value = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            Period.ValueChanged += new EventHandler(Simple_ValueChanged);
            Period.MouseDown += new MouseEventHandler(Period_MouseDown);
            componentResourceManager.ApplyResources((object)DoResetTeamFoulsAtPeriodStart, "DoResetTeamFoulsAtPeriodStart");
            DoResetTeamFoulsAtPeriodStart.Name = "DoResetTeamFoulsAtPeriodStart";
            DoResetTeamFoulsAtPeriodStart.Tag = (object)"0";
            DoResetTeamFoulsAtPeriodStart.UseVisualStyleBackColor = true;
            DoResetTeamFoulsAtPeriodStart.Click += new EventHandler(DoResetChanged_Click);
            componentResourceManager.ApplyResources((object)MaxPlayer, "MaxPlayer");
            MaxPlayer.Maximum = new Decimal(new int[4]
            {
        99,
        0,
        0,
        0
            });
            MaxPlayer.Name = "MaxPlayer";
            MaxPlayer.ReadOnly = true;
            MaxPlayer.Tag = (object)"12";
            MaxPlayer.Value = new Decimal(new int[4]
            {
        12,
        0,
        0,
        0
            });
            MaxPlayer.ValueChanged += new EventHandler(Simple_ValueChanged);
            componentResourceManager.ApplyResources((object)label2, "label2");
            label2.Name = "label2";
            componentResourceManager.ApplyResources((object)btnEnableChangeMaxSets, "btnEnableChangeMaxSets");
            btnEnableChangeMaxSets.Name = "btnEnableChangeMaxSets";
            btnEnableChangeMaxSets.UseVisualStyleBackColor = true;
            btnEnableChangeMaxSets.Click += new EventHandler(btnEnableChangeMaxSets_Click);
            componentResourceManager.ApplyResources((object)label16, "label16");
            label16.Name = "label16";
            nudMaxPeriods.DataBindings.Add(new Binding("Value", (object)Settings.Default, "MaxPeriodsBasketball", true, DataSourceUpdateMode.OnPropertyChanged));
            componentResourceManager.ApplyResources((object)nudMaxPeriods, "nudMaxPeriods");
            nudMaxPeriods.Minimum = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            nudMaxPeriods.Name = "nudMaxPeriods";
            nudMaxPeriods.ReadOnly = true;
            nudMaxPeriods.Value = Settings.Default.MaxPeriodsBasketball;
            nudMaxPeriods.Click += new EventHandler(nudMaxPeriods_Click);
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add((Control)btnEnableChangeMaxSets);
            Controls.Add((Control)label16);
            Controls.Add((Control)nudMaxPeriods);
            Controls.Add((Control)MaxPlayer);
            Controls.Add((Control)label2);
            Controls.Add((Control)DoResetTeamFoulsAtPeriodStart);
            Controls.Add((Control)Period);
            Controls.Add((Control)label1);
            Controls.Add((Control)label9);
            Controls.Add((Control)label8);
            Controls.Add((Control)ShotTimeLong);
            Controls.Add((Control)label14);
            Controls.Add((Control)ShotTimeShort);
            Controls.Add((Control)MaxTimeouts);
            Controls.Add((Control)label6);
            Controls.Add((Control)label15);
            Controls.Add((Control)label11);
            Controls.Add((Control)label3);
            Controls.Add((Control)label7);
            Controls.Add((Control)BreakTime);
            Controls.Add((Control)label10);
            Controls.Add((Control)DoResetTimeoutsAtPeriodstart);
            Controls.Add((Control)PeriodTime);
            Controls.Add((Control)label13);
            Controls.Add((Control)MaxPlayerFouls);
            Controls.Add((Control)TimeoutTime);
            Controls.Add((Control)label5);
            Controls.Add((Control)MaxTeamfouls);
            Controls.Add((Control)label12);
            Controls.Add((Control)label4);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(SettingsBasketball);
            FormClosing += new FormClosingEventHandler(SettingsBasketball_FormClosing);
            KeyDown += new KeyEventHandler(SettingsBasketball_KeyDown);
            MaxTimeouts.EndInit();
            TimeoutTime.EndInit();
            PeriodTime.EndInit();
            ShotTimeLong.EndInit();
            ShotTimeShort.EndInit();
            MaxPlayerFouls.EndInit();
            MaxTeamfouls.EndInit();
            BreakTime.EndInit();
            Period.EndInit();
            MaxPlayer.EndInit();
            nudMaxPeriods.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public SettingsBasketball(
          string Servername,
          string Databasename,
          string UserID,
          string Password,
          int GameKindID)
        {
            InitializeComponent();
            Settings.Default.Reload();
            _servername = Servername;
            _databasename = Databasename;
            _user_id = UserID;
            _password = Password;
            _gamekind_id = GameKindID;
            _dbfunc = new DataBaseFunctions(_servername, _databasename, _user_id, _password);
            _dbfunc.ConnectDatabase();
            _act_period = 1;
            _settings_functions.showParameters((Form)this, _dbfunc, _gamekind_id, _act_period);
        }

        private void SettingsBasketball_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_dbfunc.CheckIfPeriodsComplete(_gamekind_id, (int)nudMaxPeriods.Value))
            {
                int num = (int)MessageBox.Show("Missing period(s) in saved settings!");
            }
            _dbfunc.DeleteUndesiredPeriods(_gamekind_id, (int)nudMaxPeriods.Value);
            Settings.Default.Save();
            _settings_functions.saveParameters((Form)this, _dbfunc, _gamekind_id, _act_period);
            _dbfunc = (DataBaseFunctions)null;
        }

        private void SettingsBasketball_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control || !e.Alt)
                return;
            switch (e.KeyCode)
            {
                case Keys.P:
                    if (MessageBox.Show("Delete game parameter for period " + _act_period.ToString() + "?", "Caution:", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        break;
                    _dbfunc.DeleteGameParameter(_gamekind_id, _act_period);
                    Period.Value = new Decimal(1);
                    _act_period = 1;
                    break;
                case Keys.X:
                    if (MessageBox.Show("Delete game parameter completely?", "Caution:", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        break;
                    Period.Value = new Decimal(1);
                    _act_period = 1;
                    _dbfunc.DeleteGameParameter(_gamekind_id);
                    break;
            }
        }

        private void Period_MouseDown(object sender, MouseEventArgs e)
        {
            _settings_functions.saveParameters((Form)this, _dbfunc, _gamekind_id, _act_period);
            if ((int)Period.Value <= (int)nudMaxPeriods.Value)
            {
                _act_period = Convert.ToInt32(Period.Value);
                _settings_functions.showParameters((Form)this, _dbfunc, _gamekind_id, _act_period);
            }
            else
                Period.Value = nudMaxPeriods.Value;
        }

        private void Simple_ValueChanged(object sender, EventArgs e)
        {
            ((Control)sender).Tag = (object)((NumericUpDown)sender).Value;
        }

        private void Seconds_ValueChanged_1(object sender, EventArgs e)
        {
            ((Control)sender).Tag = (object)Convert.ToInt64(((NumericUpDown)sender).Value * new Decimal(1000));
        }

        private void Minutes_ValueChanged(object sender, EventArgs e)
        {
            ((Control)sender).Tag = (object)Convert.ToInt64(((NumericUpDown)sender).Value * new Decimal(60000));
        }

        private void DoResetChanged_Click(object sender, EventArgs e)
        {
            ((Control)sender).Tag = (object)(((CheckBox)sender).Checked ? 1 : 0);
        }

        private void btnEnableChangeMaxSets_Click(object sender, EventArgs e)
        {
            nudMaxPeriods.Enabled = true;
        }

        private void nudMaxPeriods_Click(object sender, EventArgs e)
        {
            nudMaxPeriods.Enabled = false;
        }
    }
}