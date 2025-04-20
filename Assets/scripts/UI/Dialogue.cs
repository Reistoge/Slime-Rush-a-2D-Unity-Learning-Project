

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
   
    [SerializeField] string message;
    [SerializeField] AudioClip characterSoundClip;
    [SerializeField] float minPitch = 1f;
    [SerializeField] float maxPitch = 1.2f;
    [SerializeField] UnityEvent onDialogueEnd;
    [SerializeField] bool playOnStart;
      TextMeshPro textMeshPro;

    public float MinPitch { get => minPitch; set => minPitch = value; }
    public float MaxPitch { get => maxPitch; set => maxPitch = value; }
    public AudioClip CharacterSoundClip { get => characterSoundClip; set => characterSoundClip = value; }
    public string Message { get => message; set => message = value; }
    public UnityEvent OnDialogueEnd { get => onDialogueEnd; set => onDialogueEnd = value; }
    public TextMeshPro TextMeshPro { get => textMeshPro; set => textMeshPro = value; }

    private void OnEnable(){
        textMeshPro= this.GetComponent<TextMeshPro>();
        // Invoke("ShowDialogue", 1f);
    }
    private void Start()
    {
        if(playOnStart){
            ShowDialogue();
        }
    }
    public void ShowDialogue(float t ){
        Invoke("ShowDialogue", t);
    }
    public void ShowDialogue()
    {
        
        if(message == textMeshPro.text) return;
        TextDisplayManager.Instance.ShowText(this);
    }
   

}
