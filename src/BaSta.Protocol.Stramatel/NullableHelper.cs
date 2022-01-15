using System;

namespace BaSta.Protocol.Stramatel;

public static class NullableHelper
{
    public static void SetValueIfNotNull<TType>(TType? value, Action<TType> setValueAction) where TType : struct
    {
        if (value.HasValue)
            setValueAction(value.Value);
    }

    public static void SetValueIfNotNull<TType>(TType? value, Action<TType> setValueAction)
    {
        if (value == null)
            return;

        setValueAction(value);
    }
}