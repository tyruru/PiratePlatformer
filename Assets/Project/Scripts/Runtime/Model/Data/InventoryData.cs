using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryData
{
    [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

    public delegate void OnInventoryChanged(string id, int value);

    public OnInventoryChanged OnChanged;
    
    public void Add(string id, int value)
    {
        if(value <= 0)
            return;

        var itemDef = DefsFacade.I.Items.Get(id);
        if(itemDef.IsVoid)
            return;

        var isFull = _inventory.Count >= DefsFacade.I.Player.InventorySize;
        if (!isFull)
        {
            if (itemDef.HasTag(ItemTag.Stackable))
            {
                AddToStack(id, value);
            }
            else
            {
                AddNonStack(id, value);
            }
        }

        OnChanged?.Invoke(id, Count(id));
    }

    public InventoryItemData[] GetAll(params ItemTag[] tags)
    {
        var retValue = new List<InventoryItemData>();
        
        foreach (var item in _inventory)
        {
            var itemDef = DefsFacade.I.Items.Get(item.id);
            var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));
            if (isAllRequirementsMet)
                retValue.Add(item);
        }

        return retValue.ToArray();
    }
    
    private void AddToStack(string id, int value)
    {
        var item = GetItem(id);
        if (item == null)
        {
            item = new InventoryItemData(id);
            _inventory.Add(item);
        }
     
        item.value += value;
    }

    private void AddNonStack(string id, int value)
    {
        var itemLasts = DefsFacade.I.Player.InventorySize - _inventory.Count;
        value = Mathf.Min(itemLasts, value);
        
        for (int i = 0; i < value; i++)
        {
            var item = new InventoryItemData(id) { value = 1 };
            _inventory.Add(item);
        }
    }

    public void Remove(string id, int value)
    {
        var itemDef = DefsFacade.I.Items.Get(id);
        if(itemDef.IsVoid)
            return;
        
        var item = GetItem(id);
        
        if(item == null)
            return;

        if (itemDef.HasTag(ItemTag.Stackable))
        {
            RemoveFromStack(item, value);
        }
        else
        {
            RemoveFromNonStack(item, value);
        }
        
        
        OnChanged?.Invoke(id, Count(id));
    }

    private void RemoveFromStack(InventoryItemData item, int value)
    {
        item.value -= value;
        
        if(item.value <= 0)
            _inventory.Remove(item);
    }

    private void RemoveFromNonStack(InventoryItemData item, int value)
    {
        for (int i = 0; i < value; i++)
        {
            _inventory.Remove(item);
        }
    }
    
    
    private InventoryItemData GetItem(string id)
    {
        foreach (var itemData in _inventory)
        {
            if (itemData.id == id)
                return itemData;
        }

        return null;
    }

    public int Count(string id)
    {
        var ob = _inventory.FirstOrDefault(item => item.id == id);
       
        return ob?.value ?? 0;
    }

    public bool IsEnough(params ItemWithCount[] items)
    {
        var joined = new Dictionary<string, int>();

        foreach (var item in items)
        {
            if (joined.ContainsKey(item.ItemId))
                joined[item.ItemId] += item.Count;
            else
                joined.Add(item.ItemId, item.Count);
        }
        
        foreach (var kvp in joined)
        {
            var count = Count(kvp.Key);
            if (count < kvp.Value)
                return false;
        }

        return true;
    }
}

[Serializable]
public class InventoryItemData
{
    [InventoryId] public string id;
    public int value;

    public InventoryItemData(string id)
    {
        this.id = id;
    }
}


