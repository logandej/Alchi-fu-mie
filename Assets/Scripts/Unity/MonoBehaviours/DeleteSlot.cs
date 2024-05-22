using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // V�rifie si le composant DraggableItem est attach� � l'objet en cours de drag-and-drop
        DraggableItem draggableItem = null;
        if (eventData.pointerDrag.TryGetComponent(out draggableItem))
        {
            // Si le composant DraggableItem est attach�, supprime l'objet
            Destroy(draggableItem.gameObject);
        }
    }

}
