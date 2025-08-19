using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ProbabilityDropComponent : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private DropData[] _drop;
    [SerializeField] private DropEvent _onDropCalculated;
    [SerializeField] private bool _spawnOnEnable;

    private void OnEnable()
    {
        if(_spawnOnEnable)
        {
            CalculateDrop();
        }
    }

    [ContextMenu("CalculateDrop")]
    public void CalculateDrop()
    {
        var itemsToDrop = new GameObject[_count];
        var itemCount = 0;
        var total = _drop.Sum(data => data.Probability);
        var sortedDrop = _drop.OrderBy(data => data.Probability);
        
        while (itemCount < _count)
        {
            var random = UnityEngine.Random.value * total;
            var current = 0f;
            foreach (var dropData in sortedDrop)
            {
                current += dropData.Probability;
                if (current >= random)
                {
                    itemsToDrop[itemCount] = dropData.Drop;
                    itemCount++;
                    break;
                }
            }
        }
        
        _onDropCalculated?.Invoke(itemsToDrop);
    }

    [Serializable]
    public class DropData
    {
        public GameObject Drop;
        [Range(0f, 100f)]public float Probability;
    }

  
    public void SetCount(int numCoinsToDispose)
    {
        _count = numCoinsToDispose;
    }
}

[Serializable]
public class DropEvent : UnityEvent<GameObject[]>
{
        
}

