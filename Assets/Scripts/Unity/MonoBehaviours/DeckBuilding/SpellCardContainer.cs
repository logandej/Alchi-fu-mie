using AFM_DLL;
using AFM_DLL.Models.Cards;
using AFM_DLL.Models.Enum;
using AFM_DLL.Models.PlayerInfo;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

class SpellCardContainer : DeckItemContainer
{
	public SpellType SpellType;
	public GameObject OriginalParent;

	public override bool AddToDeck()
	{
		if (DeckManager.Instance.PlayerDeck.AddSpell(SpellCard.FromType(SpellType)))
		{
			DeckManager.Instance.SaveDeck();

			if (DeckManager.Instance.PlayerDeck.Spells.Count(c => c.SpellType == SpellType) == 3)
			{
				var original = OriginalParent.GetComponentsInChildren<SpellCardContainer>().Single(c => c.SpellType == SpellType);
				original.GetComponent<Image>().color = Color.gray;
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
                original.GetComponent<Image>().color = Color.white;
                original.GetComponent<DraggableItem>().enabled = true;
            }

            DeckManager.Instance.SaveDeck();
            return true;
        }
        return false;
    }
}