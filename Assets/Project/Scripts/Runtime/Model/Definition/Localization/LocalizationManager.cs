using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    public readonly static LocalizationManager I;

    private StringPersistentProperty _localeKey = new StringPersistentProperty("eng", "localization/current");

    private Dictionary<string, string> _localization = new();

    public event Action OnLocaleChanged ;
    
    public string LocalKey => _localeKey.Value;

    
    static LocalizationManager()
    {
        I = new LocalizationManager();
    }

    public LocalizationManager()
    {
        LoadLocale(_localeKey.Value);
    }


    private void LoadLocale(string localToLoad)
    {
        var def = Resources.Load<LocaleDef>($"Locales/{localToLoad}");
        _localization = def.GetData();
        _localeKey.Value = localToLoad;
        OnLocaleChanged?.Invoke();
    }

    public string Localize(string key)
    {
        return _localization.TryGetValue(key, out var value) ? value : $"###{key}###";
    }

    public void SetLocale(string localeKey)
    {
        LoadLocale(localeKey);
    }
}
