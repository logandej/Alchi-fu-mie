using AFM_DLL;
using AFM_DLL.Models.Cards;
using UnityEngine;

public class ElementCardContainer : MonoBehaviour
{
    public Element CardElement;

    public void AddToDeck()
    {
        Debug.Log("Added to deck element " + CardElement.ToString());
        DeckManager.Instance.PlayerDeck.AddElement(new ElementCard(CardElement));
        DeckManager.Instance.SaveDeck();
    }

    public void RemoveFromDeck()
    {
        Debug.Log("Removed from deck element " + CardElement.ToString());
        DeckManager.Instance.PlayerDeck.RemoveElement(new ElementCard(CardElement));
        DeckManager.Instance.SaveDeck();
    }
}