using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] AudioClip clipOnDrop;
    [SerializeField] ParticleSystem particleOnDrop;

    [SerializeField]
    private bool multipleElements = false;
    // Permet de savoir si plusieurs éléments peuvent être placés dans ce slot

    [SerializeField]
    private bool replaceElementOnDrag = false;
    // Permet de savoir si l'élément actuel doit être remplacé par l'élément déplacé

    [SerializeField]
    private bool replaceWithPreviousPlace = true;
    // Permet de savoir si l'élément actuel doit être remplacé à son emplacement précédent ou échangé avec l'élément déplacé

    public DraggableItem currentItem;
    // Référence à l'élément actuellement placé dans ce slot

    public void OnDrop(PointerEventData eventData)
    {
        // Si plusieurs éléments sont autorisés ou si le slot est vide, place l'élément déplacé dans ce slot
        if (multipleElements || transform.childCount == 0)
        {
            PlaceItem(eventData.pointerDrag);
        }
        // Si l'élément actuel doit être remplacé par l'élément déplacé
        else if (transform.childCount > 0 && replaceElementOnDrag)
        {
            print("child > 0");
            // Si l'élément actuel doit être remplacé à son emplacement précédent
            if (replaceWithPreviousPlace)
            {
               
                RemoveCurrentPrevious();
                PlaceItem(eventData.pointerDrag);
            }
            // Si l'élément actuel doit être échangé avec l'élément déplacé
            else
            {
                print("switch");
                SwitchCurrent(eventData.pointerDrag);
            }
            // Place l'élément déplacé dans ce slot
           
        }
    }

    public void RemoveCurrentPrevious()
    {
        // Déplace l'élément actuel à son emplacement précédent
        currentItem.PlaceToSlot(currentItem.previousParentTransform);
        currentItem = null;

    }

 

    public void SwitchCurrent(GameObject dropped)
    {
        // Échange l'emplacement de l'élément actuel avec celui de l'élément déplacé
        Transform parent = dropped.GetComponent<DraggableItem>().parentAfterDrag;
        if (parent.GetComponent<InventorySlot>() != null)
        {
            print("current item " + currentItem.name + "has to move to parent" + parent.name);
            currentItem.PlaceToSlot(parent);
            currentItem = null;
            PlaceItem(dropped);
        }
    }

    public void PlaceItem(GameObject dropped)
    {
        try
        {
            if (particleOnDrop != null)
                particleOnDrop.Play();
            AudioManager.Instance.PlayEffect(clipOnDrop);
            // Récupère la référence à l'élément déplacé
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            // Met à jour la référence à l'élément actuellement placé dans ce slot
            currentItem = draggableItem;
            // Place l'élément déplacé dans ce slot
            draggableItem.PlaceToSlot(transform);
        }
        catch
        {
            Debug.LogWarning("Eumm attention il y a une erreur de try");
        }
    }


    public bool GetMultipleElements() { return multipleElements; }
}
