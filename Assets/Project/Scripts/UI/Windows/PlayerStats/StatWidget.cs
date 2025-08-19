
using System;
using System.Globalization;
using Project.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _currentValue;
    [SerializeField] private TextMeshProUGUI _increseValue;
    [SerializeField] private ProgressBarWidget _progress;
    [SerializeField] private GameObject _selector;

    private GameSessionModel _gameSessionModel;
    private StatDef _data;
    
    private void Start()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        UpdateView();
    }

    public void SetData(StatDef data, int index)
    {
        _data = data;
        if(_gameSessionModel != null)
            UpdateView();
    }
    
    private void UpdateView()
    {
        var statsModel = _gameSessionModel.StatsModel;
        
        _icon.sprite = _data.Icon;
        _name.text = LocalizationManager.I.Localize(_data.Name);
        var currentLevelValue = statsModel.GetValue(_data.Id);
        _currentValue.text = statsModel.GetValue(_data.Id).ToString(CultureInfo.InvariantCulture);
        
        var currentLevel = statsModel.GetCurrentLevel(_data.Id);
        var nextLevel = currentLevel + 1;
        var nextLevelValue = statsModel.GetValue(_data.Id, nextLevel);
        var increaseValue = nextLevelValue - (int)currentLevelValue;
        _increseValue.text = increaseValue.ToString(CultureInfo.InvariantCulture);
        _increseValue.gameObject.SetActive(increaseValue > 0);

        var maxLevels = DefsFacade.I.Player.GetStat(_data.Id).Levels.Length-1;
        _progress.SetProgress(currentLevel / (float)maxLevels);
        _selector.SetActive(statsModel.InterfaceSelectedStat.Value == _data.Id);
    }

    public void OnSelect()
    {
        _gameSessionModel.StatsModel.InterfaceSelectedStat.Value = _data.Id;
    }

}
