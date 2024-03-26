using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource enemeySource;
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
        if (SceneManager.GetActiveScene().name == "Respawn")
        {
            audioSource.Pause();
        } else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (SceneManager.GetActiveScene().name != "StartScene" && enemies.Length != 0)
        {
            if (!enemeySource.isPlaying)
            {
                StartCoroutine(FadeIn(enemeySource, 1f)); // Start the fade in over 1 second
            }
        }
        else
        {
            enemeySource.Stop();
        }
        if (SceneManager.GetActiveScene().name != "StartScene")
        {
            if (!audioSource.isPlaying || audioSource.clip != mainTheme)
            {
                PlayAudioClip(mainTheme, mainThemeMixerGroup);
            }
        }
    }

    IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += Time.deltaTime / duration;

            yield return null;
        }

        audioSource.volume = 1f;
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