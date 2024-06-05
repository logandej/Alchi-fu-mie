using AFM_DLL;
using AFM_DLL.Models.Cards;

class ElementCardContainer : DeckItemContainer
{
    public Element CardElement;

    public override bool AddToDeck()
    {
        if (DeckManager.Instance.PlayerDeck.AddElement(new ElementCard(CardElement)))
        {
            DeckManager.Instance.SaveDeck();
            return true;
        }
        return false;
    }

    public override bool RemoveFromDeck()
    {
        if (DeckManager.Instance.PlayerDeck.RemoveElement(new ElementCard(CardElement)))
        {
            DeckManager.Instance.SaveDeck();
            return true;
        }
        return false;
    }
}