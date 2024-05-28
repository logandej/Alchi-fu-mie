using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardDisplay : MonoBehaviour
{
    public SpellCardScriptable Data;
    // Start is called before the first frame update
    void Start()
    {
        if (Data == null)
            Debug.LogError("Data Should Be Initialzed");
        if (Data.SpellCard == null)
        {
            Data.SpellCard = SpellCard.FromType(Data.SpellType);
        }
        GetComponentInChildren<SpriteRenderer>().sprite = Data.Sprite;
    }

}
