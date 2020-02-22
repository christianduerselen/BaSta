using System;

namespace BaSta.TimeSync
{
    internal interface ITimeSyncSettingsGroup
    {
        string Name { get; }

        string? GetSetting(string name);
    }

    internal static class ITimeSyncSettingsGroupExtensions
    {
        public static T GetValue<T>(this ITimeSyncSettingsGroup settings, string name, Func<string, T> convertFunc, T defaultValue)
        {
            string? keyValue = null;

            try
            {
                keyValue = settings.GetSetting(name);
                T value = convertFunc(keyValue);
                return value;
            }
            catch (Exception ex)
            {
                if (defaultValue != null)
                    return defaultValue;

                throw new Exception($"Value '{keyValue}' for setting '{name}' not applicable in group '{settings.Name}'!", ex);
            }
        }
    }
}