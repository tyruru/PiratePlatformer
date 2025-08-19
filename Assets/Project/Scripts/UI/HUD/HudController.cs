using Project.Scripts.Model;
using Unity.VisualScripting;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private ProgressBarWidget _healthBarWidget;
    [SerializeField] private CurrentPerkWidget _currentPerkWidget;

    private GameSessionModel _gameSessionModel;
    private CompositeDisposable _trash = new();
    private void Awake()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        _trash.Retain(_gameSessionModel.PlayerData.Hp.SubscribeAndInvoke(OnHealthChanged));
        _trash.Retain(_gameSessionModel.PerksModel.Subscribe(OnPerkChanged));
        
        OnPerkChanged();
    }

    private void OnPerkChanged()
    {
        var usedPerkId = _gameSessionModel.PerksModel.Used;
        var hasPerk = !string.IsNullOrEmpty(usedPerkId);
        if (hasPerk)
        {
            var perkDef = DefsFacade.I.Perks.Get(usedPerkId);
            _currentPerkWidget.SetPerk(perkDef);
        }
        
        _currentPerkWidget.gameObject.SetActive(hasPerk);
    }

    private void OnHealthChanged(int newValue, int oldValue)
    {
        var maxHealth = _gameSessionModel.StatsModel.GetValue(StatId.Hp);
        var value = (float)newValue / maxHealth;
        _healthBarWidget.SetProgress(value);
    }

    public void OnSettings()
    {
        WindowUtils.CreateWindow(LoadPaths.Resources.InGameWindowPath);
    }
    
    public void OnStats()
    {
        WindowUtils.CreateWindow(LoadPaths.Resources.HeroLevelsWindowPath);
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}
