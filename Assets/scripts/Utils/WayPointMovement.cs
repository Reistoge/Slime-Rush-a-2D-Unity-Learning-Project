using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class WayPointMovement : MonoBehaviour
{
    protected IEnumerator moveToPosition(Transform finalPos, float speed)
    {
        moveVariables[index].OnStartMovement?.Invoke();
        isMoving = true;
        if(finalPos == null) yield break;
        finalPos.rotation = Quaternion.Euler(0f, 0f, finalPos.rotation.z);
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        // Calculate total distance to determine duration
        float distance = Vector2.Distance(transform.position, finalPos.position);
        float duration = distance / speed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Smooth out the interpolation
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(startPosition, new Vector2(finalPos.position.x, finalPos.position.y) , smoothT);
            transform.rotation = Quaternion.Lerp(startRotation,
                Quaternion.Euler(0f, 0f, finalPos.rotation.z), smoothT);

            yield return null;
        }

        // Ensure we reach the exact final position
        transform.position = new Vector2(finalPos.position.x, finalPos.position.y);
        transform.rotation = Quaternion.Euler(0f, 0f, finalPos.rotation.z);

        moveVariables[index].OnEndMovement?.Invoke();
        isMoving = false;
    }

    protected void createQueueMove(MoveBehaviour[] values)
    {
        IEnumerator coroutine = null;

        foreach (MoveBehaviour v in values)
        {
            coroutine = moveToPosition(v.wayPoint, v.velocity);
            coroutineQueue.Enqueue(coroutine);
        }
    }

    protected IEnumerator DequeueCoroutines(MoveBehaviour[] values)
    {
        coroutineQueue.Clear();
        createQueueMove(moveVariables);
        index = 0;
        while (coroutineQueue.Count > 0)
        {

            //            print("start "+  index);
            yield return new WaitForSeconds(delay);
            yield return coroutineQueue.Dequeue();
            //          print("end "+ index);
            index++;
            if (coroutineQueue.Count == 1 && AlwaysMove && isMoving == false)
            {
                // print("end cycle");
                index = 0;
                createQueueMove(values);
            }


        }


    }

    public void startMove()
    {

        movingCorutine = StartCoroutine(DequeueCoroutines(moveVariables));
    }
    public void startMove(float time)
    {
        Invoke("startMove", time);
    }
    public void stopMoving()
    {
        StopCoroutine(movingCorutine);
    }

    protected Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    [Serializable]
    public class MoveBehaviour
    {
        [SerializeField] public Transform wayPoint;
        [SerializeField] public float velocity;
        [SerializeField] public UnityEvent OnStartMovement;
        [SerializeField] public UnityEvent OnEndMovement;


    }


    [SerializeField] MoveBehaviour[] moveVariables;
    int index = 0;
    [SerializeField] bool pingPong;
    [SerializeField] bool isMoving;
    [SerializeField] float delay;
    [SerializeField] bool playOnStart = true;
    [SerializeField] GameObject wayPoints;
    Coroutine movingCorutine;

    public MoveBehaviour[] MoveVariables { get => moveVariables; set => moveVariables = value; }
    protected bool AlwaysMove { get => pingPong; set => pingPong = value; }
    public GameObject WayPoints { get => wayPoints; set => wayPoints = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (playOnStart) startMove();
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(WayPointMovement)), CanEditMultipleObjects]
public class MoveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WayPointMovement handler = (WayPointMovement)target;
        if (GUILayout.Button("Add waypoint"))
        {
            List<WayPointMovement.MoveBehaviour> temp = handler.MoveVariables.ToList();
            GameObject newWaypoint = new GameObject("p" + handler.MoveVariables.Length);
            newWaypoint.transform.position = handler.transform.position;
            newWaypoint.transform.SetParent(handler.WayPoints.transform);
            temp.Add(new WayPointMovement.MoveBehaviour());
            handler.MoveVariables = temp.ToArray();
            handler.MoveVariables[handler.MoveVariables.Length - 1].wayPoint = newWaypoint.transform;
        }
    }
}
#endif

