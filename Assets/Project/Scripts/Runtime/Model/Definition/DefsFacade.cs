using UnityEngine;

[CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
public class DefsFacade : ScriptableObject
{
    [SerializeField] private ItemsRepository _items;
    [SerializeField] private ThrowableRepository _throwableItems;
    [SerializeField] private PotionRepository _potions;
    [SerializeField] private PerkRepository _perks;
    [SerializeField] private PlayerDef _player;

    public ItemsRepository Items => _items;
    public ThrowableRepository ThrowableItems => _throwableItems;
    public PotionRepository Potions => _potions;
    public PerkRepository Perks => _perks;

    public PlayerDef Player => _player;

    private static DefsFacade _instance;

    public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

    private static DefsFacade LoadDefs()
    {
        return _instance = Resources.Load<DefsFacade>("DefsFacade");
    }
    
}
