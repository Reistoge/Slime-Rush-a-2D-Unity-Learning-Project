using UnityEngine;

public class HeartContainer : MonoBehaviour
{
    [Header("Heart Settings")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject blueHeartPrefab;
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

        PlayerScript player = GameManager.Instance.PlayerInScene?.GetComponent<PlayerScript>();
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
            CreateBlueHeart(shouldBeBroken); // Create a new heart
            GameManager.Instance.getRuntimeData().playerHp++;
            GameManager.Instance.getRuntimeData().blueHearts++;

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
        newHeart.name = "Heart "; // Name the heart for easier debugging
        SetHeartState(newHeart.GetComponent<Animator>(), isBroken); // set the animator's bools to the broken state -> broke heart = true and fix heart = false 

        if (isBroken) brokenHearts++;
        totalHearts++;
        //GameManager.Instance.getRuntimeData().redHearts++;
    }
    private void CreateBlueHeart(bool isBroken)
    {
        Vector3 position = CalculateHeartPosition(); // Childs * separationBetweenHearts
        GameObject newHeart = Instantiate(blueHeartPrefab, position, Quaternion.identity, transform);
        newHeart.name = "BlueHeart "; // Name the heart for easier debugging
        SetHeartState(newHeart.GetComponent<Animator>(), isBroken); // set the animator's bools to the broken state -> broke heart = true and fix heart = false 

        if (isBroken) brokenHearts++;
        totalHearts++;
        //GameManager.Instance.getRuntimeData().blueHearts++;
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

        PlayerScript player = GameManager.Instance.PlayerInScene?.GetComponent<PlayerScript>();
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

    public void LoadHearts(PlayerScript player)
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

        int currentHp = GameManager.Instance.getRuntimeData().playerHp;
        int redHearts = GameManager.Instance.PlayerConfig.maxHp;
        int blueHearts = GameManager.Instance.getRuntimeData().blueHearts;
        int allHearts = GameManager.Instance.getRuntimeData().playerMaxHp;

        int blueHeartsBroken = blueHearts - (currentHp - redHearts);

        if (allHearts != (redHearts + blueHearts))
        {
            Debug.LogError("The total hearts do not match the player's max health configuration.");
        }
        // print("Total Hearts: " + allHearts + ", Red Hearts: " + redHearts + ", Blue Hearts: " + blueHearts);
        // print("Current HP: " + currentHp + ", Blue Hearts Broken: " + blueHeartsBroken);



        // int heartCount = GameManager.Instance.getRuntimeData().playerMaxHp;
        // int hp = GameManager.Instance.getRuntimeData().playerHp;
        // blueHeartCount = GameManager.Instance.getRuntimeData().blueHearts;
        // for (int i = 0; i < heartCount; i++)
        // {
        //     CreateHeart(i >= hp);
        // }
        // for (int i = 0; i < blueHeartCount; i++)
        // {
        //     CreateBlueHeart(i>=hp-blueHeartCount); // Create blue hearts for missing health
        // }
        for (int i = 0; i < redHearts; i++)
        {
            CreateHeart(i >= currentHp);
        }
        for (int i = 0; i < blueHearts; i++)
        {
            CreateBlueHeart(i >= (blueHearts-blueHeartsBroken)); // Create blue hearts for missing health
        }


    }

    private PlayerScript GetPlayerReference()
    {
        GameObject playerObject = GameManager.Instance.PlayerInScene
            ?? GameManager.Instance.SelectedPlayer;

        return playerObject?.GetComponent<PlayerScript>();
    }
}