using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SoundManager 
{
    public static void PlaySound(GameObject source, AudioClip sound, float volume = 1)
    {
        if(sound != null)
        {
            AudioSource audioSource = source.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.clip = sound;
            audioSource.PlayOneShot(sound, 1);
            GameObject.Destroy(audioSource, sound.length);
        }
    }

    public static void PlaySound(GameObject source, List<AudioClip> sounds, float volume = 1)
    {
        PlaySound(source, Utils.GetRandomClip(sounds), volume);
    }



    public static void StopAll(GameObject source)
    {
        foreach (AudioSource audio in source.GetComponents<AudioSource>())
        {
            GameObject.Destroy(audio);
        }
    }

    public static void StopSound(GameObject source, AudioClip sound)
    {
        if(sound != null)
        {
            List<AudioSource> audioSources = source.GetComponents<AudioSource>().ToList();
            foreach (AudioSource audioSource in audioSources)
            {
                if (audioSource.clip != null && audioSource.clip == sound)
                {
                    audioSource.Stop();
                    GameObject.Destroy(audioSource);
                }
            }
        }
    }

}
