using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    private int currentTrack = 0;
    public AudioClip[] tracks = new AudioClip[3];
    public AudioClip youDied;
    public float[] volumes = new float[3];
    private bool isPlayerDead = false;

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
        audioSource = GetComponent<AudioSource>();
        PlayNextTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying && !isPlayerDead)
        {
            PlayNextTrack();
        }
    }

    void PlayNextTrack()
    {
        audioSource.clip = tracks[currentTrack];
        audioSource.volume = volumes[currentTrack];
        audioSource.Play();

        currentTrack = (currentTrack + 1) % tracks.Length;
    }

    public void PlayDeathMusic()
    {
        isPlayerDead = true;
        audioSource.clip = youDied;
        audioSource.Play();
    }

    public void ResumeMusic()
    {
        isPlayerDead = false;
        PlayNextTrack();
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
}


