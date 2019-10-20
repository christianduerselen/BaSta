using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class Preview : Form
  {
    private Mci _mci_player;
    private IContainer components;
    private PictureBox pictureBox1;

    public void ShowGraphicPreviewAsync(string filename)
    {
      if (InvokeRequired)
      {
        Invoke((Delegate) new Preview.ShowGraphicPreviewAsyncDelegate(ShowGraphicPreviewAsync), (object) filename);
      }
      else
      {
        if (_mci_player != null && _mci_player.IsOpen)
        {
          _mci_player.Stop();
          _mci_player.Dispose();
          _mci_player = (Mci) null;
        }
        pictureBox1.Size = Size;
        _mci_player = new Mci();
        _mci_player.Open(filename, (Control) pictureBox1);
        try
        {
          _mci_player.SetRectangle(0, 0, pictureBox1.Width, pictureBox1.Height);
        }
        catch
        {
        }
        try
        {
          _mci_player.Volume = 0;
        }
        catch
        {
        }
        try
        {
          _mci_player.Play(false);
        }
        catch
        {
        }
      }
    }

    public void HideGraphicPreviewAsync()
    {
      if (InvokeRequired)
      {
        Invoke((Delegate) new Preview.HideGraphicPreviewAsyncDelegate(HideGraphicPreviewAsync), new object[0]);
      }
      else
      {
        if (_mci_player != null && _mci_player.IsOpen)
        {
          _mci_player.Stop();
          _mci_player.Dispose();
          _mci_player = (Mci) null;
        }
        pictureBox1.Image = (Image) null;
      }
    }

    public Preview()
    {
      InitializeComponent();
    }

    private void Preview_Load(object sender, EventArgs e)
    {
      pictureBox1.Location = new Point(0, 0);
      pictureBox1.Size = Size;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      pictureBox1 = new PictureBox();
      ((ISupportInitialize) pictureBox1).BeginInit();
      SuspendLayout();
      pictureBox1.Location = new Point(0, 0);
      pictureBox1.Name = "pictureBox1";
      pictureBox1.Size = new Size(76, 46);
      pictureBox1.TabIndex = 0;
      pictureBox1.TabStop = false;
      AutoScaleDimensions = new SizeF(6f, 13f);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = Color.Black;
      ClientSize = new Size(245, 171);
      Controls.Add((Control) pictureBox1);
      FormBorderStyle = FormBorderStyle.None;
      Name = nameof (Preview);
      Text = nameof (Preview);
      Load += new EventHandler(Preview_Load);
      ((ISupportInitialize) pictureBox1).EndInit();
      ResumeLayout(false);
    }

    public delegate void ShowGraphicPreviewAsyncDelegate(string filename);

    public delegate void HideGraphicPreviewAsyncDelegate();
  }
}
