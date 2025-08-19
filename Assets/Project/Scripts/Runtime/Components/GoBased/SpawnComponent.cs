using UnityEngine;

public class SpawnComponent : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _invertXScale;

    [SerializeField] private bool _usePool;
    
    [ContextMenu("Spawn")]
    public void Spawn()
    {
        SpawnInstance();
    }

    public GameObject SpawnInstance()
    {
        var instance = _usePool
            ? Pool.Instance.Get(_prefab, _target.position) 
            : SpawnUtils.Spawn(_prefab, _target.position);
        
        var scale = transform.lossyScale;
        scale.x *= _invertXScale ? -1 : 1;
        instance.transform.localScale = scale;
        instance.SetActive(true);
        
        return instance;
    }

    public void SetPrefab(GameObject prefab)
    {
        _prefab = prefab;
    }
}
