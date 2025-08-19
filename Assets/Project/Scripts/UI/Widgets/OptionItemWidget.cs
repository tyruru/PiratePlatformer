
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OptionItemWidget : MonoBehaviour, IItemRenderer<OptionData>
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private SelectOption _onSelect;

    private OptionData _data;
    
    public void SetData(OptionData localInfo, int index)
    {
        _data = localInfo;
        _label.text = localInfo.Text;
    }

    public void OnSelect()
    {
        _onSelect.Invoke(_data);
    }

    [Serializable]
    public class SelectOption : UnityEvent<OptionData>
    {
        
    }
    
}
