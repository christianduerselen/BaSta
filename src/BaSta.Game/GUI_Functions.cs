using System.Drawing;
using System.Windows.Forms;

namespace BaSta.Game
{
    public class GUI_Functions
    {
        private Color _highlight_color = Color.DimGray;
        private Color _normal_color = Color.DarkGray;
        private Brush _highlight_edge_brush = Brushes.Red;

        public Color HighLightBackColor
        {
            get
            {
                return _highlight_color;
            }
            set
            {
                _highlight_color = value;
            }
        }

        public Color NormalBackColor
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

        public Brush HighLightEdgeColor
        {
            get
            {
                return _highlight_edge_brush;
            }
            set
            {
                _highlight_edge_brush = value;
            }
        }

        public void _show_highlighted_selection(Label[] TargetLabel)
        {
            for (int index = 0; index < TargetLabel.Length; ++index)
            {
                if (TargetLabel[index] != null)
                    TargetLabel[index].BackColor = _highlight_color;
            }
        }

        public void _show_highlighted_selection(Label[] TargetLabel, Color NewBackColor)
        {
            _highlight_color = NewBackColor;
            _show_highlighted_selection(TargetLabel);
        }

        public void _show_highlighted_selection(
          Label[] TargetLabel,
          Color NewBackColor,
          Color OldBackColor)
        {
            _normal_color = OldBackColor;
            _highlight_color = NewBackColor;
            _show_highlighted_selection(TargetLabel);
        }

        public void _reset_highlighted_selection(Label[] TargetLabel)
        {
            for (int index = 0; index < TargetLabel.Length; ++index)
            {
                if (TargetLabel[index] != null)
                    TargetLabel[index].BackColor = _normal_color;
            }
        }

        public void _reset_highlighted_selection(Label[] TargetLabel, Color OldBackColor)
        {
            _normal_color = OldBackColor;
            _reset_highlighted_selection(TargetLabel);
        }

        public void _show_edged_selection(Form TargetForm, Label[] TargetLabel)
        {
            int num1 = 2048;
            int num2 = 2048;
            int num3 = 0;
            int num4 = 0;
            for (int index = 0; index < TargetLabel.Length; ++index)
            {
                if (TargetLabel[index] != null)
                {
                    if (TargetLabel[index].Location.X < num1)
                        num1 = TargetLabel[index].Location.X;
                    if (TargetLabel[index].Location.Y < num2)
                        num2 = TargetLabel[index].Location.Y;
                    if (TargetLabel[index].Location.X + TargetLabel[index].Size.Width > num3)
                        num3 = TargetLabel[index].Location.X + TargetLabel[index].Size.Width;
                    if (TargetLabel[index].Location.Y + TargetLabel[index].Size.Height > num4)
                        num4 = TargetLabel[index].Location.Y + TargetLabel[index].Size.Height;
                }
            }
            TargetForm.CreateGraphics().DrawRectangle(new Pen(_highlight_edge_brush), new Rectangle(new Point(num1 - 1, num2 - 1), new Size(num3 - num1 + 1, num4 - num2 + 1)));
        }

        public void _reset_edged_selection(Form TargetForm)
        {
            TargetForm.CreateGraphics().Clear(TargetForm.BackColor);
        }
    }
}