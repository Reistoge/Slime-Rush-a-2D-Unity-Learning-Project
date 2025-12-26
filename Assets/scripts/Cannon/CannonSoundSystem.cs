public class CannonSoundSystem : SoundSystem
{


    public void playChargeSfx()
    {
        // charge clips will be always the first clip
        AS.priority = 0;
        AS.pitch = 1f;
        AS.volume = 0.2f;
        AS.priority = 220;
         
        playDefaultClip();
    }
    public void playShootSfx()
    {
        // shoot sfx will be always the second clip
        setClip(clips[1]);
        AS.priority = 0;
        AS.pitch = 1f;
        AS.volume = 0.2f;
        AS.priority = 220;
       
        AS.Play();

    }
    public void playRotateSfx()
    {
        setClip(clips[2]);
        AS.pitch = 2.2f;
        AS.volume = 0.2f;
        AS.priority = 220;
        
        AS.Play();
    }
    public void playReadySfx()
    {
        AS.volume = 0.2F;
         
        // a random number to set the clip of the ready sound effect
        if (UnityEngine.Random.Range(1, 10) > 5)
        {
            setClip(clips[3]);

        }
        else
        {
            setClip(clips[4]);
        }
         
        AS.Play();

    }
    new public void stop(){
        AS.Stop();
    }


}
