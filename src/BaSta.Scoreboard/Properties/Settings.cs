using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BaSta.Scoreboard.Properties
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = (Settings) Synchronized(new Settings());

        [DefaultSettingValue("(local)")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public string ServerName
        {
            get => (string) this[nameof(ServerName)];
            set => this[nameof(ServerName)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("Game")]
        [UserScopedSetting]
        public string DatabaseName
        {
            get => (string) this[nameof(DatabaseName)];
            set => this[nameof(DatabaseName)] = value;
        }

        [DebuggerNonUserCode]
        [UserScopedSetting]
        [DefaultSettingValue("Funkwerk_ITK_2008")]
        public string DatabasePassword
        {
            get => (string) this[nameof(DatabasePassword)];
            set => this[nameof(DatabasePassword)] = value;
        }

        [DefaultSettingValue("0, 0")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public Point StartLocation
        {
            get => (Point) this[nameof(StartLocation)];
            set => this[nameof(StartLocation)] = value;
        }

        [DefaultSettingValue("c:\\")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public string SelectedPicturePath
        {
            get => (string) this[nameof(SelectedPicturePath)];
            set => this[nameof(SelectedPicturePath)] = value;
        }

        [DefaultSettingValue("0")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public int VideoSource
        {
            get => (int) this[nameof(VideoSource)];
            set => this[nameof(VideoSource)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("c:\\")]
        [DebuggerNonUserCode]
        public string SelectedAnimationPath
        {
            get => (string) this[nameof(SelectedAnimationPath)];
            set => this[nameof(SelectedAnimationPath)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("0")]
        [DebuggerNonUserCode]
        public int LastSelectedEffect
        {
            get => (int) this[nameof(LastSelectedEffect)];
            set => this[nameof(LastSelectedEffect)] = value;
        }

        [DefaultSettingValue("-1")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public int LastSelectedKindOfGameIndex
        {
            get => (int) this[nameof(LastSelectedKindOfGameIndex)];
            set => this[nameof(LastSelectedKindOfGameIndex)] = value;
        }

        [DebuggerNonUserCode]
        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public bool ShowGraphicInFullscreen
        {
            get => (bool) this[nameof(ShowGraphicInFullscreen)];
            set => this[nameof(ShowGraphicInFullscreen)] = value;
        }

        [DefaultSettingValue("")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public string LastText
        {
            get => (string) this[nameof(LastText)];
            set => this[nameof(LastText)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("Black")]
        [DebuggerNonUserCode]
        public Color LastBackColor
        {
            get => (Color) this[nameof(LastBackColor)];
            set => this[nameof(LastBackColor)] = value;
        }

        [DefaultSettingValue("White")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public Color LastForeColor
        {
            get => (Color) this[nameof(LastForeColor)];
            set => this[nameof(LastForeColor)] = value;
        }

        [DebuggerNonUserCode]
        [UserScopedSetting]
        [DefaultSettingValue("Microsoft Sans Serif, 8.25pt")]
        public Font LastFont
        {
            get => (Font) this[nameof(LastFont)];
            set => this[nameof(LastFont)] = value;
        }

        [DefaultSettingValue("True")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public bool AlignLeft
        {
            get => (bool) this[nameof(AlignLeft)];
            set => this[nameof(AlignLeft)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        [UserScopedSetting]
        public bool AlignRight
        {
            get => (bool) this[nameof(AlignRight)];
            set => this[nameof(AlignRight)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        [DebuggerNonUserCode]
        public bool AlignCenter
        {
            get => (bool) this[nameof(AlignCenter)];
            set => this[nameof(AlignCenter)] = value;
        }

        [DefaultSettingValue("Left")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public HorizontalAlignment LastTextAlignement
        {
            get => (HorizontalAlignment) this[nameof(LastTextAlignement)];
            set => this[nameof(LastTextAlignement)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        [UserScopedSetting]
        public string LastTextFile
        {
            get => (string) this[nameof(LastTextFile)];
            set => this[nameof(LastTextFile)] = value;
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool StretchVideoToDisplaySize
        {
            get => (bool) this[nameof(StretchVideoToDisplaySize)];
            set => this[nameof(StretchVideoToDisplaySize)] = value;
        }

        [DefaultSettingValue("False")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public bool AllowGraphicFromLAN
        {
            get => (bool) this[nameof(AllowGraphicFromLAN)];
            set => this[nameof(AllowGraphicFromLAN)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        [DebuggerNonUserCode]
        public string GoalSequenceName
        {
            get => (string) this[nameof(GoalSequenceName)];
            set => this[nameof(GoalSequenceName)] = value;
        }

        [DebuggerNonUserCode]
        [UserScopedSetting]
        [DefaultSettingValue("0, 0")]
        public Point DefaultGraphicWindowLocation
        {
            get => (Point) this[nameof(DefaultGraphicWindowLocation)];
            set => this[nameof(DefaultGraphicWindowLocation)] = value;
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("400, 300")]
        public Size DefaultGraphicWindowSize
        {
            get => (Size) this[nameof(DefaultGraphicWindowSize)];
            set => this[nameof(DefaultGraphicWindowSize)] = value;
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("400, 300")]
        public Size SCB_Size
        {
            get => (Size) this[nameof(SCB_Size)];
            set => this[nameof(SCB_Size)] = value;
        }

        [DefaultSettingValue("c:\\")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public string SelectedPPT_Path
        {
            get => (string) this[nameof(SelectedPPT_Path)];
            set => this[nameof(SelectedPPT_Path)] = value;
        }

        [DefaultSettingValue("0")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public int LastSelectedTabIndex
        {
            get => (int) this[nameof(LastSelectedTabIndex)];
            set => this[nameof(LastSelectedTabIndex)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("")]
        [UserScopedSetting]
        public string TextBackgroundFileName
        {
            get => (string) this[nameof(TextBackgroundFileName)];
            set => this[nameof(TextBackgroundFileName)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("-1")]
        [DebuggerNonUserCode]
        public int LastSelectedSequenceIndex
        {
            get => (int) this[nameof(LastSelectedSequenceIndex)];
            set => this[nameof(LastSelectedSequenceIndex)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("Arial, 15.75pt")]
        [DebuggerNonUserCode]
        public Font GameTimeInsertFont
        {
            get => (Font) this[nameof(GameTimeInsertFont)];
            set => this[nameof(GameTimeInsertFont)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("Red")]
        [UserScopedSetting]
        public Color GameTimeInsertColor
        {
            get => (Color) this[nameof(GameTimeInsertColor)];
            set => this[nameof(GameTimeInsertColor)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("0, 0")]
        [UserScopedSetting]
        public Point GameTimeInsertLocation
        {
            get => (Point) this[nameof(GameTimeInsertLocation)];
            set => this[nameof(GameTimeInsertLocation)] = value;
        }

        [DefaultSettingValue("False")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public bool DoInsertgameTime
        {
            get => (bool) this[nameof(DoInsertgameTime)];
            set => this[nameof(DoInsertgameTime)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("Arial, 15.75pt")]
        [DebuggerNonUserCode]
        public Font DayTimeInsertFont
        {
            get => (Font) this[nameof(DayTimeInsertFont)];
            set => this[nameof(DayTimeInsertFont)] = value;
        }

        [DefaultSettingValue("Red")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public Color DayTimeInsertColor
        {
            get => (Color) this[nameof(DayTimeInsertColor)];
            set => this[nameof(DayTimeInsertColor)] = value;
        }

        [DebuggerNonUserCode]
        [UserScopedSetting]
        [DefaultSettingValue("0, 0")]
        public Point DayTimeInsertLocation
        {
            get => (Point) this[nameof(DayTimeInsertLocation)];
            set => this[nameof(DayTimeInsertLocation)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        [UserScopedSetting]
        public bool DoInsertDayTime
        {
            get => (bool) this[nameof(DoInsertDayTime)];
            set => this[nameof(DoInsertDayTime)] = value;
        }
    }
}