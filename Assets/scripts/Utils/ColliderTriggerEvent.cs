using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent eventTrigger;
    [SerializeField] CommonActionStrategy actionStrategy; // Assign in Inspector
    [SerializeField] private DestroyAction destroyAction; // Assign in Inspector
    [SerializeField] private DeactivateAction deactivateAction; // Assign in Inspector

    public DestroyAction DestroyAction { get => destroyAction; set => destroyAction = value; }
    public DeactivateAction DeactivateAction { get => deactivateAction; set => deactivateAction = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        eventTrigger?.Invoke(); 
        actionStrategy?.Play(this, collision);
        print("asd");
    }
    public void setStrategy(CommonActionStrategy actionStrategy){
        this.actionStrategy = actionStrategy;
    }
}

// Abstract Base Class (Strategy Pattern)
public abstract class CommonActionStrategy : ScriptableObject
{
    [SerializeField] private float waitFor = 0f; // Delay before execution

    public void Play(MonoBehaviour caller, Collider2D col)
    {
        caller.StartCoroutine(ExecuteAfterDelay(col));
    }

    private IEnumerator ExecuteAfterDelay(Collider2D col)
    {
        yield return new WaitForSeconds(waitFor);
        ExecuteAction(col);
    }

    protected abstract void ExecuteAction(Collider2D col);
}

// Destroy Action Strategy
[CreateAssetMenu(menuName = "ActionStrategies/DestroyAction")]
public class DestroyAction : CommonActionStrategy
{
    protected override void ExecuteAction(Collider2D col)
    {
        if (col != null)
        {
            Destroy(col.gameObject);
        }
    }
}

//  Deactivate Action Strategy
[CreateAssetMenu(menuName = "ActionStrategies/DeactivateAction")]
public class DeactivateAction : CommonActionStrategy
{
    protected override void ExecuteAction(Collider2D col)
    {
        if (col != null)
        {
            col.gameObject.SetActive(false);
        }
    }
}
