using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem3D : MonoBehaviour
{
    [Header("Unity Parameters")]
    public Rigidbody Rigidbody { private set; get; }
    private float _startYPos;
    private BoardInteraction _board;
    public DroppableSlot3D CurrentSlot { private set; get; } // Nouvelle variable pour stocker le slot actuel

    [SerializeField] float heightFromBoard = 5;
    [SerializeField] float speedDrag = 10;
    [SerializeField] float rotation = 4;

    public GameManager.Type Type;
    public bool IsBlue;
    public Vector3 HandPosition = Vector3.zero;

    public SpriteController spriteController;

    void Start()
    {
        _board = GetComponentInParent<BoardInteraction>();
        Rigidbody = GetComponent<Rigidbody>();
        _startYPos = 0; // Better to not hardcode that one but whatever
        HandPosition = transform.localPosition;
        Lock();
       
    }

    private void OnMouseDown()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;
        if (CurrentSlot != null)
        {
            CurrentSlot.Deactive();
            SetCurrentSlot(null);

        }
        Unlock();
        spriteController.SizeBig();
    }

    private void OnMouseEnter()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

        if (CurrentSlot == null && !GameManager.Instance.CursorOccupied)
        {
            HandPosition.x = transform.localPosition.x;
            TransitionManager.ChangeLocalPosition(this.gameObject, HandPosition+new Vector3(0,0.1f,0.1f), 0.1f);
            GetComponentInChildren<SpriteRenderer>().sortingOrder += 1;
        }
    }

    private void OnMouseExit()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

        if (CurrentSlot == null && !GameManager.Instance.CursorOccupied)
        {
            HandPosition.x = transform.localPosition.x;
            TransitionManager.ChangeLocalPosition(this.gameObject, HandPosition, 0.1f);
            GetComponentInChildren<SpriteRenderer>().sortingOrder -= 1;
        }
    }

    private void OnMouseDrag()
    {

        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

        GameManager.Instance.CursorOccupied = true;

        Vector3 newWorldPosition = new Vector3(_board.CurrentMousePosition.x, _startYPos + heightFromBoard, _board.CurrentMousePosition.z);

        var difference = newWorldPosition - transform.position;

        var speed = speedDrag * difference;
        Rigidbody.isKinematic = false;
        Rigidbody.velocity = speed;
     
        Rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z * rotation, 0, -speed.x * rotation));
    }




    private void OnMouseUp()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

        if (CurrentSlot != null)
        {
            CurrentSlot.Occup(this);
            spriteController.SizeSmall();
        }
        else
        {
            BoardSideController boardSide = IsBlue ? GameManager.Instance.BoardController.BoardSideBlue : GameManager.Instance.BoardController.BoardSideRed;
            boardSide.PlaceToHand(this);
            // Retourner la carte à sa position d'origine si le slot est occupé ou si aucun slot n'est défini
            Rigidbody.velocity = Vector3.zero;
            spriteController.SizeNormal();
            Lock();
        }
        GameManager.Instance.CursorOccupied = false;

    }

    // Méthode pour définir le slot actuel
    public void SetCurrentSlot(DroppableSlot3D slot)
    {
        
        CurrentSlot = slot;
    }

    /// <summary>
    /// Changes contraints of the rigidbody, Position and Rotation are freezed
    /// </summary>
    public void Lock()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Changes constraints of the rigidbody to none;
    /// </summary>
    public void Unlock()
    {
        Rigidbody.constraints = RigidbodyConstraints.None;
        GetComponent<Collider>().isTrigger = false;
    }
}
