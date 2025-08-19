using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Defs/ThrowableItems", fileName = "ThrowableItems")]

public class ThrowableRepository : DefRepository<ThrowableDef>
{
    
}

[Serializable]
public struct ThrowableDef : IStringID
{
    [InventoryId] [SerializeField] private string _id;
    [SerializeField] private GameObject _projectile;

    public string Id => _id;

    public GameObject Projectile => _projectile;
}
