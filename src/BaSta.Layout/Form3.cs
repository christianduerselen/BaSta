using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BaSta.Layout
{
  public class Form3 : Form
  {
    private IContainer components = (IContainer) null;
    private string _checkfile_name = string.Empty;
    private ListBox listBox1;
    private Button btnCancel;
    private Button btnOK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listBox1 = new ListBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.SuspendLayout();
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new Point(12, 12);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new Size(264, 82);
      this.listBox1.TabIndex = 5;
      this.listBox1.DoubleClick += new EventHandler(this.listBox1_DoubleClick);
      this.btnCancel.Location = new Point(12, 100);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(129, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Abbruch";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Location = new Point(147, 100);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(129, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(292, 128);
      this.ControlBox = false;
      this.Controls.Add((Control) this.listBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Name = nameof (Form3);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Datei zur Prüfung auswählen:";
      this.ResumeLayout(false);
    }

    public string CheckFileName
    {
      get
      {
        return this._checkfile_name;
      }
    }

    public Form3()
    {
      this.InitializeComponent();
      foreach (FileInfo file in new DirectoryInfo(Application.StartupPath).GetFiles("*.fld"))
        this.listBox1.Items.Add((object) file.Name.Substring(0, file.Name.Length - 4));
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this._checkfile_name = string.Empty;
      this.Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this._checkfile_name = this.listBox1.SelectedIndex <= -1 ? string.Empty : this.listBox1.Text + ".fld";
      this.Close();
    }

    private void listBox1_DoubleClick(object sender, EventArgs e)
    {
      this.btnOK_Click((object) this, (EventArgs) null);
    }
  }
}
