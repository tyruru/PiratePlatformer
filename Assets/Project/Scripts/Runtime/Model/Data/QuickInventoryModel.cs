
using System;
using UnityEngine;

public class QuickInventoryModel : IDisposable
{
    private readonly PlayerDataModel _data;
    
    public readonly IntProperty SelectedIndex = new();

    public event Action OnChanged;
    public InventoryItemData[] Inventory {get; private set; }

    public InventoryItemData SelectedItem
    {
        get
        {
            if (Inventory.Length > 0 && Inventory.Length > SelectedIndex.Value)
                return Inventory[SelectedIndex.Value];

            return null;
        }
    }

    public ItemDef SelectedItemDef => DefsFacade.I.Items.Get(SelectedItem?.id);
    public QuickInventoryModel(PlayerDataModel playerData)
    {
        _data = playerData;

        Inventory = _data.Inventory.GetAll(ItemTag.Usable);
        _data.Inventory.OnChanged += OnChangedInventory;
    }

    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }

    private void OnChangedInventory(string id, int value)
    {
        Inventory = _data.Inventory.GetAll(ItemTag.Usable);
        SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
        OnChanged?.Invoke();
    }

    public void SetNextIndex()
    {
        SelectedIndex.Value = (int)Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
    }

    public void Dispose()
    {
        _data.Inventory.OnChanged -= OnChangedInventory;
    }
}
