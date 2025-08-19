using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizeText : MonoBehaviour
{
    [SerializeField] private string _key;
    [SerializeField] private bool _capitalize;

    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        LocalizationManager.I.OnLocaleChanged += OnLocaleChanged;
        Localize();
    }

    private void OnLocaleChanged()
    {
        Localize();
    }

    private void Localize()
    {
        var localized = LocalizationManager.I.Localize(_key);
        
        _text.text = _capitalize ? localized.ToUpper() : localized;
    }

    private void OnDestroy()
    {
        LocalizationManager.I.OnLocaleChanged -= OnLocaleChanged;
    }
}
