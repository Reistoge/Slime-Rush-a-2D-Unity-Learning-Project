using UnityEngine;
using UnityEngine.UIElements;
 
 
public class Coin : MonoBehaviour
{
    // how it would be for multiple coins ??
    
    [SerializeField] int value;
    Animator anim;
    private void Start()
    {
        Anim = transform.GetChild(0).GetComponent<Animator>();
    }
    public void getCoin()
    {
        // THIS METHOD is call by the player triggers the coin collider
        gameObject.GetComponent<SoundSystem>().playDefaultClip();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // we call the animation of the animation child to start the "destruction" of the coin.

        transform.GetChild(0).GetComponent<AnimationCoin>().getCoin();
    }
    public void disableCoin()
    {
        
        gameObject.SetActive(false);
    }
    public void enableCoin()
    {
        
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        // gameObject.SetActive(true);
    }
    public int Value { get => value; set => this.value = value; }
    public Animator Anim { get => anim; set => anim = value; }
}
