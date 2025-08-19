
using System;
using UnityEditor;

public static class SerializedPropertyExtensions
{
    public static bool GetEnum<T>(this SerializedProperty property, out T retValue) where T : Enum
    {
        retValue = default;

        var names = property.enumNames;
        if (names == null || names.Length == 0)
            return false;

        var enumName = names[property.enumValueIndex];
        retValue = (T) Enum.Parse(typeof(T), enumName);
        return true;
    }
}
