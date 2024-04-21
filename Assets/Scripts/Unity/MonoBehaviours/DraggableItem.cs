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
        // D�sactive le composant AspectRatioFitter pour �viter les probl�mes de redimensionnement pendant le drag
        if(aspectRatioFitter!=null)
            aspectRatioFitter.enabled = false;
        // Enregistre le parent actuel de l'objet
        previousParentTransform = transform.parent;
        // Enregistre le parent actuel comme parent apr�s le drag
        parentAfterDrag = transform.parent;
        // D�place l'objet � la racine de la hi�rarchie pour �viter les probl�mes de positionnement pendant le drag
        transform.SetParent(transform.root);
        // D�place l'objet en dernier enfant de la hi�rarchie pour qu'il soit affich� au-dessus des autres objets
        transform.SetAsLastSibling();
        // D�sactive le raycasting sur l'image pour �viter les probl�mes de s�lection pendant le drag
        image.raycastTarget = false;

        InventorySlot inventory = transform.parent.GetComponent<InventorySlot>();
        if (inventory != null)
        {
            inventory.currentItem = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // D�place l'objet � la position de la souris
        transform.position = Input.mousePosition;
      
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // R�active le composant AspectRatioFitter pour restaurer le redimensionnement de l'objet
        if (aspectRatioFitter != null)
            aspectRatioFitter.enabled = true;
        // Remplace l'objet � son parent apr�s le drag
        transform.SetParent(parentAfterDrag);
        // R�initialise la position locale de l'objet
        transform.localPosition = Vector3.zero;
        // R�active le raycasting sur l'image pour restaurer la s�lection de l'objet
        image.raycastTarget = true;
    }

    public void PlaceToSlot(Transform transformSlot)
    {
        // Enregistre le parent actuel comme parent pr�c�dent
        previousParentTransform = parentAfterDrag;
        // Enregistre le nouveau parent comme parent apr�s le drag
        parentAfterDrag = transformSlot;
        // D�place l'objet vers le nouveau parent
        transform.SetParent(parentAfterDrag);
        // R�initialise la position locale de l'objet
        transform.localPosition = Vector3.zero;
        transformSlot.GetComponent<InventorySlot>().currentItem = this;
    }
}
