using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    [SerializeField] AudioClip clipOnDrag;
    [SerializeField] GameObject particleFollow;

    [HideInInspector]
    public Transform previousParentTransform;
    [HideInInspector]
    public Transform parentAfterDrag;
    public AspectRatioFitter aspectRatioFitter;
    public bool duplicateOnDrag;

    private Camera cameraUI;
    private void Start()
    {
        if (particleFollow != null)
            particleFollow.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
       
        cameraUI = GameManager.Instance.cameraUI;
        Debug.Log("Begin Drag");
        if (duplicateOnDrag)
        {
            Instantiate(this.gameObject, transform.position, transform.rotation, transform.parent);
            duplicateOnDrag = false;
        }

        if (particleFollow != null)
            particleFollow.SetActive(true);

        AudioManager.Instance.PlayEffect(clipOnDrag);

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

        if (previousParentTransform.TryGetComponent<InventorySlot>(out var inventory))
        {
            inventory.CurrentItem = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        // Convertit les coordonnées de la souris de l'écran aux coordonnées mondiales
        Vector3 mousePosition = cameraUI.ScreenToWorldPoint(Input.mousePosition);

        // Ajuste la hauteur Z pour correspondre à la position actuelle de l'objet
        mousePosition.z = 100; 

        // Déplace l'objet à la nouvelle position
        transform.position = mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Réactive le composant AspectRatioFitter pour restaurer le redimensionnement de l'objet
        if (aspectRatioFitter != null && parentAfterDrag.GetComponent<InventorySlot>() != null && !parentAfterDrag.GetComponent<InventorySlot>().GetMultipleElements())
        {
            aspectRatioFitter.enabled = true;
        }
        if(particleFollow!=null)
            particleFollow.SetActive(false);
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
        if (aspectRatioFitter != null && parentAfterDrag.GetComponent<InventorySlot>().GetMultipleElements())
        {
            aspectRatioFitter.enabled = false;
        }
        // Déplace l'objet vers le nouveau parent
        transform.SetParent(parentAfterDrag);
        // Réinitialise la position locale de l'objet
        transform.localPosition = Vector3.zero;
        transformSlot.GetComponent<InventorySlot>().CurrentItem = this;
    }
}
