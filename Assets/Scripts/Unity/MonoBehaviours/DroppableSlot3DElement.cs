using AFM_DLL.Models.BoardData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (!draggable.GetComponent<ElementCardDisplay>().ElementCard.AddToBoard(BoardController.Instance.Board, draggable.IsBlue, BoardPosition))
            Debug.LogError("Ajout au board non réussi");

        if (!BoardController.Instance.Board.GetAllyBoardSide(_isBlue).AllElementsOfSide.Any(c=>c==null) && _isBlue)
        {
            BoardController.Instance.ShowReady();
        }
        base.Occup(draggable);


    }

    public override void Deactive()
    {
        if (_draggableItem != null)
        {
            _draggableItem.GetComponent<ElementCardDisplay>().ElementCard.RemoveFromBoard(BoardController.Instance.Board, _draggableItem.IsBlue, BoardPosition);
            if(_isBlue)
                BoardController.Instance.HideReady();

        }
        base.Deactive();

    }



    /// <summary>
    /// Evaluate the card, boolean winner ?
    /// </summary>
    public void StartEvaluateElementCard(int result)
    {
        TransitionManager.ChangeLocalPosition(_draggableItem.gameObject, _draggableItem.transform.localPosition + Vector3.up,0.2f);
        if (result == -1)
        {
            StartCoroutine(LoseColumn());
        }
    }

    IEnumerator LoseColumn()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlayEffectPitch("mexplosion");
        _draggableItem.GetComponent<ElementCardDisplay>().Delete();
        DestroyDraggable();

    }

    public void StopEvaluateElementCard()
    {
        TransitionManager.ChangeLocalPosition(_draggableItem.gameObject, _draggableItem.transform.localPosition - Vector3.up, 0.2f);
    }
}
