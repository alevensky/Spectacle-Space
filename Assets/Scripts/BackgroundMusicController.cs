using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public AudioClip[] music;
    private AudioSource backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic = GetComponent<AudioSource>();
        PlayMusic();
    }

    float RandomNumber()
    {
        float numb = Random.Range(0, 3);
        return numb;
    }

    void PlayMusic()
    {
        float selection = RandomNumber();
        if (selection <= 1)
        {
            backgroundMusic.clip = music[0];
            backgroundMusic.Play();
        }
        else if (selection <= 2)
        {
            backgroundMusic.clip = music[1];
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.clip = music[2];
            backgroundMusic.Play();
        }
    }
}
