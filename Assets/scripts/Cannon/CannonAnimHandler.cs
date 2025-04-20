using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract class CannonAnimHandler : MonoBehaviour
// {
//     Cannon cannon;
//     Animator anim;
//     public void Start()
//     {
//         anim = GetComponent<Animator>();
//     }
//     public void onEnterEntityAnim()
//     {
//         anim.Play("onEnterEntity", -1, 0f);
//     }
//     public void playShootAnim()
//     {
//         anim.Play("shoot", -1, 0f);
//         anim.SetFloat("chargeSpeed", 1);
//     }
//     public void playShootAnim(float speed)
//     {
//         anim.Play("shoot", -1, 0f);
//         anim.SetFloat("chargeSpeed", speed);
//     }
//     public void increaseChargeSpeedAnim(float chargeMultiplier)
//     {
//         anim.SetFloat("chargeSpeed", getChargeMultiplier() * chargeMultiplier);
//     }
//     public void setChargeMultiplier(float value)
//     {
//         anim.SetFloat("chargeSpeed", value);
//     }
//     public float getChargeMultiplier()
//     {
//         return anim.GetFloat("chargeSpeed");
//     }
//     IEnumerator waitForAnimation()
//     {
//         if (cannon.InsideObject.CompareTag("Player"))
//         {
//             cannon.InsideObject.GetComponent<PlayerScript>().AnimatorHandler.playInvisible();
//             yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
//             if (GameManager.instance.InBarrel && cannon.inBarrel)
//             {
//                 cannon.InsideObject.GetComponent<PlayerScript>().AnimatorHandler.enterBarrel();

//             }
//         }




//     }
//     public void hideObject()
//     {
//         if (cannon.InsideObject != null)
//         {
//             cannon.GetComponent<Rigidbody2D>().GetComponent<PlayerScript>().AnimatorHandler.onPlayerOut();
//         }

//     }
//     public void switchCharging()
//     {
//         cannon.switchCharging();
//     }
//     public void waitToEndEnterCannonAnimation()
//     {
//         StartCoroutine(waitForAnimation());
//     }
// }
