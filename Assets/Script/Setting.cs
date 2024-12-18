using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using Unity.Collections;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public Sprite Mucsicdefault;
    public Sprite Mucsicmute;
    public Sprite Sounddefault;
    public Sprite Soundmute;

    [SerializeField] GameObject Settingpanel;
    public GameObject BtnMusic;
    public GameObject BtnSFX;

    private AudioManagers audioManager;
    private int switchStateSFX = 1;
    private int switchStateMusic = 1;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagers>();
    }

    void Start()
    {
        audioManager = AudioManagers.instance;

        // Load the state of music and SFX
        switchStateMusic = PlayerPrefs.GetInt("Music", 1);
        switchStateSFX = PlayerPrefs.GetInt("SFX", 1);

        // Apply saved settings
        UpdateMusicState();
        UpdateSFXState();
    }

    void UpdateMusicState()
    {
        if (switchStateMusic == 0)
        {
            BtnMusic.GetComponent<UnityEngine.UI.Image>().sprite = Mucsicmute;
            audioManager.SoundBackground.Pause();
        }
        else
        {
            BtnMusic.GetComponent<UnityEngine.UI.Image>().sprite = Mucsicdefault;
            if (!audioManager.SoundBackground.isPlaying)
            {
                audioManager.SoundBackground.Play();
            }
        }
    }

    void UpdateSFXState()
    {
        if (switchStateSFX == 0)
        {
            BtnSFX.GetComponent<UnityEngine.UI.Image>().sprite = Soundmute;
            audioManager.SFXSource.mute = true;
        }
        else
        {
            BtnSFX.GetComponent<UnityEngine.UI.Image>().sprite = Sounddefault;
            audioManager.SFXSource.mute = false;
        }
    }

    public void onclickMusic()
    {
        // Toggle music state
        switchStateMusic = 1 - switchStateMusic;
        UpdateMusicState();
        audioManager.PlaySFX(audioManager.Touch);
    }

    public void onclickSFX()
    {
        // Toggle SFX state
        switchStateSFX = 1 - switchStateSFX;
        UpdateSFXState();
        audioManager.PlaySFX(audioManager.Touch);
    }

    public void SaveSetting()
    {
        // Save the state of music and SFX
        PlayerPrefs.SetInt("Music", switchStateMusic);
        PlayerPrefs.SetInt("SFX", switchStateSFX);
        PlayerPrefs.Save();
    }


}
