using AFM_DLL;
using AFM_DLL.Models.Cards;
using AFM_DLL.Models.Enum;
using AFM_DLL.Models.PlayerInfo;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class SpellCardContainer : DeckItemContainer
{
	public SpellType SpellType { get; private set; }
	public GameObject OriginalParent;
    [SerializeField] SpellCardScriptable spellScriptable;
    [SerializeField] Image spellImage;
    public Image spellFrame;
    [SerializeField] TMP_Text manaCostText;
    [SerializeField] TMP_Text descriptionText;

    private void Awake()
    {
        SpellType = spellScriptable.SpellType;
        spellImage.sprite = spellScriptable.Sprite;
        SpellCard spellCard = SpellCard.FromType(spellScriptable.SpellType);
        manaCostText.text = spellCard.GetManaCost().ToString();
        descriptionText.text = spellCard.GetDescription();
    }
    public override bool AddToDeck()
	{
		if (DeckManager.Instance.PlayerDeck.AddSpell(SpellCard.FromType(SpellType)))
		{
			DeckManager.Instance.SaveDeck();

			if (DeckManager.Instance.PlayerDeck.Spells.Count(c => c.SpellType == SpellType) == 3)
			{
				var original = OriginalParent.GetComponentsInChildren<SpellCardContainer>().Single(c => c.SpellType == SpellType);
                original.GetComponentInChildren<SpellCardContainer>().spellFrame.color = Color.gray;
				original.GetComponent<DraggableItem>().enabled = false;
			}

			return true;
		}
		return false;
	}

	public override bool RemoveFromDeck()
    {
        if (DeckManager.Instance.PlayerDeck.RemoveSpell(SpellCard.FromType(SpellType)))
        {
            if (DeckManager.Instance.PlayerDeck.Spells.Count(c => c.SpellType == SpellType) == 2)
            {
                var original = OriginalParent.GetComponentsInChildren<SpellCardContainer>().Single(c => c.SpellType == SpellType);
                original.GetComponentInChildren<SpellCardContainer>().spellFrame.color = Color.white;
                original.GetComponent<DraggableItem>().enabled = true;
            }

            DeckManager.Instance.SaveDeck();
            return true;
        }
        return false;
    }
}