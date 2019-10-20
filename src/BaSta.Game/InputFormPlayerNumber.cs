using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class InputFormPlayerNumber : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private NumericUpDown nudPlayerNumber;

        public int PlayerNumber { get; private set; } = -1;

        public InputFormPlayerNumber(int playerNumber)
        {
            InitializeComponent();
            PlayerNumber = playerNumber;
            nudPlayerNumber.Value = PlayerNumber;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            PlayerNumber = -1;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PlayerNumber = Convert.ToInt32(nudPlayerNumber.Value);
            Close();
        }

        private void InitializeComponent()
        {
            btnCancel = new Button();
            btnOK = new Button();
            nudPlayerNumber = new NumericUpDown();
            nudPlayerNumber.BeginInit();
            SuspendLayout();
            btnCancel.Location = new Point(12, 49);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(118, 32);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOK.Location = new Point(136, 49);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(118, 32);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += new EventHandler(btnOK_Click);
            nudPlayerNumber.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            nudPlayerNumber.Location = new Point(71, 12);
            nudPlayerNumber.Name = "nudPlayerNumber";
            nudPlayerNumber.Size = new Size(120, 31);
            nudPlayerNumber.TabIndex = 2;
            nudPlayerNumber.TextAlign = HorizontalAlignment.Center;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(264, 90);
            ControlBox = false;
            Controls.Add((Control)nudPlayerNumber);
            Controls.Add((Control)btnOK);
            Controls.Add((Control)btnCancel);
            Name = nameof(InputFormPlayerNumber);
            StartPosition = FormStartPosition.CenterParent;
            Text = "PlayerNumber";
            nudPlayerNumber.EndInit();
            ResumeLayout(false);
        }
    }
}