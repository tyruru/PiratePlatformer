using System;
using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.Events;

public class CheckPointComponent : MonoBehaviour
{
    [SerializeField] private string _id;
    [SerializeField] private UnityEvent _setChecked;
    [SerializeField] private UnityEvent _setUnchecked;
    [SerializeField] private SpawnComponent _heroSpawn;
    
    private GameSessionModel _gameSessionModel;

    public string Id => _id;
    
    private void Start()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
        if (_gameSessionModel.IsChecked(_id))
            _setChecked.Invoke();
        else
            _setUnchecked.Invoke();
        
    }

    public void Check()
    {
        _gameSessionModel.SetChecked(_id);
        _setChecked?.Invoke();
    }
    
    public void SpawnHero()
    {
        _heroSpawn.Spawn();
    }
}
