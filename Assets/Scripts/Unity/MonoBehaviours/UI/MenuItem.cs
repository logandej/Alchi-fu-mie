using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MenuItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] List<GameObject> objectShowOnEnter; // Liste des objets à afficher lorsque le curseur entre dans la zone du bouton
    [SerializeField] List<GameObject> objectShowOnClick; // Liste des objets à afficher lors du clic sur le bouton
    [SerializeField] AudioClip enterClip; // Clip audio à jouer lorsque le curseur entre dans la zone du bouton
    [SerializeField] AudioClip clickClip; // Clip audio à jouer lors du clic sur le bouton
    private AudioSource audioSource; // Composant AudioSource pour jouer les clips audio
    public UnityEvent eventSelected { get; private set; } // Événement déclenché lorsque le bouton est sélectionné
    public bool isSelected { get; private set; } = false; // Indique si le bouton est actuellement sélectionné

    // Méthode appelée lors de l'initialisation de l'objet
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Récupérer le composant AudioSource attaché à cet objet
        eventSelected = new UnityEvent(); // Créer une nouvelle instance de UnityEvent pour l'événement de sélection
    }

    // Méthode pour sélectionner le bouton
    public void Select()
    {
        eventSelected.Invoke(); // Déclencher l'événement de sélection
        ListObjects.Show(objectShowOnClick); // Afficher les objets associés au clic
        isSelected = true; // Mettre à jour l'état de sélection
    }

    // Méthode pour désélectionner le bouton
    public void Deselect()
    {
        ListObjects.Hide(objectShowOnClick); // Cacher les objets associés au clic
        isSelected = false; // Mettre à jour l'état de sélection
    }

    // Méthode appelée lors du clic sur le bouton
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSelected)
        {
            Select(); // Sélectionner le bouton
            audioSource.clip = clickClip; // Définir le clip audio à jouer
            audioSource.Play(); // Jouer le clip audio
            ListObjects.Hide(objectShowOnEnter); // Cacher les objets associés à l'entrée du curseur

        }
    }

    // Méthode appelée lorsque le curseur entre dans la zone du bouton
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) // Vérifier si le bouton n'est pas déjà sélectionné
        {
            audioSource.clip = enterClip; // Définir le clip audio à jouer
            audioSource.Play(); // Jouer le clip audio
            ListObjects.Show(objectShowOnEnter); // Afficher les objets associés à l'entrée du curseur
        }
    }

    // Méthode appelée lorsque le curseur quitte la zone du bouton
    public void OnPointerExit(PointerEventData eventData)
    {
        ListObjects.Hide(objectShowOnEnter); // Cacher les objets associés à l'entrée du curseur
    }


}

