using System;
using System.Collections.Generic;
using System.Reflection;
using BaSta.TimeSync.Input;
using BaSta.TimeSync.Output;
using NLog;

namespace BaSta.TimeSync
{
    internal static class TimeSyncTaskFactory
    {
        private const string TypeKeyName = "Type";
        private const string NameKeyName = "Name";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly IDictionary<string, Type> InputTypes = new Dictionary<string, Type>
        {
            { StramatelClassicFileInputTask.TypeName, typeof(StramatelClassicFileInputTask) },
            { StramatelClassicSerialInputTask.TypeName, typeof(StramatelClassicSerialInputTask) }
        };

        private static readonly IDictionary<string, Type> OutputTypes = new Dictionary<string, Type>
        {
            { ScoutTimeOutput.TypeName, typeof(ScoutTimeOutput) }
        };

        internal static ITimeSyncInputTask CreateInput(ITimeSyncSettingsGroup settings)
        {
            return CreateTask(settings, InputTypes) as ITimeSyncInputTask;
        }

        internal static ITimeSyncOutputTask CreateOutput(ITimeSyncSettingsGroup settings)
        {
            return CreateTask(settings, OutputTypes) as ITimeSyncOutputTask;
        }

        private static ITimeSyncTask CreateTask(ITimeSyncSettingsGroup settings, IDictionary<string, Type> typeDictionary)
        {
            string? typeName = settings.GetSetting(TypeKeyName);

            if (string.IsNullOrEmpty(typeName))
                throw new Exception($"No type defined in settings group '{settings.Name}'!");

            string? taskName = settings.GetSetting(NameKeyName);
            if (string.IsNullOrEmpty(taskName))
                throw new Exception($"No name defined in settings group '{settings.Name}'!");

            Type type;
            if (!typeDictionary.TryGetValue(typeName, out type))
            {
                Logger.Error($"Type '{typeName}' not supported! Available: {string.Join(" | ", OutputTypes.Keys)}");
                return null;
            }

            try
            {
                object? instance = Activator.CreateInstance(type);

                MethodInfo? method = type.GetMethod("LoadSettings");
                method.Invoke(instance, new object?[] { taskName, settings });

                return instance as ITimeSyncTask;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error trying to create ");

                return null;
            }
        }
    }
}