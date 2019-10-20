using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace BaSta.Game.Properties
{
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    [CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        public static Settings Default { get; } = (Settings) Synchronized(new Settings());

        [UserScopedSetting]
        [DefaultSettingValue("-1")]
        [DebuggerNonUserCode]
        public int LastGameKindIndex
        {
            get => (int) this[nameof(LastGameKindIndex)];
            set => this[nameof(LastGameKindIndex)] = value;
        }

        [DefaultSettingValue("-1")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public int LastTeamID
        {
            get => (int) this[nameof(LastTeamID)];
            set => this[nameof(LastTeamID)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("C:\\")]
        [DebuggerNonUserCode]
        public string LastLogoFolder
        {
            get => (string) this[nameof(LastLogoFolder)];
            set => this[nameof(LastLogoFolder)] = value;
        }

        [DefaultSettingValue("Game")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public string DatabaseName
        {
            get => (string) this[nameof(DatabaseName)];
            set => this[nameof(DatabaseName)] = value;
        }

        [DefaultSettingValue("Funkwerk_ITK_2008")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public string DatabasePassword
        {
            get => (string) this[nameof(DatabasePassword)];
            set => this[nameof(DatabasePassword)] = value;
        }

        [DefaultSettingValue("")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public string ServerName
        {
            get => (string) this[nameof(ServerName)];
            set => this[nameof(ServerName)] = value;
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        public bool AvailableGamesSortByIncreasingDate
        {
            get => (bool) this[nameof(AvailableGamesSortByIncreasingDate)];
            set => this[nameof(AvailableGamesSortByIncreasingDate)] = value;
        }

        [DefaultSettingValue("0, 0")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public Point StartLocation
        {
            get => (Point) this[nameof(StartLocation)];
            set => this[nameof(StartLocation)] = value;
        }

        [DefaultSettingValue("0, 0")]
        [DebuggerNonUserCode]
        [UserScopedSetting]
        public Point StartLocationBasketball
        {
            get => (Point) this[nameof(StartLocationBasketball)];
            set => this[nameof(StartLocationBasketball)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("1")]
        [DebuggerNonUserCode]
        public int ConsolePort
        {
            get => (int) this[nameof(ConsolePort)];
            set => this[nameof(ConsolePort)] = value;
        }

        [DefaultSettingValue("6")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public decimal MaxPeriodsBasketball
        {
            get => (decimal) this[nameof(MaxPeriodsBasketball)];
            set => this[nameof(MaxPeriodsBasketball)] = value;
        }

        [DefaultSettingValue("30")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public int CountdownMinutesBeforeGame
        {
            get => (int) this[nameof(CountdownMinutesBeforeGame)];
            set => this[nameof(CountdownMinutesBeforeGame)] = value;
        }

        [DefaultSettingValue("8000")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public int UDP_Port
        {
            get => (int) this[nameof(UDP_Port)];
            set => this[nameof(UDP_Port)] = value;
        }

        [DefaultSettingValue("True")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public bool ShowBasketballTimeoutTime
        {
            get => (bool) this[nameof(ShowBasketballTimeoutTime)];
            set => this[nameof(ShowBasketballTimeoutTime)] = value;
        }

        [DefaultSettingValue("False")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public bool NamesToUpper
        {
            get => (bool) this[nameof(NamesToUpper)];
            set => this[nameof(NamesToUpper)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        [DebuggerNonUserCode]
        public bool LAN
        {
            get => (bool) this[nameof(LAN)];
            set => this[nameof(LAN)] = value;
        }

        [DefaultSettingValue("1000, 600")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public Size DefaultSize
        {
            get => (Size) this[nameof(DefaultSize)];
            set => this[nameof(DefaultSize)] = value;
        }
        
        [DefaultSettingValue("3000")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public int HornLength
        {
            get => (int) this[nameof(HornLength)];
            set => this[nameof(HornLength)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("3000")]
        [DebuggerNonUserCode]
        public int ShotclockHornLength
        {
            get => (int) this[nameof(ShotclockHornLength)];
            set => this[nameof(ShotclockHornLength)] = value;
        }

        [DefaultSettingValue("True")]
        [UserScopedSetting]
        [DebuggerNonUserCode]
        public bool BasketballEdgeLightAtShotclockZero
        {
            get => (bool) this[nameof(BasketballEdgeLightAtShotclockZero)];
            set => this[nameof(BasketballEdgeLightAtShotclockZero)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        [UserScopedSetting]
        public bool BasketballEdgeLightSettingsVisible
        {
            get => (bool) this[nameof(BasketballEdgeLightSettingsVisible)];
            set => this[nameof(BasketballEdgeLightSettingsVisible)] = value;
        }

        [DebuggerNonUserCode]
        [DefaultSettingValue("False")]
        [UserScopedSetting]
        public bool BasketballStartShotclockIndependent
        {
            get => (bool) this[nameof(BasketballStartShotclockIndependent)];
            set => this[nameof(BasketballStartShotclockIndependent)] = value;
        }
    }
}