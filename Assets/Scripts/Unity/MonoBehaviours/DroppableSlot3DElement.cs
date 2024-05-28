using AFM_DLL.Models.BoardData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableSlot3DElement : DroppableSlot3D
{
    [Header("DLL INFOS")]
    public BoardPosition BoardPosition;

 


    protected override void CheckSelection(DraggableItem3D draggableItem)
    {
        if(draggableItem.Type == GameManager.Type.Element)
        {
            base.CheckSelection(draggableItem);
        }
    }

    public override void Occup(DraggableItem3D draggable)
    {
        Debug.Log("HandCountOccupBefore" + BoardController.Instance.Board.GetAllyBoardSide(draggable.IsBlue).Player.Hand.Elements.Count);
        if (!draggable.GetComponent<ElementCardDisplay>().Data.ElementCard.AddToBoard(BoardController.Instance.Board, draggable.IsBlue, BoardPosition))
            Debug.LogError("Ajout au board non réussi");
        Debug.Log("HandCountOccupAfter" + BoardController.Instance.Board.GetAllyBoardSide(draggable.IsBlue).Player.Hand.Elements.Count);


        base.Occup(draggable);


    }

    public override void Deactive()
    {
        if (_draggableItem != null)
        {
            Debug.Log("HandCountDeactiveBefore" + BoardController.Instance.Board.GetAllyBoardSide(_draggableItem.IsBlue).Player.Hand.Elements.Count);
            _draggableItem.GetComponent<ElementCardDisplay>().Data.ElementCard.RemoveFromBoard(BoardController.Instance.Board, _draggableItem.IsBlue, BoardPosition);
            Debug.Log("HandCountDeactiveAfter" + BoardController.Instance.Board.GetAllyBoardSide(_draggableItem.IsBlue).Player.Hand.Elements.Count);
        }
        base.Deactive();

    }
}
