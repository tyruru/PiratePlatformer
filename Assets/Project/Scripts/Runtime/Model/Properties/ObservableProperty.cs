
using System;
using UnityEngine;

[Serializable]
public class ObservableProperty<T>
{
    [SerializeField] protected T _value;
    
    public delegate void OnPropertyChanged(T newValue, T oldValue);
    public event OnPropertyChanged OnChanged;
    
    public IDisposable Subscribe(OnPropertyChanged call)
    {
        OnChanged += call;
        return new ActionDisposable(() => OnChanged -= call);
    }
    
    public IDisposable SubscribeAndInvoke(OnPropertyChanged call)
    {
        OnChanged += call;
        var dispose =  new ActionDisposable(() => OnChanged -= call);
        call(_value, _value);
        return dispose;
    }
    
    public virtual T Value
    {
        get => _value;
        set
        {
            if(_value.Equals(value))
                return;

            var oldValue = _value;
            _value = value;
            InvokeChangedEvent(_value, oldValue);
        }
    }

    protected void InvokeChangedEvent(T newValue, T oldValue)
    {
        OnChanged?.Invoke(newValue, oldValue);
    }
}
