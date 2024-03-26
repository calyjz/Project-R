//https://youtu.be/DU7cgVsU2rM?si=seUPM2MgrirJKU0d
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioData
{
    public string name;
    public AudioClip audioClip;
    public AudioMixerGroup audioMixerGroup;
}

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private List<AudioData> audioDataList = new List<AudioData>();

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioMixerGroup> audioMixerGroups = new Dictionary<string, AudioMixerGroup>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GameObject.DontDestroyOnLoad(this.gameObject);

        foreach (var data in audioDataList)
        {
            audioClips[data.name] = data.audioClip;
            audioMixerGroups[data.name] = data.audioMixerGroup;
        }
    }

    public AudioSource PlaySoundFXClip(string audioClipName, Transform spawnTransform)
    {
        // Check if the audio clip exists in the dictionary
        if (!audioClips.ContainsKey(audioClipName))
        {
            Debug.LogError("Audio clip not found: " + audioClipName);
            return null;
        }

        // Check if the audio mixer group exists in the dictionary
        if (!audioMixerGroups.ContainsKey(audioClipName))
        {
            Debug.LogError("Audio mixer group not found: " + audioClipName);
            return null;
        }

        // Get the audio clip and audio mixer group from the dictionaries
        AudioClip audioClip = audioClips[audioClipName];
        AudioMixerGroup audioMixerGroup = audioMixerGroups[audioClipName];

        // Spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Assign Audioclip
        audioSource.clip = audioClip;

        // Assign volume
        audioSource.volume = 1;

        // Assign output to AudioMixerGroup
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        // Play sound
        audioSource.Play();

        // Get length of clip
        float clipLength = audioSource.clip.length;

        // Destroy clip after certain amount of time
        Destroy(audioSource.gameObject, clipLength);

        return audioSource;
    }
}

