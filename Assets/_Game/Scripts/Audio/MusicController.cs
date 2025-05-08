using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MusicController
{
    private static AudioSource _currentMusicSource;

    public static AudioSource PlayMusic2D(AudioClip clip, float volume)
    {
        GameObject audioObject = new GameObject("2DAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();

        _currentMusicSource = audioSource;

        return audioSource;
    }

    public static void StopMusic()
    {
        _currentMusicSource.Stop();
        Object.Destroy(_currentMusicSource.gameObject);
    }
}
