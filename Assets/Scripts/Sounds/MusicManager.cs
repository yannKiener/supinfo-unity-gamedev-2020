using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {


    public List<AudioClip> musicList;
    public float volume = 1;

	private AudioSource audioSource;

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = volume;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomMusic();
        }
    }

    public void PlayRandomMusic()
    {
        audioSource.clip = Utils.GetRandomClip(musicList);

        audioSource.Play();
	}

    public void SetNewPlaylist(List<AudioClip> musicList)
    {
        this.musicList = musicList;
        PlayRandomMusic();
    }
}
