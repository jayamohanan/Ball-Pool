using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    public AudioClip tapSound;
    public AudioClip fallSound;
    public AudioClip failSound;
    public AudioClip glassBreakSound;
    public AudioClip clingSound;
    public AudioClip wowSound;
    public AudioClip bottomBreakSound;
    public AudioClip boingSound;

    public bool vibrate;//
    public bool soundOn;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
            Destroy(gameObject);
    }
    public void PlayTapSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(tapSound, 0.3f);
        Vibrate(15);
    }
    public void PlayFallSound(float volume = 1)
    {
        if (soundOn)
            audioSource.PlayOneShot(fallSound, volume);
        Vibrate(15);
    }
    public void PlayFailSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(failSound);
        Vibrate(15);
    }
    public void PlayGlassBreakSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(glassBreakSound,0.4f);
        Vibrate(15);
    }
    public void PlayClingSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(clingSound,0.3f);
        Vibrate(15);
    }
    public void PlayWowSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(wowSound);
        Vibrate(15);
    }
    public void PlayBottomBreakSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(bottomBreakSound,0.3f);
        Vibrate(15);
    }
    public void PlayBoingSound()
    {
        if (soundOn)
            audioSource.PlayOneShot(boingSound, 0.3f);
        Vibrate(15);
    }
    
    public void Vibrate(int duration = 20)
    {
#if !UNITY_EDITOR
        if (vibrate)
            Vibration.Vibrate(duration);
#endif
    }
}
