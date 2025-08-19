using System;

[Serializable]
public class StringProperty : ObservableProperty<string>
{
    public StringProperty() : base()
    {
        _value = "";
    }
}
