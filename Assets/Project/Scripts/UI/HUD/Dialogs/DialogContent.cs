using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogContent : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _icon;
    
    public TextMeshProUGUI Text => _text;

    public void TrySetIcon(Sprite sprite)
    {
        if(_icon != null)
            _icon.sprite = sprite;
    }
    
}
