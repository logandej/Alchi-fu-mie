using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellCardDisplay : MonoBehaviour
{
    public SpellCardScriptable Data;
    public SpellCard SpellCard;

    [SerializeField] ParticleSystem _spellParticle;
    private SpriteController _spriteController;

    [SerializeField] TMP_Text _manaCost;
    [SerializeField] TMP_Text _description;

    // Start is called before the first frame update
    void Start()
    {
        if (Data == null)
            Debug.LogError("Data Should Be Initialzed");
        _spriteController = GetComponentInChildren<SpriteController>();
        _spriteController.SetChildSprite(Data.Sprite);

        _manaCost.text = SpellCard.GetManaCost().ToString();
        _description.text = SpellCard.GetDescription().ToString();

    }

    public void StartSpell()
    {
        var effect = Instantiate(_spellParticle, transform.position, transform.rotation);
        effect.gameObject.SetActive(true);
        effect.Play();
    }

}
