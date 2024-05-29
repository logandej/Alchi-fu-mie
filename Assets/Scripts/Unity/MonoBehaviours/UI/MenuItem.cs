using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MenuItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
  
    private Sprite defaultSprite;
    [Header("Change Background Sprite (optional)")]
    [SerializeField] Sprite enterSprite;
    [SerializeField] Sprite clickSprite;
    private Button button;

    [Header("Additional To Show (optional)")]
    [SerializeField] List<GameObject> objectShowOnEnter; // Liste des objets � afficher lorsque le curseur entre dans la zone du bouton
    [SerializeField] List<GameObject> objectShowOnClick; // Liste des objets � afficher lors du clic sur le bouton

    [Header("Audio (mandatory)")]
    [SerializeField] AudioClip enterClip; // Clip audio � jouer lorsque le curseur entre dans la zone du bouton
    [SerializeField] AudioClip clickClip; // Clip audio � jouer lors du clic sur le bouton
    private AudioSource audioSource; // Composant AudioSource pour jouer les clips audio
    public UnityEvent EventSelected { get; private set; } // �v�nement d�clench� lorsque le bouton est s�lectionn�
    public bool IsSelected { get; private set; } = false; // Indique si le bouton est actuellement s�lectionn�

    [Header("Other Settings")]
    [SerializeField] bool oneClickOnly = false;

    // M�thode appel�e lors de l'initialisation de l'objet
    private void Awake()
    {
        button = GetComponent<Button>();
        defaultSprite=button.image.sprite;
        audioSource = GetComponent<AudioSource>(); // R�cup�rer le composant AudioSource attach� � cet objet
        EventSelected = new UnityEvent(); // Cr�er une nouvelle instance de UnityEvent pour l'�v�nement de s�lection
    }

    // M�thode pour s�lectionner le bouton
    public void Select()
    {
       
        EventSelected.Invoke(); // D�clencher l'�v�nement de s�lection
        ListObjects.Show(objectShowOnClick); // Afficher les objets associ�s au clic
        if (clickSprite != null)
            button.image.sprite = clickSprite;
        IsSelected = true; // Mettre � jour l'�tat de s�lection
    }

    // M�thode pour d�s�lectionner le bouton
    public void Deselect()
    {
        button.image.sprite = defaultSprite;
        ListObjects.Hide(objectShowOnClick); // Cacher les objets associ�s au clic
        IsSelected = false; // Mettre � jour l'�tat de s�lection
    }

    // M�thode appel�e lors du clic sur le bouton
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            Select(); // S�lectionner le bouton
            audioSource.clip = clickClip; // D�finir le clip audio � jouer
            audioSource.Play(); // Jouer le clip audio
            ListObjects.Hide(objectShowOnEnter); // Cacher les objets associ�s � l'entr�e du curseur
            if (oneClickOnly)
                Deselect();

        }
    }

    // M�thode appel�e lorsque le curseur entre dans la zone du bouton
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsSelected) // V�rifier si le bouton n'est pas d�j� s�lectionn�
        {
            if (enterSprite != null)
                button.image.sprite = enterSprite;
            audioSource.clip = enterClip; // D�finir le clip audio � jouer
            audioSource.Play(); // Jouer le clip audio
            ListObjects.Show(objectShowOnEnter); // Afficher les objets associ�s � l'entr�e du curseur
        }
    }

    // M�thode appel�e lorsque le curseur quitte la zone du bouton
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!IsSelected)
            button.image.sprite = defaultSprite;
        ListObjects.Hide(objectShowOnEnter); // Cacher les objets associ�s � l'entr�e du curseur
    }


}

