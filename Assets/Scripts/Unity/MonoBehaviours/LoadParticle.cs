using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadParticle : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles;

    public void Play(int index)
    {
        particles[index].Play();
    }
}
