//https://youtu.be/DU7cgVsU2rM?si=seUPM2MgrirJKU0d
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void PlayerSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume, AudioMixerGroup audioMixerGroup)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign Audioclip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //assign output to AudioMixerGroup
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        //play sound
        audioSource.Play();

        //get length of clip
        float clipLength = audioSource.clip.length;

        //destroy clip after certain amount of time
        Destroy(audioSource.gameObject, clipLength);
    }
}
