using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class InputDisplayLength : Form
  {
    private IContainer components;
    private NumericUpDown numericUpDown1;
    private Label label1;
    private ComboBox cmbEffect;
    private Label label2;
    private Label label3;
    private CheckBox cbPlayAcusticSignal;
    private Label lblExtraSoundFileName;
    private OpenFileDialog openFileDialog1;
    private Button btnExtraSound;

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InputDisplayLength));
      numericUpDown1 = new NumericUpDown();
      label1 = new Label();
      cmbEffect = new ComboBox();
      label2 = new Label();
      label3 = new Label();
      cbPlayAcusticSignal = new CheckBox();
      lblExtraSoundFileName = new Label();
      openFileDialog1 = new OpenFileDialog();
      btnExtraSound = new Button();
      numericUpDown1.BeginInit();
      SuspendLayout();
      numericUpDown1.AccessibleDescription = (string) null;
      numericUpDown1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) numericUpDown1, "numericUpDown1");
      numericUpDown1.Font = (Font) null;
      numericUpDown1.Maximum = new Decimal(new int[4]
      {
        3600,
        0,
        0,
        0
      });
      numericUpDown1.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      numericUpDown1.Name = "numericUpDown1";
      numericUpDown1.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      label1.AccessibleDescription = (string) null;
      label1.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) label1, "label1");
      label1.Font = (Font) null;
      label1.Name = "label1";
      cmbEffect.AccessibleDescription = (string) null;
      cmbEffect.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) cmbEffect, "cmbEffect");
      cmbEffect.BackgroundImage = (Image) null;
      cmbEffect.DropDownStyle = ComboBoxStyle.DropDownList;
      cmbEffect.Font = (Font) null;
      cmbEffect.FormattingEnabled = true;
      cmbEffect.Items.AddRange(new object[29]
      {
        (object) componentResourceManager.GetString("cmbEffect.Items"),
        (object) componentResourceManager.GetString("cmbEffect.Items1"),
        (object) componentResourceManager.GetString("cmbEffect.Items2"),
        (object) componentResourceManager.GetString("cmbEffect.Items3"),
        (object) componentResourceManager.GetString("cmbEffect.Items4"),
        (object) componentResourceManager.GetString("cmbEffect.Items5"),
        (object) componentResourceManager.GetString("cmbEffect.Items6"),
        (object) componentResourceManager.GetString("cmbEffect.Items7"),
        (object) componentResourceManager.GetString("cmbEffect.Items8"),
        (object) componentResourceManager.GetString("cmbEffect.Items9"),
        (object) componentResourceManager.GetString("cmbEffect.Items10"),
        (object) componentResourceManager.GetString("cmbEffect.Items11"),
        (object) componentResourceManager.GetString("cmbEffect.Items12"),
        (object) componentResourceManager.GetString("cmbEffect.Items13"),
        (object) componentResourceManager.GetString("cmbEffect.Items14"),
        (object) componentResourceManager.GetString("cmbEffect.Items15"),
        (object) componentResourceManager.GetString("cmbEffect.Items16"),
        (object) componentResourceManager.GetString("cmbEffect.Items17"),
        (object) componentResourceManager.GetString("cmbEffect.Items18"),
        (object) componentResourceManager.GetString("cmbEffect.Items19"),
        (object) componentResourceManager.GetString("cmbEffect.Items20"),
        (object) componentResourceManager.GetString("cmbEffect.Items21"),
        (object) componentResourceManager.GetString("cmbEffect.Items22"),
        (object) componentResourceManager.GetString("cmbEffect.Items23"),
        (object) componentResourceManager.GetString("cmbEffect.Items24"),
        (object) componentResourceManager.GetString("cmbEffect.Items25"),
        (object) componentResourceManager.GetString("cmbEffect.Items26"),
        (object) componentResourceManager.GetString("cmbEffect.Items27"),
        (object) componentResourceManager.GetString("cmbEffect.Items28")
      });
      cmbEffect.Name = "cmbEffect";
      label2.AccessibleDescription = (string) null;
      label2.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) label2, "label2");
      label2.Font = (Font) null;
      label2.Name = "label2";
      label3.AccessibleDescription = (string) null;
      label3.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) label3, "label3");
      label3.Font = (Font) null;
      label3.Name = "label3";
      cbPlayAcusticSignal.AccessibleDescription = (string) null;
      cbPlayAcusticSignal.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) cbPlayAcusticSignal, "cbPlayAcusticSignal");
      cbPlayAcusticSignal.BackgroundImage = (Image) null;
      cbPlayAcusticSignal.Font = (Font) null;
      cbPlayAcusticSignal.Name = "cbPlayAcusticSignal";
      cbPlayAcusticSignal.UseVisualStyleBackColor = true;
      lblExtraSoundFileName.AccessibleDescription = (string) null;
      lblExtraSoundFileName.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) lblExtraSoundFileName, "lblExtraSoundFileName");
      lblExtraSoundFileName.Font = (Font) null;
      lblExtraSoundFileName.Name = "lblExtraSoundFileName";
      componentResourceManager.ApplyResources((object) openFileDialog1, "openFileDialog1");
      openFileDialog1.FileOk += new CancelEventHandler(openFileDialog1_FileOk);
      btnExtraSound.AccessibleDescription = (string) null;
      btnExtraSound.AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) btnExtraSound, "btnExtraSound");
      btnExtraSound.BackgroundImage = (Image) null;
      btnExtraSound.Font = (Font) null;
      btnExtraSound.Name = "btnExtraSound";
      btnExtraSound.UseVisualStyleBackColor = true;
      btnExtraSound.Click += new EventHandler(btnExtraSound_Click);
      AccessibleDescription = (string) null;
      AccessibleName = (string) null;
      componentResourceManager.ApplyResources((object) this, "$this");
      AutoScaleMode = AutoScaleMode.Font;
      BackgroundImage = (Image) null;
      Controls.Add((Control) btnExtraSound);
      Controls.Add((Control) lblExtraSoundFileName);
      Controls.Add((Control) cbPlayAcusticSignal);
      Controls.Add((Control) label3);
      Controls.Add((Control) label2);
      Controls.Add((Control) cmbEffect);
      Controls.Add((Control) label1);
      Controls.Add((Control) numericUpDown1);
      Font = (Font) null;
      FormBorderStyle = FormBorderStyle.FixedSingle;
      Icon = (Icon) null;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = nameof (InputDisplayLength);
      numericUpDown1.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }

    public int DisplayLength
    {
      get
      {
        return Convert.ToInt32(numericUpDown1.Value);
      }
    }

    public int EffectIndex
    {
      get
      {
        return cmbEffect.SelectedIndex;
      }
    }

    public bool PlayAcusticSignal
    {
      get
      {
        return cbPlayAcusticSignal.Checked;
      }
    }

    public string ExtraSoundFileName
    {
      get
      {
        return lblExtraSoundFileName.Text;
      }
    }

    public InputDisplayLength(string PartName, int DefaultEffectIndex, int DefaultLength)
    {
      InitializeComponent();
      cmbEffect.SelectedIndex = DefaultEffectIndex;
      numericUpDown1.Value = (Decimal) DefaultLength;
      if (!(PartName == "Score"))
        return;
      Size = new Size(275, 88);
    }

    private void btnExtraSound_Click(object sender, EventArgs e)
    {
      int num = (int) openFileDialog1.ShowDialog();
      if (!(openFileDialog1.FileName.Trim() == string.Empty))
        return;
      lblExtraSoundFileName.Text = string.Empty;
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      File.Copy(openFileDialog1.FileName, Application.StartupPath + "\\" + openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1), true);
      lblExtraSoundFileName.Text = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1);
    }
  }
}
