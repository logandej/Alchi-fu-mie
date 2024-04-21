using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    [HideInInspector]
    public Transform previousParentTransform;
    [HideInInspector]
    public Transform parentAfterDrag;
    public AspectRatioFitter aspectRatioFitter;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        // Désactive le composant AspectRatioFitter pour éviter les problèmes de redimensionnement pendant le drag
        if(aspectRatioFitter!=null)
            aspectRatioFitter.enabled = false;
        // Enregistre le parent actuel de l'objet
        previousParentTransform = transform.parent;
        // Enregistre le parent actuel comme parent après le drag
        parentAfterDrag = transform.parent;
        // Déplace l'objet à la racine de la hiérarchie pour éviter les problèmes de positionnement pendant le drag
        transform.SetParent(transform.root);
        // Déplace l'objet en dernier enfant de la hiérarchie pour qu'il soit affiché au-dessus des autres objets
        transform.SetAsLastSibling();
        // Désactive le raycasting sur l'image pour éviter les problèmes de sélection pendant le drag
        image.raycastTarget = false;

        InventorySlot inventory = transform.parent.GetComponent<InventorySlot>();
        if (inventory != null)
        {
            inventory.currentItem = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Déplace l'objet à la position de la souris
        transform.position = Input.mousePosition;
      
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Réactive le composant AspectRatioFitter pour restaurer le redimensionnement de l'objet
        if (aspectRatioFitter != null)
            aspectRatioFitter.enabled = true;
        // Remplace l'objet à son parent après le drag
        transform.SetParent(parentAfterDrag);
        // Réinitialise la position locale de l'objet
        transform.localPosition = Vector3.zero;
        // Réactive le raycasting sur l'image pour restaurer la sélection de l'objet
        image.raycastTarget = true;
    }

    public void PlaceToSlot(Transform transformSlot)
    {
        // Enregistre le parent actuel comme parent précédent
        previousParentTransform = parentAfterDrag;
        // Enregistre le nouveau parent comme parent après le drag
        parentAfterDrag = transformSlot;
        // Déplace l'objet vers le nouveau parent
        transform.SetParent(parentAfterDrag);
        // Réinitialise la position locale de l'objet
        transform.localPosition = Vector3.zero;
        transformSlot.GetComponent<InventorySlot>().currentItem = this;
    }
}
