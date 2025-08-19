
using UnityEngine;

[CreateAssetMenu (menuName = "Defs/Dialog",fileName = "Dialog")]
public class DialogDef : ScriptableObject
{
    [SerializeField] private DialogData _dialogData;
    public DialogData DialogData => _dialogData;
}
