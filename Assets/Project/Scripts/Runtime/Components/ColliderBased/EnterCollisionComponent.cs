
using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterCollisionComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private EnterEvent _action;
    private void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.CompareTag(_tag))
        {
            _action?.Invoke(target.gameObject);
        }
    }

    [Serializable]
    public class EnterEvent : UnityEvent<GameObject>
    {
    }
}
