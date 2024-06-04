using AFM_DLL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroCard", menuName = "Card/HeroCard")]
public class HeroScriptable : ScriptableObject
{
    public Sprite sprite;
    public Element elementType;
    public ParticleSystem attackParticle;
    public ParticleSystem onAppearingParticle;
    public AudioClip onAppearingClip;
    public List<AudioClip> attackClips;
}
