using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableSlot3DSpell : DroppableSlot3D
{
    protected override void CheckSelection(DraggableItem3D draggableItem)
    {
        if (draggableItem.Type == GameManager.Type.Spell)
        {
            base.CheckSelection(draggableItem);
        }
    }

    public override void Occup(DraggableItem3D draggable)
    {
        if (!draggable.GetComponent<SpellCardDisplay>().Data.SpellCard.AddToBoard(GameManager.Instance.BoardController.Board, draggable.IsBlue, null))
            Debug.LogError("Ajout au board non réussi");
        base.Occup(draggable);

    }

    public override void Deactive()
    {
        if (_draggableItem != null)
            _draggableItem.GetComponent<SpellCardDisplay>().Data.SpellCard.RemoveFromBoard(GameManager.Instance.BoardController.Board, _draggableItem.IsBlue, null);
        base.Deactive();
    }
}
