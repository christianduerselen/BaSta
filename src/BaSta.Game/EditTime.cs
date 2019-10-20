using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class EditTime : Form
    {
        private NumericUpDown nudMinutes;
        private NumericUpDown nudSeconds;
        private NumericUpDown nudTenth;
        private Label label1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label2;
        private NumericUpDown nudShotSeconds;
        private NumericUpDown nudShotTenth;
        private Label label3;
        private long _game_milliseconds;
        private long _shot_milliseconds;

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EditTime));
            nudMinutes = new NumericUpDown();
            nudSeconds = new NumericUpDown();
            nudTenth = new NumericUpDown();
            label1 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            label3 = new Label();
            nudShotTenth = new NumericUpDown();
            label2 = new Label();
            nudShotSeconds = new NumericUpDown();
            nudMinutes.BeginInit();
            nudSeconds.BeginInit();
            nudTenth.BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            nudShotTenth.BeginInit();
            nudShotSeconds.BeginInit();
            SuspendLayout();
            componentResourceManager.ApplyResources((object)nudMinutes, "nudMinutes");
            nudMinutes.Maximum = new Decimal(new int[4]
            {
        120,
        0,
        0,
        0
            });
            nudMinutes.Name = "nudMinutes";
            componentResourceManager.ApplyResources((object)nudSeconds, "nudSeconds");
            nudSeconds.Maximum = new Decimal(new int[4]
            {
        59,
        0,
        0,
        0
            });
            nudSeconds.Name = "nudSeconds";
            componentResourceManager.ApplyResources((object)nudTenth, "nudTenth");
            nudTenth.Maximum = new Decimal(new int[4]
            {
        9,
        0,
        0,
        0
            });
            nudTenth.Name = "nudTenth";
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.Name = "label1";
            componentResourceManager.ApplyResources((object)groupBox1, "groupBox1");
            groupBox1.Controls.Add((Control)label1);
            groupBox1.Controls.Add((Control)nudMinutes);
            groupBox1.Controls.Add((Control)nudTenth);
            groupBox1.Controls.Add((Control)nudSeconds);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            componentResourceManager.ApplyResources((object)groupBox2, "groupBox2");
            groupBox2.Controls.Add((Control)label3);
            groupBox2.Controls.Add((Control)nudShotTenth);
            groupBox2.Controls.Add((Control)label2);
            groupBox2.Controls.Add((Control)nudShotSeconds);
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            componentResourceManager.ApplyResources((object)label3, "label3");
            label3.Name = "label3";
            componentResourceManager.ApplyResources((object)nudShotTenth, "nudShotTenth");
            nudShotTenth.Maximum = new Decimal(new int[4]
            {
        9,
        0,
        0,
        0
            });
            nudShotTenth.Name = "nudShotTenth";
            componentResourceManager.ApplyResources((object)label2, "label2");
            label2.Name = "label2";
            componentResourceManager.ApplyResources((object)nudShotSeconds, "nudShotSeconds");
            nudShotSeconds.Maximum = new Decimal(new int[4]
            {
        60,
        0,
        0,
        0
            });
            nudShotSeconds.Name = "nudShotSeconds";
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add((Control)groupBox2);
            Controls.Add((Control)groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(EditTime);
            FormClosing += new FormClosingEventHandler(EditTime_FormClosing);
            KeyDown += new KeyEventHandler(EditTime_KeyDown);
            nudMinutes.EndInit();
            nudSeconds.EndInit();
            nudTenth.EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            nudShotTenth.EndInit();
            nudShotSeconds.EndInit();
            ResumeLayout(false);
        }

        public long GameMilliSeconds
        {
            get
            {
                return _game_milliseconds;
            }
        }

        public long ShotMilliseconds
        {
            get
            {
                return _shot_milliseconds;
            }
        }

        public EditTime(long GameMilliseconds, long ShotMilliseconds)
        {
            InitializeComponent();
            _game_milliseconds = GameMilliseconds;
            _shot_milliseconds = ShotMilliseconds;
            nudMinutes.Value = Convert.ToDecimal(_game_milliseconds / 60000L);
            _game_milliseconds %= 60000L;
            nudSeconds.Value = Convert.ToDecimal(_game_milliseconds / 1000L);
            _game_milliseconds %= 1000L;
            nudTenth.Value = Convert.ToDecimal(_game_milliseconds / 100L);
            nudShotSeconds.Value = Convert.ToDecimal(_shot_milliseconds / 1000L);
            nudShotTenth.Value = Convert.ToDecimal(_shot_milliseconds % 1000L / 100L);
        }

        private void EditTime_FormClosing(object sender, FormClosingEventArgs e)
        {
            _game_milliseconds = Convert.ToInt64(nudMinutes.Value * new Decimal(60000) + nudSeconds.Value * new Decimal(1000) + nudTenth.Value * new Decimal(100));
            _shot_milliseconds = Convert.ToInt64(nudShotSeconds.Value * new Decimal(1000) + nudShotTenth.Value * new Decimal(100));
        }

        private void EditTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            Close();
        }
    }
}
