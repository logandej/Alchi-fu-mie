using UnityEngine;

public class DroppableSlot3D : MonoBehaviour
{

    protected DraggableItem3D _draggableItem;
    public enum State
    {
        Deactivated,
        Selected,
        Occupied
    }

    public State CurrentState;

    [Header("Effects")]
    [SerializeField] GameObject _visualIndicator;
    [SerializeField] ParticleSystem _snapVisualIndicator;

    [Header("Audio")]
    private AudioSource _audioSource;
    [SerializeField] AudioClip _onDropClip;
    [SerializeField] AudioClip _moveUpClip;
    [SerializeField] AudioClip _moveDownClip;

    [Header("Animation")]
    [SerializeField] float _targetHeight = 1;
    private float _defaultHeight;

    

    [SerializeField] Transform _parentTransform;

   



    protected void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _visualIndicator.SetActive(false);
        //Définition de la hauteur de base de la pierre
        _defaultHeight = transform.localPosition.y;
    }

    // Actions quand une carte rentre dans la zone de trigger 
    private void OnTriggerEnter(Collider other)
    {      
        DraggableItem3D draggableItem = other.GetComponent<DraggableItem3D>();
        if (draggableItem != null && CurrentState == State.Deactivated)
        {
            CheckSelection(draggableItem);
        }
    }

    protected virtual void CheckSelection(DraggableItem3D draggableItem)
    {
        draggableItem.SetCurrentSlot(this);
        draggableItem.spriteController.SetTransparent();
      

        Select();
    }

    //Actions quand on retire la carte 
    private void OnTriggerExit(Collider other)
    {
        DraggableItem3D draggableItem = other.GetComponent<DraggableItem3D>();
        if (draggableItem != null)
        {
            if (CurrentState==State.Selected)
            {
                if (draggableItem.CurrentSlot == this)
                {
                    draggableItem.spriteController.SetOpaque();
                    draggableItem.SetCurrentSlot(null);
                }
                Deactive();
            }           
        }
    }
    ///
    /// <summary>
    /// Met le status de du slot à désactivé
    /// </summary>
    public virtual void Deactive()
    {   
        this._draggableItem = null;
        CurrentState = State.Deactivated;
        MoveDown();
        _visualIndicator.SetActive(false);
        _audioSource.clip = _moveDownClip;
        _audioSource.Play();
    }

    public void DestroyDraggable()
    {
        if (this._draggableItem != null)
        {
            Destroy(this._draggableItem.gameObject);
        }
        this._draggableItem = null;
        CurrentState = State.Deactivated;
        MoveDown();
    }


    /// <summary>
    /// Met le status du slot en Select, prêt à accueillir la carte
    /// </summary>
    private void Select()
    {
        MoveUp();
        CurrentState = State.Selected;
        _visualIndicator.SetActive(true);
        _audioSource.clip = _moveUpClip;
        _audioSource.Play();
    }
    /// <summary>
    /// Met le status du slot à occupé
    /// </summary>
    public virtual void Occup(DraggableItem3D draggable)
    {
        CurrentState = State.Occupied;
        this._draggableItem = draggable;
        _audioSource.clip = _onDropClip;
        _audioSource.Play();
        _snapVisualIndicator.Play();
        _visualIndicator.SetActive(false);

        draggable.spriteController.SetOpaque();
        draggable.transform.SetParent(_parentTransform);
        draggable.Rigidbody.velocity = Vector3.zero;
        draggable.Rigidbody.isKinematic = true;
        TransitionManager.ChangeLocalPosition(draggable.gameObject, Vector3.zero, 0.3f);
    }
    /// <summary>
    ///Monte la pierre 
    /// </summary>
    private void MoveUp()
    {
        TransitionManager.ChangeLocalPosition(this.gameObject, new Vector3(transform.localPosition.x, _targetHeight, transform.localPosition.z), 0.7f);
        _audioSource.clip = _moveUpClip;
        _audioSource.Play();
    }
    /// <summary>
    /// Descend la pierre
    /// </summary>
    private void MoveDown()
    {
        TransitionManager.ChangeLocalPosition(this.gameObject, new Vector3(transform.localPosition.x, _defaultHeight, transform.localPosition.z), 0.7f);
        _audioSource.clip = _moveDownClip;
        _audioSource.Play();
    }





}
