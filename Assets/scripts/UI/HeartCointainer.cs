using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    [Header("Heart Settings")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartInitialPosition;
    [SerializeField] private float separationBetweenHearts = 32f;

    [Header("State")]
    [SerializeField] private int totalHearts;
    [SerializeField] private int brokenHearts;

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        PlayerScript.OnPlayerInstantiated += LoadHearts;
        PlayerScript.OnPlayerIsDamaged += BreakHearts;
        PlayerScript.OnPlayerIsHealed += FixHearts;
        PlayerScript.OnPlayerAddHeart += AddHeart;
    }

    private void UnsubscribeFromEvents()
    {
        PlayerScript.OnPlayerInstantiated -= LoadHearts;
        PlayerScript.OnPlayerIsDamaged -= BreakHearts;
        PlayerScript.OnPlayerIsHealed -= FixHearts;
        PlayerScript.OnPlayerAddHeart -= AddHeart;
    }

    private void AddHeart(int amount)
    {
        if (amount <= 0) return;

        PlayerScript player = GameManager.instance.PlayerInScene?.GetComponent<PlayerScript>();
        if (player == null) return;

        // First, fix one broken heart if player has missing health
        bool hasFixedHeart = false;
        if (player.Hp < player.MaxHp)
        {
            FixFirstBrokenHeart();
            hasFixedHeart = true;
        }

        // Then add new heart(s)
        for (int i = 0; i < amount; i++)
        {
            bool shouldBeBroken = hasFixedHeart && i == 0; // Only the new heart should be broken if we fixed one
            CreateHeart(shouldBeBroken);
        }
    }

    private void FixFirstBrokenHeart()
    {
        foreach (Transform child in transform)
        {
            Animator heartAnimator = child.GetComponent<Animator>();
            if (heartAnimator != null && heartAnimator.GetBool("brokeHeart"))
            {
                SetHeartState(heartAnimator, false);
                brokenHearts--;
                return;
            }
        }
    }

    private void CreateHeart(bool isBroken)
    {
        Vector3 position = CalculateHeartPosition(); // Childs * separationBetweenHearts
        GameObject newHeart = Instantiate(heartPrefab, position, Quaternion.identity, transform);
        
        SetHeartState(newHeart.GetComponent<Animator>(), isBroken); // set the animator's bools to the broken state -> broke heart = true and fix heart = false 
        
        if (isBroken) brokenHearts++; 
        totalHearts++;
    }

    private Vector3 CalculateHeartPosition()
    {
        // we calculate the next position based on the number of hearts and the separation between each one (delta y).
        Vector3 basePosition = heartInitialPosition.position;
        float deltaY = separationBetweenHearts * totalHearts;
        return new Vector3(basePosition.x, basePosition.y + deltaY, 1);
    }

    private void SetHeartState(Animator heartAnimator, bool isBroken)
    {
        if (heartAnimator == null) return;
        
        heartAnimator.SetBool("brokeHeart", isBroken);
        heartAnimator.SetBool("fixHeart", !isBroken);
    }

    private void FixHearts(int amount)
    {
        if (amount <= 0 || brokenHearts == 0) return;

        PlayerScript player = GameManager.instance.PlayerInScene?.GetComponent<PlayerScript>();
        if (player == null) return;

        int healedCount = 0;
        foreach (Transform child in transform)
        {
            if (healedCount >= amount || healedCount >= brokenHearts) break;

            Animator heartAnimator = child.GetComponent<Animator>();
            if (heartAnimator != null && heartAnimator.GetBool("brokeHeart"))
            {
                SetHeartState(heartAnimator, false);
                healedCount++;
                brokenHearts--;
                 
            }
        }
    }

    private void BreakHearts(int damage)
    {
        if (damage <= 0) return;

        for (int i = totalHearts - 1; i >= 0 && damage > 0; i--)
        {
            if (i >= transform.childCount) continue;

            Animator heartAnimator = transform.GetChild(i).GetComponent<Animator>();
            if (heartAnimator != null && !heartAnimator.GetBool("brokeHeart"))
            {
                SetHeartState(heartAnimator, true);
                damage--;
                brokenHearts++;
            }
        }

        if (damage > 0)
        {
            Debug.Log("No more hearts to break, player should die");
        }
    }

    public void LoadHearts()
    {
        if (heartPrefab == null)
        {
            Debug.LogError("Heart prefab is not assigned!");
            return;
        }

        ClearExistingHearts();
        LoadHeartsVertical();
    }

    private void ClearExistingHearts()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        totalHearts = 0;
        brokenHearts = 0;
    }

    private void LoadHeartsVertical()
    {
        PlayerScript player = GetPlayerReference();
        if (player == null) return;

        int heartCount = player.PlayerConfig.maxHp;
        for (int i = 0; i < heartCount; i++)
        {
            CreateHeart(i >= player.Hp);
        }
    }

    private PlayerScript GetPlayerReference()
    {
        GameObject playerObject = GameManager.instance.PlayerInScene 
            ?? GameManager.instance.SelectedPlayer;
        
        return playerObject?.GetComponent<PlayerScript>();
    }
}