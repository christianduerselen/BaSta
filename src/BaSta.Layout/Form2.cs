using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Layout
{
  public class Form2 : Form
  {
    private bool _layout_selected = false;
    private string _database_layoutname = string.Empty;
    private IContainer components = (IContainer) null;
    private Button btnOK;
    private Button btnCancel;
    private ListBox listBox1;

    public bool LayoutSelected
    {
      get
      {
        return this._layout_selected;
      }
    }

    public string DatabaseLayoutName
    {
      get
      {
        if (this._layout_selected)
          return this._database_layoutname;
        return string.Empty;
      }
      set
      {
        this._database_layoutname = value;
      }
    }

    public Form2(ArrayList _layouts)
    {
      this.InitializeComponent();
      for (int index = 0; index < _layouts.Count; ++index)
        this.listBox1.Items.Add((object) _layouts[index].ToString());
      if (this.listBox1.Items.Count <= 0)
        return;
      this.listBox1.SelectedIndex = 0;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this._layout_selected = false;
      this.Close();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this._layout_selected = true;
      this.Close();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      this._database_layoutname = this.listBox1.SelectedItem.ToString();
    }

    private void listBox1_DoubleClick(object sender, EventArgs e)
    {
      this.btnOK_Click((object) this, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.listBox1 = new ListBox();
      this.SuspendLayout();
      this.btnOK.Location = new Point(144, 99);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(129, 22);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Location = new Point(9, 99);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(129, 22);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Abbruch";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.listBox1.FormattingEnabled = true;
      this.listBox1.Location = new Point(9, 11);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new Size(264, 82);
      this.listBox1.TabIndex = 2;
      this.listBox1.DoubleClick += new EventHandler(this.listBox1_DoubleClick);
      this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(285, 128);
      this.ControlBox = false;
      this.Controls.Add((Control) this.listBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (Form2);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Layou laden";
      this.ResumeLayout(false);
    }
  }
}
