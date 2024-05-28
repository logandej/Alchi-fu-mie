using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AFM_DLL.Models.Enum;
using AFM_DLL.Models.Cards;

[CreateAssetMenu(fileName ="SpellCard", menuName ="Card/SpellCard")]
public class SpellCardScriptable : ScriptableObject
{
    public SpellCard SpellCard;
    public string Description;
    public SpellType SpellType;
    public Sprite Sprite;
}
