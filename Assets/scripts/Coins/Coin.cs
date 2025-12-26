using UnityEngine;

/// <summary>
/// Represents a collectible coin in the game.
/// Handles coin collection, animation, and value.
/// </summary>
public class Coin : MonoBehaviour
{
    [SerializeField] private int value;

    private Animator anim;
    private coinsLevelHandler coinsLevelHandler;

    private void OnEnable()
    {
        Anim = transform.GetChild(0).GetComponent<Animator>();
        anim.Play("spawn", -1, 0f);
    }

    private void Start()
    {
        if (transform.parent)
        {
            coinsLevelHandler = transform.parent.GetComponent<coinsLevelHandler>();
        }
    }

    /// <summary>
    /// Called when the player collects this coin.
    /// Plays sound and triggers collection animation.
    /// </summary>
    public void getCoin()
    {
        gameObject.GetComponent<SoundSystem>().playDefaultClip();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // Trigger the collection animation on the child object
        transform.GetChild(0).GetComponent<AnimationCoin>().getCoin();
    }

    /// <summary>
    /// Disables the coin after collection.
    /// Notifies the level handler if present.
    /// </summary>
    public void disableCoin()
    {
        if (coinsLevelHandler)
        {
            coinsLevelHandler.coinCollected();
        }

        if (transform.parent != null && transform.name == "enemyCore")
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Re-enables the coin's collider.
    /// </summary>
    public void enableCoin()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    /// <summary>Gets or sets the value of this coin.</summary>
    public int Value { get => value; set => this.value = value; }

    /// <summary>Gets or sets the animator component.</summary>
    public Animator Anim { get => anim; set => anim = value; }
}
