using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class MsgLookForServer : Form
    {
        private Label label1;

        public MsgLookForServer(string ServerName)
        {
            InitializeComponent();
            label1.Text += ServerName;
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MsgLookForServer));
            label1 = new Label();
            SuspendLayout();
            label1.AccessibleDescription = (string)null;
            label1.AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)label1, "label1");
            label1.Font = (Font)null;
            label1.ForeColor = SystemColors.InfoText;
            label1.Name = "label1";
            AccessibleDescription = (string)null;
            AccessibleName = (string)null;
            componentResourceManager.ApplyResources((object)this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)null;
            ControlBox = false;
            Controls.Add((Control)label1);
            Font = (Font)null;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)null;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(MsgLookForServer);
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}