using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private List<SoundSO> soundList;
    private readonly List<AudioSource> soundPlayerList = new List<AudioSource>();

    private void Awake()
    {
        soundList = Resources.LoadAll<SoundSO>("Sounds").ToList();
    }
    public void PlaySound(string nameString)
    {
        AudioSource soundPlayer = GetSoundPlayer();
        soundPlayer.transform.SetParent(transform);

        SoundSO soundSO = GetSoundSO(nameString);
        if (!soundSO) return;

        PlayClip(soundPlayer, soundSO);
    }  
    private AudioSource GetSoundPlayer()
    {
        foreach (AudioSource audioSource in soundPlayerList)
        {
            if (audioSource.isPlaying) continue;

            return audioSource;
        }

        AudioSource newAudioSource = CreateNewAudioSource();
        return newAudioSource;
    }
    private AudioSource CreateNewAudioSource()
    {
        GameObject newGameObject = new GameObject("Sound");
        AudioSource newAudioSource = newGameObject.AddComponent<AudioSource>();
        soundPlayerList.Add(newAudioSource);
        return newAudioSource;
    }
    private SoundSO GetSoundSO(string nameString)
    {
        foreach (SoundSO sound in soundList)
        {
            if (nameString != sound.name) continue;
            
            return sound;
        }

        Debug.Log("Cannot find clip with name: " + nameString);
        return null;
    }
    private static void PlayClip(AudioSource soundPlayer, SoundSO soundSO)
    {
        soundPlayer.clip = soundSO.clip;
        soundPlayer.volume = soundSO.volume;
        soundPlayer.pitch = soundSO.pitch;
        soundPlayer.Play();
    }
}
