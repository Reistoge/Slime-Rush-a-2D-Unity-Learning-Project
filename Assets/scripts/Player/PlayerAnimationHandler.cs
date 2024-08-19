using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerAnimationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void enterBarrel()
    {
        anim.SetBool("inBarrel", true);
        anim.Play("hover", -1, 0);
    }

    public void playRandomAnimationInsideBarrel()
    {
        resetHideSpeed();
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                anim.Play("idleBarrel1", -1, 0f);
                break;
            case 1:
                anim.Play("idleBarrel2", -1, 0f);
                break;
            case 2:
                anim.Play("hiding", -1, 0f);
                break;
        }
    }

    public void hideAnim()
    {
        anim.Play("hiding", -1, 0f);
    }

    public void onPlayerOut()
    {
        anim.SetBool("inBarrel", false);
        anim.SetFloat("hidingSpeed", 3f);
        anim.Play("onShootCannon", -1, 0f);
        transform.position = transform.parent.position;
    }

    public void resetHideSpeed()
    {
        anim.SetFloat("hidingSpeed", 1f);
    }

    public void playDash()
    {
        anim.Play("dash");
    }

    public void stopDash()
    {
        anim.SetBool("isDashing", false);
    }

    public void playDamageAnim()
    {
        anim.Play("takeDamage", -1, 0f);
    }
}
