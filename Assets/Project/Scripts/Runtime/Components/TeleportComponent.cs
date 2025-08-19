using System.Collections;
using Project.Scripts;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class TeleportComponent : MonoBehaviour
{
    [SerializeField] private Transform _destTransform;
    [SerializeField] private float _alphaTime = 1f;
    [SerializeField] private float _moveTime = 1f;

    public void Teleport(GameObject target)
    {
        StartCoroutine(AnimateTeleport(target));
    }

    private IEnumerator AnimateTeleport(GameObject target)
    {
        var sprite = target.GetComponent<SpriteRenderer>();
        var inputReader = target.GetComponent<HeroInputReader>();
        inputReader?.SetLock(true);
        
        yield return AlphaAnimation(sprite, 0);
        target.SetActive(false);

        yield return MoveAnimation(target);
        
        target.SetActive(true);
        yield return AlphaAnimation(sprite, 1);
        
        inputReader?.SetLock(false);
    }

    private IEnumerator MoveAnimation(GameObject target)
    {
        var moveTime = 0f;
        while (moveTime < _moveTime)
        {
            moveTime += Time.deltaTime;
            var progress = moveTime / _moveTime;
            target.transform.position = Vector3.Lerp(target.transform.position, _destTransform.position, progress);

            yield return null;
        }
    }
    
    private IEnumerator AlphaAnimation(SpriteRenderer sprite, float destAlpha)
    {
        var time = 0f;
        var spriteAlpha = sprite.color.a;

        while (time < _alphaTime)
        {
            time += Time.deltaTime;
            var progres = time / _alphaTime;
            var tmpAlpha = Mathf.Lerp(spriteAlpha, destAlpha, progres);
            var color = sprite.color;
            color.a = tmpAlpha;
            sprite.color = color;
            
            yield return null;
        }
    }
}
