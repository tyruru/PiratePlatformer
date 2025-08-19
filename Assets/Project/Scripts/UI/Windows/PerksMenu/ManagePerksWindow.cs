using System;
using Project.Scripts.Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManagePerksWindow : AnimatedWindow
{
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private ItemWidget _price;
    [SerializeField] private TextMeshProUGUI _info;
    [SerializeField] private Transform _perksContainer;
    
    private PredefinedDataGroup<PerkDef, PerkWidget> _dataGroup;
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    private GameSessionModel _gameSessionModel;
    protected override void Start()
    {
        base.Start();
        
        _dataGroup = new PredefinedDataGroup<PerkDef, PerkWidget>(_perksContainer);
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        
        _trash.Retain(_gameSessionModel.PerksModel.Subscribe(OnPerkChanged));
        _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
        _trash.Retain(_useButton.onClick.Subscribe(OnUse));
        
        OnPerkChanged();
    }

    private void OnPerkChanged()
    {
        _dataGroup.SetData(DefsFacade.I.Perks.All);
        
        var selected = _gameSessionModel.PerksModel.InterfaceSelection.Value;
        
        _useButton.gameObject.SetActive(_gameSessionModel.PerksModel.IsUnlocked(selected));
        _useButton.interactable = _gameSessionModel.PerksModel.Used != selected;
        
        _buyButton.gameObject.SetActive(!_gameSessionModel.PerksModel.IsUnlocked(selected));
        _buyButton.interactable = _gameSessionModel.PerksModel.CanBuy(selected);
        
        var def = DefsFacade.I.Perks.Get(selected);
        _price.SetData(def.Price);
        
        _info.text = LocalizationManager.I.Localize(def.Info);
    }
    
    private void OnUse()
    {
        var selected = _gameSessionModel.PerksModel.InterfaceSelection.Value;
        _gameSessionModel.PerksModel.SelectPerk(selected);
    }

    private void OnBuy()
    {
        var selected = _gameSessionModel.PerksModel.InterfaceSelection.Value;
        _gameSessionModel.PerksModel.Unlock(selected);
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
