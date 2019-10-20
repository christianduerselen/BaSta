using System;
using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class InvokeFunctions
  {
    public void showScbGraphicAsync(
      LED_Board target,
      Image bitmap,
      bool smallgraphic,
      int effectindex)
    {
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.showScbGraphicAsyncDelegate(showScbGraphicAsync), (object) target, (object) bitmap, (object) smallgraphic, (object) effectindex);
      else
        target.ShowGraphic(bitmap, smallgraphic, effectindex);
    }

    public void setListboxIndexAsync(ListBox target, int index)
    {
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setListboxIndexAsyncDelegate(setListboxIndexAsync), (object) target, (object) index);
      else
        target.SelectedIndex = index;
    }

    public void setControlVisibleAsync(Control target, bool visible)
    {
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setControlVisibleAsyncDelegate(setControlVisibleAsync), (object) target, (object) visible);
      else
        target.Visible = visible;
    }

    public void setControlEnabledAsync(Control target, bool enabled)
    {
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setControlEnabledAsyncDelegate(setControlEnabledAsync), (object) target, (object) enabled);
      else
        target.Enabled = enabled;
    }

    public void setProgressBarValueAsync(ProgressBar target, int value)
    {
      if (target.InvokeRequired)
      {
        target.Invoke((Delegate) new InvokeFunctions.setProgressBarValueAsyncDelegate(setProgressBarValueAsync), (object) target, (object) value);
      }
      else
      {
        if (target.Maximum < value)
          value = target.Maximum;
        target.Value = value;
      }
    }

    public int GetTabControlSelectedIndexAsync(TabControl target)
    {
      if (!target.InvokeRequired)
        return target.SelectedIndex;
      target.Invoke((Delegate) new InvokeFunctions.GetTabControlSelectedIndexAsyncDelegate(GetTabControlSelectedIndexAsync), (object) target);
      return 256;
    }

    public Point GetTabControlLocationAsync(TabControl target)
    {
      if (!target.InvokeRequired)
        return target.Location;
      target.Invoke((Delegate) new InvokeFunctions.GetTabControlLocationAsyncDelegate(GetTabControlLocationAsync), (object) target);
      return new Point();
    }

    public void ShowGraphicAsync(LED_Board target, string _filename, bool _graphic_in_window)
    {
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.ShowGraphicAsyncDelegate(ShowGraphicAsync), (object) target, (object) _filename, (object) _graphic_in_window);
      else
        target.ShowGraphic(_filename, _graphic_in_window);
    }

    public void SetPictureBoxImageAsync(PictureBox target, Image image)
    {
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
      else
        target.Image = image;
    }

    public void SetPictureBoxLocationAndSizeAsync(
      Form sourceform,
      PictureBox target,
      Point new_location,
      Size new_size,
      bool bringtofront)
    {
      if (target.InvokeRequired)
      {
        target.Invoke((Delegate) new InvokeFunctions.SetPictureBoxLocationAndSizeAsyncDelegate(SetPictureBoxLocationAndSizeAsync), (object) sourceform, (object) target, (object) new_location, (object) new_size, (object) bringtofront);
      }
      else
      {
        target.Size = new_size;
        int x = new_location.X;
        int y = new_location.Y;
        if (x < 0)
          x = 0;
        if (x + new_size.Width > sourceform.Width)
          x = sourceform.Width - new_size.Width;
        if (y < 0)
          x = 0;
        if (y + new_size.Height > sourceform.Height)
          x = sourceform.Height - new_size.Height;
        target.Location = new Point(x, y);
        if (bringtofront)
          target.BringToFront();
        else
          target.SendToBack();
      }
    }

    public void setCheckBoxCheckedAsync(CheckBox target, bool is_checked)
    {
      if (target == null)
        return;
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setCheckBoxCheckedAsyncDelegate(setCheckBoxCheckedAsync), (object) target, (object) is_checked);
      else
        target.Checked = is_checked;
    }

    public void setCheckBoxTextAsync(CheckBox target, string msg)
    {
      if (target == null)
        return;
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setCheckBoxTextAsyncDelegate(setCheckBoxTextAsync), (object) target, (object) msg);
      else
        target.Text = msg;
    }

    public void setButtonTextAsync(Button target, string msg)
    {
      if (target == null)
        return;
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setButtonTextAsyncDelegate(setButtonTextAsync), (object) target, (object) msg);
      else
        target.Text = msg;
    }

    public void setLabelTextAsync(Label target, string msg)
    {
      if (target == null)
        return;
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setLabelTextAsyncDelegate(setLabelTextAsync), (object) target, (object) msg);
      else
        target.Text = msg;
    }

    public void setLabelColorAsync(Label target, Color color)
    {
      if (target == null)
        return;
      if (target.InvokeRequired)
        target.Invoke((Delegate) new InvokeFunctions.setLabelColorAsyncDelegate(setLabelColorAsync), (object) target, (object) color);
      else
        target.ForeColor = color;
    }

    public void addItemToListBoxAsync(ListBox target, string msg)
    {
      if (target.InvokeRequired)
      {
        target.Invoke((Delegate) new InvokeFunctions.addItemToListBoxAsyncDelegate(addItemToListBoxAsync), (object) target, (object) msg);
      }
      else
      {
        target.Items.Add((object) msg);
        target.SelectedIndex = target.Items.Count - 1;
      }
    }

    public void scbShowScoreAsync(LED_Board target)
    {
      if (target.InvokeRequired)
      {
        target.Invoke((Delegate) new InvokeFunctions.scbShowScoreAsyncDelegate(scbShowScoreAsync), (object) target);
      }
      else
      {
        target.DoShowScore = true;
        target.ShowVideo = false;
        target.HideGraphic();
        target.State = LED_Board.DisplayState.Score;
      }
    }

    public delegate void showScbGraphicAsyncDelegate(
      LED_Board target,
      Image bitmap,
      bool smallgraphic,
      int effectindex);

    public delegate void setListboxIndexAsyncDelegate(ListBox target, int index);

    public delegate void setControlVisibleAsyncDelegate(Control target, bool visible);

    public delegate void setControlEnabledAsyncDelegate(Control target, bool enabled);

    public delegate void setProgressBarValueAsyncDelegate(ProgressBar target, int value);

    public delegate int GetTabControlSelectedIndexAsyncDelegate(TabControl target);

    public delegate Point GetTabControlLocationAsyncDelegate(TabControl target);

    public delegate void ShowGraphicAsyncDelegate(
      LED_Board target,
      string _filename,
      bool _graphic_in_window);

    public delegate void SetPictureBoxImageAsyncDelegate(PictureBox target, Image image);

    public delegate void SetPictureBoxLocationAndSizeAsyncDelegate(
      Form sourceform,
      PictureBox target,
      Point new_location,
      Size new_size,
      bool bringtofront);

    public delegate void setCheckBoxCheckedAsyncDelegate(CheckBox target, bool is_checked);

    public delegate void setCheckBoxTextAsyncDelegate(CheckBox target, string msg);

    public delegate void setButtonTextAsyncDelegate(Button target, string msg);

    public delegate void setLabelTextAsyncDelegate(Label target, string msg);

    public delegate void setLabelColorAsyncDelegate(Label target, Color color);

    public delegate void addItemToListBoxAsyncDelegate(ListBox target, string msg);

    public delegate void scbShowScoreAsyncDelegate(LED_Board target);
  }
}
