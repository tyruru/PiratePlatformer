using System;
using Project.Scripts.Utils;

public class PerksModel : IDisposable
{
    private readonly PlayerDataModel _playerData;
    
    public readonly StringProperty InterfaceSelection = new StringProperty();
    private readonly CompositeDisposable _trash = new();

    public readonly Cooldown Cooldown = new();

    public event Action OnChanged;
    public string Used => _playerData.Perks.Used.Value;
    public bool IsSuperThrowSupported => _playerData.Perks.Used.Value == "super-throw" && Cooldown.IsReady;
    public bool IsDoubleJumpSupported => _playerData.Perks.Used.Value == "double-jump" && Cooldown.IsReady;
    public bool IsShieldSupported => _playerData.Perks.Used.Value == "shield" && Cooldown.IsReady;
    
    public PerksModel(PlayerDataModel playerData)
    {
        _playerData = playerData;
        InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id; 
        
        _trash.Retain(_playerData.Perks.Used.Subscribe((x,y ) => OnChanged?.Invoke()));
        _trash.Retain(InterfaceSelection.Subscribe((x,y ) => OnChanged?.Invoke()));
    }

    public IDisposable Subscribe(Action call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }
    

    public void Unlock(string id)
    {
        var def = DefsFacade.I.Perks.Get(id);
        var isEnoughResources = _playerData.Inventory.IsEnough(def.Price);

        if (isEnoughResources)
        {
            _playerData.Inventory.Remove(def.Price.ItemId, def.Price.Count);
            _playerData.Perks.AddPerk(id);
            OnChanged?.Invoke();    
        }
        
    }
  
    public void SelectPerk(string selected)
    {
        var perkDef = DefsFacade.I.Perks.Get(selected);
        Cooldown.Value = perkDef.Cooldown;
        _playerData.Perks.Used.Value = selected;
    }
    
    public bool IsUsed(string perkId)
    {
        return _playerData.Perks.Used.Value == perkId;
    }

    public bool IsUnlocked(string perkId)
    {
        return _playerData.Perks.IsPerkUnlocked(perkId);
    }
    
    public bool CanBuy(string perkId)
    {
        var def = DefsFacade.I.Perks.Get(perkId);
        return _playerData.Inventory.IsEnough(def.Price);
    }
    
    public void Dispose()
    {
        _trash.Dispose();
    }

}
