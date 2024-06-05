using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] AudioSource music;
    [SerializeField] AudioSource effect;
    [SerializeField] AudioSource voice;
 
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

    public void PlayMusic(string audio)
    {
        music.clip = Resources.Load<AudioClip>("Audio/Music/"+audio);
        music.Play();
    }

    public void PlayEffect(AudioClip clip)
    {
        if (clip != null)
        {
            effect.clip = clip;
            effect.Play();
        }
    }

    public void PlayEffect(string audio)
    {
        effect.clip = Resources.Load<AudioClip>("Audio/SFX/" + audio);
        effect.Play();
    }

    public void PlayVoice(AudioClip clip)
    {
        if (clip != null)
        {
            voice.clip = clip;
            voice.Play();
        }
    }
}

