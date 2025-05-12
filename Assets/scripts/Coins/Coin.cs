using UnityEngine;
using UnityEngine.UIElements;
 
 
public class Coin : MonoBehaviour
{
    // how it would be for multiple coins ??
    
    [SerializeField] int value;
 
    Animator anim;
    coinsLevelHandler coinsLevelHandler;
    
    void OnEnable(){
        Anim = transform.GetChild(0).GetComponent<Animator>();
        anim.Play("spawn",-1,0f);
  
        //GameManager.instance.instantiateAppearEffect(transform,1);
    }

    private void Start()
    {
         
        if(transform.parent) coinsLevelHandler=transform.parent.GetComponent<coinsLevelHandler>();
        
    }
    public void getCoin()
    {
        // THIS METHOD is call by the player triggers the coin collider
        gameObject.GetComponent<SoundSystem>().playDefaultClip();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // we call the animation of the animation child to start the "destruction" of the coin.

        transform.GetChild(0).GetComponent<AnimationCoin>().getCoin();

        // is core
        
    }
    public void disableCoin()
    {
        if(coinsLevelHandler) coinsLevelHandler.coinCollected();
        if(transform.parent!=null && transform.name == "enemyCore"){
            
            transform.parent.gameObject.SetActive(false);
            
        }
     
        else{
            gameObject.SetActive(false);

        }
    }
    public void enableCoin()
    {
        
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        // gameObject.SetActive(true);
    }
    public int Value { get => value; set => this.value = value; }
    public Animator Anim { get => anim; set => anim = value; }
}
