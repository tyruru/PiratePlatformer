using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/Potions", fileName = "Potions")]

public class PotionRepository : DefRepository<PotionDef>
{
    
}

[Serializable]
public struct PotionDef : IStringID
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private float _value;
    [SerializeField] private Effect _effect;
    [SerializeField] private float _time;
    public string Id => _id;
    public float Value => _value;
    public float Time => _time;
    public Effect Effect => _effect;
}

public enum Effect
{
    Health,
    SpeedUp
}