using AFM_DLL.Models.Cards;
using AFM_DLL.Models.PlayerInfo;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private ElementCardDisplay[] elementCards;
    private SpellCardDisplay[] spellCards;

    [SerializeField] BoardSideController sideController;
    private AIDeckScriptable deck;
    // Start is called before the first frame update
    private void Awake()
    {
        deck = DeckManager.Instance.aIDeckScriptable;
        sideController.Deck = new Deck()
        {
            Hero = new Hero("AIBit" , deck.Hero.elementType)
        };

        foreach (ElementCardScriptable element in deck.ElementDeckList)
        {
            sideController.Deck.AddElement(new ElementCard(element.Element));
        }

        foreach (SpellCardScriptable spell in deck.SpellDeckList)
        {
            sideController.Deck.AddSpell(SpellCard.FromType(spell.SpellType));
        }
    }

    public void StartRound()
    {
        elementCards = sideController.HandTransform.GetComponentsInChildren<ElementCardDisplay>();
        spellCards = sideController.HandSpellTransform.GetComponentsInChildren<SpellCardDisplay>();
        Invoke("BeginRound", 1);
    }

    public async Task BeginRound()
    {
        await Task.Delay(2000);
        for (int i = 0; i < 3; i++)
        {
            await PlayCard(elementCards[i].GetComponent<DraggableItem3D>(), i);
        }

        int randomSpellCard = Random.Range(0,sideController.BoardSide.Player.Hand.Spells.Count);
        if (sideController.BoardSide.Player.Hand.Spells[randomSpellCard].CanBePlayed(sideController.BoardSide.Player.ManaPoints) && deck.useSpell)
        {
            float rand = Random.Range(0f, 1f);
            if (rand < deck.spellFrequence)
            {
                await PlayCard(spellCards[randomSpellCard].GetComponent<DraggableItem3D>(), 3);
                await Task.Delay(1000);
            }

        }
        if (sideController.BoardSide.Player.ManaPoints >= 2 && deck.useChangeHero)
        {
            float rand = Random.Range(0f, 1f);
            print("RAND Hero=" + rand);

            if (rand < deck.heroFrequence)
            {
                await ChangeHero(elementCards[3].GetComponent<DraggableItem3D>());
            }
        }

        print("AI IS READY");
        PartyManager.Instance.BoardController.SetPlayerReady(sideController.IsBlue);
    }



    private async Task PlayCard(DraggableItem3D draggable, int index)
    {
        draggable.SelectCard();
        print("AI SELECT CARD");
        await Task.Delay(1000);
        print("AI DRAGGING CARD");
        draggable.DragCard(sideController.DroppableSlotList[index].transform.position.x, sideController.DroppableSlotList[index].transform.position.z);
        TransitionManager.ChangePosition(draggable.gameObject,sideController.DroppableSlotList[index].transform.position+Vector3.up/2, 1f);
        await Task.Delay(2000);
        draggable.DropCard();
        await Task.Delay(1000);

    }

    private async Task ChangeHero(DraggableItem3D draggable)
    {
        draggable.SelectCard();
        await Task.Delay(1000);
        draggable.DragCard(sideController.GetHeroSlotPosition().x, sideController.GetHeroSlotPosition().z);
        TransitionManager.ChangePosition(draggable.gameObject, sideController.GetHeroSlotPosition() + Vector3.up / 2, 1f);
        await Task.Delay(2000);
        draggable.DropCard();
        await Task.Delay(1000);

    }



}
