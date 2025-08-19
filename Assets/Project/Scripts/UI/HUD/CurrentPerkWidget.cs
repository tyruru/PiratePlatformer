using System;
using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPerkWidget : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _cooldownIcon;

    private GameSessionModel _gameSessionModel;

    private void Start()
    {
        _gameSessionModel = FindObjectOfType<GameSessionModel>();
    }

    public void SetPerk(PerkDef perkDef)
    {
        _icon.sprite = perkDef.Icon;
    }

    private void Update()
    {
        var cooldown = _gameSessionModel.PerksModel.Cooldown;
        _cooldownIcon.fillAmount = cooldown.RemainingTime / cooldown.Value;
    }
}
