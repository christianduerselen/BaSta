using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class EditPenalty : Form
    {
        private string _playernumber = string.Empty;
        private long _milliseconds;
        private IContainer components;
        private Label label1;
        private NumericUpDown nudMinutes;
        private NumericUpDown nudTenth;
        private NumericUpDown nudSeconds;
        private NumericUpDown nudPlayerNo;
        private Label lblPlayerNo;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem aToolStripMenuItem;
        private ToolStripMenuItem bToolStripMenuItem;
        private ToolStripMenuItem cToolStripMenuItem;
        private ToolStripMenuItem dToolStripMenuItem;
        private ToolStripMenuItem mToolStripMenuItem;

        public long Milliseconds
        {
            get
            {
                return _milliseconds;
            }
            set
            {
                _milliseconds = value;
            }
        }

        public string PlayerNumber
        {
            get
            {
                if (nudPlayerNo.Value > new Decimal(0))
                    return nudPlayerNo.Value.ToString();
                return _playernumber;
            }
            set
            {
                try
                {
                    nudPlayerNo.Value = Convert.ToDecimal(value);
                }
                catch
                {
                    nudPlayerNo.Value = new Decimal(0);
                }
            }
        }

        public EditPenalty(string PlayerNo, long PenaltyMilliseconds)
        {
            InitializeComponent();
            long num1 = PenaltyMilliseconds;
            _playernumber = PlayerNo;
            try
            {
                nudPlayerNo.Value = Convert.ToDecimal(PlayerNo);
                _playernumber = string.Empty;
                lblPlayerNo.Text = string.Empty;
            }
            catch
            {
                nudPlayerNo.Value = new Decimal(0);
                lblPlayerNo.Text = _playernumber;
            }
            nudMinutes.Value = Convert.ToDecimal(num1 / 60000L);
            long num2 = num1 % 60000L;
            nudSeconds.Value = Convert.ToDecimal(num2 / 1000L);
            nudTenth.Value = Convert.ToDecimal(num2 % 1000L / 100L);
        }

        private void EditPenalty_FormClosing(object sender, FormClosingEventArgs e)
        {
            _milliseconds = Convert.ToInt64(nudMinutes.Value * new Decimal(60000) + nudSeconds.Value * new Decimal(1000) + nudTenth.Value * new Decimal(100));
        }

        private void EditPenalty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
                return;
            Close();
        }

        private void nudPlayerNo_ValueChanged(object sender, EventArgs e)
        {
            if (nudPlayerNo.Value < new Decimal(1))
                lblPlayerNo.Text = _playernumber;
            else
                lblPlayerNo.Text = string.Empty;
        }

        private void setPenaltyToOfficial_Click(object sender, EventArgs e)
        {
            _playernumber = ((ToolStripItem)sender).Text;
            nudPlayerNo.Value = new Decimal(0);
            lblPlayerNo.Text = _playernumber;
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
            label1 = new Label();
            nudMinutes = new NumericUpDown();
            nudTenth = new NumericUpDown();
            nudSeconds = new NumericUpDown();
            nudPlayerNo = new NumericUpDown();
            lblPlayerNo = new Label();
            contextMenuStrip1 = new ContextMenuStrip(components);
            aToolStripMenuItem = new ToolStripMenuItem();
            bToolStripMenuItem = new ToolStripMenuItem();
            cToolStripMenuItem = new ToolStripMenuItem();
            dToolStripMenuItem = new ToolStripMenuItem();
            mToolStripMenuItem = new ToolStripMenuItem();
            nudMinutes.BeginInit();
            nudTenth.BeginInit();
            nudSeconds.BeginInit();
            nudPlayerNo.BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            label1.AutoSize = true;
            label1.ImeMode = ImeMode.NoControl;
            label1.Location = new Point(31, 8);
            label1.Name = "label1";
            label1.Size = new Size(267, 13);
            label1.TabIndex = 3;
            label1.Text = "Nr.                    Min.                   Sek.                 Zehntel";
            nudMinutes.Location = new Point(90, 24);
            nudMinutes.Maximum = new Decimal(new int[4]
            {
        120,
        0,
        0,
        0
            });
            nudMinutes.Name = "nudMinutes";
            nudMinutes.Size = new Size(72, 20);
            nudMinutes.TabIndex = 0;
            nudMinutes.TextAlign = HorizontalAlignment.Center;
            nudTenth.Location = new Point(246, 24);
            nudTenth.Maximum = new Decimal(new int[4]
            {
        9,
        0,
        0,
        0
            });
            nudTenth.Name = "nudTenth";
            nudTenth.Size = new Size(72, 20);
            nudTenth.TabIndex = 2;
            nudTenth.TextAlign = HorizontalAlignment.Center;
            nudSeconds.Location = new Point(168, 24);
            nudSeconds.Maximum = new Decimal(new int[4]
            {
        59,
        0,
        0,
        0
            });
            nudSeconds.Name = "nudSeconds";
            nudSeconds.Size = new Size(72, 20);
            nudSeconds.TabIndex = 1;
            nudSeconds.TextAlign = HorizontalAlignment.Center;
            nudPlayerNo.ContextMenuStrip = contextMenuStrip1;
            nudPlayerNo.Location = new Point(12, 24);
            nudPlayerNo.Maximum = new Decimal(new int[4]
            {
        99,
        0,
        0,
        0
            });
            nudPlayerNo.Name = "nudPlayerNo";
            nudPlayerNo.Size = new Size(72, 20);
            nudPlayerNo.TabIndex = 6;
            nudPlayerNo.TextAlign = HorizontalAlignment.Center;
            nudPlayerNo.ValueChanged += new EventHandler(nudPlayerNo_ValueChanged);
            lblPlayerNo.AutoSize = true;
            lblPlayerNo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            lblPlayerNo.Location = new Point(34, 47);
            lblPlayerNo.Name = "lblPlayerNo";
            lblPlayerNo.Size = new Size(0, 13);
            lblPlayerNo.TabIndex = 7;
            contextMenuStrip1.Items.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) aToolStripMenuItem,
        (ToolStripItem) bToolStripMenuItem,
        (ToolStripItem) cToolStripMenuItem,
        (ToolStripItem) dToolStripMenuItem,
        (ToolStripItem) mToolStripMenuItem
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(94, 114);
            aToolStripMenuItem.Name = "aToolStripMenuItem";
            aToolStripMenuItem.Size = new Size(152, 22);
            aToolStripMenuItem.Text = "A";
            aToolStripMenuItem.Click += new EventHandler(setPenaltyToOfficial_Click);
            bToolStripMenuItem.Name = "bToolStripMenuItem";
            bToolStripMenuItem.Size = new Size(152, 22);
            bToolStripMenuItem.Text = "B";
            bToolStripMenuItem.Click += new EventHandler(setPenaltyToOfficial_Click);
            cToolStripMenuItem.Name = "cToolStripMenuItem";
            cToolStripMenuItem.Size = new Size(152, 22);
            cToolStripMenuItem.Text = "C";
            cToolStripMenuItem.Click += new EventHandler(setPenaltyToOfficial_Click);
            dToolStripMenuItem.Name = "dToolStripMenuItem";
            dToolStripMenuItem.Size = new Size(152, 22);
            dToolStripMenuItem.Text = "D";
            dToolStripMenuItem.Click += new EventHandler(setPenaltyToOfficial_Click);
            mToolStripMenuItem.Name = "mToolStripMenuItem";
            mToolStripMenuItem.Size = new Size(152, 22);
            mToolStripMenuItem.Text = "M";
            mToolStripMenuItem.Click += new EventHandler(setPenaltyToOfficial_Click);
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(341, 65);
            Controls.Add((Control)lblPlayerNo);
            Controls.Add((Control)label1);
            Controls.Add((Control)nudPlayerNo);
            Controls.Add((Control)nudMinutes);
            Controls.Add((Control)nudTenth);
            Controls.Add((Control)nudSeconds);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = nameof(EditPenalty);
            StartPosition = FormStartPosition.CenterParent;
            Text = "Strafzeit editieren";
            FormClosing += new FormClosingEventHandler(EditPenalty_FormClosing);
            KeyDown += new KeyEventHandler(EditPenalty_KeyDown);
            nudMinutes.EndInit();
            nudTenth.EndInit();
            nudSeconds.EndInit();
            nudPlayerNo.EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
