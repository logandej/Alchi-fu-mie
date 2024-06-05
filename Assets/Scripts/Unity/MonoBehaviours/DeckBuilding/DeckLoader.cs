using AFM_DLL.Models.Cards;
using AFM_DLL.Models.Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeckLoader : MonoBehaviour
{
    [SerializeField] private InventorySlot _heroCardSlots;
    [SerializeField] private List<InventorySlot> _elementCardSlots;
    [SerializeField] private List<InventorySlot> _spellCardSlots;

    [SerializeField] private List<HeroContainer> _heroContainers;
    [SerializeField] private List<ElementCardContainer> _elementCardContainers;
    [SerializeField] private GameObject _spellCardContainerParent;

    private void Start()
    {
        var i = 0;
        foreach (ElementCard card in DeckManager.Instance.PlayerDeck.Elements)
        {
            var toClone = _elementCardContainers.Single(c => c.CardElement == card.ActiveElement);
            var clone = Instantiate(toClone, toClone.transform.parent);
            var draggable = clone.GetComponent<DraggableItem>();
            draggable.SourceParent = toClone.transform.parent;
            draggable.duplicateOnDrag = false;
            draggable.parentAfterDrag = _elementCardSlots[i].transform;
            _elementCardSlots[i].InitCurrentItem(draggable);
            i++;
        }

        var spellCards = _spellCardContainerParent.GetComponentsInChildren<SpellCardContainer>();

        i = 0;
        foreach (SpellCard card in DeckManager.Instance.PlayerDeck.Spells)
        {
            var toClone = spellCards.Single(c => c.SpellType == card.SpellType);
            var clone = Instantiate(toClone, toClone.transform.parent);
            var draggable = clone.GetComponent<DraggableItem>();
            draggable.SourceParent = _spellCardContainerParent.transform;
            draggable.duplicateOnDrag = false;
            draggable.parentAfterDrag = _spellCardSlots[i].transform;
            _spellCardSlots[i].InitCurrentItem(draggable);
            clone.GetComponent<AspectRatioFitter>().enabled = true;
            i++;
        }

        foreach (var card in spellCards)
        {
            if (DeckManager.Instance.PlayerDeck.Spells.Count(c => c.SpellType == card.SpellType) == 3)
            {
                card.GetComponent<Image>().color = Color.gray;
                card.GetComponent<DraggableItem>().enabled = false;
            }
        }

        var hero = DeckManager.Instance.PlayerDeck.Hero;
        if (hero != null)
        {
            var toMove = _heroContainers.Single(c => c.HeroElement == hero.ActiveElement);
            var draggable = toMove.GetComponent<DraggableItem>();
            draggable.duplicateOnDrag = false;
            draggable.parentAfterDrag = draggable.transform.parent;
            _heroCardSlots.InitCurrentItem(draggable);
        }

        
    }

}