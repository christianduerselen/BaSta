using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using BaSta.Layout.Properties;

namespace BaSta.Layout
{
  public class Form1 : Form
  {
    private Color _select_color = Color.DarkRed;
    private bool _layout_changed = false;
    private Image _default_grid_img = (Image) null;
    private Image _act_grid_image = (Image) null;
    private Image _temp_grid_image = (Image) null;
    private Dictionary<string, string> _parameter = new Dictionary<string, string>();
    private Form1.SelectMode _select_mode = Form1.SelectMode.MouseSelect;
    private Label _graphic_window = (Label) null;
    private SqlConnection conn = (SqlConnection) null;
    private string _database_layoutname = string.Empty;
    private bool _ctrl_pressed = false;
    private ArrayList _selected_labels = new ArrayList();
    private string _database_layout_name = string.Empty;
    private string _checkfile_name = string.Empty;
    private bool _mouse_is_down = false;
    private Rectangle _selected_rectangle = new Rectangle();
    private bool _mouse_select = false;
    private ArrayList _copy_labels = new ArrayList();
    private IContainer components = (IContainer) null;
    private Point _mouse_base_position;
    private Point _rect_start;
    private Panel panel1;
    private Label lblSCB_Size;
    private HelpProvider helpProvider1;
    private Button btnLoadCSV;
    private Button btnSaveCSV;
    private Button btnNewLabel;
    private Label lblFieldLocation;
    private Label lblFieldSize;
    private SaveFileDialog saveFileDialog1;
    private OpenFileDialog openFileDialog1;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem farbeToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem alternativFarbeToolStripMenuItem;
    private FontDialog fontDialog1;
    private ColorDialog colorDialog1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem löschenToolStripMenuItem;
    private TextBox textBox1;
    private TextBox textBox2;
    private Label label1;
    private Label label2;
    private ToolStripMenuItem textausrichtungToolStripMenuItem;
    private ToolStripMenuItem obenToolStripMenuItem;
    private ToolStripMenuItem mitteToolStripMenuItem;
    private ToolStripMenuItem linksToolStripMenuItem;
    private ToolStripMenuItem zentriertToolStripMenuItem;
    private ToolStripMenuItem rechtsToolStripMenuItem;
    private ToolStripMenuItem untenToolStripMenuItem;
    private ToolStripMenuItem linksToolStripMenuItem1;
    private ToolStripMenuItem zentriertToolStripMenuItem1;
    private ToolStripMenuItem rechtsToolStripMenuItem1;
    private ToolStripMenuItem linksToolStripMenuItem2;
    private ToolStripMenuItem zentriertToolStripMenuItem2;
    private ToolStripMenuItem rechtsToolStripMenuItem2;
    private ToolStripMenuItem kopierenToolStripMenuItem;
    private ToolStripMenuItem einfügenToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private Button btnNewlayout;
    private Label lblLabelName;
    private Label lblLayoutname;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem insertToolStripMenuItem;
    private ListBox lbMissingFieldNames;
    private Label label3;
    private ContextMenuStrip contextMenuStrip3;
    private ToolStripMenuItem prüfdateiWählenToolStripMenuItem;
    private Label lblCursorPosition;
    private ToolStripSeparator toolStripSeparator5;
    private ToolStripMenuItem grafikfensterDefinierenToolStripMenuItem;
    private ToolStripMenuItem felderBearbeitenToolstripMenüItem;
    private ToolStripMenuItem zeigeNurGrafikfensterToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem zeigeAllesToolStripMenuItem;
    private ToolStripMenuItem tsmFont;

    private bool _database_open()
    {
      bool flag;
      try
      {
        this.conn = new SqlConnection("Server = (local);Database = Ball; User ID=sa; Password = ''");
        this.conn.Open();
        flag = true;
      }
      catch
      {
        flag = false;
      }
      return flag;
    }

    private void _database_close()
    {
      try
      {
        if (this.conn.State == ConnectionState.Open)
          this.conn.Close();
        this.conn.Dispose();
      }
      catch
      {
      }
    }

    private bool _database_layout_available(string LayoutName)
    {
      bool flag = false;
      SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM SCB_Layouts WHERE Layoutname = '" + LayoutName + "'", this.conn).ExecuteReader();
      while (sqlDataReader.Read())
        flag = true;
      sqlDataReader.Close();
      return flag;
    }

    private ArrayList _database_layouts()
    {
      ArrayList arrayList = new ArrayList();
      SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM SCB_Layouts", this.conn).ExecuteReader();
      while (sqlDataReader.Read())
        arrayList.Add((object) sqlDataReader["Layoutname"].ToString().Trim());
      sqlDataReader.Close();
      return arrayList;
    }

    private int _database_layout_id(string LayoutName)
    {
      SqlDataReader sqlDataReader = new SqlCommand("SELECT * FROM SCB_Layouts WHERE LayoutName = '" + LayoutName + "'", this.conn).ExecuteReader();
      int num = -1;
      while (sqlDataReader.Read())
        num = (int) sqlDataReader["ID"];
      sqlDataReader.Close();
      return num;
    }

    private void _database_open_layout(string LayoutName)
    {
      if (!(LayoutName.Trim() != string.Empty))
        return;
      SqlDataReader sqlDataReader1 = new SqlCommand("SELECT * FROM SCB_Params", this.conn).ExecuteReader();
      int width = 0;
      int height = 0;
      int num1 = 0;
      int num2 = 0;
      while (sqlDataReader1.Read())
      {
        num1 = (int) sqlDataReader1.GetInt16(2);
        num2 = (int) sqlDataReader1.GetInt16(3);
        width = (int) sqlDataReader1.GetInt16(4);
        height = (int) sqlDataReader1.GetInt16(5);
      }
      sqlDataReader1.Close();
      this.Location = new Point(num1 - 150, num2 - 20);
      this.panel1.Size = new Size(width, height);
      this.SetSCB_Size();
      this.UnSelectedLabels();
      this.panel1.Controls.Clear();
      SqlDataReader sqlDataReader2 = new SqlCommand("SELECT * FROM SCB_Fields WHERE LayoutID = " + (object) this._database_layout_id(LayoutName), this.conn).ExecuteReader();
      while (sqlDataReader2.Read())
      {
        sqlDataReader2.GetInt32(0);
        sqlDataReader2.GetInt32(1);
        sqlDataReader2.GetInt32(2);
        string str1 = sqlDataReader2.GetString(3);
        short int16_1 = sqlDataReader2.GetInt16(4);
        short int16_2 = sqlDataReader2.GetInt16(5);
        short int16_3 = sqlDataReader2.GetInt16(6);
        short int16_4 = sqlDataReader2.GetInt16(7);
        Color color = Color.FromArgb(sqlDataReader2.GetInt32(8));
        Color.FromArgb(sqlDataReader2.GetInt32(9));
        Font font = new Font(sqlDataReader2.GetString(10).Trim(), (float) sqlDataReader2.GetInt16(11), !(sqlDataReader2.GetString(12).Trim() == "Bold") ? (!(sqlDataReader2.GetString(12).Trim() == "Italic") ? FontStyle.Regular : FontStyle.Italic) : FontStyle.Bold);
        sqlDataReader2.GetBoolean(13);
        sqlDataReader2.GetInt16(14);
        sqlDataReader2.GetInt16(15);
        Color.FromArgb(sqlDataReader2.GetInt32(16));
        string str2 = sqlDataReader2.GetString(17);
        string str3 = sqlDataReader2.GetString(18).Trim();
        string str4 = sqlDataReader2.GetString(19).Trim();
        sqlDataReader2.GetString(20).Trim();
        sqlDataReader2.GetBoolean(21);
        if (!str1.Contains("Edge") && !str1.Contains("Rect"))
        {
          Label label = new Label();
          label.Name = str1.Trim();
          label.Size = new Size((int) int16_3 - (int) int16_1, (int) int16_4 - (int) int16_2);
          label.Location = new Point((int) int16_1, (int) int16_2);
          label.Font = font;
          label.ForeColor = color;
          label.BackColor = Color.DarkGray;
          label.Text = str2.Trim();
          switch (str3)
          {
            case "far":
              switch (str4)
              {
                case "far":
                  label.TextAlign = ContentAlignment.BottomRight;
                  break;
                case "center":
                  label.TextAlign = ContentAlignment.MiddleRight;
                  break;
                case "near":
                  label.TextAlign = ContentAlignment.TopRight;
                  break;
              }
              break;
            case "center":
              switch (str4)
              {
                case "far":
                  label.TextAlign = ContentAlignment.BottomCenter;
                  break;
                case "center":
                  label.TextAlign = ContentAlignment.MiddleCenter;
                  break;
                case "near":
                  label.TextAlign = ContentAlignment.TopCenter;
                  break;
              }
              break;
            case "near":
              switch (str4)
              {
                case "far":
                  label.TextAlign = ContentAlignment.BottomLeft;
                  break;
                case "center":
                  label.TextAlign = ContentAlignment.MiddleLeft;
                  break;
                case "near":
                  label.TextAlign = ContentAlignment.TopLeft;
                  break;
              }

              break;
          }
          label.ContextMenuStrip = this.contextMenuStrip1;
          this.panel1.Controls.Add((Control) label);
          label.MouseClick += new MouseEventHandler(this._lbl_MouseClick);
          label.MouseDown += new MouseEventHandler(this._temp_MouseDown);
          label.MouseUp += new MouseEventHandler(this._temp_MouseUp);
          label.MouseMove += new MouseEventHandler(this._temp_MouseMove);
          label.MouseEnter += new EventHandler(this._temp_MouseEnter);
          label.MouseLeave += new EventHandler(this._temp_MouseLeave);
          this.UnSelectedLabels();
          this._selected_labels.Add((object) label);
          ((Control) this._selected_labels[0]).BackColor = this._select_color;
        }
      }
      sqlDataReader2.Close();
    }

    private void _database_delete(string LayoutName)
    {
      int num = this._database_layout_id(LayoutName);
      SqlCommand sqlCommand = new SqlCommand("DELETE FROM SCB_Fields WHERE LayoutID = " + (object) num, this.conn);
      sqlCommand.ExecuteNonQuery();
      sqlCommand.CommandText = "DELETE FROM SCB_Layouts WHERE ID = " + (object) num;
      sqlCommand.ExecuteNonQuery();
    }

    private void _database_save_param(Point Location, Size Size)
    {
      SqlCommand sqlCommand = new SqlCommand();
      if (MessageBox.Show("Neue Position der Tafel speichern?", "Tafelparameter speichern", MessageBoxButtons.YesNo) == DialogResult.Yes)
        sqlCommand.CommandText = "UPDATE SCB_Params SET SCB_PosX = " + Location.X.ToString() + ", SCB_PosY = " + Location.Y.ToString() + ", SCB_DimX = " + Size.Width.ToString() + ", SCB_DimY = " + Size.Height.ToString();
      else
        sqlCommand.CommandText = "UPDATE SCB_Params SET SCB_DimX = " + Size.Width.ToString() + ", SCB_DimY = " + Size.Height.ToString();
      sqlCommand.Connection = this.conn;
      sqlCommand.ExecuteNonQuery();
    }

    private void _database_save_layout(string LayoutName)
    {
      SqlCommand sqlCommand = new SqlCommand("INSERT INTO SCB_Layouts(SCB_ID, Layoutname) VALUES ( 1,'" + LayoutName + "')", this.conn);
      sqlCommand.CommandType = CommandType.Text;
      sqlCommand.ExecuteNonQuery();
    }

    private void _database_save_field(int LayoutID, Label Field)
    {
      string str1 = "far";
      string str2 = "far";
      if (Field.TextAlign.ToString().Contains("Right"))
        str1 = "far";
      if (Field.TextAlign.ToString().Contains("Center"))
        str1 = "center";
      if (Field.TextAlign.ToString().Contains("Left"))
        str1 = "near";
      if (Field.TextAlign.ToString().Contains("Bottom"))
        str2 = "far";
      if (Field.TextAlign.ToString().Contains("Middle"))
        str2 = "center";
      if (Field.TextAlign.ToString().Contains("Top"))
        str2 = "near";
      object[] objArray1 = new object[31];
      objArray1[0] = (object) "INSERT INTO SCB_Fields (SCB_ID, LayoutID, Name, x1, y1, x2, y2, Color, BackColor, Fontname, FontHeight, FontAttribut, Edge, RoundX, RoundY, EdgeColor, Text, TextAlignX, TextAlignY, ImageName, Filled ) VALUES ( 1,";
      objArray1[1] = (object) LayoutID.ToString();
      objArray1[2] = (object) ", '";
      objArray1[3] = (object) Field.Name;
      objArray1[4] = (object) "', ";
      object[] objArray2 = objArray1;
      Point location = Field.Location;
      string str3 = Convert.ToInt16(location.X).ToString();
      objArray2[5] = (object) str3;
      objArray1[6] = (object) ", ";
      object[] objArray3 = objArray1;
      location = Field.Location;
      string str4 = Convert.ToInt16(location.Y).ToString();
      objArray3[7] = (object) str4;
      objArray1[8] = (object) ", ";
      object[] objArray4 = objArray1;
      location = Field.Location;
      short int16 = Convert.ToInt16(location.X + Field.Width);
      string str5 = int16.ToString();
      objArray4[9] = (object) str5;
      objArray1[10] = (object) ", ";
      object[] objArray5 = objArray1;
      location = Field.Location;
      int16 = Convert.ToInt16(location.Y + Field.Height);
      string str6 = int16.ToString();
      objArray5[11] = (object) str6;
      objArray1[12] = (object) ", ";
      object[] objArray6 = objArray1;
      int argb = Field.ForeColor.ToArgb();
      string str7 = argb.ToString();
      objArray6[13] = (object) str7;
      objArray1[14] = (object) ", ";
      object[] objArray7 = objArray1;
      argb = Color.Black.ToArgb();
      string str8 = argb.ToString();
      objArray7[15] = (object) str8;
      objArray1[16] = (object) ", '";
      objArray1[17] = (object) Field.Font.Name;
      objArray1[18] = (object) "', ";
      objArray1[19] = (object) Convert.ToInt32(Field.Font.SizeInPoints);
      objArray1[20] = (object) ", '";
      objArray1[21] = (object) Field.Font.Style.ToString();
      objArray1[22] = (object) "', 0, 1, 1, ";
      object[] objArray8 = objArray1;
      argb = Color.White.ToArgb();
      string str9 = argb.ToString();
      objArray8[23] = (object) str9;
      objArray1[24] = (object) ", '";
      objArray1[25] = (object) Field.Text;
      objArray1[26] = (object) "', '";
      objArray1[27] = (object) str1;
      objArray1[28] = (object) "', '";
      objArray1[29] = (object) str2;
      objArray1[30] = (object) "', '', 0)";
      SqlCommand sqlCommand = new SqlCommand(string.Concat(objArray1), this.conn);
      sqlCommand.CommandType = CommandType.Text;
      sqlCommand.ExecuteNonQuery();
    }

    public Form1(string arg)
    {
      this.InitializeComponent();
      Settings.Default.Reload();
      this.SetSCB_Size();
      this.DrawGrid();
      if (arg == string.Empty)
        return;
      this.openFileDialog1.FileName = Application.StartupPath + "\\" + arg + ".csv";
      try
      {
        this.openFileDialog1_FileOk((object) this, (CancelEventArgs) null);
      }
      catch
      {
      }
    }

    private void Form1_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Shift && !e.Alt && e.Control)
        this._ctrl_pressed = true;
      bool flag = false;
      if (!e.Shift && !e.Alt && !e.Control)
      {
        this.lblFieldLocation.Text = string.Empty;
        this.lblFieldSize.Text = string.Empty;
        switch (e.KeyCode)
        {
          case Keys.Left:
            int num1 = this.panel1.Width;
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              if (((Control) this._selected_labels[index]).Left < num1)
                num1 = ((Control) this._selected_labels[index]).Left;
            }
            if (num1 > 0)
            {
              for (int index = 0; index < this._selected_labels.Count; ++index)
              {
                ((Control) this._selected_labels[index]).BringToFront();
                if (!flag && ((Control) this._selected_labels[index]).Left > 0)
                  --((Label) this._selected_labels[index]).Left;
                else
                  flag = true;
              }
              this.DrawGrid();
              break;
            }
            break;
          case Keys.Up:
            int num2 = this.panel1.Height;
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              if (((Control) this._selected_labels[index]).Top < num2)
                num2 = ((Control) this._selected_labels[index]).Top;
            }
            if (num2 > 0)
            {
              for (int index = 0; index < this._selected_labels.Count; ++index)
              {
                ((Control) this._selected_labels[index]).BringToFront();
                if (!flag && ((Control) this._selected_labels[index]).Top > 0)
                  --((Label) this._selected_labels[index]).Top;
                else
                  flag = true;
              }
              this.DrawGrid();
              break;
            }
            break;
          case Keys.Right:
            int num3 = 0;
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              if (((Control) this._selected_labels[index]).Left + ((Control) this._selected_labels[index]).Width > num3)
                num3 = ((Control) this._selected_labels[index]).Left + ((Control) this._selected_labels[index]).Width;
            }
            if (num3 < this.panel1.Width)
            {
              for (int index = 0; index < this._selected_labels.Count; ++index)
              {
                ((Control) this._selected_labels[index]).BringToFront();
                ++((Label) this._selected_labels[index]).Left;
              }
              this.DrawGrid();
              break;
            }
            break;
          case Keys.Down:
            int num4 = 0;
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              if (((Control) this._selected_labels[index]).Top + ((Control) this._selected_labels[index]).Height > num4)
                num4 = ((Control) this._selected_labels[index]).Top + ((Control) this._selected_labels[index]).Height;
            }
            if (num4 < this.panel1.Height)
            {
              for (int index = 0; index < this._selected_labels.Count; ++index)
              {
                ((Control) this._selected_labels[index]).BringToFront();
                ++((Label) this._selected_labels[index]).Top;
              }
              this.DrawGrid();
              break;
            }
            break;
        }
      }
      if (!e.Shift && e.Alt & !e.Control)
      {
        switch (e.KeyCode)
        {
          case Keys.Left:
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              if (((Control) this._selected_labels[index]).Width > 10)
                --((Label) this._selected_labels[index]).Width;
            }
            this.DrawGrid();
            break;
          case Keys.Up:
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              if (((Control) this._selected_labels[index]).Height > 5)
                --((Label) this._selected_labels[index]).Height;
            }
            this.DrawGrid();
            break;
          case Keys.Right:
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              ++((Label) this._selected_labels[index]).Width;
            }
            this.DrawGrid();
            break;
          case Keys.Down:
            for (int index = 0; index < this._selected_labels.Count; ++index)
            {
              ((Control) this._selected_labels[index]).BringToFront();
              ++((Label) this._selected_labels[index]).Height;
            }
            this.DrawGrid();
            break;
        }
      }
      if (this._selected_labels != null && this._selected_labels.Count == 1)
      {
        ((Control) this._selected_labels[0]).BringToFront();
        this.lblFieldLocation.Text = ((Control) this._selected_labels[0]).Location.ToString();
        this.lblFieldSize.Text = ((Control) this._selected_labels[0]).Size.ToString();
      }
      this.SetDefaultLabelParameter();
      if (!e.Shift || e.Alt || e.Control)
        return;
      switch (e.KeyCode)
      {
        case Keys.Left:
          if (this.panel1.Width > 64)
            --this.panel1.Width;
          this.SetSCB_Size();
          this.DrawGrid();
          break;
        case Keys.Up:
          if (this.panel1.Height > 64)
            --this.panel1.Height;
          this.SetSCB_Size();
          this.DrawGrid();
          break;
        case Keys.Right:
          ++this.panel1.Width;
          this.SetSCB_Size();
          this.DrawGrid();
          break;
        case Keys.Down:
          ++this.panel1.Height;
          this.SetSCB_Size();
          this.DrawGrid();
          break;
      }
    }

    private void Form1_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Control)
        return;
      this._ctrl_pressed = false;
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
      Settings.Default.Save();
      DialogResult dialogResult = DialogResult.No;
      if (this._layout_changed)
        dialogResult = MessageBox.Show("Jetzt speichern?", "Das Layout wurde geändert!", MessageBoxButtons.YesNoCancel);
      if (dialogResult != DialogResult.Yes)
        return;
      this.btnSaveCSV_Click((object) this, (EventArgs) null);
    }

    private void btnNewLabel_Click(object sender, EventArgs e)
    {
      Label label = new Label();
      label.Location = Settings.Default.DefaultFieldLocation;
      label.Size = Settings.Default.DefaultFieldSize;
      label.ForeColor = Settings.Default.DefaultForeColor;
      label.Font = Settings.Default.DefaultFont;
      label.Name = Settings.Default.DefaultFieldName;
      if (this.lbMissingFieldNames.SelectedIndex > -1)
        label.Name = this.lbMissingFieldNames.Text.Trim();
      label.Text = Settings.Default.DefaultText;
      label.Tag = (object) Settings.Default.DefaultAltColor.ToArgb();
      label.TextAlign = Settings.Default.DefaultTextAlign;
      this.panel1.Controls.Add((Control) label);
      this.UnSelectedLabels();
      label.BackColor = this._select_color;
      label.MouseClick += new MouseEventHandler(this._lbl_MouseClick);
      label.MouseUp += new MouseEventHandler(this._temp_MouseUp);
      label.MouseDown += new MouseEventHandler(this._temp_MouseDown);
      label.MouseMove += new MouseEventHandler(this._temp_MouseMove);
      label.MouseEnter += new EventHandler(this._temp_MouseEnter);
      label.MouseLeave += new EventHandler(this._temp_MouseLeave);
      label.ContextMenuStrip = this.contextMenuStrip1;
      label.BringToFront();
      this._selected_labels.Add((object) label);
      this.textBox1.Text = ((Control) this._selected_labels[0]).Name;
      this.textBox2.Text = ((Control) this._selected_labels[0]).Text;
      this.lblFieldLocation.Text = ((Control) this._selected_labels[0]).Location.ToString();
      this.lblFieldSize.Text = ((Control) this._selected_labels[0]).Size.ToString();
      this._layout_changed = true;
      this.DrawGrid();
      this._do_check_fields();
    }

    private void _lbl_MouseClick(object sender, MouseEventArgs e)
    {
    }

    private void SetSCB_Size()
    {
      this.Width = 166 + this.panel1.Width;
      if (64 + this.panel1.Height < 320)
        this.Height = 320;
      else
        this.Height = 64 + this.panel1.Height;
      this.lblSCB_Size.Text = this.panel1.Size.ToString();
      this._default_grid_img = (Image) null;
      this.DrawGrid();
    }

    private void DrawGrid()
    {
      if (this._default_grid_img == null)
      {
        this._default_grid_img = (Image) new Bitmap(this.panel1.Width, this.panel1.Height);
        Graphics graphics = Graphics.FromImage(this._default_grid_img);
        graphics.Clear(Color.Black);
        for (int index = this.panel1.Width / 2; index < this.panel1.Width; index += 10)
          graphics.DrawLine(new Pen((Brush) new SolidBrush(Color.DarkBlue), 1f), index, 0, index, this.panel1.Height);
        for (int index = this.panel1.Width / 2; index > 0; index -= 10)
          graphics.DrawLine(new Pen((Brush) new SolidBrush(Color.DarkBlue), 1f), index, 0, index, this.panel1.Height);
        for (int index = this.panel1.Height / 2; index < this.panel1.Height; index += 10)
          graphics.DrawLine(new Pen((Brush) new SolidBrush(Color.DarkBlue), 1f), 0, index, this.panel1.Width, index);
        for (int index = this.panel1.Height / 2; index > 0; index -= 10)
          graphics.DrawLine(new Pen((Brush) new SolidBrush(Color.DarkBlue), 1f), 0, index, this.panel1.Width, index);
        graphics.DrawLine(new Pen((Brush) new SolidBrush(Color.DarkRed), 1f), 0, this.panel1.Height / 2, this.panel1.Width, this.panel1.Height / 2);
        graphics.DrawLine(new Pen((Brush) new SolidBrush(Color.DarkRed), 1f), this.panel1.Width / 2, 0, this.panel1.Width / 2, this.panel1.Height);
        this._act_grid_image = (Image) this._default_grid_img.Clone();
      }
      Graphics graphics1 = this.panel1.CreateGraphics();
      graphics1.DrawImage(this._act_grid_image, new Point(0, 0));
      if (this._graphic_window != null)
        graphics1.DrawRectangle(new Pen((Brush) new SolidBrush(Color.Yellow), 1f), new Rectangle(this._graphic_window.Location, this._graphic_window.Size));
      foreach (Control control in (ArrangedElementCollection) this.panel1.Controls)
        control.Visible = this.zeigeAllesToolStripMenuItem.Checked;
    }

    private void UnSelectedLabels()
    {
      this._selected_labels = (ArrayList) null;
      foreach (Control control in (ArrangedElementCollection) this.panel1.Controls)
      {
        control.BackColor = Color.DimGray;
        ((Label) control).BorderStyle = BorderStyle.None;
      }
      if (this._selected_labels == null)
        this._selected_labels = new ArrayList();
      this._selected_labels.Clear();
      this.lblFieldLocation.Text = string.Empty;
      this.lblFieldSize.Text = string.Empty;
      this.textBox1.Text = string.Empty;
      this.textBox2.Text = string.Empty;
      this.DrawGrid();
    }

    private void btnSaveCSV_Click(object sender, EventArgs e)
    {
      bool flag = false;
      string str = string.Empty;
      for (int index1 = 0; index1 < this.panel1.Controls.Count; ++index1)
      {
        Label control = (Label) this.panel1.Controls[index1];
        for (int index2 = 0; index2 < this.panel1.Controls.Count; ++index2)
        {
          if (index1 != index2 && control.Name == this.panel1.Controls[index2].Name)
          {
            flag = true;
            str = control.Name;
          }
        }
      }
      if (flag)
      {
        int num1 = (int) MessageBox.Show("Feldname '" + str + "' ist mehrfach vorhanden!");
      }
      else
      {
        this.saveFileDialog1.InitialDirectory = Settings.Default.DefaultWorkingDirectory;
        this.saveFileDialog1.CheckFileExists = false;
        this.saveFileDialog1.OverwritePrompt = false;
        if (this.lblLayoutname.Text.Trim() != string.Empty)
          this.saveFileDialog1.FileName = Settings.Default.DefaultWorkingDirectory + "\\" + this.lblLayoutname.Text;
        int num2 = (int) this.saveFileDialog1.ShowDialog();
        this.DrawGrid();
      }
    }

    private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      StreamWriter streamWriter1 = new StreamWriter(this.saveFileDialog1.FileName);
      if (this.saveFileDialog1.FilterIndex == 1)
      {
        if (this._graphic_window == null)
          streamWriter1.WriteLine(this.panel1.Width.ToString() + ";" + this.panel1.Height.ToString() + ";0;0;0;0");
        else
          streamWriter1.WriteLine(this.panel1.Width.ToString() + ";" + this.panel1.Height.ToString() + ";" + this._graphic_window.Location.X.ToString() + ";" + this._graphic_window.Location.Y.ToString() + ";" + this._graphic_window.Size.Width.ToString() + ";" + this._graphic_window.Size.Height.ToString());
        foreach (Label control in (ArrangedElementCollection) this.panel1.Controls)
        {
          if (control.Tag == null)
            control.Tag = (object) control.ForeColor.ToArgb();
          if (!this._parameter.ContainsKey(control.Name))
            streamWriter1.WriteLine(control.Name + ";" + control.Left.ToString() + ";" + control.Top.ToString() + ";" + control.Width.ToString() + ";" + control.Height.ToString() + ";" + control.Font.Name + ";" + control.Font.SizeInPoints.ToString() + ";" + control.Font.Bold.ToString() + ";" + control.ForeColor.ToArgb().ToString() + ";" + control.Tag.ToString() + ";" + control.TextAlign.ToString() + ";" + control.Text);
          else
            streamWriter1.WriteLine(control.Name + ";" + control.Left.ToString() + ";" + control.Top.ToString() + ";" + control.Width.ToString() + ";" + control.Height.ToString() + ";" + control.Font.Name + ";" + control.Font.SizeInPoints.ToString() + ";" + control.Font.Bold.ToString() + ";" + control.ForeColor.ToArgb().ToString() + ";" + control.Tag.ToString() + ";" + control.TextAlign.ToString() + ";" + control.Text + ";" + this._parameter[control.Name]);
        }
      }
      if (this.saveFileDialog1.FilterIndex == 2)
      {
        foreach (Control control in (ArrangedElementCollection) this.panel1.Controls)
        {
          Color color = Color.Transparent;
          Console.WriteLine(color.ToArgb().ToString());
          Label label = (Label) control;
          string str1 = !label.Font.Bold ? "Regular" : "Bold";
          string str2 = label.TextAlign != ContentAlignment.BottomLeft && label.TextAlign != ContentAlignment.MiddleLeft && label.TextAlign != ContentAlignment.TopLeft ? (label.TextAlign != ContentAlignment.BottomCenter && label.TextAlign != ContentAlignment.MiddleCenter && label.TextAlign != ContentAlignment.TopCenter ? "far" : "center") : "near";
          string str3 = label.TextAlign != ContentAlignment.BottomCenter && label.TextAlign != ContentAlignment.BottomLeft && label.TextAlign != ContentAlignment.BottomRight ? (label.TextAlign != ContentAlignment.MiddleLeft && label.TextAlign != ContentAlignment.MiddleCenter && label.TextAlign != ContentAlignment.MiddleRight ? "near" : "center") : "far";
          StreamWriter streamWriter2 = streamWriter1;
          string[] strArray1 = new string[27];
          strArray1[0] = "0;0;";
          strArray1[1] = label.Name;
          strArray1[2] = ";";
          strArray1[3] = label.Left.ToString();
          strArray1[4] = ";";
          strArray1[5] = label.Top.ToString();
          strArray1[6] = ";";
          strArray1[7] = (label.Left + label.Width).ToString();
          strArray1[8] = ";";
          strArray1[9] = (label.Top + label.Height).ToString();
          strArray1[10] = ";";
          string[] strArray2 = strArray1;
          color = label.ForeColor;
          string str4 = color.ToArgb().ToString();
          strArray2[11] = str4;
          strArray1[12] = ";";
          string[] strArray3 = strArray1;
          color = Color.Black;
          string str5 = color.ToArgb().ToString();
          strArray3[13] = str5;
          strArray1[14] = ";";
          strArray1[15] = label.Font.Name;
          strArray1[16] = ";";
          strArray1[17] = label.Font.SizeInPoints.ToString();
          strArray1[18] = ";";
          strArray1[19] = str1;
          strArray1[20] = ";False;1;1;-1;";
          strArray1[21] = label.Text;
          strArray1[22] = ";";
          strArray1[23] = str2;
          strArray1[24] = ";";
          strArray1[25] = str3;
          strArray1[26] = ";;False";
          string str6 = string.Concat(strArray1);
          streamWriter2.WriteLine(str6);
        }
      }
      streamWriter1.Close();
      this.lblLayoutname.Text = this.saveFileDialog1.FileName.Substring(this.saveFileDialog1.FileName.LastIndexOf('\\') + 1);
      this._layout_changed = false;
    }

    private void fontToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.fontDialog1.Font = Settings.Default.DefaultFont;
      int num = (int) this.fontDialog1.ShowDialog();
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Control) this._selected_labels[index]).Font = this.fontDialog1.Font;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void farbeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.colorDialog1.Color = Settings.Default.DefaultForeColor;
      int num = (int) this.colorDialog1.ShowDialog();
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Control) this._selected_labels[index]).ForeColor = this.colorDialog1.Color;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void alternativFarbeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.colorDialog1.Color = Settings.Default.DefaultAltColor;
      int num = (int) this.colorDialog1.ShowDialog();
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Control) this._selected_labels[index]).Tag = (object) this.colorDialog1.Color.ToArgb();
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void löschenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
      {
        ((Control) this._selected_labels[index]).MouseClick -= new MouseEventHandler(this._lbl_MouseClick);
        ((Control) this._selected_labels[index]).MouseUp -= new MouseEventHandler(this._temp_MouseUp);
        ((Control) this._selected_labels[index]).MouseDown -= new MouseEventHandler(this._temp_MouseDown);
        ((Control) this._selected_labels[index]).MouseMove -= new MouseEventHandler(this._temp_MouseMove);
        ((Control) this._selected_labels[index]).MouseEnter -= new EventHandler(this._temp_MouseEnter);
        ((Control) this._selected_labels[index]).MouseLeave -= new EventHandler(this._temp_MouseLeave);
        this.panel1.Controls.Remove((Control) this._selected_labels[index]);
      }
      this._selected_labels.Clear();
      this._layout_changed = true;
      this.DrawGrid();
      this._do_check_fields();
    }

    private void btnLoadCSV_Click(object sender, EventArgs e)
    {
      this.openFileDialog1.InitialDirectory = Application.StartupPath;
      int num = (int) this.openFileDialog1.ShowDialog();
      this._select_checkfile();
      this.DrawGrid();
      this.felderBearbeitenToolstripMenüItem_Click((object) this, (EventArgs) null);
    }

    private void _select_checkfile()
    {
      Form3 form3 = new Form3();
      int num = (int) form3.ShowDialog();
      this._checkfile_name = form3.CheckFileName;
      this._do_check_fields();
    }

    private void _do_check_fields()
    {
      if (!(this._checkfile_name != string.Empty) || !File.Exists(Application.StartupPath + "\\" + this._checkfile_name))
        return;
      ArrayList arrayList = new ArrayList();
      foreach (Control control in (ArrangedElementCollection) this.panel1.Controls)
        arrayList.Add((object) control.Name);
      this.lbMissingFieldNames.Items.Clear();
      StreamReader streamReader = new StreamReader(Application.StartupPath + "\\" + this._checkfile_name);
      for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
      {
        if (!arrayList.Contains((object) str.Trim()) && !str.Contains("Edge") && !str.Contains("Rect"))
          this.lbMissingFieldNames.Items.Add((object) str.Trim());
      }
      streamReader.Close();
    }

    private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
    {
      this.DrawGrid();
      this.panel1.Controls.Clear();
      Label label1 = new Label();
      ContentAlignment contentAlignment1 = ContentAlignment.MiddleCenter;
      Settings.Default.DefaultWorkingDirectory = this.openFileDialog1.FileName.Substring(0, this.openFileDialog1.FileName.LastIndexOf('\\'));
      Settings.Default.Save();
      StreamReader streamReader = new StreamReader(this.openFileDialog1.FileName);
      this.lblLayoutname.Text = this.openFileDialog1.FileName.Substring(this.openFileDialog1.FileName.LastIndexOf('\\') + 1);
      string str1 = streamReader.ReadLine();
      string[] strArray1 = str1.Split(';');
      if (strArray1.Length == 2 || strArray1.Length == 6)
      {
        this.panel1.Size = new Size(Convert.ToInt32(strArray1[0]), Convert.ToInt32(strArray1[1]));
        this._graphic_window = (Label) null;
        if (strArray1.Length == 6)
        {
          this._graphic_window = new Label();
          this._graphic_window.Location = new Point(Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[3]));
          this._graphic_window.Size = new Size(Convert.ToInt32(strArray1[4]), Convert.ToInt32(strArray1[5]));
        }
        this.SetSCB_Size();
        for (string str2 = streamReader.ReadLine(); str2 != null; str2 = streamReader.ReadLine())
        {
          string[] strArray2 = str2.Split(';');
          Label label2 = new Label();
          label2.Name = strArray2[0];
          label2.Left = Convert.ToInt32(strArray2[1]);
          label2.Top = Convert.ToInt32(strArray2[2]);
          label2.Width = Convert.ToInt32(strArray2[3]);
          label2.Height = Convert.ToInt32(strArray2[4]);
          FontStyle style = !(strArray2[7] == "True") ? FontStyle.Regular : FontStyle.Bold;
          label2.Font = new Font(strArray2[5], Convert.ToSingle(strArray2[6]), style);
          label2.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[8]));
          label2.Tag = (object) Convert.ToInt32(strArray2[9]);
          ContentAlignment contentAlignment2;
          switch (strArray2[10])
          {
            case "BottomLeft":
              contentAlignment2 = ContentAlignment.BottomLeft;
              break;
            case "BottomCenter":
              contentAlignment2 = ContentAlignment.BottomCenter;
              break;
            case "BottomRight":
              contentAlignment2 = ContentAlignment.BottomRight;
              break;
            case "TopLeft":
              contentAlignment2 = ContentAlignment.TopLeft;
              break;
            case "TopCenter":
              contentAlignment2 = ContentAlignment.TopCenter;
              break;
            case "TopRight":
              contentAlignment2 = ContentAlignment.TopRight;
              break;
            case "MiddleLeft":
              contentAlignment2 = ContentAlignment.MiddleLeft;
              break;
            case "MiddleCenter":
              contentAlignment2 = ContentAlignment.MiddleCenter;
              break;
            case "MiddleRight":
              contentAlignment2 = ContentAlignment.MiddleRight;
              break;
            default:
              contentAlignment2 = ContentAlignment.MiddleLeft;
              break;
          }
          label2.TextAlign = contentAlignment2;
          label2.Text = strArray2[11];
          if (strArray2.Length > 12)
          {
            string str3 = strArray2[12] + ";" + strArray2[13] + ";" + strArray2[14];
            this._parameter.Add(label2.Name, str3);
          }
          label2.ContextMenuStrip = this.contextMenuStrip1;
          this.panel1.Controls.Add((Control) label2);
          label2.MouseClick += new MouseEventHandler(this._lbl_MouseClick);
          label2.MouseDown += new MouseEventHandler(this._temp_MouseDown);
          label2.MouseUp += new MouseEventHandler(this._temp_MouseUp);
          label2.MouseMove += new MouseEventHandler(this._temp_MouseMove);
          label2.MouseEnter += new EventHandler(this._temp_MouseEnter);
          label2.MouseLeave += new EventHandler(this._temp_MouseLeave);
          this.UnSelectedLabels();
          this._selected_labels.Add((object) label2);
          ((Control) this._selected_labels[0]).BackColor = this._select_color;
        }
      }
      else
      {
        for (; str1 != null; str1 = streamReader.ReadLine())
        {
          string[] strArray2 = str1.Split(';');
          Label label2 = new Label();
          label2.Name = strArray2[2];
          label2.Left = Convert.ToInt32(strArray2[3]);
          label2.Top = Convert.ToInt32(strArray2[4]);
          label2.Width = Convert.ToInt32(strArray2[5]) - Convert.ToInt32(strArray2[3]);
          label2.Height = Convert.ToInt32(strArray2[6]) - Convert.ToInt32(strArray2[4]);
          FontStyle style = !(strArray2[11] == "Bold") ? FontStyle.Regular : FontStyle.Bold;
          label2.Font = new Font(strArray2[9], Convert.ToSingle(strArray2[10]), style);
          label2.ForeColor = Color.FromArgb(Convert.ToInt32(strArray2[7]));
          label2.Tag = (object) Convert.ToInt32(strArray2[7]);
          strArray2[17] = strArray2[17].ToUpper();
          strArray2[18] = strArray2[18].ToUpper();
          switch (strArray2[18])
          {
            case "NEAR":
              switch (strArray2[17])
              {
                case "NEAR":
                  contentAlignment1 = ContentAlignment.TopLeft;
                  break;
                case "FAR":
                  contentAlignment1 = ContentAlignment.TopRight;
                  break;
                case "CENTER":
                  contentAlignment1 = ContentAlignment.TopCenter;
                  break;
              }
              break;
            case "FAR":
              switch (strArray2[17])
              {
                case "NEAR":
                  contentAlignment1 = ContentAlignment.BottomLeft;
                  break;
                case "FAR":
                  contentAlignment1 = ContentAlignment.BottomRight;
                  break;
                case "CENTER":
                  contentAlignment1 = ContentAlignment.BottomCenter;
                  break;
              }
              break;
            case "CENTER":
              switch (strArray2[17])
              {
                case "NEAR":
                  contentAlignment1 = ContentAlignment.MiddleLeft;
                  break;
                case "FAR":
                  contentAlignment1 = ContentAlignment.MiddleRight;
                  break;
                case "CENTER":
                  contentAlignment1 = ContentAlignment.MiddleCenter;
                  break;
              }
              break;
          }
          label2.TextAlign = contentAlignment1;
          label2.Text = strArray2[16];
          label2.ContextMenuStrip = this.contextMenuStrip1;
          if (!label2.Name.StartsWith("Edge") && !label2.Name.StartsWith("Rect"))
          {
            this.panel1.Controls.Add((Control) label2);
            label2.MouseClick += new MouseEventHandler(this._lbl_MouseClick);
            label2.MouseDown += new MouseEventHandler(this._temp_MouseDown);
            label2.MouseUp += new MouseEventHandler(this._temp_MouseUp);
            label2.MouseMove += new MouseEventHandler(this._temp_MouseMove);
            label2.MouseEnter += new EventHandler(this._temp_MouseEnter);
            label2.MouseLeave += new EventHandler(this._temp_MouseLeave);
            this.UnSelectedLabels();
            this._selected_labels.Add((object) label2);
            ((Control) this._selected_labels[0]).BackColor = this._select_color;
          }
        }
      }
      streamReader.Close();
      this.felderBearbeitenToolstripMenüItem_Click((object) this, (EventArgs) null);
      this.textBox1.Text = ((Control) this._selected_labels[0]).Name;
      this.textBox2.Text = ((Control) this._selected_labels[0]).Text;
      this.lblFieldLocation.Text = ((Control) this._selected_labels[0]).Location.ToString();
      this.lblFieldSize.Text = ((Control) this._selected_labels[0]).Size.ToString();
      this.SetDefaultLabelParameter();
      this._layout_changed = false;
      this.DrawGrid();
    }

    private void _temp_MouseLeave(object sender, EventArgs e)
    {
      this.lblLabelName.Text = string.Empty;
    }

    private void _temp_MouseEnter(object sender, EventArgs e)
    {
      this.lblLabelName.Text = ((Control) sender).Name;
    }

    private void _temp_MouseMove(object sender, MouseEventArgs e)
    {
      if (this._ctrl_pressed || !this._selected_labels.Contains((object) (Label) sender))
        return;
      Point location = e.Location;
      int num1 = location.X - this._mouse_base_position.X;
      location = e.Location;
      int num2 = location.Y - this._mouse_base_position.Y;
      if (this._selected_labels.Count > 0 && this._mouse_is_down)
      {
        for (int index = 0; index < this._selected_labels.Count; ++index)
        {
          Label selectedLabel = (Label) this._selected_labels[index];
          selectedLabel.BringToFront();
          Label label = selectedLabel;
          location = selectedLabel.Location;
          int x = location.X + num1;
          location = selectedLabel.Location;
          int y = location.Y + num2;
          Point point = new Point(x, y);
          label.Location = point;
        }
      }
      this.DrawGrid();
    }

    private void _temp_MouseUp(object sender, MouseEventArgs e)
    {
      this._mouse_is_down = false;
      if (this._selected_labels.Contains((object) (Label) sender))
        return;
      if (!this._ctrl_pressed)
        this.UnSelectedLabels();
      if (this._selected_labels.Count < 1 || this._ctrl_pressed)
      {
        this._selected_labels.Add((object) (Label) sender);
        ((Control) sender).BackColor = this._select_color;
        this.lblFieldLocation.Text = string.Empty;
        this.lblFieldSize.Text = string.Empty;
        this.textBox1.Text = "{Mehrfachauswahl}";
        this.textBox1.Enabled = false;
        if (this._selected_labels.Count == 1)
        {
          this.textBox1.Enabled = true;
          this.textBox1.Text = ((Control) this._selected_labels[0]).Name;
          this.textBox2.Text = ((Control) this._selected_labels[0]).Text;
          this.lblFieldLocation.Text = ((Control) this._selected_labels[0]).Location.ToString();
          this.lblFieldSize.Text = ((Control) this._selected_labels[0]).Size.ToString();
        }
        this.SetDefaultLabelParameter();
      }
    }

    private void _temp_MouseDown(object sender, MouseEventArgs e)
    {
      this._mouse_base_position = e.Location;
      this._mouse_is_down = true;
    }

    private void textBox2_TextChanged(object sender, EventArgs e)
    {
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (this._selected_labels.Count != 1 || !(this.textBox1.Text != "{Mehrfachauswahl}"))
        return;
      ((Control) this._selected_labels[0]).Name = this.textBox1.Text;
    }

    private void linksToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.MiddleLeft;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void zentriertToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.MiddleCenter;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void rechtsToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.MiddleRight;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void linksToolStripMenuItem1_Click_1(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.TopLeft;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void zentriertToolStripMenuItem1_Click_1(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.TopCenter;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void rechtsToolStripMenuItem1_Click_1(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.TopRight;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void linksToolStripMenuItem2_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.BottomLeft;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void zentriertToolStripMenuItem2_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.BottomCenter;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void rechtsToolStripMenuItem2_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Label) this._selected_labels[index]).TextAlign = ContentAlignment.BottomRight;
      this._layout_changed = true;
      this.DrawGrid();
    }

    private void SetDefaultLabelParameter()
    {
      if (this._selected_labels == null || this._selected_labels.Count != 1)
        return;
      Settings.Default.DefaultFieldSize = ((Control) this._selected_labels[0]).Size;
      Settings.Default.DefaultFont = ((Control) this._selected_labels[0]).Font;
      Settings.Default.DefaultForeColor = ((Control) this._selected_labels[0]).ForeColor;
      Settings.Default.DefaultAltColor = Color.FromArgb(Convert.ToInt32(((Control) this._selected_labels[0]).Tag));
      Settings.Default.DefaultTextAlign = ((Label) this._selected_labels[0]).TextAlign;
      Settings.Default.DefaultText = ((Control) this._selected_labels[0]).Text;
      Settings.Default.DefaultFieldName = ((Control) this._selected_labels[0]).Name;
      Settings.Default.Save();
    }

    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
        e.Handled = true;
      else
        this._do_check_fields();
    }

    private void textBox2_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right) || (e.Alt || e.Control) || e.Shift)
      {
        e.Handled = true;
      }
      else
      {
        for (int index = 0; index < this._selected_labels.Count; ++index)
          ((Control) this._selected_labels[index]).Text = this.textBox2.Text;
        this._layout_changed = true;
      }
    }

    private void panel1_MouseClick(object sender, MouseEventArgs e)
    {
      this.DrawGrid();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      this.DrawGrid();
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
    }

    private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
    {
      this.DrawGrid();
    }

    private void panel1_MouseDown(object sender, MouseEventArgs e)
    {
      if (!this._ctrl_pressed)
        this.UnSelectedLabels();
      this._rect_start = new Point(e.X, e.Y);
    }

    private void panel1_MouseMove(object sender, MouseEventArgs e)
    {
      Label lblCursorPosition = this.lblCursorPosition;
      int num1 = e.X;
      string str1 = num1.ToString();
      num1 = e.Y;
      string str2 = num1.ToString();
      string str3 = "Cursorposition: " + str1 + "," + str2;
      lblCursorPosition.Text = str3;
      this.lblCursorPosition.Refresh();
      if (e.Button != MouseButtons.Left)
        return;
      if (this._select_mode != Form1.SelectMode.DefineGraphicWindow)
        this._mouse_select = true;
      this._temp_grid_image = (Image) this._default_grid_img.Clone();
      Graphics graphics = Graphics.FromImage(this._temp_grid_image);
      Point point1 = new Point();
      Point point2 = new Point();
      int num2 = e.X;
      if (num2 > this.panel1.Width)
        num2 = this.panel1.Width;
      if (this._rect_start.X > num2)
      {
        point1.X = num2;
        point2.X = this._rect_start.X - num2;
      }
      else
      {
        point1.X = this._rect_start.X;
        point2.X = num2 - this._rect_start.X;
      }
      if (this._rect_start.Y > e.Y)
      {
        point1.Y = e.Y;
        point2.Y = this._rect_start.Y - e.Y;
      }
      else
      {
        point1.Y = this._rect_start.Y;
        point2.Y = e.Y - this._rect_start.Y;
      }
      if (point1.X < 0)
        point1.X = 0;
      if (point1.X > this.panel1.Width)
        point1.X = this.panel1.Width;
      if (point1.Y < 0)
        point1.Y = 0;
      if (point1.Y > this.panel1.Height)
        point1.Y = this.panel1.Height;
      if (point2.X < 0)
        point2.X = 0;
      if (point2.X > this.panel1.Width)
        point2.X = this.panel1.Width;
      if (point2.Y < 0)
        point2.Y = 0;
      if (point2.Y > this.panel1.Height)
        point2.Y = this.panel1.Height;
      this._selected_rectangle = new Rectangle(point1.X, point1.Y, point2.X, point2.Y);
      graphics.DrawRectangle(new Pen((Brush) new SolidBrush(Color.White), 1f), this._selected_rectangle);
      this.panel1.CreateGraphics().DrawImage(this._temp_grid_image, new Point(0, 0));
    }

    private void panel1_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      if (this._select_mode == Form1.SelectMode.MouseSelect)
      {
        if (!this._ctrl_pressed)
          this.UnSelectedLabels();
        if (this._mouse_select)
        {
          this._mouse_select = false;
          foreach (Control control in (ArrangedElementCollection) this.panel1.Controls)
          {
            if (this._selected_rectangle.IntersectsWith(new Rectangle(control.Location, control.Size)))
            {
              this._selected_labels.Add((object) (Label) control);
              control.BackColor = this._select_color;
              this.lblFieldLocation.Text = string.Empty;
              this.lblFieldSize.Text = string.Empty;
              this.textBox1.Text = "{Mehrfachauswahl}";
              this.textBox1.Enabled = false;
              this.textBox2.Text = control.Text;
              if (this._selected_labels.Count == 1)
              {
                this.textBox1.Enabled = true;
                this.textBox1.Text = ((Control) this._selected_labels[0]).Name;
                this.textBox2.Text = ((Control) this._selected_labels[0]).Text;
                this.lblFieldLocation.Text = ((Control) this._selected_labels[0]).Location.ToString();
                this.lblFieldSize.Text = ((Control) this._selected_labels[0]).Size.ToString();
              }
            }
          }
          this.panel1.CreateGraphics().DrawImage(this._default_grid_img, new Point());
        }
      }
      else
      {
        Console.WriteLine(this.panel1.Height);
        Console.WriteLine((this._selected_rectangle.Height + this._selected_rectangle.Y).ToString());
        this._graphic_window = new Label();
        this._graphic_window.Size = new Size(this._selected_rectangle.Width, this._selected_rectangle.Height);
        this._graphic_window.Location = new Point(this._selected_rectangle.X, this._selected_rectangle.Y);
      }
      this.DrawGrid();
    }

    private void kopierenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this._copy_labels = (ArrayList) this._selected_labels.Clone();
      this.UnSelectedLabels();
    }

    private void einfügenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.UnSelectedLabels();
      Label label1 = new Label();
      Label label2 = new Label();
      for (int index = 0; index < this._copy_labels.Count; ++index)
      {
        Label copyLabel = (Label) this._copy_labels[index];
        Label label3 = new Label();
        label3.Location = copyLabel.Location;
        label3.Size = copyLabel.Size;
        label3.ForeColor = copyLabel.ForeColor;
        label3.Font = copyLabel.Font;
        label3.Name = copyLabel.Name + "_1";
        label3.Text = copyLabel.Text;
        label3.Tag = copyLabel.Tag;
        label3.TextAlign = copyLabel.TextAlign;
        this.panel1.Controls.Add((Control) label3);
        label3.BringToFront();
        label3.BackColor = this._select_color;
        label3.MouseClick += new MouseEventHandler(this._lbl_MouseClick);
        label3.MouseUp += new MouseEventHandler(this._temp_MouseUp);
        label3.MouseDown += new MouseEventHandler(this._temp_MouseDown);
        label3.MouseMove += new MouseEventHandler(this._temp_MouseMove);
        label3.MouseEnter += new EventHandler(this._temp_MouseEnter);
        label3.MouseLeave += new EventHandler(this._temp_MouseLeave);
        label3.ContextMenuStrip = this.contextMenuStrip1;
        this._selected_labels.Add((object) label3);
        this._layout_changed = true;
      }
      this._do_check_fields();
    }

    private void btnNewlayout_Click(object sender, EventArgs e)
    {
      DialogResult dialogResult = DialogResult.No;
      if (this._layout_changed)
        dialogResult = MessageBox.Show("Jetzt speichern?", "Das Layout wurde geändert!", MessageBoxButtons.YesNoCancel);
      if (dialogResult == DialogResult.Yes)
        this.btnSaveCSV_Click((object) this, (EventArgs) null);
      if ((dialogResult != DialogResult.No || this._layout_changed) && dialogResult != DialogResult.No)
        return;
      for (int index = 0; index < this.panel1.Controls.Count; ++index)
        this.panel1.Controls[index].MouseClick -= new MouseEventHandler(this._lbl_MouseClick);
      this.panel1.Controls.Clear();
      this._layout_changed = false;
      this._select_checkfile();
    }

    private void lbMissingFieldNames_DoubleClick(object sender, EventArgs e)
    {
      if (this.lbMissingFieldNames.SelectedIndex <= -1)
        return;
      this.btnNewLabel_Click((object) this, (EventArgs) null);
      this._do_check_fields();
    }

    private void lbMissingFieldNames_KeyDown(object sender, KeyEventArgs e)
    {
      e.Handled = true;
    }

    private void prüfdateiWählenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this._select_checkfile();
    }

    private void grafikfensterDefinierenToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.grafikfensterDefinierenToolStripMenuItem.Checked = true;
      this.felderBearbeitenToolstripMenüItem.Checked = false;
      this._select_mode = Form1.SelectMode.DefineGraphicWindow;
    }

    private void felderBearbeitenToolstripMenüItem_Click(object sender, EventArgs e)
    {
      this.grafikfensterDefinierenToolStripMenuItem.Checked = false;
      this.felderBearbeitenToolstripMenüItem.Checked = true;
      this._select_mode = Form1.SelectMode.MouseSelect;
    }

    private void zeigeNurGrafikfensterToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.zeigeAllesToolStripMenuItem.Checked = false;
      this.zeigeNurGrafikfensterToolStripMenuItem.Checked = true;
      this.DrawGrid();
    }

    private void zeigeAllesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.zeigeAllesToolStripMenuItem.Checked = true;
      this.zeigeNurGrafikfensterToolStripMenuItem.Checked = false;
      this.DrawGrid();
    }

    private void tsmFont_Click(object sender, EventArgs e)
    {
      this.fontDialog1.Font = Settings.Default.DefaultFont;
      int num = (int) this.fontDialog1.ShowDialog();
      for (int index = 0; index < this._selected_labels.Count; ++index)
        ((Control) this._selected_labels[index]).Font = this.fontDialog1.Font;
      this._layout_changed = true;
      this.DrawGrid();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
      this.lblSCB_Size = new Label();
      this.helpProvider1 = new HelpProvider();
      this.lblFieldLocation = new Label();
      this.lblFieldSize = new Label();
      this.panel1 = new Panel();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.insertToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator5 = new ToolStripSeparator();
      this.felderBearbeitenToolstripMenüItem = new ToolStripMenuItem();
      this.grafikfensterDefinierenToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.zeigeAllesToolStripMenuItem = new ToolStripMenuItem();
      this.zeigeNurGrafikfensterToolStripMenuItem = new ToolStripMenuItem();
      this.lbMissingFieldNames = new ListBox();
      this.contextMenuStrip3 = new ContextMenuStrip(this.components);
      this.prüfdateiWählenToolStripMenuItem = new ToolStripMenuItem();
      this.lblCursorPosition = new Label();
      this.btnLoadCSV = new Button();
      this.btnSaveCSV = new Button();
      this.btnNewLabel = new Button();
      this.saveFileDialog1 = new SaveFileDialog();
      this.openFileDialog1 = new OpenFileDialog();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.farbeToolStripMenuItem = new ToolStripMenuItem();
      this.alternativFarbeToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.textausrichtungToolStripMenuItem = new ToolStripMenuItem();
      this.obenToolStripMenuItem = new ToolStripMenuItem();
      this.linksToolStripMenuItem1 = new ToolStripMenuItem();
      this.zentriertToolStripMenuItem1 = new ToolStripMenuItem();
      this.rechtsToolStripMenuItem1 = new ToolStripMenuItem();
      this.mitteToolStripMenuItem = new ToolStripMenuItem();
      this.linksToolStripMenuItem = new ToolStripMenuItem();
      this.zentriertToolStripMenuItem = new ToolStripMenuItem();
      this.rechtsToolStripMenuItem = new ToolStripMenuItem();
      this.untenToolStripMenuItem = new ToolStripMenuItem();
      this.linksToolStripMenuItem2 = new ToolStripMenuItem();
      this.zentriertToolStripMenuItem2 = new ToolStripMenuItem();
      this.rechtsToolStripMenuItem2 = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.kopierenToolStripMenuItem = new ToolStripMenuItem();
      this.einfügenToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.löschenToolStripMenuItem = new ToolStripMenuItem();
      this.fontDialog1 = new FontDialog();
      this.colorDialog1 = new ColorDialog();
      this.textBox1 = new TextBox();
      this.textBox2 = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.btnNewlayout = new Button();
      this.lblLabelName = new Label();
      this.lblLayoutname = new Label();
      this.label3 = new Label();
      this.tsmFont = new ToolStripMenuItem();
      this.contextMenuStrip2.SuspendLayout();
      this.contextMenuStrip3.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.SuspendLayout();
      this.lblSCB_Size.ForeColor = Color.White;
      this.helpProvider1.SetHelpString((Control) this.lblSCB_Size, "Displaygröße");
      this.lblSCB_Size.Location = new Point(147, 3);
      this.lblSCB_Size.Name = "lblSCB_Size";
      this.helpProvider1.SetShowHelp((Control) this.lblSCB_Size, true);
      this.lblSCB_Size.Size = new Size(151, 15);
      this.lblSCB_Size.TabIndex = 1;
      this.lblSCB_Size.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldLocation.BorderStyle = BorderStyle.FixedSingle;
      this.lblFieldLocation.ForeColor = Color.White;
      this.helpProvider1.SetHelpString((Control) this.lblFieldLocation, "Feldposition");
      this.lblFieldLocation.Location = new Point(15, 203);
      this.lblFieldLocation.Name = "lblFieldLocation";
      this.helpProvider1.SetShowHelp((Control) this.lblFieldLocation, true);
      this.lblFieldLocation.Size = new Size(129, 21);
      this.lblFieldLocation.TabIndex = 6;
      this.lblFieldLocation.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFieldSize.BorderStyle = BorderStyle.FixedSingle;
      this.lblFieldSize.ForeColor = Color.White;
      this.helpProvider1.SetHelpString((Control) this.lblFieldSize, "Feldgröße");
      this.lblFieldSize.Location = new Point(15, 226);
      this.lblFieldSize.Name = "lblFieldSize";
      this.helpProvider1.SetShowHelp((Control) this.lblFieldSize, true);
      this.lblFieldSize.Size = new Size(129, 21);
      this.lblFieldSize.TabIndex = 7;
      this.lblFieldSize.TextAlign = ContentAlignment.MiddleLeft;
      this.panel1.BackColor = Color.Black;
      this.panel1.ContextMenuStrip = this.contextMenuStrip2;
      this.panel1.DataBindings.Add(new Binding("Size", (object) Settings.Default, "SCB_Default_Size", true, DataSourceUpdateMode.OnPropertyChanged));
      this.helpProvider1.SetHelpString((Control) this.panel1, "Displayfläche");
      this.panel1.Location = new Point(150, 20);
      this.panel1.Name = "panel1";
      this.helpProvider1.SetShowHelp((Control) this.panel1, true);
      this.panel1.Size = Settings.Default.SCB_Default_Size;
      this.panel1.TabIndex = 0;
      this.panel1.MouseMove += new MouseEventHandler(this.panel1_MouseMove);
      this.panel1.MouseClick += new MouseEventHandler(this.panel1_MouseClick);
      this.panel1.MouseDown += new MouseEventHandler(this.panel1_MouseDown);
      this.panel1.MouseUp += new MouseEventHandler(this.panel1_MouseUp);
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.insertToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator5,
        (ToolStripItem) this.felderBearbeitenToolstripMenüItem,
        (ToolStripItem) this.grafikfensterDefinierenToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.zeigeAllesToolStripMenuItem,
        (ToolStripItem) this.zeigeNurGrafikfensterToolStripMenuItem
      });
      this.contextMenuStrip2.Name = "contextMenuStrip1";
      this.contextMenuStrip2.Size = new Size(200, 126);
      this.insertToolStripMenuItem.Name = "insertToolStripMenuItem";
      this.insertToolStripMenuItem.Size = new Size(199, 22);
      this.insertToolStripMenuItem.Text = "Einfügen";
      this.insertToolStripMenuItem.Click += new EventHandler(this.einfügenToolStripMenuItem_Click);
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new Size(196, 6);
      this.felderBearbeitenToolstripMenüItem.Checked = true;
      this.felderBearbeitenToolstripMenüItem.CheckState = CheckState.Checked;
      this.felderBearbeitenToolstripMenüItem.Name = "felderBearbeitenToolstripMenüItem";
      this.felderBearbeitenToolstripMenüItem.Size = new Size(199, 22);
      this.felderBearbeitenToolstripMenüItem.Text = "Felder bearbeiten";
      this.felderBearbeitenToolstripMenüItem.Click += new EventHandler(this.felderBearbeitenToolstripMenüItem_Click);
      this.grafikfensterDefinierenToolStripMenuItem.Name = "grafikfensterDefinierenToolStripMenuItem";
      this.grafikfensterDefinierenToolStripMenuItem.Size = new Size(199, 22);
      this.grafikfensterDefinierenToolStripMenuItem.Text = "Grafikfenster definieren";
      this.grafikfensterDefinierenToolStripMenuItem.Click += new EventHandler(this.grafikfensterDefinierenToolStripMenuItem_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new Size(196, 6);
      this.zeigeAllesToolStripMenuItem.Checked = true;
      this.zeigeAllesToolStripMenuItem.CheckState = CheckState.Checked;
      this.zeigeAllesToolStripMenuItem.Name = "zeigeAllesToolStripMenuItem";
      this.zeigeAllesToolStripMenuItem.Size = new Size(199, 22);
      this.zeigeAllesToolStripMenuItem.Text = "Zeige alles";
      this.zeigeAllesToolStripMenuItem.Click += new EventHandler(this.zeigeAllesToolStripMenuItem_Click);
      this.zeigeNurGrafikfensterToolStripMenuItem.Name = "zeigeNurGrafikfensterToolStripMenuItem";
      this.zeigeNurGrafikfensterToolStripMenuItem.Size = new Size(199, 22);
      this.zeigeNurGrafikfensterToolStripMenuItem.Text = "Zeige nur Grafikfenster";
      this.zeigeNurGrafikfensterToolStripMenuItem.Click += new EventHandler(this.zeigeNurGrafikfensterToolStripMenuItem_Click);
      this.lbMissingFieldNames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.lbMissingFieldNames.ContextMenuStrip = this.contextMenuStrip3;
      this.lbMissingFieldNames.FormattingEnabled = true;
      this.helpProvider1.SetHelpString((Control) this.lbMissingFieldNames, "Doppelklick erzeugt Feld mit gewähltem Namen");
      this.lbMissingFieldNames.Location = new Point(15, 288);
      this.lbMissingFieldNames.Name = "lbMissingFieldNames";
      this.helpProvider1.SetShowHelp((Control) this.lbMissingFieldNames, true);
      this.lbMissingFieldNames.Size = new Size(128, 121);
      this.lbMissingFieldNames.Sorted = true;
      this.lbMissingFieldNames.TabIndex = 15;
      this.lbMissingFieldNames.DoubleClick += new EventHandler(this.lbMissingFieldNames_DoubleClick);
      this.lbMissingFieldNames.KeyDown += new KeyEventHandler(this.lbMissingFieldNames_KeyDown);
      this.contextMenuStrip3.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.prüfdateiWählenToolStripMenuItem
      });
      this.contextMenuStrip3.Name = "contextMenuStrip1";
      this.contextMenuStrip3.Size = new Size(167, 26);
      this.prüfdateiWählenToolStripMenuItem.Name = "prüfdateiWählenToolStripMenuItem";
      this.prüfdateiWählenToolStripMenuItem.Size = new Size(166, 22);
      this.prüfdateiWählenToolStripMenuItem.Text = "Prüfdatei wählen";
      this.prüfdateiWählenToolStripMenuItem.Click += new EventHandler(this.prüfdateiWählenToolStripMenuItem_Click);
      this.lblCursorPosition.ForeColor = Color.White;
      this.helpProvider1.SetHelpString((Control) this.lblCursorPosition, "Displaygröße");
      this.lblCursorPosition.Location = new Point(383, 3);
      this.lblCursorPosition.Name = "lblCursorPosition";
      this.helpProvider1.SetShowHelp((Control) this.lblCursorPosition, true);
      this.lblCursorPosition.Size = new Size(151, 15);
      this.lblCursorPosition.TabIndex = 17;
      this.lblCursorPosition.TextAlign = ContentAlignment.MiddleLeft;
      this.btnLoadCSV.Location = new Point(15, 51);
      this.btnLoadCSV.Name = "btnLoadCSV";
      this.btnLoadCSV.Size = new Size(132, 24);
      this.btnLoadCSV.TabIndex = 2;
      this.btnLoadCSV.Text = "Laden";
      this.btnLoadCSV.UseVisualStyleBackColor = true;
      this.btnLoadCSV.Click += new EventHandler(this.btnLoadCSV_Click);
      this.btnSaveCSV.Location = new Point(15, 81);
      this.btnSaveCSV.Name = "btnSaveCSV";
      this.btnSaveCSV.Size = new Size(132, 24);
      this.btnSaveCSV.TabIndex = 3;
      this.btnSaveCSV.Text = "Speichern";
      this.btnSaveCSV.UseVisualStyleBackColor = true;
      this.btnSaveCSV.Click += new EventHandler(this.btnSaveCSV_Click);
      this.btnNewLabel.Location = new Point(15, 250);
      this.btnNewLabel.Name = "btnNewLabel";
      this.btnNewLabel.Size = new Size(129, 24);
      this.btnNewLabel.TabIndex = 5;
      this.btnNewLabel.Text = "Neues Feld";
      this.btnNewLabel.UseVisualStyleBackColor = true;
      this.btnNewLabel.Click += new EventHandler(this.btnNewLabel_Click);
      this.saveFileDialog1.Filter = "Layoutdateien (*.csv)|*.csv|Layoutdatei mit altem Format(*.csv)|*.csv";
      this.saveFileDialog1.FileOk += new CancelEventHandler(this.saveFileDialog1_FileOk);
      this.openFileDialog1.Filter = "Layoutdateien (*.csv)|*.csv|Alle Dateien (*.*)|*.*";
      this.openFileDialog1.InitialDirectory = "c:\\";
      this.openFileDialog1.FileOk += new CancelEventHandler(this.openFileDialog1_FileOk);
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[10]
      {
        (ToolStripItem) this.tsmFont,
        (ToolStripItem) this.farbeToolStripMenuItem,
        (ToolStripItem) this.alternativFarbeToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.textausrichtungToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.kopierenToolStripMenuItem,
        (ToolStripItem) this.einfügenToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.löschenToolStripMenuItem
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(165, 198);
      this.contextMenuStrip1.Closed += new ToolStripDropDownClosedEventHandler(this.contextMenuStrip1_Closed);
      this.farbeToolStripMenuItem.Name = "farbeToolStripMenuItem";
      this.farbeToolStripMenuItem.Size = new Size(164, 22);
      this.farbeToolStripMenuItem.Text = "Farbe";
      this.farbeToolStripMenuItem.Click += new EventHandler(this.farbeToolStripMenuItem_Click);
      this.alternativFarbeToolStripMenuItem.Name = "alternativFarbeToolStripMenuItem";
      this.alternativFarbeToolStripMenuItem.Size = new Size(164, 22);
      this.alternativFarbeToolStripMenuItem.Text = "Alternativ-Farbe";
      this.alternativFarbeToolStripMenuItem.Click += new EventHandler(this.alternativFarbeToolStripMenuItem_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(161, 6);
      this.textausrichtungToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.obenToolStripMenuItem,
        (ToolStripItem) this.mitteToolStripMenuItem,
        (ToolStripItem) this.untenToolStripMenuItem
      });
      this.textausrichtungToolStripMenuItem.Name = "textausrichtungToolStripMenuItem";
      this.textausrichtungToolStripMenuItem.Size = new Size(164, 22);
      this.textausrichtungToolStripMenuItem.Text = "Textausrichtung";
      this.obenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.linksToolStripMenuItem1,
        (ToolStripItem) this.zentriertToolStripMenuItem1,
        (ToolStripItem) this.rechtsToolStripMenuItem1
      });
      this.obenToolStripMenuItem.Name = "obenToolStripMenuItem";
      this.obenToolStripMenuItem.Size = new Size(114, 22);
      this.obenToolStripMenuItem.Text = "Oben";
      this.linksToolStripMenuItem1.Name = "linksToolStripMenuItem1";
      this.linksToolStripMenuItem1.Size = new Size((int) sbyte.MaxValue, 22);
      this.linksToolStripMenuItem1.Text = "Links";
      this.linksToolStripMenuItem1.Click += new EventHandler(this.linksToolStripMenuItem1_Click_1);
      this.zentriertToolStripMenuItem1.Name = "zentriertToolStripMenuItem1";
      this.zentriertToolStripMenuItem1.Size = new Size((int) sbyte.MaxValue, 22);
      this.zentriertToolStripMenuItem1.Text = "Zentriert";
      this.zentriertToolStripMenuItem1.Click += new EventHandler(this.zentriertToolStripMenuItem1_Click_1);
      this.rechtsToolStripMenuItem1.Name = "rechtsToolStripMenuItem1";
      this.rechtsToolStripMenuItem1.Size = new Size((int) sbyte.MaxValue, 22);
      this.rechtsToolStripMenuItem1.Text = "Rechts";
      this.rechtsToolStripMenuItem1.Click += new EventHandler(this.rechtsToolStripMenuItem1_Click_1);
      this.mitteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.linksToolStripMenuItem,
        (ToolStripItem) this.zentriertToolStripMenuItem,
        (ToolStripItem) this.rechtsToolStripMenuItem
      });
      this.mitteToolStripMenuItem.Name = "mitteToolStripMenuItem";
      this.mitteToolStripMenuItem.Size = new Size(114, 22);
      this.mitteToolStripMenuItem.Text = "Mitte";
      this.linksToolStripMenuItem.Name = "linksToolStripMenuItem";
      this.linksToolStripMenuItem.Size = new Size((int) sbyte.MaxValue, 22);
      this.linksToolStripMenuItem.Text = "Links";
      this.linksToolStripMenuItem.Click += new EventHandler(this.linksToolStripMenuItem_Click_1);
      this.zentriertToolStripMenuItem.Name = "zentriertToolStripMenuItem";
      this.zentriertToolStripMenuItem.Size = new Size((int) sbyte.MaxValue, 22);
      this.zentriertToolStripMenuItem.Text = "Zentriert";
      this.zentriertToolStripMenuItem.Click += new EventHandler(this.zentriertToolStripMenuItem_Click_1);
      this.rechtsToolStripMenuItem.Name = "rechtsToolStripMenuItem";
      this.rechtsToolStripMenuItem.Size = new Size((int) sbyte.MaxValue, 22);
      this.rechtsToolStripMenuItem.Text = "Rechts";
      this.rechtsToolStripMenuItem.Click += new EventHandler(this.rechtsToolStripMenuItem_Click_1);
      this.untenToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.linksToolStripMenuItem2,
        (ToolStripItem) this.zentriertToolStripMenuItem2,
        (ToolStripItem) this.rechtsToolStripMenuItem2
      });
      this.untenToolStripMenuItem.Name = "untenToolStripMenuItem";
      this.untenToolStripMenuItem.Size = new Size(114, 22);
      this.untenToolStripMenuItem.Text = "Unten";
      this.linksToolStripMenuItem2.Name = "linksToolStripMenuItem2";
      this.linksToolStripMenuItem2.Size = new Size((int) sbyte.MaxValue, 22);
      this.linksToolStripMenuItem2.Text = "Links";
      this.linksToolStripMenuItem2.Click += new EventHandler(this.linksToolStripMenuItem2_Click);
      this.zentriertToolStripMenuItem2.Name = "zentriertToolStripMenuItem2";
      this.zentriertToolStripMenuItem2.Size = new Size((int) sbyte.MaxValue, 22);
      this.zentriertToolStripMenuItem2.Text = "Zentriert";
      this.zentriertToolStripMenuItem2.Click += new EventHandler(this.zentriertToolStripMenuItem2_Click);
      this.rechtsToolStripMenuItem2.Name = "rechtsToolStripMenuItem2";
      this.rechtsToolStripMenuItem2.Size = new Size((int) sbyte.MaxValue, 22);
      this.rechtsToolStripMenuItem2.Text = "Rechts";
      this.rechtsToolStripMenuItem2.Click += new EventHandler(this.rechtsToolStripMenuItem2_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(161, 6);
      this.kopierenToolStripMenuItem.Name = "kopierenToolStripMenuItem";
      this.kopierenToolStripMenuItem.Size = new Size(164, 22);
      this.kopierenToolStripMenuItem.Text = "Kopieren";
      this.kopierenToolStripMenuItem.Click += new EventHandler(this.kopierenToolStripMenuItem_Click);
      this.einfügenToolStripMenuItem.Name = "einfügenToolStripMenuItem";
      this.einfügenToolStripMenuItem.Size = new Size(164, 22);
      this.einfügenToolStripMenuItem.Text = "Einfügen";
      this.einfügenToolStripMenuItem.Click += new EventHandler(this.einfügenToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(161, 6);
      this.löschenToolStripMenuItem.Name = "löschenToolStripMenuItem";
      this.löschenToolStripMenuItem.Size = new Size(164, 22);
      this.löschenToolStripMenuItem.Text = "Löschen";
      this.löschenToolStripMenuItem.Click += new EventHandler(this.löschenToolStripMenuItem_Click);
      this.textBox1.Location = new Point(15, 142);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(129, 20);
      this.textBox1.TabIndex = 8;
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.textBox1.KeyDown += new KeyEventHandler(this.textBox1_KeyDown);
      this.textBox2.Location = new Point(15, 180);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(129, 20);
      this.textBox2.TabIndex = 9;
      this.textBox2.TextChanged += new EventHandler(this.textBox2_TextChanged);
      this.textBox2.KeyDown += new KeyEventHandler(this.textBox2_KeyDown);
      this.label1.AutoSize = true;
      this.label1.ForeColor = Color.White;
      this.label1.Location = new Point(12, 126);
      this.label1.Name = "label1";
      this.label1.Size = new Size(53, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Feldname";
      this.label2.AutoSize = true;
      this.label2.ForeColor = Color.White;
      this.label2.Location = new Point(12, 169);
      this.label2.Name = "label2";
      this.label2.Size = new Size(28, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Text";
      this.btnNewlayout.Location = new Point(15, 21);
      this.btnNewlayout.Name = "btnNewlayout";
      this.btnNewlayout.Size = new Size(132, 24);
      this.btnNewlayout.TabIndex = 12;
      this.btnNewlayout.Text = "Neues Layout";
      this.btnNewlayout.UseVisualStyleBackColor = true;
      this.btnNewlayout.Click += new EventHandler(this.btnNewlayout_Click);
      this.lblLabelName.Location = new Point(15, 109);
      this.lblLabelName.Name = "lblLabelName";
      this.lblLabelName.Size = new Size(129, 17);
      this.lblLabelName.TabIndex = 13;
      this.lblLabelName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLayoutname.Location = new Point(18, 3);
      this.lblLayoutname.Name = "lblLayoutname";
      this.lblLayoutname.Size = new Size(129, 15);
      this.lblLayoutname.TabIndex = 14;
      this.lblLayoutname.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.AutoSize = true;
      this.label3.ForeColor = Color.White;
      this.label3.Location = new Point(15, 275);
      this.label3.Name = "label3";
      this.label3.Size = new Size(86, 13);
      this.label3.TabIndex = 16;
      this.label3.Text = "Fehlende Felder:";
      this.tsmFont.Name = "tsmFont";
      this.tsmFont.Size = new Size(164, 22);
      this.tsmFont.Text = "Schrift";
      this.tsmFont.Click += new EventHandler(this.tsmFont_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.ControlDark;
      this.ClientSize = new Size(544, 418);
      this.Controls.Add((Control) this.lblCursorPosition);
      this.Controls.Add((Control) this.lbMissingFieldNames);
      this.Controls.Add((Control) this.lblLabelName);
      this.Controls.Add((Control) this.lblLayoutname);
      this.Controls.Add((Control) this.btnNewlayout);
      this.Controls.Add((Control) this.btnSaveCSV);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.btnLoadCSV);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.lblFieldSize);
      this.Controls.Add((Control) this.lblFieldLocation);
      this.Controls.Add((Control) this.btnNewLabel);
      this.Controls.Add((Control) this.lblSCB_Size);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label3);
      this.DoubleBuffered = true;
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.HelpButton = true;
      this.helpProvider1.SetHelpString((Control) this, "Hauptform");
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(550, 450);
      this.Name = nameof (Form1);
      this.helpProvider1.SetShowHelp((Control) this, false);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Layouteditor 2008";
      this.Load += new EventHandler(this.Form1_Load);
      this.Paint += new PaintEventHandler(this.Form1_Paint);
      this.KeyUp += new KeyEventHandler(this.Form1_KeyUp);
      this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);
      this.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
      this.contextMenuStrip2.ResumeLayout(false);
      this.contextMenuStrip3.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum SelectMode
    {
      None,
      MouseSelect,
      DefineGraphicWindow,
    }
  }
}
