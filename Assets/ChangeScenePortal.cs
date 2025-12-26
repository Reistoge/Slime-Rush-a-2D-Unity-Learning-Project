using System.Collections;
using UnityEngine;

public class ChangeScenePortal : MonoBehaviour
{
    [SerializeField] Portal p;

    [SerializeField] LoadSceneWithTransitionSO scene;
    [SerializeField] float changeScaleDuration;
    Coroutine lerpScaleCoroutine;
    void OnEnable()
    {
        p.ActionsBeforeCenteringTarget.Add(startChangeScale);
        p.SimpleActionsAfterCentering.Add(changeScene);

    }

    public void startChangeScale(Transform t)
    {
        GameManager.Instance.CanMove = false;
        lerpScaleCoroutine = StartCoroutine(lerpPlayerScaleToZero(t,changeScaleDuration));
    }
    public IEnumerator lerpPlayerScaleToZero(Transform player, float duration = 1f)
    {
        Vector3 initialScale = player.localScale;
        Vector3 targetScale = Vector3.one * 0.01f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            player.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.localScale = targetScale;
        lerpScaleCoroutine = null;
        player.gameObject.SetActive(false);
        GameManager.Instance.CanMove = true;
    }
    
    public void changeScene()
    {
        GameEvents.triggerOnSceneChanged();
        GameManager.Instance.loadSceneWithTransition(this.scene);

    }
    
}

 