using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableSlot3D : MonoBehaviour
{
    public DraggableItem3D draggableItem;
    public enum State
    {
        Deactivated,
        Selected,
        Occupied
    }

    public State state;

    [SerializeField] GameObject visualIndicator;
    [SerializeField] ParticleSystem snapVisualIndicator;
    [SerializeField] AudioClip onDropClip;
    private AudioSource audioSource;

    [SerializeField] AudioClip moveUpClip;
    [SerializeField] AudioClip moveDownClip;
    [SerializeField] float targetHeight = 1;
    private float defaultHeight;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private float startTime;
    private float duration;

    

    [SerializeField] Transform parentTransform;
    private bool isTranslating = false;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        visualIndicator.SetActive(false);
        defaultHeight = transform.localPosition.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        DraggableItem3D draggableItem = other.GetComponent<DraggableItem3D>();
        if (draggableItem != null && state == State.Deactivated)
        {
            draggableItem.SetCurrentSlot(this);
            Select();

        }
    }


    private void OnTriggerExit(Collider other)
    {
        DraggableItem3D draggableItem = other.GetComponent<DraggableItem3D>();
        if (draggableItem != null)
        {
            if (state==State.Selected)
            {
                if(draggableItem._currentSlot==this)
                    draggableItem.SetCurrentSlot(null);
                Deactive();

            }
           
            
        }
    }

    public void Deactive()
    {
        this.draggableItem = null;
        state = State.Deactivated;
        MoveDown();
        visualIndicator.SetActive(false);
        audioSource.clip = moveDownClip;
        audioSource.Play();

    }

    public void Select()
    {
        MoveUp();
        state = State.Selected;
        visualIndicator.SetActive(true);
        audioSource.clip = moveUpClip;
        audioSource.Play();


    }

    public void Occup(DraggableItem3D draggable)
    {
        state = State.Occupied;
        this.draggableItem = draggable;
        audioSource.clip = onDropClip;
        audioSource.Play();
        snapVisualIndicator.Play();
        visualIndicator.SetActive(false);

        draggable.transform.SetParent(parentTransform);
        draggable._rigidbody.velocity = Vector3.zero;
        draggable._rigidbody.isKinematic = true;
        draggable.transform.localPosition = Vector3.zero;

        Debug.Log("occup");

    }


    private void MoveUp()
    {
        TranslateToHeight(targetHeight, 0.7f);
        audioSource.clip = moveUpClip;
        audioSource.Play();
    }

    private void MoveDown()
    {
        TranslateToHeight(defaultHeight, 1.2f);
        audioSource.clip = moveDownClip;
        audioSource.Play();
    }

    void Update()
    {
        if (isTranslating)
        {
            // Calculer le pourcentage du temps écoulé
            float percentageComplete = (Time.time - startTime) / duration;

            // Si le pourcentage est supérieur à 1, la translation est terminée
            if (percentageComplete >= 1f)
            {
                transform.localPosition = targetPosition;
                isTranslating = false;
                return;
            }

            // Interpoler entre la position initiale et la position cible
            transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, percentageComplete);
        }
    }



    // Fonction pour démarrer la translation vers une hauteur spécifique avec une durée spécifiée
    public void TranslateToHeight(float targetHeight, float moveDuration)
    {
        initialPosition = transform.localPosition;
        targetPosition = new Vector3(initialPosition.x, targetHeight, initialPosition.z);
        startTime = Time.time;
        duration = moveDuration;
        isTranslating = true;
    }



}
