using AFM_DLL.Models.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckLoader : MonoBehaviour
{
    [SerializeField] private InventorySlot _heroCardSlots;
    [SerializeField] private List<InventorySlot> _elementCardSlots;
    [SerializeField] private List<InventorySlot> _spellCardSlots;

    [SerializeField] private List<HeroContainer> _heroContainers;
    [SerializeField] private List<ElementCardContainer> _elementCardContainers;
    [SerializeField] private List<SpellCardContainer> _spellCardContainers;

    private void Start()
    {
        var i = 0;
        foreach (ElementCard card in DeckManager.Instance.PlayerDeck.Elements)
        {
            var toClone = _elementCardContainers.Single(c => c.CardElement == card.ActiveElement);
            var clone = Instantiate(toClone, toClone.transform.parent);
            var draggable = clone.GetComponent<DraggableItem>();
            draggable.duplicateOnDrag = false;
            draggable.parentAfterDrag = draggable.transform.parent;
            _elementCardSlots[i].InitCurrentItem(draggable);
            i++;
        }

        i = 0;
        foreach (SpellCard card in DeckManager.Instance.PlayerDeck.Spells)
        {
            var toClone = _spellCardContainers.Single(c => c.SpellType == card.GetSpellType());
            var clone = Instantiate(toClone, toClone.transform.parent);
            var draggable = clone.GetComponent<DraggableItem>();
            draggable.duplicateOnDrag = false;
            draggable.parentAfterDrag = draggable.transform.parent;
            _spellCardSlots[i].InitCurrentItem(draggable);
            i++;
        }

        var hero = DeckManager.Instance.PlayerDeck.Hero;
        if (hero != null)
        {
            var toMove = _heroContainers.Single(c => c.HeroElement == hero.ActiveElement);
            var draggable = toMove.GetComponent<DraggableItem>();
            draggable.duplicateOnDrag = false;
            draggable.parentAfterDrag = draggable.transform.parent;
            if (draggable.TryGetComponent<RectTransform>(out var rect))
                rect.sizeDelta = Vector2.one * 250;
            _heroCardSlots.InitCurrentItem(draggable);
        }
    }

}