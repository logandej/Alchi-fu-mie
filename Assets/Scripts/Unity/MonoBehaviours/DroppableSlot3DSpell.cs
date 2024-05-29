using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableSlot3DSpell : DroppableSlot3D
{
    protected override void CheckSelection(DraggableItem3D draggableItem)
    {
        if (draggableItem.Type == GameManager.Type.Spell && draggableItem.GetComponent<SpellCardDisplay>().SpellCard.CanBePlayed(BoardController.Instance.Board.GetAllyBoardSide(draggableItem.IsBlue).Player.ManaPoints)) ;
        {
            base.CheckSelection(draggableItem);
        }
    }

    public override void Occup(DraggableItem3D draggable)
    {
        if (!draggable.GetComponent<SpellCardDisplay>().SpellCard.AddToBoard(PartyManager.Instance.BoardController.Board, draggable.IsBlue, null))
            Debug.LogError("Ajout au board non réussi");
        base.Occup(draggable);

    }

    public override void Deactive()
    {
        if (_draggableItem != null)
            _draggableItem.GetComponent<SpellCardDisplay>().SpellCard.RemoveFromBoard(PartyManager.Instance.BoardController.Board, _draggableItem.IsBlue, null);
        base.Deactive();
    }

    public void StartSpell()
    {
        TransitionManager.ChangeLocalPosition(_draggableItem.gameObject, _draggableItem.transform.localPosition + Vector3.up/3, 0.2f);
        _draggableItem.GetComponent<SpellCardDisplay>().StartSpell();
        
    }


    public void StopSpell()
    {
        TransitionManager.ChangeLocalPosition(_draggableItem.gameObject, _draggableItem.transform.localPosition - Vector3.up/3, 0.2f);
    }
}
