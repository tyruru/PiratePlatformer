
using System;
using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerStatsWindow : AnimatedWindow
{
    [SerializeField] private Transform _statContainer;
    [SerializeField] private StatWidget _prefab;
    [FormerlySerializedAs("_buyButton")] [SerializeField] private Button _upgradeButton;
    [SerializeField] private ItemWidget _price;

    private DataGroup<StatDef, StatWidget> _dataGroup;

    private GameSessionModel _gameSessionModel;
    private readonly CompositeDisposable _trash = new CompositeDisposable();
    protected override void Start()
    {
        base.Start();
        
        _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _statContainer);
        
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        _gameSessionModel.StatsModel.InterfaceSelectedStat.Value = DefsFacade.I.Player.Stats[0].Id;
        _trash.Retain(_gameSessionModel.StatsModel.Subscribe(OnStatsChanged));
        _trash.Retain(_upgradeButton.onClick.Subscribe(OnUpgradeButtonClicked));

        OnStatsChanged();
    }

    private void OnUpgradeButtonClicked()
    {
        var selected = _gameSessionModel.StatsModel.InterfaceSelectedStat.Value;
        _gameSessionModel.StatsModel.LevelUp(selected);
    }

    private void OnStatsChanged()
    {
        var stats = DefsFacade.I.Player.Stats;
       _dataGroup.SetData(stats);
       
       var selected = _gameSessionModel.StatsModel.InterfaceSelectedStat.Value;
       var nextLevel = _gameSessionModel.StatsModel.GetCurrentLevel(selected) + 1;
       var def = _gameSessionModel.StatsModel.GetLevelDef(selected, nextLevel);
       if (def == null)
       {
           _price.gameObject.SetActive(false);
           _upgradeButton.gameObject.SetActive(false);
           return;
       }

       _price.SetData(def.Price);
       
       _price.gameObject.SetActive(def.Price.Count != 0);
       _upgradeButton.gameObject.SetActive(def.Price.Count != 0);
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
