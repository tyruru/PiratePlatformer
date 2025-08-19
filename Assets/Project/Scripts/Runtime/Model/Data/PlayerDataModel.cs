using System;
using UnityEngine;

[Serializable]
public class PlayerDataModel
{
    [SerializeField] private InventoryData _inventory;
    
    public IntProperty Hp = new();
    public FloatProperty Fuel = new();
    public PerksDataModel Perks = new();
    public LevelData Levels = new();
    public InventoryData Inventory => _inventory;
    
    public PlayerDataModel Clone()
    {
        var json = JsonUtility.ToJson(this);

        return JsonUtility.FromJson<PlayerDataModel>(json);
    }
}
