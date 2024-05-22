using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem3D : MonoBehaviour
{
    
    public Rigidbody _rigidbody { private set; get; }
    private float _startYPos;
    private BoardController _board;
    public DroppableSlot3D _currentSlot { private set; get; } // Nouvelle variable pour stocker le slot actuel

    [SerializeField] float heightFromBoard = 5;
    [SerializeField] float speedDrag = 10;
    [SerializeField] float rotation = 4;

    public GameManager.Type type;

    void Start()
    {
        _board = GetComponentInParent<BoardController>();
        _rigidbody = GetComponent<Rigidbody>();
        _startYPos = 0; // Better to not hardcode that one but whatever
    }

    private void OnMouseDown()
    {
        if (_currentSlot != null)
        {
            _currentSlot.Deactive();
            SetCurrentSlot(null);
        }
    }

    private void OnMouseDrag()
    {
        Vector3 newWorldPosition = new Vector3(_board.CurrentMousePosition.x, _startYPos + heightFromBoard, _board.CurrentMousePosition.z);

        var difference = newWorldPosition - transform.position;

        var speed = speedDrag * difference;
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = speed;
     
        _rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z * rotation, 0, -speed.x * rotation));
    }

    private void OnMouseUp()
    {
       
        if (_currentSlot != null)
        {
            _currentSlot.Occup(this);
        }
        else
        {
            GameManager.Instance.PlaceToHand(this);
            // Retourner la carte à sa position d'origine si le slot est occupé ou si aucun slot n'est défini
            _rigidbody.velocity = Vector3.zero;
        }
    }

    // Méthode pour définir le slot actuel
    public void SetCurrentSlot(DroppableSlot3D slot)
    {
        _currentSlot = slot;
    }
}
