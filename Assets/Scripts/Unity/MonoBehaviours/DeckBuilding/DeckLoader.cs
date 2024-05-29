using AFM_DLL.Models.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckLoader : MonoBehaviour
{
    [SerializeField] private InventorySlot _heroCardSlots;
    [SerializeField] private List<InventorySlot> _elementCardSlots;
    [SerializeField] private List<InventorySlot> _spellCardSlots;

    [SerializeField] private List<ElementCardContainer> _elementCardContainers;

    private void Start()
    {
        var i = 0;
        foreach (ElementCard card in DeckManager.Instance.PlayerDeck.Elements)
        {
            var toClone = _elementCardContainers.Single(c => c.CardElement == card.ActiveElement);
            var clone = Instantiate(toClone, _elementCardSlots[i].transform);
            var draggable = clone.GetComponent<DraggableItem>();
            draggable.duplicateOnDrag = false;
            _elementCardSlots[i].InitCurrentItem(draggable);
            i++;
        }
    }

}