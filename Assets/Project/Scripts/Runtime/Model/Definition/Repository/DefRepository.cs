using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DefRepository<T> : ScriptableObject where T : IStringID
{
    [SerializeField] protected T[] _collection;
    
    public T Get(string id)
    {
        if(string.IsNullOrEmpty(id))
            return default;
        
        return _collection.FirstOrDefault(x => x.Id == id);
    }

    public T[] All => new List<T>(_collection).ToArray();
}
