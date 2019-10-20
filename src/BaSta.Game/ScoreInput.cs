using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class ScoreInput : Form
    {
        private NumericUpDown nudScore;

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ScoreInput));
            nudScore = new NumericUpDown();
            nudScore.BeginInit();
            SuspendLayout();
            nudScore.AccessibleDescription = (string)null;
            nudScore.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudScore, "nudScore");
            nudScore.Font = (Font)null;
            nudScore.Maximum = new Decimal(new int[4]
            {
        999,
        0,
        0,
        0
            });
            nudScore.Name = "nudScore";
            AccessibleDescription = (string)null;
            AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)null;
            Controls.Add((Control)nudScore);
            Font = (Font)null;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)null;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(ScoreInput);
            KeyDown += new KeyEventHandler(ScoreInput_KeyDown);
            nudScore.EndInit();
            ResumeLayout(false);
        }

        public int Score => Convert.ToInt32(nudScore.Value);

        public ScoreInput(int Score)
        {
            InitializeComponent();
            Location = Control.MousePosition;
            nudScore.Value = (Decimal)Score;
        }

        private void ScoreInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            Close();
        }
    }
}