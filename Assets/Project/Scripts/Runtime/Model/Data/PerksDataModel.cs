using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PerksDataModel
{
    [SerializeField] private StringProperty _used = new();
    [SerializeField] private List<string> _unlocked = new();
    
    public StringProperty Used => _used;
    
    public void AddPerk(string perkId)
    {
        if (!_unlocked.Contains(perkId))
        {
            _unlocked.Add(perkId);
        }
    }
    
    public bool IsPerkUnlocked(string perkId)
    {
        return _unlocked.Contains(perkId);
    }
}
