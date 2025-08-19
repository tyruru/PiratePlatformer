using System;
using UnityEngine;

[Serializable]
public class StatDef
{
    [SerializeField] private string _name;
    [SerializeField] private StatId _id;
    [SerializeField] private Sprite _icon;
    [SerializeField] private StatLevelDef[] _level;

    public string Name => _name;
    public StatId Id => _id;

    public Sprite Icon => _icon;

    public StatLevelDef[] Levels => _level;
}

[Serializable]
public class StatLevelDef
{
    [SerializeField] private float _value;
    [SerializeField] private ItemWithCount _price;

    public float Value => _value;
    public ItemWithCount Price => _price;
}

public enum StatId
{
    Hp,
    Speed,
    RangeDamage,
    CriticalDamage
}
