using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AI Deck Data", menuName ="AI Deck")]
public class AIDeckScriptable : ScriptableObject
{
    public bool useSpell;

    [Range(0, 1)]
    public float spellFrequence;

    public bool useChangeHero;
    [Range(0, 1)]
    public float heroFrequence;

    public HeroScriptable Hero;
    public List<ElementCardScriptable> ElementDeckList;
    public List<SpellCardScriptable> SpellDeckList;
}
