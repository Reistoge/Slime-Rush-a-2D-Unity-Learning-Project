using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private bool isPermanent = false;
    [SerializeField] private HealStrategy healStrategy = HealStrategy.Heal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null)
        {
            UseItem(player);
            Destroy(gameObject);
        }
    }

    private void UseItem(PlayerScript player)
    {
        switch (healStrategy)
        {
            case HealStrategy.Heal:
                player.heal(healAmount);
                break;
            case HealStrategy.AddHeart:
                player.addHeart(isPermanent);
                break;
            default:
                Debug.LogWarning($"Invalid strategy selected: {healStrategy}");
                break;
        }
    }

    private enum HealStrategy
    {
        Heal,
        AddHeart
    }
}