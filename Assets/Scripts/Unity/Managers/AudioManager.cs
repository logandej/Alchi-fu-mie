using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource music;
    [SerializeField] AudioSource effect;
 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            music.clip = clip;
            music.Play();
        }
    }

    public void PlayEffect(AudioClip clip)
    {
        if (clip != null)
        {
            effect.clip = clip;
            effect.Play();
        }
    }
}

