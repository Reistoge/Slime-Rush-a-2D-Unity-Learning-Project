using System.Collections;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;

[RequireComponent(typeof(AudioSource))]
public class TextDisplayManager : GenericSingleton<TextDisplayManager>
{

    [SerializeField] float characterDisplayDelay = 0.05f;
    [SerializeField] bool enableShake = true;
    [SerializeField] float shakeIntensity = 1.0f;
    [SerializeField] float shakeDuration = 0.2f;
    float minPitch = 1f;
    float maxPitch = 1.2f;
    // each dialogue could have different sound and and text effect, how to achieve this ?
    // event when the dialogue ends.

    // is better to get the dialogue object data. 
    private Coroutine displayCoroutine;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip defaultSoundClip;

    public AudioSource AudioSource { get => audioSource; set => audioSource = value; }
    public float MinPitch { get => minPitch; set => minPitch = value; }
    public float MaxPitch { get => maxPitch; set => maxPitch = value; }
    void OnEnable()
    {
        AudioSource = GetComponent<AudioSource>();
        if (AudioSource.clip == null) AudioSource.clip = defaultSoundClip;
    }
 

    public void PlaySound()
    {


        if (AudioSource && !AudioSource.isPlaying)
        {
            AudioSource.pitch = Random.Range(minPitch, maxPitch);
            AudioSource.Play();
        }
    }

    public void ShowText(Dialogue d)
    {
        if (AudioSource) TextDisplayManager.Instance.AudioSource.clip = d.CharacterSoundClip;
        MinPitch = d.MinPitch;
        MaxPitch = d.MaxPitch;
        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        displayCoroutine = StartCoroutine(DisplayTextWithEffects(d));

    }

    private IEnumerator DisplayTextWithEffects(Dialogue d)
    {

        d.TextMeshPro.text = "";
        for (int i = 0; i < d.Message.Length; i++)
        {

            d.TextMeshPro.text += d.Message[i];

            if (enableShake) StartCoroutine(ShakeEffect(d));
            if (d.Message[i] != ' ') PlaySound();

            yield return new WaitForSeconds(characterDisplayDelay);

        }
        d.OnDialogueEnd?.Invoke();
    }

    private IEnumerator ShakeEffect(Dialogue d)
    {
        Vector3 originalPosition = d.TextMeshPro.transform.localPosition;

        float elapsedTime = 0;
        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            d.TextMeshPro.transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        d.TextMeshPro.transform.localPosition = originalPosition;
    }
}
