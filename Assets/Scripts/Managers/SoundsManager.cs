using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public enum Sound
    {
        BackgroundMusic,
        PlayerFootsteps,
        PlayerSwordSwing,
        PlayerSwordHit,
        PlayerDamaged,
        EnemyDamaged,
        TreasureChestOpen,
    }

    public static SoundsManager instance;
    AudioSource myAudio;
    public SoundAudioClip[] soundAudioClipArray;



    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayClip(Sound sound)
    {
        myAudio.PlayOneShot(GetAudioClip(sound), 1);
    }

    AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in instance.soundAudioClipArray)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " was not found!");
        return null;
    }
}

[System.Serializable]
public class SoundAudioClip
{
    public SoundsManager.Sound sound;
    public AudioClip audioClip;
}
