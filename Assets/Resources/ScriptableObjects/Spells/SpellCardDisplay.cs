using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCardDisplay : MonoBehaviour
{
    public SpellCardScriptable Data;
    public SpellCard SpellCard;

    [SerializeField] ParticleSystem _spellParticle;
    private SpriteController _spriteController;
    // Start is called before the first frame update
    void Start()
    {
        if (Data == null)
            Debug.LogError("Data Should Be Initialzed");
        _spriteController = GetComponentInChildren<SpriteController>();
        _spriteController.SetChildSprite(Data.Sprite);

    }

    public void StartSpell()
    {
        var effect = Instantiate(_spellParticle, transform.position, transform.rotation);
        effect.gameObject.SetActive(true);
        effect.Play();
    }

}
