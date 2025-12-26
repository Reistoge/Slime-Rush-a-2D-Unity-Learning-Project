using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public abstract class ControllerButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    // Start is called before the first frame update
    // intentar de que al mantener en la pantalla este siga en el eje x.
    protected Collider2D targetCollider; // Assign the collider you want to check

    protected void Start()
    {
        if (GameManager.Instance.PlayerInScene != null)
        {
            targetCollider = GameManager.Instance.PlayerInScene.GetComponent<Collider2D>();
            if (targetCollider == null)
            {
                targetCollider = GameObject.FindWithTag("Player").GetComponent<Collider2D>();
            }
        }


    }
    protected void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (targetCollider != null)
            {



                checkMouseTouchPlayer();

            }

        }

    }


    protected bool checkMouseTouchPlayer()
    {
        // this method basically checks what is touched by this area.
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (targetCollider.bounds.IntersectRay(mouseRay))
        {
            print(" you clicked the player ");


            targetCollider.gameObject.transform.rotation = Quaternion.Euler(targetCollider.gameObject.transform.rotation.x, targetCollider.gameObject.transform.rotation.y, 0);
            // Code to execute when the mouse touches the collider
            return true;
        }
        return false;
    }
    protected bool CheckMouseNearPlayerXPosition()
    {
        // Get the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = targetCollider.transform.position.z; // Set the Z position to match the collider

        // Calculate the X distance between the mouse position and the collider center
        float xDistance = Mathf.Abs(mousePosition.x - targetCollider.transform.position.x);

        // Define a threshold for the X position (adjust as needed)
        float xThreshold = 8.0f; // Example threshold value

        if (xDistance < xThreshold)
        {
            print("You clicked near the player on the X-axis.");
            // Code to execute when the mouse touches the collider on the X-axis
            return true;
        }

        return false;
    }
    protected void followSlider()
    {
        float followSpeed = 2f;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Match the Z position of the object

        // Calculate the difference between the mouse position and the object's current position
        float xDifference = mousePosition.x - transform.position.x;

        // Move the object towards the mouse position along the X-axis
        targetCollider.gameObject.transform.position += Vector3.right * xDifference * followSpeed * Time.deltaTime;
    }

    public abstract void OnPointerUp(PointerEventData eventData);
    public abstract void OnPointerDown(PointerEventData eventData);


}
