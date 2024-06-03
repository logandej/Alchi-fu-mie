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
    private SpriteController _spriteController;

    private Element? _overrideElement;

    private void Start()
    {
        if (Data == null)
            Debug.LogError("Data Should Be Initialzed");
        _spriteController = GetComponentInChildren<SpriteController>();
        _spriteController.SetChildSprite(Data.Sprite);
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
            _spriteController.SetChildSprite(data.Sprite);

        }
        else
        {
            print("Nothing change"+_overrideElement);
            _spriteController.SetChildSprite(Data.Sprite);
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
