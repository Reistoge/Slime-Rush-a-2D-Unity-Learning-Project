using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Boundaries : MonoBehaviour
{

    [SerializeField] float amount;
    [SerializeField] float speed;
    [SerializeField] bool shake;
    [SerializeField] float magnitude;
    [SerializeField] bool moving;
    [SerializeField] GameObject wallLeft;
    [SerializeField] GameObject wallRight;
    [SerializeField] LayerMask excludeLayers;
    [SerializeField] UnityEvent onMoveWallsEnd;
    Coroutine moveWallsLeft;
    Coroutine moveWallsRight;

    public void moveWalls()
    {
        if (wallLeft && wallRight)
        {
            wallLeft.GetComponent<Collider2D>().excludeLayers = excludeLayers; // Specify the layer(s) to exclude

            wallRight.GetComponent<Collider2D>().excludeLayers = excludeLayers;
            moveWallsLeft = StartCoroutine(moveWall(wallLeft, amount, speed));
            StartCoroutine(moveWall(wallRight, -amount, speed));

        }
    }

    IEnumerator moveWall(GameObject wall, float amount, float speed)
    {
        // wall.transform.GetChild(0).gameObject.SetActive(true);
        moving = true;
        Vector3 startPos = wall.transform.position;
        Vector3 endPos = new Vector3(wall.transform.position.x + amount, startPos.y, wall.transform.position.z);


        float duration = Mathf.Abs(amount / speed); // Ensure positive duration
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = 0;
            float y = 0;

            if (shake)
            {
                x = Random.Range(-1f, 1f) * magnitude;
                y = Random.Range(-1f, 1f) * magnitude;
            }

            // Correct Lerp factor (elapsed / duration)
            float t = elapsed / duration;
            wall.transform.position = Vector3.Lerp(startPos, endPos, t) + new Vector3(x, y, 0);

            elapsed += Time.unscaledDeltaTime; // Use scaled deltaTime
            yield return null; // More efficient than `WaitForEndOfFrame()`
        }

        wall.transform.position = endPos;
        moving = false;
        moveWallsLeft = null;
        onMoveWallsEnd?.Invoke();

    }
  



}
