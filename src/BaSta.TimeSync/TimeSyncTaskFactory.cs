using System;
using System.Collections.Generic;
using System.Reflection;
using BaSta.Link;
using BaSta.Link.Bodet;
using BaSta.Link.Stramatel;
using BaSta.Link.SwissTiming;
using BaSta.Link.Wige;
using NLog;

namespace BaSta.TimeSync;

internal static class TimeSyncTaskFactory
{
    private const string TypeKeyName = "Type";
    private const string NameKeyName = "Name";

    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private static readonly IDictionary<string, Type> InputTypes = new Dictionary<string, Type>
    {
        { UimTimeInput.TypeName, typeof(UimTimeInput) },
        { BodetSerialInput.TypeName, typeof(BodetSerialInput) },
        { StramatelSerialInput.TypeName, typeof(StramatelSerialInput) },
        { WigeSerialInput.TypeName, typeof(WigeSerialInput) }
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
            Logger.Error(ex, $"Error trying to create task for '{taskName}': {ex.GetInnerMessages()}");

            return null;
        }
    }
}

internal static class ExceptionExceptions
{
    internal static string GetInnerMessages(this Exception ex, int depth = 0)
    {
        return $"{ex.Message}" + (ex.InnerException != null ? $"{Environment.NewLine}{new string(' ', depth + 1)}> {ex.InnerException.GetInnerMessages(depth + 1)}" : string.Empty);
    }
}