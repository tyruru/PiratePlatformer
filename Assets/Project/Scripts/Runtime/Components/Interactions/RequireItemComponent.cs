using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

public class RequireItemComponent : MonoBehaviour
{
    [SerializeField] private InventoryItemData[] _required;
    [SerializeField] private bool _removeAfterUse;

    [SerializeField] private UnityEvent OnSuccess;
    [SerializeField] private UnityEvent OnFail;
    
    public void Check() 
    {
        var session = FindObjectOfType<GameSessionModel>();
        var areAllRequirments = true;
        foreach (var item in _required)
        {
            var numItems = session.PlayerData.Inventory.Count(item.id);
            if (numItems < item.value)
                areAllRequirments = false;
        }
        
        if (areAllRequirments)
        {
            if(_removeAfterUse)
                foreach (var item in _required)
                    session.PlayerData.Inventory.Remove(item.id, item.value);
            
            OnSuccess?.Invoke();
        }
        else
        {
            OnFail?.Invoke();
        }
    }
}
