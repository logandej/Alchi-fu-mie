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

        if (previousParentTransform.TryGetComponent<InventorySlot>(out var inventory))
        {
            inventory.CurrentItem = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        // Convertit les coordonn�es de la souris de l'�cran aux coordonn�es mondiales
        Vector3 mousePosition = cameraUI.ScreenToWorldPoint(Input.mousePosition);

        // Ajuste la hauteur Z pour correspondre � la position actuelle de l'objet
        mousePosition.z = 100; 

        // D�place l'objet � la nouvelle position
        transform.position = mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // R�active le composant AspectRatioFitter pour restaurer le redimensionnement de l'objet
        if (aspectRatioFitter != null && parentAfterDrag.GetComponent<InventorySlot>() != null && !parentAfterDrag.GetComponent<InventorySlot>().GetMultipleElements())
        {
            aspectRatioFitter.enabled = true;
        }
        if(particleFollow!=null)
            particleFollow.SetActive(false);
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
        if (aspectRatioFitter != null && parentAfterDrag.GetComponent<InventorySlot>().GetMultipleElements())
        {
            aspectRatioFitter.enabled = false;
        }
        // D�place l'objet vers le nouveau parent
        transform.SetParent(parentAfterDrag);
        // R�initialise la position locale de l'objet
        transform.localPosition = Vector3.zero;
        transformSlot.GetComponent<InventorySlot>().CurrentItem = this;
    }
}
