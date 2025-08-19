
public abstract class PrefsPersistentProperty<T> : PersistantProperty<T>
{
    protected readonly string Key;

    protected PrefsPersistentProperty(T defaultValue, string key) : base(defaultValue)
    {
        Key = key;
    }
    
    
}
