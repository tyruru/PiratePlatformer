using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    [SerializeField] private GameObject _normal;
    [SerializeField] private GameObject _pressed;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        
        if(_normal != null)
            _normal.SetActive(state != SelectionState.Pressed);
        
        if(_pressed != null)
            _pressed.SetActive(state == SelectionState.Pressed);
    }
}
