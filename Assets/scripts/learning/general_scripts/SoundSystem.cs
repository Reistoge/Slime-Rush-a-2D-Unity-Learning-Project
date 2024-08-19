using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    protected AudioSource AS;
    [SerializeField] protected AudioClip[] clips;

    protected void OnEnable(){
        AS = GetComponent<AudioSource>();

    }
     
    protected AudioClip findClip(string name)
    {
        // we search for the clip by his name
        // class use only 
        foreach (AudioClip clip in this.clips)
        {
            if (clip.name.Equals(name))
            {
                return clip;
            }
        }
        return null;
    }
    protected void setClip(AudioClip clip)
    {
        // we set load the clip to the audiosource component
        // protected because is class use only
        this.AS.clip = clip;
    }
    public void playClip(string name)
    {

        if (findClip(name) == null)
        {
            print("this clip doesnt exist in this object");
        }

        else
        {
            setClip(findClip(name));
            AS.Play();
        }
    }
    public void playDefaultClip()
    {
        setClip(clips[0]);
        AS.Play();

    }
    public void stop()
    {
        AS.Stop();
    }
    public void writeToConsole()
    {
        Debug.Log("Im not null");
    }
    public void downVolume(float value)
    {
        if (AS.volume - value <= 0)
        {
            AS.volume = 0;
        }
        else
        {
            AS.volume = AS.volume - value;
        }
    }

}

