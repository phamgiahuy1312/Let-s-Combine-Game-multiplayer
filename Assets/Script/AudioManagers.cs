using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagers : MonoBehaviour
{

    public AudioSource SoundBackground;
    public AudioSource SFXSource;


    public static AudioManagers instance;

    [Header("=============Audio Clip================")]
    public AudioClip Background;
    public AudioClip Win;
    public AudioClip Lose;
    public AudioClip Touch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SoundBackground.clip = Background;
        SoundBackground.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}


