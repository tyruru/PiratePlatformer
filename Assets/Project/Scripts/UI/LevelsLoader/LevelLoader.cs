using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _transitionTime = 1f;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void LoadLevel()
    {
        InitLoader();
    }

    private static void InitLoader()
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    private static readonly int Enabled = Animator.StringToHash("Enabled");

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Show(string sceneName)
    {
        StartCoroutine(StartAnimation(sceneName));
    }

    private IEnumerator StartAnimation(string sceneName)
    {
        _animator.SetBool(Enabled, true);
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(sceneName);
        _animator.SetBool(Enabled, false);
    }
}
