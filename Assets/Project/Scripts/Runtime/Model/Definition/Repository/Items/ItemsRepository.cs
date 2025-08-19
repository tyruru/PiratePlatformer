using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "Items")]
public class ItemsRepository : DefRepository<ItemDef>
{
   
    
#if UNITY_EDITOR
    public ItemDef[] ItemsForEditor => _collection;
#endif
}



[Serializable]
public struct ItemDef : IStringID
{
    [SerializeField] private string _id;
    [SerializeField] private ItemTag[] _tags;
    [SerializeField] private Sprite _icon;
    public string Id => _id;
    public bool IsVoid => string.IsNullOrEmpty(_id);
    public Sprite Icon => _icon;

    public bool HasTag(ItemTag tag)
    {
        return _tags?.Contains(tag) ?? false;
    }
}
