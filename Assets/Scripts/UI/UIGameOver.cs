using UnityEngine;

/// <summary>
/// Manages the Game Over UI display.
/// Shows the game over screen when the player dies.
/// </summary>
public class UIGameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOver;

    private void OnEnable()
    {
        PlayerScript.OnPlayerDied += enableGameOverUI;
    }

    private void OnDisable()
    {
        PlayerScript.OnPlayerDied -= enableGameOverUI;
    }

    /// <summary>
    /// Enables the game over UI and cleans up runtime data.
    /// </summary>
    private void enableGameOverUI()
    {
        GameManager.Instance.destroyRuntimeData();
        gameOver.SetActive(true);
    }
}
