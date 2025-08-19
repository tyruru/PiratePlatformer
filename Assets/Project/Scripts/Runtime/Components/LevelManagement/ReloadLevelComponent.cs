using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSessionModel>();
            session.LoadLastSave();    
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}