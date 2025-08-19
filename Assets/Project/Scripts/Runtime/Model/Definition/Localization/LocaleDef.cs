using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
public class LocaleDef : ScriptableObject
{
    //ru https://docs.google.com/spreadsheets/d/e/2PACX-1vRl9q4-UzGesxgKposcptFyFOZ8ZZHBN9QA9xgPawq7VlQUJBKtkNCHQVhbNV0I0_jJYrb6SdVmiVj1/pub?gid=0&single=true&output=tsv
    // eng https://docs.google.com/spreadsheets/d/e/2PACX-1vRl9q4-UzGesxgKposcptFyFOZ8ZZHBN9QA9xgPawq7VlQUJBKtkNCHQVhbNV0I0_jJYrb6SdVmiVj1/pub?gid=1714917023&single=true&output=tsv
    // uk https://docs.google.com/spreadsheets/d/e/2PACX-1vRl9q4-UzGesxgKposcptFyFOZ8ZZHBN9QA9xgPawq7VlQUJBKtkNCHQVhbNV0I0_jJYrb6SdVmiVj1/pub?gid=1776020846&single=true&output=tsv

    [SerializeField] private string _url;
    [SerializeField] private List<LocaleItem> _localeItems;

    private UnityWebRequest _request;
    
    [ContextMenu("Update locale")]
    public void LoadLocale()
    {
        if(_request != null)
            return;
        
        _request = UnityWebRequest.Get(_url);
        _request.SendWebRequest().completed += OnDataLoaded;
    }

#if UNITY_EDITOR
    [ContextMenu("Download locale from file")]
    public void UpdateLocaleFromFile()
    {
        var path = UnityEditor.EditorUtility.OpenFilePanel("Select locale file", "", "tsv");
        if (string.IsNullOrEmpty(path))
            return;

        var data = File.ReadAllText(path);
        ParseData(data);
    }
#endif
    
    private void ParseData(string data)
    {
        var rows = data.Split('\n');
        _localeItems.Clear();
        foreach (var row in rows)
        {
            AddLocaleItem(row);
        }
    }
    public Dictionary<string, string> GetData()
    {
        var dictionary = new Dictionary<string, string>();
        foreach (var item in _localeItems)
        {
            dictionary.Add(item.Key, item.Value);
        }

        return dictionary;
    }

    private void OnDataLoaded(AsyncOperation operation)
    {
        if (operation.isDone)
        {
            var data = _request.downloadHandler.text;
            ParseData(data);
        }            
        _request = null;
    }

    private void AddLocaleItem(string row)
    {
        try
        {
            var parts = row.Split('\t');
            _localeItems.Add(new LocaleItem(){Key = parts[0], Value = parts[1]});
        }
        catch (Exception e)
        {
            Debug.LogError($"Can't parse row {row}. \n {e}");
        }
    }


    [Serializable]
    private class LocaleItem
    {
        public string Key;
        public string Value;
        
    }
}
