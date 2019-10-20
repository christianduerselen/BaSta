using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class WaitForm : Form
  {
    private IContainer components;
    private Label label1;

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      label1 = new Label();
      SuspendLayout();
      label1.AutoSize = true;
      label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      label1.Location = new Point(12, 9);
      label1.Name = "label1";
      label1.Size = new Size(193, 16);
      label1.TabIndex = 1;
      label1.Text = "Looking for available servers ...";
      label1.TextAlign = ContentAlignment.MiddleCenter;
      AutoScaleDimensions = new SizeF(6f, 13f);
      AutoScaleMode = AutoScaleMode.Font;
      AutoSize = true;
      ClientSize = new Size(212, 34);
      ControlBox = false;
      Controls.Add((Control) label1);
      Name = nameof (WaitForm);
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Please wait ...";
      ResumeLayout(false);
      PerformLayout();
    }

    public WaitForm(string Text)
    {
      InitializeComponent();
      label1.Text = Text;
    }
  }
}
