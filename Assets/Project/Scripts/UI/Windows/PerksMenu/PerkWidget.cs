using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
{
    [SerializeField] private Image _icon;
    [SerializeField] private GameObject _isLocked;
    [SerializeField] private GameObject _isSelected;
    [SerializeField] private GameObject _isUsed;

    private GameSessionModel _gameSessionModel;
    private PerkDef _data;

    private void Start()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        
        UpdateView();
    }

    public void SetData(PerkDef data, int index)
    {
        _data = data;

        if (_gameSessionModel)
            UpdateView();
    }

    private void UpdateView()
    {
        _icon.sprite = _data.Icon;
        _isUsed.SetActive(_gameSessionModel.PerksModel.IsUsed(_data.Id));
        _isSelected.SetActive(_gameSessionModel.PerksModel.InterfaceSelection.Value == _data.Id);
        _isLocked.SetActive(!_gameSessionModel.PerksModel.IsUnlocked(_data.Id));
    }

    public void OnSelect()
    {
        _gameSessionModel.PerksModel.InterfaceSelection.Value = _data.Id;
    }
    
}
