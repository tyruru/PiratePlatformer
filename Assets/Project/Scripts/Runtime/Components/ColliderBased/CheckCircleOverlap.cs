using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class CheckCircleOverlap : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private string[] _tags;
    [SerializeField] private OnOverlapEvent _onOverlapEvent;
    
    private readonly Collider2D[] _interactionResult = new Collider2D[10];

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = HandlesUtils.TransparentRed;
        Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
    }
#endif
    public void Check()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult, _mask);

        var overlaps = new List<GameObject>();
        for (var i = 0; i < size; i++)
        {
            var overlapResult = _interactionResult[i];
            var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
            
            if(isInTags)
                _onOverlapEvent.Invoke(overlapResult.gameObject);
        }
    }
    
    [Serializable]
    public class OnOverlapEvent : UnityEvent<GameObject>
    {
    }
}
