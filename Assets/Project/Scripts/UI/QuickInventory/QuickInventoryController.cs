using System.Collections.Generic;
using Project.Scripts.Model;
using UnityEngine;

public class QuickInventoryController : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private InventoryItemWidget _prefab;

    private readonly CompositeDisposable _trash = new();
    private readonly List<InventoryItemWidget> _createdItems = new();
    
    private GameSessionModel _gameSessionModel;

    private void Awake()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        _trash.Retain(_gameSessionModel.QuickInventory.Subscribe(Rebuild));
        Rebuild();
    }

    private void Rebuild()
    {
        var inventory = _gameSessionModel.QuickInventory.Inventory;

        //create required item
        for (var i =  _createdItems.Count; i < inventory.Length; i++)
        {
            var item = Instantiate(_prefab, _container);
            _createdItems.Add(item);
        }

        //update data and activate
        for (int i = 0; i < inventory.Length; i++)
        {
            _createdItems[i].SetData(inventory[i], i);
            _createdItems[i].gameObject.SetActive(true);
        }

        // hide unused items
        for (int i = inventory.Length; i < _createdItems.Count; i++)
        {
            _createdItems[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}

