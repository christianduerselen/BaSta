using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  internal class ScoreboardLabel
  {
    private Label _label = new Label();
    private Color _normal_color;
    private Color _alternate_color;

    public ScoreboardLabel()
    {
    }

    public ScoreboardLabel(Label MyLabel, Color AlternateColor)
    {
      _label = MyLabel;
      _normal_color = _label.ForeColor;
      _alternate_color = AlternateColor;
    }

    public Label MyLabel
    {
      get
      {
        return _label;
      }
      set
      {
        _label = value;
      }
    }

    public Color NormalColor
    {
      get
      {
        return _normal_color;
      }
      set
      {
        _normal_color = value;
      }
    }

    public Color AlternateColor
    {
      get
      {
        return _alternate_color;
      }
      set
      {
        _alternate_color = value;
      }
    }
  }
}
