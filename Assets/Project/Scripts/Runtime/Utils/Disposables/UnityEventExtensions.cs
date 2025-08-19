
using System;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    public static IDisposable Subscribe(this UnityEvent unityEvent, UnityAction callback)
    {
        unityEvent.AddListener(callback);
        return new ActionDisposable(() => unityEvent.RemoveListener(callback));
    }
    
    public static IDisposable Subscribe<T>(this UnityEvent<T> unityEvent, UnityAction<T> callback)
    {
        unityEvent.AddListener(callback);
        return new ActionDisposable(() => unityEvent.RemoveListener(callback));
    }
}
