using System;
using UnityEngine;
using UnityEngine.Events;

public class ShowDialogComponent : MonoBehaviour
{
    [SerializeField] private Mode _mode;
    [SerializeField] private DialogData _bound;
    [SerializeField] private DialogDef _external;
    [SerializeField] private UnityEvent _onComplete;

    private DialogBoxController _boxController;

    private const string SimpleDialogBoxTag = "SimpleDialog";
    private const string PersonalizedDialogBoxTag = "PersonalizedDialog";
    
    public DialogData Data
    {
        get
        {
            switch (_mode)
            {
                case Mode.Bound:
                    return _bound;
                case Mode.External:
                    return _external.DialogData;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    public void Show()
    {
        _boxController = FindDialogBoxController();
        
        _boxController.ShowDialog(Data, _onComplete);
    }

    private DialogBoxController FindDialogBoxController()
    {
        if (_boxController != null)
            return _boxController;
        
        switch (Data.Type)
        {
            case DialogType.Simple:
                return GameObject.FindWithTag(SimpleDialogBoxTag).GetComponent<DialogBoxController>();
            case DialogType.Personalized:
                return GameObject.FindWithTag(PersonalizedDialogBoxTag).GetComponent<DialogBoxController>();
            default:
                throw new ArgumentOutOfRangeException();
                
        }
    }

    public void Show(DialogDef dialogDef)
    {
        _external = dialogDef;
        Show();
    }
    
    public enum Mode
    {
        Bound,
        External
    }
}
