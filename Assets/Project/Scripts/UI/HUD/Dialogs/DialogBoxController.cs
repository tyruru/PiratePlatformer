using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DialogBoxController : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    
    [SerializeField] private GameObject _container;
    [SerializeField] private Animator _animator;
    
    [Space]
    [SerializeField] private float _textSpeed = 0.09f;
    
    [FormerlySerializedAs("_typing")]
    [Header("Sounds")]
    [SerializeField] private AudioClip _typingClip;
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    
    [Space] [SerializeField] protected DialogContent _content;
    
    private DialogData _data;
    private int _currentSentenceId;
    private AudioSource _sfxSource;
    private Coroutine _typingRoutine;
    
    private UnityEvent _onComplete;

    protected Sentence CurrentSentence => _data.Sentences[_currentSentenceId];
    
    protected virtual DialogContent CurrentContent => _content;
    
    private void Start()
    {
        _sfxSource = AudioUtils.FindSfxSource();
    }

    public void ShowDialog(DialogData data, UnityEvent onComplete)
    {
        _onComplete = onComplete;
        _data = data;
        _currentSentenceId = 0;
        CurrentContent.Text.text = string.Empty;

        _container.SetActive(true);
        _sfxSource.PlayOneShot(_openClip);
        _animator.SetBool(IsOpen, true);
    }

    protected virtual void OnStartDialogAnimation()
    {
        _typingRoutine = StartCoroutine(TypeDialogTextRoutine());
    }

    public void OnSkip()
    {
        if(_typingRoutine == null)
            return;

        StopTypeAnimation();
        CurrentContent.Text.text = _data.Sentences[_currentSentenceId].Value;
    }

   

    public void OnContinue()
    {
        StopTypeAnimation();
        _currentSentenceId++;

        var isDialogComplete = _currentSentenceId >= _data.Sentences.Length;

        if (isDialogComplete)
        {
            HideDialogBox();
            _onComplete?.Invoke();
        }
        else
        {
            OnStartDialogAnimation();
        }
    }

    private void HideDialogBox()
    {
        _animator.SetBool(IsOpen, false);
        _sfxSource.PlayOneShot(_closeClip);
        CurrentContent.Text.text = string.Empty;
    }

    private void StopTypeAnimation()
    {
        if(_typingRoutine == null)
            return;
        
        StopCoroutine(_typingRoutine);
        _typingRoutine = null;
    }
    
    private IEnumerator TypeDialogTextRoutine()
    {
        CurrentContent.Text.text = string.Empty;
        var sentence = CurrentSentence;
        CurrentContent.TrySetIcon(sentence.Icon);
        
        foreach (var letter in sentence.Value)
        {
            CurrentContent.Text.text += letter;
            _sfxSource.PlayOneShot(_typingClip);
            yield return new WaitForSeconds(_textSpeed);
        }

        _typingRoutine = null;
    }

    public void OnCloseAnimationComplete()
    {
        
    }
}
