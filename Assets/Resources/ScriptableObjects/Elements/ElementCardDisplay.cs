using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCardDisplay : MonoBehaviour
{
    public ElementCardScriptable Data;

    public bool IsBlue { private set; get; }

    private void Start()
    {
        if (Data == null)
            Debug.LogError("Data Should Be Initialzed");
        if (Data.ElementCard == null)
        {
            Data.ElementCard = new ElementCard(Data.Element);
        }
        GetComponentInChildren<SpriteRenderer>().sprite = Data.Sprite;
    }
}
