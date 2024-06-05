using AFM_DLL;
using AFM_DLL.Models.Cards;
using AFM_DLL.Models.Enum;
using AFM_DLL.Models.PlayerInfo;

class SpellCardContainer : DeckItemContainer
{
	public SpellType SpellType;

	public override bool AddToDeck()
	{
		if (DeckManager.Instance.PlayerDeck.AddSpell(SpellCard.FromType(SpellType)))
		{
			DeckManager.Instance.SaveDeck();
			return true;
		}
		return false;
	}

	public override bool RemoveFromDeck()
    {
        if (DeckManager.Instance.PlayerDeck.RemoveSpell(SpellCard.FromType(SpellType)))
        {
            DeckManager.Instance.SaveDeck();
            return true;
        }
        return false;
    }
}