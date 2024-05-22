using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Vérifie si le composant DraggableItem est attaché à l'objet en cours de drag-and-drop
        DraggableItem draggableItem = null;
        if (eventData.pointerDrag.TryGetComponent(out draggableItem))
        {
            // Si le composant DraggableItem est attaché, supprime l'objet
            Destroy(draggableItem.gameObject);
        }
    }

}
