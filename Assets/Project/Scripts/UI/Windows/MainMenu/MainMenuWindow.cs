using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuWindow : AnimatedWindow
{
    private Action _closeAction;
    
    public void OnShowSettings()
    {
        WindowUtils.CreateWindow(LoadPaths.Resources.SettingsWindowPath);
    }

    public void OnStartGame()
    {
        _closeAction = () =>
        {
            var loader = FindObjectOfType<LevelLoader>();
            loader.Show("Level1");            
        };
        Close();
    }

    public void OnLanguages()
    {
        WindowUtils.CreateWindow(LoadPaths.Resources.LocalizationWindowPath);
    }

    public void OnExit()
    {
        _closeAction = () => 
        { 
            Application.Quit();
        
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif 
        };
        
        Close();
    }

    public override void OnCloseAnimationComplete()
    {
        base.OnCloseAnimationComplete();
        _closeAction.Invoke();
        
    }
}
