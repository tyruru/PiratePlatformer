using Project.Scripts.Model;
using UnityEngine;

public class RefillFuelComponent : MonoBehaviour
{
    private GameSessionModel _gameSession;


    private void Start()
    {
        _gameSession = FindObjectOfType<GameSessionModel>();
    }

    public void Refill()
    {
        _gameSession.PlayerData.Fuel.Value = 100;
    }
}
