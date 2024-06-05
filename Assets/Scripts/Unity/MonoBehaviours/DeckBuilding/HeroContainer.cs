using AFM_DLL;
using AFM_DLL.Models.Cards;
using AFM_DLL.Models.PlayerInfo;

class HeroContainer : DeckItemContainer
{
    public string HeroName;
    public Element HeroElement;

    public override bool AddToDeck()
    {
        DeckManager.Instance.PlayerDeck.Hero = new Hero(HeroName, HeroElement); ;
        DeckManager.Instance.SaveDeck();
        return true;
    }

    public override bool RemoveFromDeck()
    {
        DeckManager.Instance.PlayerDeck.Hero = null;
        DeckManager.Instance.SaveDeck();
        return true;
    }
}