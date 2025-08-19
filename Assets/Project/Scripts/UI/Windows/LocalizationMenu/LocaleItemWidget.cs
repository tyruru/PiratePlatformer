using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LocaleItemWidget : MonoBehaviour, IItemRenderer<LocaleInfo>
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _selector;
    [SerializeField] private SelectLocale _onSelected;

    private LocaleInfo _data;

    private void Start()
    {
        LocalizationManager.I.OnLocaleChanged += UpdateSelection;
    }

    private void UpdateSelection()
    {
        var isSelected = LocalizationManager.I.LocalKey == _data.LocaleId;
        if(_selector)
            _selector.SetActive(isSelected);
    }

    public void SetData(LocaleInfo localInfo, int index)
    {
        _data = localInfo;
        UpdateSelection();
        _text.text = localInfo.LocaleId.ToUpper();
    }

    public void OnSelected()
    {
        _onSelected?.Invoke(_data.LocaleId);
    }

    private void OnDestroy()
    {
        LocalizationManager.I.OnLocaleChanged -= UpdateSelection;
    }
}

[Serializable]
public class SelectLocale : UnityEvent<string>
{
    
}

public class LocaleInfo
{
    public string LocaleId;
}
