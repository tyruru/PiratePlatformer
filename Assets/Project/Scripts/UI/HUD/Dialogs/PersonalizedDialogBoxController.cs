
using UnityEngine;

public class PersonalizedDialogBoxController : DialogBoxController
{
    [SerializeField] private DialogContent _rightContent;

    protected override DialogContent CurrentContent => CurrentSentence.Side == Side.Left ? _content : _rightContent;
    protected override void OnStartDialogAnimation()
    {
        _rightContent.gameObject.SetActive(CurrentSentence.Side == Side.Right);
        _content.gameObject.SetActive(CurrentSentence.Side == Side.Left);
        base.OnStartDialogAnimation();
    }
}
