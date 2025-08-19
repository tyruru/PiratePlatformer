using Project.Scripts.Model;
using UnityEngine;

public class RestoreStateComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    public string Id => _id;
    
    private GameSessionModel _gameSessionModel;
    private void Start()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        
        var isDestroyed = _gameSessionModel.RestoreState(_id);
        if(isDestroyed)
            Destroy(gameObject);
    }
}
