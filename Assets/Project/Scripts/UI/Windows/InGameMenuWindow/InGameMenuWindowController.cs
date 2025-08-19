using Project.Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuWindowController : AnimatedWindow
{
    private float _defaultTimeScal;
    protected override void Start()
    {
        base.Start();
        _defaultTimeScal = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void OnShowSettings()
    {
        WindowUtils.CreateWindow(LoadPaths.Resources.SettingsWindowPath);
    }

    public void OnExit()
    {
        SceneManager.LoadScene("MainMenu");
        
        var session = FindObjectOfType<GameSessionModel>();
        Destroy(session);
    }
    
    private void OnDestroy()
    {
        Time.timeScale = _defaultTimeScal;
    }
}
