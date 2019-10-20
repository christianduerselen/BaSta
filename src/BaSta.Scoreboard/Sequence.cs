using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BaSta.Scoreboard
{
  public class Sequence
  {
    private string _file_name = string.Empty;
    private string _name = string.Empty;
    private List<Sequence.SequencePart> _parts = new List<Sequence.SequencePart>();

    public Sequence()
    {
    }

    public Sequence(string Path, string SequenceFileName, string MediaPath)
    {
      if (!Directory.Exists(Path))
        Directory.CreateDirectory(Path);
      _name = SequenceFileName.Substring(SequenceFileName.LastIndexOf("\\") + 1, SequenceFileName.Length - SequenceFileName.LastIndexOf("\\") - 5);
      _file_name = SequenceFileName;
      Console.WriteLine(_file_name);
      if (File.Exists(_file_name))
      {
        StreamReader streamReader = new StreamReader(SequenceFileName);
        for (string str = streamReader.ReadLine(); str != null; str = streamReader.ReadLine())
        {
          string[] strArray = str.Split(';');
          if (strArray.Length > 5)
            _parts.Add(new Sequence.SequencePart(MediaPath + "\\" + strArray[0], (long) Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), strArray[3] == "1", strArray[4], strArray[5] == "1"));
          else if (strArray.Length > 4)
            _parts.Add(new Sequence.SequencePart(MediaPath + "\\" + strArray[0], (long) Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), strArray[3] == "1", strArray[4], false));
          else if (strArray.Length > 3)
            _parts.Add(new Sequence.SequencePart(MediaPath + "\\" + strArray[0], (long) Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), strArray[3] == "1", string.Empty, false));
          else if (strArray.Length > 2)
            _parts.Add(new Sequence.SequencePart(MediaPath + "\\" + strArray[0], (long) Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]), false, string.Empty, false));
          else if (strArray.Length > 1)
            _parts.Add(new Sequence.SequencePart(MediaPath + "\\" + strArray[0], (long) Convert.ToInt32(strArray[1]), 0, false, string.Empty, false));
        }
        streamReader.Close();
      }
      else
        new StreamWriter(SequenceFileName).Close();
    }

    public string FileName
    {
      get
      {
        return _file_name;
      }
    }

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        _name = value;
      }
    }

    public List<Sequence.SequencePart> Parts
    {
      get
      {
        return _parts;
      }
      set
      {
        _parts = value;
      }
    }

    public bool AddAnimation(string FileName, bool DoPlayAcusticSignal, string ExtraSoundFilename)
    {
      bool flag = false;
      if (File.Exists(FileName))
      {
        Mci mci1 = (Mci) null;
        try
        {
          Mci mci2 = new Mci();
          mci2.Open(FileName);
          int length = mci2.Length;
          if (mci2.IsOpen)
            mci2.Close();
          mci2.Dispose();
          mci1 = (Mci) null;
          _parts.Add(new Sequence.SequencePart(FileName, (long) length, 0, DoPlayAcusticSignal, ExtraSoundFilename, false));
          flag = true;
        }
        catch
        {
          flag = false;
        }
      }
      return flag;
    }

    public bool AddPicture(
      string FileName,
      long Length,
      int EffectIndex,
      bool PlayAcusticSignal,
      string ExtraSoundFilename)
    {
      if (!File.Exists(FileName))
        return false;
      try
      {
        new PictureBox() { Image = Image.FromFile(FileName) }.Dispose();
        _parts.Add(new Sequence.SequencePart(FileName, Length, EffectIndex, PlayAcusticSignal, ExtraSoundFilename, false));
        return true;
      }
      catch
      {
        return false;
      }
    }

    public bool AddScoreDisplay(long Length, bool PlayAcusticSignal, string ExtraSoundFileName)
    {
      _parts.Add(new Sequence.SequencePart("Score", Length, 0, PlayAcusticSignal, ExtraSoundFileName, true));
      return true;
    }

    public void Save(string Path)
    {
      if (!Directory.Exists(Path))
        Directory.CreateDirectory(Path);
      StreamWriter streamWriter = new StreamWriter(Path + "\\" + _name + ".seq", false);
      for (int index = 0; index < _parts.Count; ++index)
        streamWriter.WriteLine(_parts[index].CsvLine);
      streamWriter.Close();
    }

    public void MovePart(int Index, int Direction)
    {
      if (Index <= -1)
        return;
      Sequence.SequencePart part = _parts[Index];
      if (Direction > 0)
      {
        if (Index >= _parts.Count - 1)
          return;
        _parts[Index] = _parts[Index + 1];
        _parts[Index + 1] = part;
      }
      else
      {
        if (Index <= 0)
          return;
        _parts[Index] = _parts[Index - 1];
        _parts[Index - 1] = part;
      }
    }

    public void DeletePart(int Index)
    {
      if (Index <= -1)
        return;
      _parts.RemoveAt(Index);
    }

    public class SequencePart
    {
      private string _filename = string.Empty;
      private string _extra_sound_filename = string.Empty;
      private long _length = long.MaxValue;
      private bool _is_score_display;
      private bool _play_acustic_signal;
      private int _effect_index;

      public bool IsScoreDisplay
      {
        get
        {
          return _is_score_display;
        }
      }

      public bool PlayAcusticSignal
      {
        get
        {
          return _play_acustic_signal;
        }
        set
        {
          _play_acustic_signal = value;
        }
      }

      public SequencePart()
      {
      }

      public SequencePart(
        string FileName,
        long Length,
        int EffectIndex,
        bool PlayAcusticSignal,
        string ExtraSoundFilename,
        bool IsScoreDisplay)
      {
        _filename = FileName;
        _length = Length;
        _effect_index = EffectIndex;
        int num = IsAnimation ? 1 : 0;
        _play_acustic_signal = PlayAcusticSignal;
        _extra_sound_filename = ExtraSoundFilename;
        _is_score_display = IsScoreDisplay;
      }

      public string Filename
      {
        get
        {
          return _filename;
        }
        set
        {
          _filename = value;
          int num = IsAnimation ? 1 : 0;
        }
      }

      public string ExtraSoundFileName
      {
        get
        {
          return _extra_sound_filename;
        }
      }

      public string Name
      {
        get
        {
          if (_filename.Trim() != string.Empty)
            return _filename.Substring(_filename.LastIndexOf("\\") + 1, _filename.Length - _filename.LastIndexOf("\\") - 1);
          return string.Empty;
        }
      }

      public long Length
      {
        get
        {
          return _length;
        }
        set
        {
          if (IsAnimation)
            return;
          _length = value;
        }
      }

      public int EffectIndex
      {
        get
        {
          return _effect_index;
        }
        set
        {
          _effect_index = value;
        }
      }

      public bool IsAnimation
      {
        get
        {
          string upper = _filename.ToUpper();
          if (!upper.EndsWith(".MPG") && !upper.EndsWith(".MPEG") && (!upper.EndsWith(".WMV") && !upper.EndsWith(".AVI")) && (!upper.EndsWith(".MOV") && !upper.EndsWith(".VOB")))
            return upper.EndsWith(".MOV");
          return true;
        }
      }

      public string CsvLine
      {
        get
        {
          if (!File.Exists(_filename) && !_filename.EndsWith("Score"))
            return (string) null;
          string str = _is_score_display ? "1" : "0";
          return Name + ";" + _length.ToString() + ";" + _effect_index.ToString() + ";" + (_play_acustic_signal ? "1" : "0") + ";" + _extra_sound_filename + ";" + str;
        }
      }
    }
  }
}
