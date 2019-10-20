using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class InputSpielMinute : Form
    {
        private int _spielminute = -1;
        private NumericUpDown nudGameMinute;
        private Button btnCancel;
        private Button btnOK;

        public int Spielminute
        {
            get
            {
                return _spielminute;
            }
        }

        public InputSpielMinute(int Spielminute)
        {
            InitializeComponent();
            nudGameMinute.Value = Convert.ToDecimal(Spielminute);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _spielminute = -1;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _spielminute = Convert.ToInt32(nudGameMinute.Value);
            Close();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(InputSpielMinute));
            nudGameMinute = new NumericUpDown();
            btnCancel = new Button();
            btnOK = new Button();
            nudGameMinute.BeginInit();
            SuspendLayout();
            nudGameMinute.AccessibleDescription = (string)null;
            nudGameMinute.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)nudGameMinute, "nudGameMinute");
            nudGameMinute.Font = (Font)null;
            nudGameMinute.Maximum = new decimal(new[] { 180, 0, 0, 0 });
            nudGameMinute.Name = "nudGameMinute";
            btnCancel.AccessibleDescription = (string)null;
            btnCancel.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnCancel, "btnCancel");
            btnCancel.BackgroundImage = (Image)null;
            btnCancel.Font = (Font)null;
            btnCancel.Name = "btnCancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOK.AccessibleDescription = (string)null;
            btnOK.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)btnOK, "btnOK");
            btnOK.BackgroundImage = (Image)null;
            btnOK.Font = (Font)null;
            btnOK.Name = "btnOK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += new EventHandler(btnOK_Click);
            AccessibleDescription = (string)null;
            AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)null;
            ControlBox = false;
            Controls.Add((Control)btnOK);
            Controls.Add((Control)btnCancel);
            Controls.Add((Control)nudGameMinute);
            Font = (Font)null;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)null;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(InputSpielMinute);
            nudGameMinute.EndInit();
            ResumeLayout(false);
        }
    }
}