using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  internal class Viewer
  {
    private InvokeFunctions _invokes = new InvokeFunctions();
    private Form _source_form = new Form();
    private PictureBox _picturebox = new PictureBox();
    private string _default_media_filename = string.Empty;
    private System.Windows.Forms.Timer _media_timer = new System.Windows.Forms.Timer();
    private Point _original_position = new Point();
    private Size _original_size = new Size();
    private bool _initial_start = true;
    private const int _acceleration = 2;
    private bool _is_preview;
    private bool _picturebox_selected;
    private Viewer.PositionToKeep _base_position;
    private bool _picturebox_accelerated;
    private int _remaining_media_time;
    private bool _do_start;
    private bool _is_started;
    private Image _thumbnail;
    private Mci _player;
    private Thread _mouseposition_thread;
    private Thread _acceleration_thread;

    public event Viewer.MediaEndReachedDelegate MediaEndTimeReached;

    public event Viewer.MediaTimeChangedDelegate MediaTimeChanged;

    public event Viewer.SelectEventDelegate MediumSelected;

    public bool IsAnimation
    {
      get
      {
        return _player != null;
      }
    }

    public PictureBox Picturebox
    {
      get
      {
        return _picturebox;
      }
      set
      {
        _picturebox = value;
      }
    }

    public string DefaultMediaFileName
    {
      get
      {
        return _default_media_filename;
      }
      set
      {
        _default_media_filename = value;
        try
        {
          _invokes.SetPictureBoxImageAsync(_picturebox, Image.FromFile(_default_media_filename));
        }
        catch
        {
          try
          {
            _player = new Mci();
            _player.Open(_default_media_filename);
            _player.SetRectangle(0, 0, _picturebox.Width, _picturebox.Height);
          }
          catch
          {
            _player = (Mci) null;
          }
        }
      }
    }

    public bool IsPreview
    {
      get
      {
        return _is_preview;
      }
    }

    public bool PictureBoxSelected
    {
      get
      {
        return _picturebox_selected;
      }
      set
      {
        _picturebox_selected = value;
      }
    }

    public int RemainingMediaTime
    {
      get
      {
        return _remaining_media_time;
      }
      set
      {
        _remaining_media_time = value;
      }
    }

    public Viewer(string DefaultMediaName)
    {
      _default_media_filename = DefaultMediaName;
    }

    public Viewer(
      Form SourceForm,
      PictureBox Picturebox,
      string DefaultMediaFileName,
      bool IsPreview,
      Viewer.PositionToKeep BasePosition)
    {
      _base_position = BasePosition;
      _is_preview = IsPreview;
      _source_form = SourceForm;
      _picturebox = Picturebox;
      _original_position = _picturebox.Location;
      _original_size = _picturebox.Size;
      _default_media_filename = DefaultMediaFileName;
      _thumbnail = (Image) null;
      try
      {
        _thumbnail = Image.FromFile(_default_media_filename);
        _picturebox.Image = _thumbnail;
      }
      catch
      {
      }
      if (_is_preview)
      {
        _mouseposition_thread = new Thread(new ThreadStart(_get_mouseposition));
        _mouseposition_thread.IsBackground = true;
        _mouseposition_thread.Start();
        if (_base_position != Viewer.PositionToKeep.None)
        {
          _acceleration_thread = new Thread(new ThreadStart(ChangePictureboxSize));
          _acceleration_thread.IsBackground = true;
          _acceleration_thread.Start();
        }
      }
      _do_start = true;
      _remaining_media_time = 0;
      _media_timer.Interval = 1;
      _media_timer.Tick += new EventHandler(_media_timer_Tick);
      _media_timer.Start();
    }

    ~Viewer()
    {
      Dispose();
    }

    public void Dispose()
    {
      _media_timer.Stop();
      if (_player != null)
      {
        _player.Stop();
        _player = (Mci) null;
      }
      _is_preview = false;
    }

    private void _media_timer_Tick(object sender, EventArgs e)
    {
      if (_do_start)
      {
        if (_player == null)
        {
          try
          {
            _invokes.SetPictureBoxImageAsync(_picturebox, Image.FromFile(_default_media_filename));
          }
          catch
          {
            try
            {
              _player = new Mci();
              _player.Open(_default_media_filename, (Control) _picturebox);
              _player.SetRectangle(0, 0, _picturebox.Width, _picturebox.Height);
              _player.Volume = 0;
              if (!_initial_start)
                _remaining_media_time = _player.Length / 50;
              _initial_start = false;
              _player.Play(true);
            }
            catch
            {
              _player = (Mci) null;
            }
          }
        }
        _do_start = false;
        _is_started = true;
      }
      if (_remaining_media_time > 0 && _player != null)
        --_remaining_media_time;
      if (_remaining_media_time == 0)
      {
        if (MediaEndTimeReached != null)
          MediaEndTimeReached(this);
        if (_player == null)
          return;
        _player.SetRectangle(0, 0, _picturebox.Width, _picturebox.Height);
        _player.Stop();
        _player.Dispose();
        _player = (Mci) null;
        _is_started = false;
      }
      else
      {
        if (MediaTimeChanged == null)
          return;
        MediaTimeChanged(this, _remaining_media_time);
      }
    }

    private void _get_mouseposition()
    {
      Rectangle rectangle = new Rectangle(_picturebox.Location, _picturebox.Size);
      bool flag = false;
      while (_is_preview)
      {
        Application.DoEvents();
        try
        {
          Rectangle rect = new Rectangle(_source_form.PointToClient(Control.MousePosition), new Size(1, 1));
          _picturebox_selected = rectangle.IntersectsWith(rect);
          MouseButtons mouseButtons = Control.MouseButtons;
          if (!flag && _picturebox_selected && mouseButtons == MouseButtons.Left)
          {
            MediumSelected(this, _default_media_filename);
            flag = true;
          }
          else if (mouseButtons == MouseButtons.None)
            flag = false;
        }
        catch
        {
        }
        Thread.Sleep(10);
      }
    }

    public void ChangePictureboxSize()
    {
      Size originalSize = _original_size;
      Point originalPosition = _original_position;
      while (_is_preview)
      {
        try
        {
          if (_picturebox_selected)
          {
            if (!_picturebox_accelerated)
            {
              Size new_size = new Size(_original_size.Width * 2, _original_size.Height * 2);
              Point new_location;
              switch (_base_position)
              {
                case Viewer.PositionToKeep.Upper:
                  new_location = new Point(_original_position.X - _original_size.Width / 2, _original_position.Y);
                  break;
                case Viewer.PositionToKeep.Lower:
                  new_location = new Point(_original_position.X - _original_size.Width / 2, _original_position.Y - _original_size.Height);
                  break;
                case Viewer.PositionToKeep.Left:
                  new_location = new Point(_original_position.X, _original_position.Y - _original_size.Height / 2);
                  break;
                case Viewer.PositionToKeep.Right:
                  new_location = new Point(_original_position.X - _original_size.Width, _original_position.Y - _original_size.Height / 2);
                  break;
                default:
                  new_location = _original_position;
                  break;
              }
              _invokes.SetPictureBoxLocationAndSizeAsync(_source_form, _picturebox, new_location, new_size, true);
              _picturebox_accelerated = true;
            }
            else if (!_is_started)
              _do_start = true;
          }
          else
          {
            if (!_initial_start)
              _remaining_media_time = 0;
            if (_picturebox_accelerated)
            {
              _remaining_media_time = 0;
              _invokes.SetPictureBoxLocationAndSizeAsync(_source_form, _picturebox, _original_position, _original_size, false);
              _picturebox_accelerated = false;
              _is_started = false;
            }
          }
        }
        catch
        {
        }
        Thread.Sleep(50);
      }
    }

    public delegate void MediaEndReachedDelegate(Viewer sender);

    public delegate void MediaTimeChangedDelegate(Viewer sender, int remaining_time);

    public delegate void SelectEventDelegate(Viewer sender, string filename);

    public enum PositionToKeep
    {
      None,
      Upper,
      Lower,
      Left,
      Right,
    }
  }
}
