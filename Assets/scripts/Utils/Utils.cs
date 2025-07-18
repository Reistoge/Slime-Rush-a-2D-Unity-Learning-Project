
using System.Collections;
using System.Numerics;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Utils : MonoBehaviour
{

    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        if (camera == null) return Vector3.zero;
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);

    }
    public static Vector3 RotateVector(float angle, Vector3 axis, Vector3 vector)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        return rotation * vector;
    }
    public static void stopObject(GameObject g)
    {
        if (g.CompareTag("Player"))
        {
            PlayerScript p = g.GetComponent<PlayerScript>();
            p.stopDash();

        }
        g.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

    }


    public static float getCurrentAnimationClipDuration(Animator anim)
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length > 0)
        {
            return clipInfo[0].clip.length;
        }
        return 0f;
    }
    public static float getAnimationClipDuration(Animator animator, string clipName)
    {
        if (animator == null || string.IsNullOrEmpty(clipName))
            return 0f;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0f;
    }
    public static int GetActiveChildCount(Transform t)
    {
        int count = 0;
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.activeInHierarchy)
            {
                count++;

            }
        }

        //print(t.name+": "+count);
        return count;
    }





}

