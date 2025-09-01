using UnityEngine;

public class RandomItemBoxAnimHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] RandomItemBox itemBox;
    private readonly int breakHash = Animator.StringToHash("break");
    private readonly int cantPurhcaseHash = Animator.StringToHash("cantPurchase");
    private readonly int idleHash = Animator.StringToHash("idle");
    
 
    public void playBreakAnimation()
    {
        animator.Play(breakHash,-1,0);
    }
    public void playCantPurchaseAnimation()
    {
        animator.Play(cantPurhcaseHash, -1, 0);
    }
    public void destroyMainObject()
    {
        itemBox.destroyRandomBox();
    }
    public void displayItems(){
         
    }
}
