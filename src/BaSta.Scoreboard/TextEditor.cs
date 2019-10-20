using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BaSta.Scoreboard.Properties;

namespace BaSta.Scoreboard
{
  public class TextEditor : Form
  {
    private string _background_image_file_name = Settings.Default.TextBackgroundFileName;
    private string _old_text = string.Empty;
    private Color _old_forecolor = Color.White;
    private Color _old_backcolor = Color.Black;
    private IContainer components;
    private TextBox textBox1;
    private Button btnLoad;
    private Button btnSave;
    private Button btnSend;
    private Button btnSelectBackColor;
    private Button btnSelectForeColor;
    private Button btnSelectFont;
    private FontDialog fontDialog1;
    private ColorDialog colorDialogBackColor;
    private ColorDialog colorDialogForeColor;
    private SaveFileDialog saveFileDialog1;
    private OpenFileDialog openFileDialog1;
    private Button btnClear;
    private RadioButton rbAlignLeft;
    private RadioButton rbAlignCenter;
    private RadioButton rbAlignRight;
    private Button btnPreview;
    private Button btnBackgroundImage;
    private OpenFileDialog openFileDialog2;
    private LED_Board _display;
    private Image _background_image;
    private bool _show_in_window;
    private int _effect_index;
    private Font _old_font;
    private Image _old_background_image;
    private HorizontalAlignment _old_alignment;

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TextEditor));
      btnLoad = new Button();
      btnSave = new Button();
      btnSend = new Button();
      btnSelectBackColor = new Button();
      btnSelectForeColor = new Button();
      btnSelectFont = new Button();
      fontDialog1 = new FontDialog();
      colorDialogBackColor = new ColorDialog();
      colorDialogForeColor = new ColorDialog();
      saveFileDialog1 = new SaveFileDialog();
      openFileDialog1 = new OpenFileDialog();
      btnClear = new Button();
      rbAlignRight = new RadioButton();
      rbAlignCenter = new RadioButton();
      rbAlignLeft = new RadioButton();
      btnPreview = new Button();
      btnBackgroundImage = new Button();
      textBox1 = new TextBox();
      openFileDialog2 = new OpenFileDialog();
      SuspendLayout();
      btnLoad.AccessibleDescription = (string) null;
      btnLoad.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnLoad, "btnLoad");
      btnLoad.BackgroundImage = (Image) null;
      btnLoad.Font = (Font) null;
      btnLoad.Name = "btnLoad";
      btnLoad.UseVisualStyleBackColor = true;
      btnLoad.Click += new EventHandler(btnLoad_Click);
      btnSave.AccessibleDescription = (string) null;
      btnSave.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnSave, "btnSave");
      btnSave.BackgroundImage = (Image) null;
      btnSave.Font = (Font) null;
      btnSave.Name = "btnSave";
      btnSave.UseVisualStyleBackColor = true;
      btnSave.Click += new EventHandler(btnSave_Click);
      btnSend.AccessibleDescription = (string) null;
      btnSend.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnSend, "btnSend");
      btnSend.BackgroundImage = (Image) null;
      btnSend.Font = (Font) null;
      btnSend.Name = "btnSend";
      btnSend.UseVisualStyleBackColor = true;
      btnSend.Click += new EventHandler(btnSend_Click);
      btnSelectBackColor.AccessibleDescription = (string) null;
      btnSelectBackColor.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnSelectBackColor, "btnSelectBackColor");
      btnSelectBackColor.BackgroundImage = (Image) null;
      btnSelectBackColor.Font = (Font) null;
      btnSelectBackColor.Name = "btnSelectBackColor";
      btnSelectBackColor.UseVisualStyleBackColor = true;
      btnSelectBackColor.Click += new EventHandler(btnSelectBackColor_Click);
      btnSelectForeColor.AccessibleDescription = (string) null;
      btnSelectForeColor.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnSelectForeColor, "btnSelectForeColor");
      btnSelectForeColor.BackgroundImage = (Image) null;
      btnSelectForeColor.Font = (Font) null;
      btnSelectForeColor.Name = "btnSelectForeColor";
      btnSelectForeColor.UseVisualStyleBackColor = true;
      btnSelectForeColor.Click += new EventHandler(btnSelectForeColor_Click);
      btnSelectFont.AccessibleDescription = (string) null;
      btnSelectFont.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnSelectFont, "btnSelectFont");
      btnSelectFont.BackgroundImage = (Image) null;
      btnSelectFont.Font = (Font) null;
      btnSelectFont.Name = "btnSelectFont";
      btnSelectFont.UseVisualStyleBackColor = true;
      btnSelectFont.Click += new EventHandler(btnSelectFont_Click);
      saveFileDialog1.DefaultExt = "taf";
      componentResourceManager.ApplyResources((object) saveFileDialog1, "saveFileDialog1");
      saveFileDialog1.RestoreDirectory = true;
      saveFileDialog1.FileOk += new CancelEventHandler(saveFileDialog1_FileOk);
      componentResourceManager.ApplyResources((object) openFileDialog1, "openFileDialog1");
      openFileDialog1.RestoreDirectory = true;
      openFileDialog1.FileOk += new CancelEventHandler(openFileDialog1_FileOk);
      btnClear.AccessibleDescription = (string) null;
      btnClear.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnClear, "btnClear");
      btnClear.BackgroundImage = (Image) null;
      btnClear.Font = (Font) null;
      btnClear.Name = "btnClear";
      btnClear.UseVisualStyleBackColor = true;
      btnClear.Click += new EventHandler(btnClear_Click);
      rbAlignRight.AccessibleDescription = (string) null;
      rbAlignRight.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) rbAlignRight, "rbAlignRight");
      rbAlignRight.BackgroundImage = (Image) null;
      rbAlignRight.Font = (Font) null;
      rbAlignRight.Name = "rbAlignRight";
      rbAlignRight.UseVisualStyleBackColor = true;
      rbAlignRight.CheckedChanged += new EventHandler(rbAlignRight_CheckedChanged);
      rbAlignCenter.AccessibleDescription = (string) null;
      rbAlignCenter.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) rbAlignCenter, "rbAlignCenter");
      rbAlignCenter.BackgroundImage = (Image) null;
      rbAlignCenter.Font = (Font) null;
      rbAlignCenter.Name = "rbAlignCenter";
      rbAlignCenter.UseVisualStyleBackColor = true;
      rbAlignCenter.CheckedChanged += new EventHandler(rbAlignRight_CheckedChanged);
      rbAlignLeft.AccessibleDescription = (string) null;
      rbAlignLeft.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) rbAlignLeft, "rbAlignLeft");
      rbAlignLeft.BackgroundImage = (Image) null;
      rbAlignLeft.Checked = true;
      rbAlignLeft.Font = (Font) null;
      rbAlignLeft.Name = "rbAlignLeft";
      rbAlignLeft.TabStop = true;
      rbAlignLeft.UseVisualStyleBackColor = true;
      rbAlignLeft.CheckedChanged += new EventHandler(rbAlignRight_CheckedChanged);
      btnPreview.AccessibleDescription = (string) null;
      btnPreview.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnPreview, "btnPreview");
      btnPreview.BackgroundImage = (Image) null;
      btnPreview.Font = (Font) null;
      btnPreview.Name = "btnPreview";
      btnPreview.UseVisualStyleBackColor = true;
      btnPreview.Click += new EventHandler(btnPreview_Click);
      btnBackgroundImage.AccessibleDescription = (string) null;
      btnBackgroundImage.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnBackgroundImage, "btnBackgroundImage");
      btnBackgroundImage.BackgroundImage = (Image) null;
      btnBackgroundImage.Font = (Font) null;
      btnBackgroundImage.Name = "btnBackgroundImage";
      btnBackgroundImage.UseVisualStyleBackColor = true;
      btnBackgroundImage.Click += new EventHandler(btnBackgroundImage_Click);
      textBox1.AccessibleDescription = (string) null;
      textBox1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) textBox1, "textBox1");
      textBox1.BackColor = Settings.Default.LastBackColor;
      textBox1.BackgroundImage = (Image) null;
      textBox1.DataBindings.Add(new Binding("Text", (object) Settings.Default, "LastText", true, DataSourceUpdateMode.OnPropertyChanged));
      textBox1.DataBindings.Add(new Binding("BackColor", (object) Settings.Default, "LastBackColor", true, DataSourceUpdateMode.OnPropertyChanged));
      textBox1.DataBindings.Add(new Binding("ForeColor", (object) Settings.Default, "LastForeColor", true, DataSourceUpdateMode.OnPropertyChanged));
      textBox1.DataBindings.Add(new Binding("Font", (object) Settings.Default, "LastFont", true, DataSourceUpdateMode.OnPropertyChanged));
      textBox1.Font = Settings.Default.LastFont;
      textBox1.ForeColor = Settings.Default.LastForeColor;
      textBox1.Name = "textBox1";
      textBox1.Text = Settings.Default.LastText;
      componentResourceManager.ApplyResources((object) openFileDialog2, "openFileDialog2");
      openFileDialog2.RestoreDirectory = true;
      openFileDialog2.FileOk += new CancelEventHandler(openFileDialog2_FileOk);
      AccessibleDescription = (string) null;
      AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = SystemColors.Control;
      BackgroundImage = (Image) null;
      Controls.Add((Control) btnBackgroundImage);
      Controls.Add((Control) btnPreview);
      Controls.Add((Control) rbAlignRight);
      Controls.Add((Control) rbAlignCenter);
      Controls.Add((Control) rbAlignLeft);
      Controls.Add((Control) btnClear);
      Controls.Add((Control) btnLoad);
      Controls.Add((Control) btnSave);
      Controls.Add((Control) btnSend);
      Controls.Add((Control) btnSelectBackColor);
      Controls.Add((Control) btnSelectForeColor);
      Controls.Add((Control) btnSelectFont);
      Controls.Add((Control) textBox1);
      Font = (Font) null;
      FormBorderStyle = FormBorderStyle.FixedSingle;
      Icon = (Icon) null;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = nameof (TextEditor);
      Load += new EventHandler(TextEditor_Load);
      FormClosing += new FormClosingEventHandler(TextEditor_FormClosing);
      ResumeLayout(false);
      PerformLayout();
    }

    public TextEditor(LED_Board Display, bool ShowInWindow, int EffectIndex)
    {
      InitializeComponent();
      _show_in_window = ShowInWindow;
      _effect_index = EffectIndex;
      textBox1.Size = Display.Size;
      Width = Display.Width + 185;
      Height = Display.Height + 110;
      _display = Display;
      if (File.Exists(_background_image_file_name))
      {
        try
        {
          StreamReader streamReader = new StreamReader(_background_image_file_name);
          Bitmap bitmap = new Bitmap((Image) new Bitmap(streamReader.BaseStream), _display.Size);
          streamReader.Close();
          _background_image = (Image) bitmap;
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
      _reset_changeflags();
    }

    private void _reset_changeflags()
    {
      _old_text = textBox1.Text;
      _old_background_image = _background_image;
      _old_font = textBox1.Font;
      _old_forecolor = textBox1.ForeColor;
      _old_backcolor = textBox1.BackColor;
      _old_alignment = textBox1.TextAlign;
    }

    private bool _check_if_changed()
    {
      if (!(_old_backcolor != textBox1.BackColor) && !(_old_forecolor != textBox1.ForeColor) && (_old_font == textBox1.Font && !(_old_text != textBox1.Text)) && (_old_background_image == _background_image && _old_alignment == textBox1.TextAlign))
        return false;
      DialogResult dialogResult = MessageBox.Show("Möchten Sie das Tafelbild speichern?", "Das Tafelbild wurde verändert!", MessageBoxButtons.YesNoCancel);
      if (dialogResult == DialogResult.Yes)
        btnSave_Click((object) this, (EventArgs) null);
      return dialogResult == DialogResult.Cancel;
    }

    private void _set_colors()
    {
      textBox1.BackColor = colorDialogBackColor.Color;
      textBox1.ForeColor = colorDialogForeColor.Color;
    }

    private void btnSelectFont_Click(object sender, EventArgs e)
    {
      fontDialog1.Font = textBox1.Font;
      int num = (int) fontDialog1.ShowDialog();
      textBox1.Font = fontDialog1.Font;
      textBox1.Focus();
    }

    private void btnSelectForeColor_Click(object sender, EventArgs e)
    {
      colorDialogForeColor.Color = textBox1.ForeColor;
      int num = (int) colorDialogForeColor.ShowDialog((IWin32Window) this);
      _set_colors();
    }

    private void btnSelectBackColor_Click(object sender, EventArgs e)
    {
      colorDialogBackColor.Color = textBox1.BackColor;
      int num = (int) colorDialogBackColor.ShowDialog((IWin32Window) this);
      _set_colors();
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      if (_check_if_changed())
        return;
      int num = (int) openFileDialog1.ShowDialog();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      int num = (int) saveFileDialog1.ShowDialog();
    }

    private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      StreamWriter streamWriter = new StreamWriter(saveFileDialog1.FileName, false);
      Font font = textBox1.Font;
      int num = 0;
      if (rbAlignCenter.Checked)
        num = 1;
      if (rbAlignRight.Checked)
        num = 2;
      streamWriter.WriteLine(font.Name + "|" + font.Size.ToString() + "|" + font.Style.ToString() + "|" + textBox1.ForeColor.ToArgb().ToString() + "|" + textBox1.BackColor.ToArgb().ToString() + "|" + num.ToString() + "|" + _background_image_file_name);
      streamWriter.WriteLine(textBox1.Text);
      streamWriter.Close();
      _reset_changeflags();
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      StreamReader streamReader1 = new StreamReader(openFileDialog1.FileName);
      string str = streamReader1.ReadLine();
      if (openFileDialog1.FileName.EndsWith(".taf"))
      {
        string[] strArray = str.Split('|');
        FontStyle style = FontStyle.Regular;
        if (strArray[2].Contains("Bold"))
          style = FontStyle.Bold;
        if (strArray[2].Contains("Italic"))
          style |= FontStyle.Italic;
        if (strArray[2].Contains("Underline"))
          style |= FontStyle.Underline;
        if (strArray[2].Contains("Strikeout"))
          style |= FontStyle.Strikeout;
        textBox1.Font = new Font(strArray[0], Convert.ToSingle(strArray[1]), style);
        textBox1.ForeColor = Color.FromArgb(Convert.ToInt32(strArray[3]));
        textBox1.BackColor = Color.FromArgb(Convert.ToInt32(strArray[4]));
        switch (Convert.ToInt32(strArray[5]))
        {
          case 0:
            rbAlignLeft.Checked = true;
            textBox1.TextAlign = HorizontalAlignment.Left;
            break;
          case 1:
            rbAlignCenter.Checked = true;
            textBox1.TextAlign = HorizontalAlignment.Center;
            break;
          case 2:
            rbAlignRight.Checked = true;
            textBox1.TextAlign = HorizontalAlignment.Right;
            break;
          default:
            rbAlignLeft.Checked = true;
            textBox1.TextAlign = HorizontalAlignment.Left;
            break;
        }
        if (strArray.Length > 6)
        {
          _background_image_file_name = strArray[6];
          if (File.Exists(_background_image_file_name))
          {
            try
            {
              StreamReader streamReader2 = new StreamReader(_background_image_file_name);
              Bitmap bitmap = new Bitmap((Image) new Bitmap(streamReader2.BaseStream), _display.Size);
              streamReader2.Close();
              _background_image = (Image) bitmap;
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex.Message);
            }
          }
        }
        str = streamReader1.ReadLine();
      }
      textBox1.Clear();
      for (; str != null; str = streamReader1.ReadLine())
      {
        TextBox textBox1 = this.textBox1;
        textBox1.Text = textBox1.Text + str + (object) '\r' + (object) '\n';
      }
      streamReader1.Close();
    }

    private void TextEditor_Load(object sender, EventArgs e)
    {
      Settings.Default.Reload();
      colorDialogForeColor.Color = textBox1.ForeColor;
      colorDialogBackColor.Color = textBox1.BackColor;
      if (Settings.Default.LastTextAlignement == HorizontalAlignment.Left)
        rbAlignLeft.Checked = true;
      if (Settings.Default.LastTextAlignement == HorizontalAlignment.Right)
        rbAlignRight.Checked = true;
      if (Settings.Default.LastTextAlignement == HorizontalAlignment.Center)
        rbAlignCenter.Checked = true;
      textBox1.Select(textBox1.Text.Length, 0);
      if (!File.Exists(Settings.Default.LastTextFile))
        return;
      openFileDialog1.FileName = Settings.Default.LastTextFile;
      saveFileDialog1.FileName = Settings.Default.LastTextFile;
    }

    private void TextEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
      Settings.Default.LastTextFile = openFileDialog1.FileName;
      if (rbAlignLeft.Checked)
        Settings.Default.LastTextAlignement = HorizontalAlignment.Left;
      if (rbAlignRight.Checked)
        Settings.Default.LastTextAlignement = HorizontalAlignment.Right;
      if (rbAlignCenter.Checked)
        Settings.Default.LastTextAlignement = HorizontalAlignment.Center;
      Settings.Default.TextBackgroundFileName = _background_image_file_name;
      Settings.Default.Save();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      _display.ShowVideo = false;
      _display.ShowTextAsScreenshot(textBox1, new Point(Left + textBox1.Left + 9, Top + textBox1.Top + 31), _background_image_file_name, _show_in_window, _effect_index);
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      _display.Clear();
    }

    private void rbAlignRight_CheckedChanged(object sender, EventArgs e)
    {
      if (rbAlignLeft.Checked)
        textBox1.TextAlign = HorizontalAlignment.Left;
      if (rbAlignCenter.Checked)
        textBox1.TextAlign = HorizontalAlignment.Center;
      if (!rbAlignRight.Checked)
        return;
      textBox1.TextAlign = HorizontalAlignment.Right;
    }

    private void btnBackgroundImage_Click(object sender, EventArgs e)
    {
      openFileDialog2.FileName = string.Empty;
      int num = (int) openFileDialog2.ShowDialog();
      if (!(openFileDialog2.FileName.Trim() == string.Empty))
        return;
      _background_image_file_name = string.Empty;
      _background_image = (Image) null;
    }

    private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
    {
      if (e.Cancel)
      {
        _background_image_file_name = string.Empty;
        _background_image = (Image) null;
      }
      else
      {
        _background_image_file_name = openFileDialog2.FileName;
        try
        {
          StreamReader streamReader = new StreamReader(_background_image_file_name);
          Bitmap bitmap = new Bitmap((Image) new Bitmap(streamReader.BaseStream), _display.Size);
          streamReader.Close();
          _background_image = (Image) bitmap;
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }

    private void btnPreview_Click(object sender, EventArgs e)
    {
      int num = (int) new TextPreview(_display, textBox1, _background_image, _show_in_window, _effect_index).ShowDialog();
    }
  }
}
