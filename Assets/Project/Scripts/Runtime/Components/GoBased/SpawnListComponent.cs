using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnListComponent : MonoBehaviour
{
    [SerializeField] private SpawnData[] _spawners;

    public void SpawnAll()
    {
        foreach (var spawnData in _spawners)
        {
            spawnData.Component.Spawn();
        }
    }
    
    public void Spawn(string id)
    {
        var spawner = _spawners.FirstOrDefault(el => el.Id == id);
        spawner?.Component.Spawn();
    }
    
    
    [Serializable]
    public class SpawnData
    {
        public string Id;
        public SpawnComponent Component;
    }
    
}
