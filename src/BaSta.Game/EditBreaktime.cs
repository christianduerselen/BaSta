using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class EditBreaktime : Form
    {
        private GroupBox groupBox1;
        private Label label1;
        private NumericUpDown nudMinutes;
        private NumericUpDown nudTenth;
        private NumericUpDown nudSeconds;
        private long _break_milliseconds;
        
        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EditBreaktime));
            groupBox1 = new GroupBox();
            label1 = new Label();
            nudMinutes = new NumericUpDown();
            nudTenth = new NumericUpDown();
            nudSeconds = new NumericUpDown();
            groupBox1.SuspendLayout();
            nudMinutes.BeginInit();
            nudTenth.BeginInit();
            nudSeconds.BeginInit();
            SuspendLayout();
            groupBox1.AccessibleDescription = (string)null;
            groupBox1.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)groupBox1, "groupBox1");
            groupBox1.BackgroundImage = (Image)null;
            groupBox1.Controls.Add((Control)label1);
            groupBox1.Controls.Add((Control)nudMinutes);
            groupBox1.Controls.Add((Control)nudTenth);
            groupBox1.Controls.Add((Control)nudSeconds);
            groupBox1.Font = (Font)null;
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            label1.AccessibleDescription = (string)null;
            label1.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.Font = (Font)null;
            label1.Name = "label1";
            nudMinutes.AccessibleDescription = (string)null;
            nudMinutes.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudMinutes, "nudMinutes");
            nudMinutes.Font = (Font)null;
            nudMinutes.Maximum = new Decimal(new int[4]
            {
        120,
        0,
        0,
        0
            });
            nudMinutes.Name = "nudMinutes";
            nudTenth.AccessibleDescription = (string)null;
            nudTenth.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudTenth, "nudTenth");
            nudTenth.Font = (Font)null;
            nudTenth.Maximum = new Decimal(new int[4]
            {
        9,
        0,
        0,
        0
            });
            nudTenth.Name = "nudTenth";
            nudSeconds.AccessibleDescription = (string)null;
            nudSeconds.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudSeconds, "nudSeconds");
            nudSeconds.Font = (Font)null;
            nudSeconds.Maximum = new Decimal(new int[4]
            {
        59,
        0,
        0,
        0
            });
            nudSeconds.Name = "nudSeconds";
            AccessibleDescription = (string)null;
            AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)null;
            Controls.Add((Control)groupBox1);
            Font = (Font)null;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)null;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(EditBreaktime);
            FormClosing += new FormClosingEventHandler(EditBreaktime_FormClosing);
            KeyDown += new KeyEventHandler(EditBreaktime_KeyDown);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            nudMinutes.EndInit();
            nudTenth.EndInit();
            nudSeconds.EndInit();
            ResumeLayout(false);
        }

        public long BreakMilliSeconds
        {
            get
            {
                return _break_milliseconds;
            }
        }

        public EditBreaktime(long BreakMilliseconds)
        {
            InitializeComponent();
            _break_milliseconds = BreakMilliseconds;
            nudMinutes.Value = Convert.ToDecimal(_break_milliseconds / 60000L);
            _break_milliseconds %= 60000L;
            nudSeconds.Value = Convert.ToDecimal(_break_milliseconds / 1000L);
            _break_milliseconds %= 1000L;
            nudTenth.Value = Convert.ToDecimal(_break_milliseconds / 100L);
        }

        private void EditBreaktime_FormClosing(object sender, FormClosingEventArgs e)
        {
            _break_milliseconds = Convert.ToInt64(nudMinutes.Value * new Decimal(60000) + nudSeconds.Value * new Decimal(1000) + nudTenth.Value * new Decimal(100));
        }

        private void EditBreaktime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            Close();
        }
    }
}
