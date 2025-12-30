using System.Collections;
using UnityEngine;

/// <summary>
/// Specialized portal that triggers scene changes.
/// Shrinks the player before transitioning to a new scene.
/// </summary>
public class ChangeScenePortal : MonoBehaviour
{
    [SerializeField] private Portal p;
    [SerializeField] private LoadSceneWithTransitionSO scene;
    [SerializeField] private float changeScaleDuration;

    private Coroutine lerpScaleCoroutine;
    private void OnEnable()
    {
        if (p)
        {
            changeScaleDuration = p.CenterObjectDuration;
            p.ActionsBeforeCentering.Add(() =>
            {
                LegacyEvents.GameEvents.Portals.TriggerOnPlayerEnterInGameShopPortal();

            });
            p.ActionsBeforeCenteringTarget.Add(startChangeScale);
            p.ActionsAfterCentering.Add(changeScene);

        }
    }

    /// <summary>
    /// Begins the scale change animation for the object entering the portal.
    /// </summary>
    /// <param name="t">The transform to scale</param>
    public void startChangeScale(Transform t)
    {
        GameManager.Instance.CanMove = false;
        lerpScaleCoroutine = StartCoroutine(lerpPlayerScaleToZero(t, changeScaleDuration));
    }

    /// <summary>
    /// Smoothly scales the player down to zero before scene transition.
    /// </summary>
    /// <param name="player">The player transform to scale</param>
    /// <param name="duration">Time in seconds for the scale animation</param>
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

    /// <summary>
    /// Triggers the scene change after the portal animation completes.
    /// </summary>
    public void changeScene()
    {
        LegacyEvents.GameEvents.triggerOnSceneChanged();
        GameManager.Instance.loadSceneWithTransition(this.scene);
    }
}

