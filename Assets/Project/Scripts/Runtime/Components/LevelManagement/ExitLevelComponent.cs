using Project.Scripts.Model;
using UnityEngine;

public class ExitLevelComponent : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    
    public void Exit()
    {
        var session = FindObjectOfType<GameSessionModel>();
        session.Save();
        var loader = FindObjectOfType<LevelLoader>();
        loader.Show(_sceneName);
    }
}