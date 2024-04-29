using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundManager : MonoBehaviour
{
    private AudioSource bgmSource;
    private AudioSource effectSource;
    
    public AudioClip BackGroundMusic;
    public List<AudioClip> FallSound;
    public List<AudioClip> HitSound;
    public List<AudioClip> GuardSound;
    public AudioClip GrabSound;

    public AudioClip MenuScrollSound;
    public AudioClip MenuEnterSound;
    
    public enum SoundSet
    {
        BackGroundMusic = 0,
        Fall,
        Hit,
        Guard,
        Grab,
        MenuScrollSound,
        MenuEnterSound
    }

    private void Start()
    {
        foreach (var source in this.transform.GetComponentsInChildren<AudioSource>())
        {
            if (source.tag == "BackGroundAudio") bgmSource = source;
            else if (source.tag == "EffectAudio") effectSource = source;
        }

        bgmSource.clip = BackGroundMusic;
        bgmSource.Play();
    }

    public void PlayEffect(SoundSet sound)
    {
        int random;
        switch (sound)
        {
            case SoundSet.Fall:
                random = UnityEngine.Random.Range(0, FallSound.Count);
                effectSource.PlayOneShot(FallSound[random]);
                //effectSource.PlayOneShot(FallSound);
                break;
            case SoundSet.Grab:
                effectSource.PlayOneShot(GrabSound);
                //effectSource.PlayOneShot(GrabSound);
                break;
            case SoundSet.Guard:
                random = UnityEngine.Random.Range(0, GuardSound.Count);
                effectSource.PlayOneShot(GuardSound[random]);
                //effectSource.PlayOneShot(GuardSound);
                break;
            case SoundSet.Hit:
                random = UnityEngine.Random.Range(0, HitSound.Count);
                effectSource.PlayOneShot(HitSound[random]);
                //effectSource.PlayOneShot(HitSound);
                break;
            case SoundSet.MenuScrollSound:
                effectSource.PlayOneShot(MenuScrollSound);
                break;
            case SoundSet.MenuEnterSound:
                effectSource.PlayOneShot(MenuEnterSound);
                break;
            default:
                break;
        }
    }

    public void PlayEffect(AudioClip sound)
    {
        effectSource.PlayOneShot(sound);
    }
}
