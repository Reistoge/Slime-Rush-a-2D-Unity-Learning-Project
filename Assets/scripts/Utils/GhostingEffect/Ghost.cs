using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color initialColor;
    [SerializeField] Color c;
    [SerializeField] private float fadeSpeed = 2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // initialColor= spriteRenderer.color;
    }

    void OnEnable()
    {
        // initialColor.a = 1f; // Reset transparency when reused
        // spriteRenderer.color=initialColor;
       
        c = initialColor;
    }

    void Update()
    {
        // Gradually fade out the ghost
        c.a -= fadeSpeed * Time.deltaTime;
        spriteRenderer.color = c;

        // The pooling system will handle the ghost's deactivation
    }
}
