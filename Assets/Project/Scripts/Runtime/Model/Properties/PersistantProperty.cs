
public abstract class PersistantProperty<T> : ObservableProperty<T>
{
    protected T _stored;
    private T _defaultValue;
    
    protected PersistantProperty(T defaultValue)
    {
        _defaultValue = defaultValue;
    }
    
    public override T Value
    {
        get => _stored;
        set
        {
            if(_stored.Equals(value))
                return;

            var oldValue = _value;
            
            Write(value);
            _stored = _value = value;
            
            InvokeChangedEvent(value, oldValue);
        }
    }

    protected void Init()
    {
        _stored = _value = Read(_defaultValue);
    }
    protected abstract void Write(T value);
    protected abstract T Read(T defaultValue);

    public void Validate()
    {
        if (!_stored.Equals(_value))
            Value = _value;
    }
}
