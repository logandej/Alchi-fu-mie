using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private ElementCardDisplay[] elementCards;
    private SpellCardDisplay[] spellCards;

    [SerializeField] BoardSideController sideController;
    // Start is called before the first frame update
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
        if (sideController.BoardSide.Player.Hand.Spells[0].CanBePlayed(sideController.BoardSide.Player.ManaPoints))
        {
            await PlayCard(spellCards[0].GetComponent<DraggableItem3D>(), 3);
            await Task.Delay(1000);
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
        print("AI DROP CARD");
        await Task.Delay(1000);

    }

   
}
