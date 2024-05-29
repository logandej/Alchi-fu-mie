using AFM_DLL;
using AFM_DLL.Models.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCardDisplay : MonoBehaviour
{
    public ElementCardScriptable Data;
    public ElementCard ElementCard;
    [SerializeField] LoadParticle visualEffects;
    [SerializeField] ParticleSystem deleteEffect;

    private Element? _overrideElement;

    public bool IsBlue { private set; get; }

    private void Start()
    {
        if (Data == null)
            Debug.LogError("Data Should Be Initialzed");
        GetComponentInChildren<SpriteRenderer>().sprite = Data.Sprite;
        ElementCard.CardOverrideChanged += SetOverrideSpell;
        
    }

    public void SetOverrideSpell(Element? element)
    {
        _overrideElement = element;
    }

    public void ChangeVisuel()
    {
        if (_overrideElement != null)
        {
            visualEffects.Play((int)_overrideElement);
            print("ChangeVisuel !!!");
            string resourcePath = $"ScriptableObjects/Elements/{_overrideElement}";
            ElementCardScriptable data = Resources.Load<ElementCardScriptable>(resourcePath);
            GetComponentInChildren<SpriteRenderer>().sprite = data.Sprite;

        }
        else
        {
            print("Nothing change"+_overrideElement);
            GetComponentInChildren<SpriteRenderer>().sprite = Data.Sprite;
        }
        //ElementCard.CardOverrideChanged -= SetOverrideSpell;

    }

    public void Delete()
    {
        var effect = Instantiate(deleteEffect, transform.position, transform.rotation);
        effect.gameObject.SetActive(true);
        effect.Play();
    }


}
