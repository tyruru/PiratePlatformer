using System;

public class StatsModel : IDisposable
{
    private readonly PlayerDataModel _playerData;

    private readonly CompositeDisposable _trash = new CompositeDisposable();
    
    public event Action OnChanged;
    public event Action<StatId> OnUpgraded;
    
    public ObservableProperty<StatId> InterfaceSelectedStat = new ObservableProperty<StatId>();
    public StatsModel(PlayerDataModel playerData)
    {
        _playerData = playerData;
        _trash.Retain(InterfaceSelectedStat.Subscribe((x, y) => OnChanged?.Invoke()));
    }

    public IDisposable Subscribe(Action action)
    {
        OnChanged += action;
        return new ActionDisposable(() => OnChanged -= action);
    }
    
    public void LevelUp(StatId id)
    {
        var def = DefsFacade.I.Player.GetStat(id);
        var nextLevel = GetCurrentLevel(id) + 1;
        if (def.Levels.Length <= nextLevel)
            return;
        
        var price = def.Levels[nextLevel].Price;
        if (!_playerData.Inventory.IsEnough(price))
            return;
        
        _playerData.Inventory.Remove(price.ItemId, price.Count);
        _playerData.Levels.LevelUp(id);

        OnChanged?.Invoke();
        OnUpgraded?.Invoke(id);
    }

  

    public float GetValue(StatId id, int level = -1)
    {
        var levelDef = GetLevelDef(id, level);
        return levelDef?.Value ?? 0;
    }
    
    public StatLevelDef GetLevelDef(StatId id, int level = -1)
    {
        if (level == -1)
            level = GetCurrentLevel(id);
        
        var def = DefsFacade.I.Player.GetStat(id);
        if(def.Levels.Length > level)
            return def.Levels[level];
        return null;
    }

    public int GetCurrentLevel(StatId id) => _playerData.Levels.GetLevel(id);

    public void Dispose()
    {
        _trash.Dispose();
    }
}
