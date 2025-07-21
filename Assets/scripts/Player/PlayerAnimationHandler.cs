using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Drawing;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.Rendering;


public class PlayerAnimationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Animator anim;
    [SerializeField] Animator squishAnim;
    SpriteRenderer sr;
    [SerializeField] GhostManager ghostingEffect;
    [SerializeField] VisualEffectHandler particles;
    [SerializeField] VisualEffectHandler damageParticles;
    [SerializeField] Material onHitMaterial;
    [SerializeField] Material basicMaterial;
    [SerializeField] GameObject slime;
    

 
    void Start()
    {
        
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    public void squish(SquishType type)
    {

        switch (type)
        {
            case SquishType.up:
                squishAnim.Play("squishUp", -1, 0f);
                break;
            case SquishType.right:
                squishAnim.Play("squishRight", -1, 0f);
                break;
            case SquishType.down:
                squishAnim.Play("squishDown", -1, 0f);
                break;
            case SquishType.left:
                squishAnim.Play("squishLeft", -1, 0f);
                break;


        }


    }
    public void squish(string args)
    {

        switch (args.ToLower())
        {
            case "up":
                squish(SquishType.up);
                break;
            case "down":
                squish(SquishType.down);
                break;
            case "left":
                squish(SquishType.left);
                break;
            case "right":
                squish(SquishType.right);
                break;

        }

    }

    public void enterBarrel()
    {

        anim.SetBool("inBarrel", true);
        anim.Play("hover", -1, 0);

    }
    public void playInvisible()
    {
        anim.Play("invisible", -1, 0f);
    }
    public void playHurt()
    {
        ghostingEffect.stopGhosting();
        anim.Play("hurt", -1, 0f);
        damageParticles.TriggerEffect(transform.position);
        StartCoroutine(playHurtAnimation());
    }
    public void playHurt(float cantTakeDamageTime)
    {
        ghostingEffect.stopGhosting();
        anim.Play("hurt", -1, 0f);
        damageParticles.TriggerEffect(transform.position);
        StartCoroutine(playHurtAnimation());
        StartCoroutine(canTakeDamageMaterialHandler(cantTakeDamageTime));
        // squish(SquishType.down);
    }
     
    public void stopHurtAnimation()
    {
        if(anim.GetBool("canTakeDamage")==false){
            anim.SetBool("canTakeDamage", true);

        }
        //slime.GetComponent<IDamageable>().CanTakeDamage = true;
    }
   

    public void playDie()
    {
        PlayerScript player = slime.GetComponent<PlayerScript>();
        player.enabled=false;
        
        anim.SetBool("canTakeDamage", false);
        anim.ResetTrigger("damaged");
        anim.SetBool("isDashing", false);
        anim.SetBool("walk", false);
        anim.SetBool("inBarrel", false);
        anim.Play("die", -1, 0f);

        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
       
    

    }
    public void endPlayerDieAnimation(){

        sr.enabled = false;
        slime.SetActive(false);
        

    }


    IEnumerator playHurtAnimation()
    {
        anim.SetBool("canTakeDamage", false);
        yield return new WaitForSeconds(Utils.getCurrentAnimationClipDuration(anim));
        anim.SetBool("canTakeDamage", true);
    }
    IEnumerator canTakeDamageMaterialHandler(float delay)
    {
        // it will be better if this function is on the player script 
        slime.GetComponent<IDamageable>().CanTakeDamage = false;
        sr.material = onHitMaterial;
        yield return new WaitForSeconds(delay);
        sr.material = basicMaterial;
         
        slime.GetComponent<IDamageable>().CanTakeDamage = true;
    }
    

    public void triggerParticleEffect(Vector3 pos)
    {
        particles.TriggerEffect(pos);
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
    public void playChargeIn()
    {

        GameManager.Instance.CanMove = false;
        anim.Play("chargeIn", -1, 0f);
    }
    public void playChargeOut()
    {
        GameManager.Instance.CanMove = true;
        anim.SetBool("walk", false);
        anim.Play("idle1", -1, 0f);
    }

    public void onPlayerOut()
    {
        anim.SetBool("inBarrel", false);
        anim.SetFloat("hidingSpeed", 3f);
        anim.Play("onShootCannon", -1, 0f);
        transform.position = slime.transform.position;
    }

    public void resetHideSpeed()
    {
        anim.SetFloat("hidingSpeed", 1f);
    }

    public void playDash()
    {
        ghostingEffect.startGhosting();
        // print("a");


        anim.Play("dash", -1, 0f);
    }
    public void ghosting(float duration)
    {

        StartCoroutine(ghostingRoutine(duration));
    }
    IEnumerator ghostingRoutine(float duration)
    {
        ghostingEffect.startGhosting();
        yield return new WaitForSeconds(duration);
        ghostingEffect.stopGhosting();
    }
    public void playOnTouchGround()
    {
        anim.Play("onTouchGround", -1, 0f);

        particles.TriggerEffect(transform.position);

    }

    public void stopDash()
    {
        ghostingEffect.stopGhosting();
        anim.SetBool("isDashing", false);
        //anim.Play("idle", -1, 0f);
    }


    public void playDamageAnim()
    {
        anim.Play("takeDamage", -1, 0f);
    }
    public void playJump()
    {

        anim.Play("Jump", -1, 0f);
    }
    public void playWalkAnimation(float value)
    {
        if (value != 0)
        {
            if (value == -1 && sr.flipX == false)
            {
                sr.flipX = true;
            }
            else if (value == 1 && sr.flipX == true)
            {
                sr.flipX = false;
            }
            if (anim.GetBool("walk") == false)
            {
                anim.Play("walk", -1, 0f);

            }
            anim.SetBool("walk", true);


        }
        else
        {
            if (anim.GetBool("walk") == true)
            {
                //playIdle();

            }
            // sr.flipX = false;
            anim.SetBool("walk", false);

        }


    }
    public float getCurrentClipDuration()
    {
        return anim.GetCurrentAnimatorClipInfo(0).Length;
    }
    public bool checkCurrentClipName(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public float getCurrentClipDuration(Animator anim)
    {
        return anim.GetCurrentAnimatorClipInfo(0).Length;
    }
    public bool checkCurrentClipName(Animator anim, string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void stopWalk()
    {


    }
    public void playIdle()
    {
        anim.Play("idle1", -1, 0f);
    }

    internal void playFall()
    {
        anim.Play("falling", -1, 0f);
    }
    public enum SquishType
    {
        up,
        down,
        left,
        right,
        mix,

    }

}
