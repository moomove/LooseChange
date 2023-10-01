using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMusic : MonoBehaviour
{
    private AudioSource BackgroundMusic;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        BackgroundMusic = GetComponent<AudioSource>();
        //PlayMusic();
        Debug.Log("playing on awake");
    }

    public void PlayMusic()
    {
        if (BackgroundMusic.isPlaying) return;
        BackgroundMusic.Play();

        Debug.Log("play music");
    }

    public void StopMusic()
    {
        BackgroundMusic.Stop();
    }
}
