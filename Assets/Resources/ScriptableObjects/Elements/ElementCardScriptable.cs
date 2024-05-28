using AFM_DLL;
using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Element Card", menuName="Card/ElementCard")]
public class ElementCardScriptable : ScriptableObject
{
    public ElementCard ElementCard;
    public Element Element;
    public Sprite Sprite;
    
   
}
