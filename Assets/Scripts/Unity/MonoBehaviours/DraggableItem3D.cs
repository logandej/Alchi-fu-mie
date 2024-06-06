using AFM_DLL.Models.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (!IsBlue)
        {
            spriteController.Hide();
        }
       
    }

    private void OnMouseDown()
    {
        if (!IsBlue) return;
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
        
        if (!IsBlue) return;
       
        if (!PartyManager.Instance.CursorOccupied)
        {
            SelectCard();
            TryGetComponent<SpellCardDisplay>(out var spellCard);
            if (spellCard != null && CurrentSlot==null)
            {
                PartyManager.Instance.ShowSpellDescription(spellCard.SpellCard.GetDescription());
            }

        }
    }

    /// <summary>
    /// Move the card up from your hand to see the card you are going to grab
    /// </summary>
    public void SelectCard()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

       

        if (CurrentSlot == null)
        {
            HandPosition.x = transform.localPosition.x;
            TransitionManager.ChangeLocalPosition(this.gameObject, HandPosition + new Vector3(0, 0.1f, 0.1f), 0.1f);
            GetComponentInChildren<SpriteController>().AddLayerIndex(10);
        }
    }

    private void OnMouseExit()
    {
        if (!IsBlue) return;
        TryGetComponent<SpellCardDisplay>(out var spellCard);
        if (spellCard != null)
        {
            PartyManager.Instance.HideSpellDescription();
        }
        if (!PartyManager.Instance.CursorOccupied)
            DeselectCard();
    }

    /// <summary>
    /// Move the card down to your hand
    /// </summary>
    public void DeselectCard()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

        if (CurrentSlot == null)
        {
            HandPosition.x = transform.localPosition.x;
            TransitionManager.ChangeLocalPosition(this.gameObject, HandPosition, 0.1f);
            GetComponentInChildren<SpriteController>().RemoveLayerIndex(10);
        }
    }

    private void OnMouseDrag()
    {
        if (!IsBlue) return;
        PartyManager.Instance.CursorOccupied = true;
        DragCard(_board.CurrentMousePosition.x, _board.CurrentMousePosition.z);
    }

    /// <summary>
    /// Card follow a position when dragged
    /// </summary>
    /// <param name="xPosition">XWorldPositionToFollow</param>
    /// <param name="zPosition">ZWorldPositionToFollow</param>
    public void DragCard(float xPosition, float zPosition)
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;
        Vector3 newWorldPosition = new Vector3(xPosition, _startYPos + heightFromBoard, zPosition);
        var difference = newWorldPosition - transform.position;

        var speed = speedDrag * difference;
        Rigidbody.isKinematic = false;
        Rigidbody.velocity = speed;

        Rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z * rotation, 0, -speed.x * rotation));
    }


    private void OnMouseUp()
    {
        if (!IsBlue) return;
        DropCard();
        PartyManager.Instance.CursorOccupied = false;

    }

    /// <summary>
    /// Drop the card dragged to a slot are your hand if no result
    /// </summary>
    public void DropCard()
    {
        if (BoardController.Instance.Board.GetAllyBoardSide(IsBlue).IsSideReady) return;

        if (CurrentSlot != null)
        {
            CurrentSlot.Occup(this);
            spriteController.SizeSmall();
        }
        else
        {
            BoardSideController boardSide = IsBlue ? PartyManager.Instance.BoardController.BoardSideBlue : PartyManager.Instance.BoardController.BoardSideRed;
            boardSide.PlaceToHand(this);
            // Retourner la carte à sa position d'origine si le slot est occupé ou si aucun slot n'est défini
            Rigidbody.velocity = Vector3.zero;
            spriteController.SizeNormal();
            Lock();
        }
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

    public void Hide()
    {
        spriteController.Hide();
    }
    public async Task Reveal()
    {
        await spriteController.Reveal();
    }
}
