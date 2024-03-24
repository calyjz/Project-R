using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip enemyTrack;
    public AudioClip mainTheme;
    public AudioClip youDied;

    public AudioMixerGroup enemyTrackMixerGroup;
    public AudioMixerGroup mainThemeMixerGroup;

    public float[] volumes = new float[3];

    public static MusicManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PlayNextTrack();
    }



    void Update()
    {
        PlayNextTrack();
    }


    public void PlayNextTrack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (SceneManager.GetActiveScene().name != "StartScene" && enemies.Length != 0)
        {
            if (!audioSource.isPlaying || audioSource.clip != enemyTrack)
            {
                PlayAudioClip(enemyTrack, enemyTrackMixerGroup);
            }
        }
        else if (SceneManager.GetActiveScene().name != "StartScene")
        {
            if (!audioSource.isPlaying || audioSource.clip != mainTheme)
            {
                PlayAudioClip(mainTheme, mainThemeMixerGroup);
            }
        }
    }



    public void PlayAudioClip(AudioClip clip, AudioMixerGroup mixerGroup)
    {
        if (clip != null && clip != audioSource.clip)
        {
            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.Play();
        }
    }


}