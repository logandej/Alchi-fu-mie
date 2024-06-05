using AFM_DLL;
using AFM_DLL.Models.Cards;
using UnityEngine;

public abstract class DeckItemContainer : MonoBehaviour
{
    public abstract bool AddToDeck();
    public abstract bool RemoveFromDeck();
}