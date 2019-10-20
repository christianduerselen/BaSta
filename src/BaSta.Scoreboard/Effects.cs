using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class Effects
  {
    public delegate void SetPictureBoxImageAsyncDelegate(PictureBox target, Image image);

    public delegate void DrawPictureBoxImageAsyncDelegate(Image image, Rectangle rectangle);

    public enum FadingEffect
    {
      None,
      Alpha,
      TopDown,
      BottomUp,
      RightToLeft,
      LeftToRight,
      RectangleUpperLeftToLowerRight,
      RectangleUpperRightToLowerLeft,
      RectangleLowerLeftToUpperRight,
      RectangleLowerRightToUpperLeft,
      DiagonalUpperLeftToLowerRight,
      DiagonalUpperRightToLowerLeft,
      DiagonalLowerLeftToUpperRight,
      DiagonalLowerRightToUpperLeft,
      EllipticFromInside,
      EllipticFromOutside,
      RectangleFromInside,
      RectangleFromOutside,
      ZoomIn,
      ZoomOut,
      CurtainDown,
      CurtainUp,
      CurtainRight,
      CurtainLeft,
      SlideDown,
      SlideUp,
      SlideRight,
      SlideLeft,
      Mosaic,
    }

    public delegate void ReadyDelegate(Effects.IEffect sender);

    public delegate void PercentCompletedDelegate(Effects.IEffect sender, int value);

    public interface IEffect
    {
      string Description { get; }

      int Steps { get; set; }

      int Delay { get; set; }

      bool ThreadActive { get; }

      event Effects.ReadyDelegate Ready;

      event Effects.PercentCompletedDelegate PercentComplete;

      void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage);

      void Cancel();

      void Dispose();
    }

    public class None : Effects.IEffect
    {
      private int _steps = 50;
      private Effects.BasicNone _effect = new Effects.BasicNone();
      private int _delay;
      private bool _thread_active;

      public string Description
      {
        get
        {
          return nameof (None);
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _thread_active = true;
        Picturebox.Image = NextImage;
        PercentComplete((Effects.IEffect) this, 100);
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _thread_active = false;
      }
    }

    public class Alpha : Effects.IEffect
    {
      private int _steps = 7;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (Alpha), Effects.FadingStartPositionX.None, Effects.FadingStartPositionY.None, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicAlpha _effect = new Effects.BasicAlpha();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return nameof (Alpha);
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class Up : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("UpFade", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class Down : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("DownFade", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.None, Effects.FadingIncrementY.TopDown);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class Right : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("RightFade", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.None);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class Left : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("LeftFade", Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.None);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class RectangleUpperLeftToLowerRight : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("RectUpperLeftToLowerRight", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.TopDown);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class RectangleUpperRightToLowerLeft : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("RectUpperRightToLowerLeft", Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.TopDown);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class RectangleLowerLeftToUpperRight : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("RectLowerLeftToUpperRight", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _thread_active = false;
        _effect.Dispose();
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class RectangleLowerRightToUpperLeft : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 25;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("RectLowerRightToUpperLeft", Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicRectangleFromEdge _effect = new Effects.BasicRectangleFromEdge();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class DiagonalUpperLeftToLowerRight : Effects.IEffect
    {
      private int _steps = 150;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("DiagUpperLeftToLowerRight", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.TopDown);
      private Effects.BasicDiagonal _effect = new Effects.BasicDiagonal();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _thread_active = false;
        _effect.Dispose();
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class DiagonalUpperRightToLowerLeft : Effects.IEffect
    {
      private int _steps = 150;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("DiagUpperRightToLowerLeft", Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.TopDown);
      private Effects.BasicDiagonal _effect = new Effects.BasicDiagonal();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _thread_active = false;
        _effect.Dispose();
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class DiagonalLowerLeftToUpperRight : Effects.IEffect
    {
      private int _steps = 150;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("DiagLowerLeftToUpperRight", Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicDiagonal _effect = new Effects.BasicDiagonal();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class DiagonalLowerRightToUpperLeft : Effects.IEffect
    {
      private int _steps = 150;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection("DiagLowerRightToUpperLeft", Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicDiagonal _effect = new Effects.BasicDiagonal();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class EllipticFromInside : Effects.IEffect
    {
      private int _steps = 150;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (EllipticFromInside), Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicElliptic _effect = new Effects.BasicElliptic();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, true, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class EllipticFromOutside : Effects.IEffect
    {
      private int _steps = 150;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (EllipticFromOutside), Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicElliptic _effect = new Effects.BasicElliptic();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class RectangleFromInside : Effects.IEffect
    {
      private int _steps = 100;
      private int _delay = 10;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (RectangleFromInside), Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicRectangleFromPoint _effect = new Effects.BasicRectangleFromPoint();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, true, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class RectangleFromOutside : Effects.IEffect
    {
      private int _steps = 100;
      private int _delay = 10;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (RectangleFromOutside), Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicRectangleFromPoint _effect = new Effects.BasicRectangleFromPoint();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class ZoomIn : Effects.IEffect
    {
      private int _steps = 25;
      private int _delay = 15;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (ZoomIn), Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicZoom _effect = new Effects.BasicZoom();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, true, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class ZoomOut : Effects.IEffect
    {
      private int _steps = 25;
      private int _delay = 15;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (ZoomOut), Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicZoom _effect = new Effects.BasicZoom();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class CurtainDown : Effects.IEffect
    {
      private int _steps = 100;
      private int _delay = 2;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (CurtainDown), Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.None, Effects.FadingIncrementX.None, Effects.FadingIncrementY.TopDown);
      private Effects.BasicCurtain _effect = new Effects.BasicCurtain();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class CurtainUp : Effects.IEffect
    {
      private int _steps = 100;
      private int _delay = 2;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (CurtainUp), Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicCurtain _effect = new Effects.BasicCurtain();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class CurtainRight : Effects.IEffect
    {
      private int _steps = 100;
      private int _delay = 2;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (CurtainRight), Effects.FadingStartPositionX.None, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.None);
      private Effects.BasicCurtain _effect = new Effects.BasicCurtain();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class CurtainLeft : Effects.IEffect
    {
      private int _steps = 100;
      private int _delay = 2;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (CurtainLeft), Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.None);
      private Effects.BasicCurtain _effect = new Effects.BasicCurtain();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class SlideDown : Effects.IEffect
    {
      private int _steps = 30;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (SlideDown), Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.None, Effects.FadingIncrementX.None, Effects.FadingIncrementY.TopDown);
      private Effects.BasicSlide _effect = new Effects.BasicSlide();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class SlideUp : Effects.IEffect
    {
      private int _steps = 30;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (SlideUp), Effects.FadingStartPositionX.Left, Effects.FadingStartPositionY.Bottom, Effects.FadingIncrementX.None, Effects.FadingIncrementY.BottomUp);
      private Effects.BasicSlide _effect = new Effects.BasicSlide();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class SlideRight : Effects.IEffect
    {
      private int _steps = 30;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (SlideRight), Effects.FadingStartPositionX.None, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.LeftToRight, Effects.FadingIncrementY.None);
      private Effects.BasicSlide _effect = new Effects.BasicSlide();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class SlideLeft : Effects.IEffect
    {
      private int _steps = 30;
      private int _delay = 5;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (SlideLeft), Effects.FadingStartPositionX.Right, Effects.FadingStartPositionY.Top, Effects.FadingIncrementX.RightToLeft, Effects.FadingIncrementY.None);
      private Effects.BasicSlide _effect = new Effects.BasicSlide();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    public class Mosaic : Effects.IEffect
    {
      private int _steps = 50;
      private int _delay = 10;
      private Effects.FadingDirection _fading_direction = new Effects.FadingDirection(nameof (Mosaic), Effects.FadingStartPositionX.None, Effects.FadingStartPositionY.None, Effects.FadingIncrementX.None, Effects.FadingIncrementY.None);
      private Effects.BasicMosaic _effect = new Effects.BasicMosaic();
      private bool _thread_active;

      public string Description
      {
        get
        {
          return _fading_direction.Title;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public int Delay
      {
        get
        {
          return _delay;
        }
        set
        {
          _delay = value;
        }
      }

      public bool ThreadActive
      {
        get
        {
          return _thread_active;
        }
      }

      public void Fade(PictureBox Picturebox, Image PreviousImage, Image NextImage)
      {
        _effect.BasicEffectReady += new Effects.BasicEffectReadyDelegate(_effect_BasicEffectReady);
        _effect.BasicEffectPercentComplete += new Effects.BasicEffectPercentCompleteDelegate(_effect_BasicEffectPercentComplete);
        _thread_active = true;
        _effect.Fade(Picturebox, PreviousImage, NextImage, _steps, _fading_direction, false, _delay);
      }

      private void _effect_BasicEffectPercentComplete(Effects.IBasicEffect sender, int value)
      {
        PercentComplete((Effects.IEffect) this, value);
      }

      public event Effects.ReadyDelegate Ready;

      public event Effects.PercentCompletedDelegate PercentComplete;

      private void _effect_BasicEffectReady(Effects.IBasicEffect sender)
      {
        Ready((Effects.IEffect) this);
        _thread_active = false;
      }

      public void Cancel()
      {
        _effect.Cancel();
      }

      public void Dispose()
      {
        _effect.Dispose();
        _thread_active = false;
        _fading_direction = (Effects.FadingDirection) null;
        GC.Collect();
      }
    }

    private enum FadingStartPositionX
    {
      None = -1, // 0xFFFFFFFF
      Left = 0,
      Right = 1,
    }

    private enum FadingStartPositionY
    {
      None = -1, // 0xFFFFFFFF
      Top = 0,
      Bottom = 1,
    }

    private enum FadingIncrementX
    {
      RightToLeft = -1, // 0xFFFFFFFF
      None = 0,
      LeftToRight = 1,
    }

    private enum FadingIncrementY
    {
      BottomUp = -1, // 0xFFFFFFFF
      None = 0,
      TopDown = 1,
    }

    private class FadingDirection
    {
      private string _title = string.Empty;
      private Effects.FadingStartPositionX _startpos_X;
      private Effects.FadingStartPositionY _startpos_Y;
      private Effects.FadingIncrementX _incr_X;
      private Effects.FadingIncrementY _incr_Y;

      public string Title
      {
        get
        {
          return _title;
        }
        set
        {
          _title = value;
        }
      }

      public Effects.FadingStartPositionX StartPosX
      {
        get
        {
          return _startpos_X;
        }
        set
        {
          _startpos_X = value;
        }
      }

      public Effects.FadingStartPositionY StartPosY
      {
        get
        {
          return _startpos_Y;
        }
        set
        {
          _startpos_Y = value;
        }
      }

      public Effects.FadingIncrementX IncrX
      {
        get
        {
          return _incr_X;
        }
        set
        {
          _incr_X = value;
        }
      }

      public Effects.FadingIncrementY IncrY
      {
        get
        {
          return _incr_Y;
        }
        set
        {
          _incr_Y = value;
        }
      }

      public FadingDirection()
      {
      }

      public FadingDirection(
        string title,
        Effects.FadingStartPositionX StartPosX,
        Effects.FadingStartPositionY StartPosY,
        Effects.FadingIncrementX Incr_X,
        Effects.FadingIncrementY Incr_Y)
      {
        Title = title;
        _startpos_X = StartPosX;
        _startpos_Y = StartPosY;
        IncrX = Incr_X;
        IncrY = Incr_Y;
      }

      public string tostring()
      {
        return Title;
      }
    }

    private delegate void BasicEffectReadyDelegate(Effects.IBasicEffect sender);

    private delegate void BasicEffectPercentCompleteDelegate(Effects.IBasicEffect sender, int value);

    private class ImageFunctions
    {
      public Image ScaleImage(Image Original, Size Dimension)
      {
        Bitmap bitmap = new Bitmap(Dimension.Width, Dimension.Height);
        Graphics.FromImage((Image) bitmap).DrawImage(Original, 0, 0, Dimension.Width, Dimension.Height);
        return (Image) bitmap;
      }
    }

    private interface IBasicEffect
    {
      PictureBox Picturebox { get; set; }

      Effects.FadingEffect CurrentEffect { get; set; }

      Image PreviousImage { get; set; }

      Image NextImage { get; set; }

      int Counter { get; set; }

      int Steps { get; set; }

      Effects.FadingDirection Direction { get; set; }

      bool FromInside { get; set; }

      int ThreadDelay { get; set; }

      event Effects.BasicEffectReadyDelegate BasicEffectReady;

      event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay);

      void Cancel();

      void Dispose();
    }

    private class BasicNone : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _picturebox = Picturebox;
        SetPictureBoxImageAsync(_picturebox, imageFunctions.ScaleImage(NextImage, Picturebox.Size));
        _previous_image = NextImage;
        if (BasicEffectPercentComplete != null)
          BasicEffectPercentComplete((Effects.IBasicEffect) this, 100);
        if (BasicEffectReady != null)
          BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _previous_image;
      }

      public void Cancel()
      {
      }

      public void Dispose()
      {
      }
    }

    private class BasicAlpha : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.ImageFunctions _img_func = new Effects.ImageFunctions();
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        _previous_image = _img_func.ScaleImage(PreviousImage, _picturebox.Size);
        _next_image = _img_func.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        ImageAttributes imageAttr = new ImageAttributes();
        ColorMatrix newColorMatrix = new ColorMatrix();
        Rectangle rectangle = new Rectangle(new Point(0, 0), _picturebox.Size);
        _picturebox.CreateGraphics();
        Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
        Graphics graphics = Graphics.FromImage((Image) bitmap);
        while (_counter < _steps + 1)
        {
          try
          {
            float num = (float) _counter / (float) _steps;
            newColorMatrix.Matrix33 = num;
            imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(newColorMatrix);
            graphics.DrawImage(_previous_image, rectangle);
            graphics.DrawImage(_next_image, rectangle, 0, 0, _next_image.Width, _next_image.Height, GraphicsUnit.Pixel, imageAttr);
            SetPictureBoxImageAsync(_picturebox, (Image) bitmap);
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
        graphics.Dispose();
        bitmap.Dispose();
        imageAttr.Dispose();
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      private void _fading_thread()
      {
        ImageAttributes imageAttr = new ImageAttributes();
        ColorMatrix newColorMatrix = new ColorMatrix();
        Rectangle rectangle = new Rectangle(new Point(0, 0), _picturebox.Size);
        _picturebox.CreateGraphics();
        Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
        Graphics graphics = Graphics.FromImage((Image) bitmap);
        try
        {
          while (_counter < _steps + 1)
          {
            float num = (float) _counter / (float) _steps;
            newColorMatrix.Matrix33 = num;
            imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(newColorMatrix);
            graphics.DrawImage(_previous_image, rectangle);
            graphics.DrawImage(_next_image, rectangle, 0, 0, _next_image.Width, _next_image.Height, GraphicsUnit.Pixel, imageAttr);
            SetPictureBoxImageAsync(_picturebox, (Image) bitmap);
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
        }
        catch
        {
        }
        graphics.Dispose();
        bitmap.Dispose();
        imageAttr.Dispose();
      }
    }

    private class BasicRectangleFromEdge : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.ImageFunctions _img_func = new Effects.ImageFunctions();
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        _previous_image = _img_func.ScaleImage(PreviousImage, _picturebox.Size);
        SetPictureBoxImageAsync(_picturebox, _previous_image);
        _next_image = _img_func.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        int num1 = (int) Math.Abs((sbyte) _direction.IncrX);
        int num2 = (int) Math.Abs((sbyte) _direction.IncrY);
        Rectangle rectangle = new Rectangle(new Point(0, 0), _picturebox.Size);
        Graphics graphics = _picturebox.CreateGraphics();
        while (_counter < _steps + 1)
        {
          try
          {
            int width = _counter * rectangle.Width / _steps * num1 + (1 - num1) * rectangle.Width;
            int height = _counter * rectangle.Height / _steps * num2 + (1 - num2) * rectangle.Height;
            Rectangle rect = new Rectangle((int) _direction.StartPosX * (rectangle.Width - width) * num1, (int) _direction.StartPosY * (rectangle.Height - height) * num2, width, height);
            graphics.SetClip(rect);
            graphics.DrawImage(_next_image, new Rectangle(new Point(0, 0), _picturebox.Size));
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
            BasicEffectPercentComplete((Effects.IBasicEffect) this, _counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }
    }

    private class BasicDiagonal : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _previous_image = imageFunctions.ScaleImage(PreviousImage, _picturebox.Size);
        _picturebox.Image = _previous_image;
        _next_image = imageFunctions.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        Rectangle rect = new Rectangle(new Point(0, 0), _picturebox.Size);
        Graphics graphics = _picturebox.CreateGraphics();
        while (_counter < _steps + 1)
        {
          try
          {
            int num1 = _counter * rect.Width / _steps * 2;
            int num2 = _counter * rect.Height / _steps * 2;
            int x1 = (int) _direction.StartPosX * rect.Width;
            int x2 = (int) _direction.StartPosX * rect.Width + (int) _direction.IncrX * num1;
            int y1 = (int) _direction.StartPosY * rect.Height;
            int y2 = (int) _direction.StartPosY * rect.Height + (int) _direction.IncrY * num2;
            Point point1 = new Point(x1, y1);
            Point point2 = new Point(x2, y1);
            Point point3 = new Point(x1, y2);
            GraphicsPath path = new GraphicsPath();
            path.AddLine(point1, point2);
            path.AddLine(point2, point3);
            path.AddLine(point3, point1);
            graphics.SetClip(path);
            graphics.DrawImage(_next_image, rect);
            path.Dispose();
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }
    }

    private class BasicElliptic : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _previous_image = imageFunctions.ScaleImage(PreviousImage, _picturebox.Size);
        SetPictureBoxImageAsync(_picturebox, _previous_image);
        _next_image = imageFunctions.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        Rectangle rect = new Rectangle(new Point(0, 0), _picturebox.Size);
        int num1 = rect.Width / 2;
        int num2 = rect.Height / 2;
        Region region = new Region();
        while (_counter < _steps + 1)
        {
          try
          {
            GraphicsPath path = new GraphicsPath();
            Graphics graphics = _picturebox.CreateGraphics();
            if (_from_inside)
            {
              int num3 = rect.Width * _counter / _steps;
              int num4 = rect.Height * _counter / _steps;
              path.AddEllipse(num1 - num3, num2 - num4, num3 * 2, num4 * 2);
              graphics.SetClip(path);
            }
            else
            {
              int num3 = rect.Width * (_steps - _counter) / _steps;
              int num4 = rect.Height * (_steps - _counter) / _steps;
              path.AddEllipse(num1 - num3, num2 - num4, num3 * 2, num4 * 2);
              region = new Region(path);
              graphics.ExcludeClip(region);
            }
            graphics.DrawImage(_next_image, rect);
            region.Dispose();
            path.Dispose();
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }
    }

    private class BasicCurtain : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _previous_image = imageFunctions.ScaleImage(PreviousImage, _picturebox.Size);
        SetPictureBoxImageAsync(_picturebox, _previous_image);
        _next_image = imageFunctions.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        int num1 = Math.Abs((int) _direction.IncrX);
        int num2 = Math.Abs((int) _direction.IncrY);
        Rectangle rectangle1 = new Rectangle(new Point(0, 0), _picturebox.Size);
        int width = rectangle1.Width;
        int height = rectangle1.Height;
        Rectangle rectangle2 = new Rectangle();
        while (_counter < _steps + 1)
        {
          try
          {
            int num3 = _counter * rectangle1.Width / _steps;
            int num4 = _counter * rectangle1.Height / _steps;
            _picturebox.CreateGraphics().DrawImage(_next_image, new Rectangle((int) _direction.StartPosX * (rectangle1.Width - num3) * num1, (int) _direction.StartPosY * (rectangle1.Height - num4) * num2, width, height));
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }
    }

    private class BasicSlide : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.ImageFunctions _img_func = new Effects.ImageFunctions();
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        _previous_image = _img_func.ScaleImage(PreviousImage, _picturebox.Size);
        _next_image = _img_func.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        int num1 = Math.Abs((int) _direction.IncrX);
        int num2 = Math.Abs((int) _direction.IncrY);
        Rectangle rectangle = new Rectangle(new Point(0, 0), _picturebox.Size);
        int width = rectangle.Width;
        int height = rectangle.Height;
        Graphics graphics1 = _picturebox.CreateGraphics();
        Graphics graphics2 = (Graphics) null;
        while (_counter < _steps + 1)
        {
          try
          {
            int num3 = _counter * rectangle.Width / _steps;
            int num4 = _counter * rectangle.Height / _steps;
            Rectangle rect1 = new Rectangle((int) _direction.StartPosX * (rectangle.Width - num3) * num1, (int) _direction.StartPosY * (rectangle.Height - num4) * num2, width, height);
            Rectangle rect2 = new Rectangle(num3 * (int) _direction.IncrX, num4 * (int) _direction.IncrY, width, height);
            Bitmap bitmap = new Bitmap(width, height);
            graphics2 = Graphics.FromImage((Image) bitmap);
            graphics2.DrawImage(_previous_image, rect2);
            graphics2.DrawImage(_next_image, rect1);
            graphics1.DrawImage((Image) bitmap, new Point(0, 0));
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
        graphics2.Dispose();
        graphics2.Dispose();
        GC.Collect();
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }

      private void _fading_thread()
      {
      }
    }

    private class BasicRectangleFromPoint : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _previous_image = imageFunctions.ScaleImage(PreviousImage, _picturebox.Size);
        _next_image = imageFunctions.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        SetPictureBoxImageAsync(_picturebox, _previous_image);
        Rectangle rect = new Rectangle(new Point(0, 0), _picturebox.Size);
        int num1 = rect.Width / 2;
        int num2 = rect.Height / 2;
        while (_counter < _steps + 1)
        {
          try
          {
            GraphicsPath path = new GraphicsPath();
            Graphics graphics = _picturebox.CreateGraphics();
            if (_from_inside)
            {
              int num3 = rect.Width * _counter / _steps;
              int num4 = rect.Height * _counter / _steps;
              path.AddRectangle(new Rectangle(num1 - num3, num2 - num4, num3 * 2, num4 * 2));
              graphics.SetClip(path);
            }
            else
            {
              int num3 = rect.Width * (_steps - _counter) / _steps;
              int num4 = rect.Height * (_steps - _counter) / _steps;
              path.AddRectangle(new Rectangle(num1 - num3, num2 - num4, num3 * 2, num4 * 2));
              Region region = new Region(path);
              graphics.ExcludeClip(region);
              region.Dispose();
            }
            graphics.DrawImage(_next_image, rect);
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }
    }

    private class BasicZoom : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _previous_image = imageFunctions.ScaleImage(PreviousImage, _picturebox.Size);
        _next_image = imageFunctions.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        GraphicsPath graphicsPath = new GraphicsPath();
        Rectangle rect1 = new Rectangle(new Point(0, 0), _picturebox.Size);
        int num1 = rect1.Width / 2;
        int num2 = rect1.Height / 2;
        Bitmap bitmap = new Bitmap(rect1.Width, rect1.Height);
        Graphics graphics1 = (Graphics) null;
        Graphics graphics2 = (Graphics) null;
        while (_counter < _steps + 1)
        {
          try
          {
            graphics2 = Graphics.FromImage((Image) bitmap);
            graphics1 = _picturebox.CreateGraphics();
            if (_from_inside)
            {
              int width = rect1.Width * _counter / _steps;
              int height = rect1.Height * _counter / _steps;
              Rectangle rect2 = new Rectangle(num1 - width / 2, num2 - height / 2, width, height);
              graphics2.DrawImage(_previous_image, rect1);
              graphics2.DrawImage(_next_image, rect2);
            }
            else
            {
              int width = rect1.Width * (_steps - _counter) / _steps;
              int height = rect1.Height * (_steps - _counter) / _steps;
              Rectangle rect2 = new Rectangle(num1 - width / 2, num2 - height / 2, width, height);
              graphics2.DrawImage(_next_image, rect1);
              graphics2.DrawImage(_previous_image, rect2);
            }
            SetPictureBoxImageAsync(_picturebox, (Image) bitmap);
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
            graphics2.Dispose();
            graphics1.Dispose();
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
        graphics1.Dispose();
        graphics2.Dispose();
        bitmap.Dispose();
        GC.Collect();
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }
    }

    private class BasicMosaic : Effects.IBasicEffect
    {
      private PictureBox _picturebox = new PictureBox();
      private Effects.FadingDirection _direction = new Effects.FadingDirection();
      private int _thread_delay = 10;
      private ArrayList ClipRectangles = new ArrayList();
      private Region ClipRgn = new Region();
      private Random RD = new Random();
      private int _fieldwidth = 40;
      private int _fieldheight = 40;
      private Effects.FadingEffect _fading_effect;
      private Image _previous_image;
      private Image _next_image;
      private int _counter;
      private int _steps;
      private bool _from_inside;
      private BackgroundWorker _fade;
      private int _fieldsPerStep;

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

      public Effects.FadingEffect CurrentEffect
      {
        get
        {
          return _fading_effect;
        }
        set
        {
          _fading_effect = value;
        }
      }

      public Image PreviousImage
      {
        get
        {
          return _previous_image;
        }
        set
        {
          _previous_image = value;
        }
      }

      public Image NextImage
      {
        get
        {
          return _next_image;
        }
        set
        {
          _next_image = value;
        }
      }

      public int Counter
      {
        get
        {
          return _counter;
        }
        set
        {
          _counter = value;
        }
      }

      public int Steps
      {
        get
        {
          return _steps;
        }
        set
        {
          _steps = value;
        }
      }

      public Effects.FadingDirection Direction
      {
        get
        {
          return _direction;
        }
        set
        {
          _direction = value;
        }
      }

      public bool FromInside
      {
        get
        {
          return _from_inside;
        }
        set
        {
          _from_inside = value;
        }
      }

      public int ThreadDelay
      {
        get
        {
          return _thread_delay;
        }
        set
        {
          _thread_delay = value;
        }
      }

      public event Effects.BasicEffectReadyDelegate BasicEffectReady;

      public event Effects.BasicEffectPercentCompleteDelegate BasicEffectPercentComplete;

      public void Fade(
        PictureBox Picturebox,
        Image PreviousImage,
        Image NextImage,
        int Steps,
        Effects.FadingDirection Direction,
        bool FromInside,
        int ThreadDelay)
      {
        _picturebox = Picturebox;
        Effects.ImageFunctions imageFunctions = new Effects.ImageFunctions();
        _previous_image = imageFunctions.ScaleImage(PreviousImage, _picturebox.Size);
        _next_image = imageFunctions.ScaleImage(NextImage, _picturebox.Size);
        _counter = 0;
        _steps = Steps;
        _direction = Direction;
        _from_inside = FromInside;
        _thread_delay = ThreadDelay;
        _fade = new BackgroundWorker();
        _fade.WorkerReportsProgress = true;
        _fade.WorkerSupportsCancellation = true;
        _fade.DoWork += new DoWorkEventHandler(_fade_DoWork);
        _fade.ProgressChanged += new ProgressChangedEventHandler(_fade_ProgressChanged);
        _fade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
        _fade.RunWorkerAsync();
      }

      private void _fade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        BasicEffectReady((Effects.IBasicEffect) this);
        _picturebox.Image = _next_image;
      }

      private void _fade_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        BasicEffectPercentComplete((Effects.IBasicEffect) this, e.ProgressPercentage);
      }

      private void _fade_DoWork(object sender, DoWorkEventArgs e)
      {
        SetPictureBoxImageAsync(_picturebox, _previous_image);
        Rectangle rect = new Rectangle(new Point(0, 0), _picturebox.Size);
        while (_counter < _steps + 1)
        {
          try
          {
            if (_counter == 0)
            {
              for (int y = 0; y < rect.Height; y += _fieldheight)
              {
                for (int x = 0; x < rect.Width; x += _fieldwidth)
                  ClipRectangles.Add((object) new Rectangle(x, y, _fieldwidth, _fieldheight));
              }
              _fieldsPerStep = ClipRectangles.Count / _steps + 1;
              ClipRgn.MakeEmpty();
            }
            for (int index1 = 1; index1 < _fieldsPerStep; ++index1)
            {
              if (ClipRectangles.Count > 0)
              {
                int index2 = RD.Next(ClipRectangles.Count);
                ClipRgn.Union((Rectangle) ClipRectangles[index2]);
                ClipRectangles.RemoveAt(index2);
              }
            }
            Graphics graphics = _picturebox.CreateGraphics();
            graphics.Clip = ClipRgn;
            graphics.DrawImage(_next_image, rect);
            Thread.Sleep(_thread_delay);
            ++_counter;
            _fade.ReportProgress(_counter * 100 / _steps);
            BasicEffectPercentComplete((Effects.IBasicEffect) this, _counter * 100 / _steps);
          }
          catch
          {
          }
        }
        SetPictureBoxImageAsync(_picturebox, _next_image);
        _previous_image = _next_image;
      }

      public void Cancel()
      {
        if (_fade != null)
        {
          _fade.CancelAsync();
          _fade.DoWork -= new DoWorkEventHandler(_fade_DoWork);
          _fade.ProgressChanged -= new ProgressChangedEventHandler(_fade_ProgressChanged);
          _fade.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_fade_RunWorkerCompleted);
          _fade.Dispose();
          _fade = (BackgroundWorker) null;
        }
        BasicEffectReady((Effects.IBasicEffect) this);
      }

      public void Dispose()
      {
        if (_fade == null)
          return;
        _fade.CancelAsync();
        _fade.Dispose();
      }

      public Size MosaicFieldSize
      {
        get
        {
          return new Size(_fieldwidth, _fieldheight);
        }
        set
        {
          _fieldwidth = value.Width;
          _fieldheight = value.Height;
        }
      }

      private void SetPictureBoxImageAsync(PictureBox target, Image image)
      {
        if (target.InvokeRequired)
          target.Invoke((Delegate) new Effects.SetPictureBoxImageAsyncDelegate(SetPictureBoxImageAsync), (object) target, (object) image);
        else
          target.Image = image;
      }
    }
  }
}
