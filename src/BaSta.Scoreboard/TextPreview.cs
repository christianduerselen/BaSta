using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using BaSta.Scoreboard.Properties;

namespace BaSta.Scoreboard
{
  public class TextPreview : Form
  {
    private string _file_name = string.Empty;
    private IContainer components;
    private Label label1;
    private Button btnSend;
    private Button btnSaveAsImage;
    private SaveFileDialog saveFileDialog1;
    private Label lblFileName;
    private LED_Board _display;
    private TextBox _source;
    private bool _show_in_window;
    private int _effect_index;

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TextPreview));
      label1 = new Label();
      btnSend = new Button();
      btnSaveAsImage = new Button();
      saveFileDialog1 = new SaveFileDialog();
      lblFileName = new Label();
      SuspendLayout();
      label1.BorderStyle = BorderStyle.FixedSingle;
      componentResourceManager.ApplyResources((object) label1, "label1");
      label1.Name = "label1";
      componentResourceManager.ApplyResources((object) btnSend, "btnSend");
      btnSend.Name = "btnSend";
      btnSend.UseVisualStyleBackColor = true;
      btnSend.Click += new EventHandler(btnSend_Click);
      componentResourceManager.ApplyResources((object) btnSaveAsImage, "btnSaveAsImage");
      btnSaveAsImage.Name = "btnSaveAsImage";
      btnSaveAsImage.UseVisualStyleBackColor = true;
      btnSaveAsImage.Click += new EventHandler(btnSaveAsImage_Click);
      componentResourceManager.ApplyResources((object) saveFileDialog1, "saveFileDialog1");
      saveFileDialog1.RestoreDirectory = true;
      saveFileDialog1.FileOk += new CancelEventHandler(saveFileDialog1_FileOk);
      componentResourceManager.ApplyResources((object) lblFileName, "lblFileName");
      lblFileName.Name = "lblFileName";
      componentResourceManager.ApplyResources((object) this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add((Control) lblFileName);
      Controls.Add((Control) btnSaveAsImage);
      Controls.Add((Control) btnSend);
      Controls.Add((Control) label1);
      MaximizeBox = false;
      MinimizeBox = false;
      Name = nameof (TextPreview);
      FormClosing += new FormClosingEventHandler(TextPreview_FormClosing);
      ResumeLayout(false);
      PerformLayout();
    }

    public TextPreview(
      LED_Board Display,
      TextBox Source,
      Image Background,
      bool ShowInWindow,
      int EffectIndex)
    {
      InitializeComponent();
      _display = Display;
      _source = Source;
      _show_in_window = ShowInWindow;
      _effect_index = EffectIndex;
      label1.Size = _display.Size;
      Width = _display.Width + 165;
      Height = _display.Height + 110;
      label1.Image = Background;
      label1.Font = _source.Font;
      ContentAlignment contentAlignment = (ContentAlignment) 0;
      switch (Source.TextAlign)
      {
        case HorizontalAlignment.Left:
          contentAlignment = ContentAlignment.TopLeft;
          break;
        case HorizontalAlignment.Right:
          contentAlignment = ContentAlignment.TopRight;
          break;
        case HorizontalAlignment.Center:
          contentAlignment = ContentAlignment.TopCenter;
          break;
      }
      label1.TextAlign = contentAlignment;
      label1.ForeColor = _source.ForeColor;
      label1.BackColor = _source.BackColor;
      label1.Text = _source.Text;
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      _display.ShowVideo = false;
      _display.ShowTextAsScreenshot(_source, new Point(Left + label1.Left + 9, Top + label1.Top + 31), Application.StartupPath + "\\TextBackground.jpg", _show_in_window, _effect_index);
    }

    private void btnSaveAsImage_Click(object sender, EventArgs e)
    {
      if (Directory.Exists(Settings.Default.SelectedPicturePath))
        saveFileDialog1.InitialDirectory = Settings.Default.SelectedPicturePath;
      _file_name = string.Empty;
      int num = (int) saveFileDialog1.ShowDialog();
    }

    private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      _file_name = saveFileDialog1.FileName;
      lblFileName.Text = _file_name;
    }

    private void TextPreview_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!(_file_name != string.Empty))
        return;
      BringToFront();
      Point point = new Point(Left + label1.Left + 9, Top + label1.Top + 31);
      Bitmap bitmap = new Bitmap(label1.Width - 2, label1.Height - 2);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.CopyFromScreen(point.X, point.Y, 0, 0, bitmap.Size);
      graphics.Dispose();
      bitmap.Save(_file_name, ImageFormat.Jpeg);
    }
  }
}
