using System;
using Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemWidget : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _selection;
    [SerializeField] private TextMeshProUGUI _value;
    
    private readonly CompositeDisposable _trash = new();

    private int _index;

    private void Start()
    {
        var session = FindObjectOfType<GameSessionModel>();
        var index = session.QuickInventory.SelectedIndex;
        _trash.Retain(index.SubscribeAndInvoke(OnIndexChanged));
    }

    private void OnIndexChanged(int newValue, int _)
    {
        _selection.SetActive(_index == newValue);
    }

    public void SetData(InventoryItemData item, int index)
    {
        _index = index;
        var def = DefsFacade.I.Items.Get(item.id);
        _icon.sprite = def.Icon;
        _value.text = def.HasTag(ItemTag.Stackable) ? item.value.ToString() : string.Empty;
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
