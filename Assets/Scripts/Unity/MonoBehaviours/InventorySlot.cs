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
    // Permet de savoir si plusieurs �l�ments peuvent �tre plac�s dans ce slot

    [SerializeField]
    private bool replaceElementOnDrag = false;
    // Permet de savoir si l'�l�ment actuel doit �tre remplac� par l'�l�ment d�plac�

    [SerializeField]
    private bool replaceWithPreviousPlace = true;
    // Permet de savoir si l'�l�ment actuel doit �tre remplac� � son emplacement pr�c�dent ou �chang� avec l'�l�ment d�plac�

    private DraggableItem _currItem;
    public DraggableItem CurrentItem
    {
        get => _currItem;
        set
        {
            if (_currItem != null)
            {
                if (_currItem.TryGetComponent<ElementCardContainer>(out var container))
                    container.RemoveFromDeck();
            }
            if (value != null)
            {
                if (value.TryGetComponent<ElementCardContainer>(out var container))
                    container.AddToDeck();
            }
            _currItem = value;
        }
    }

    public void InitCurrentItem(DraggableItem item) => _currItem = item;

    // R�f�rence � l'�l�ment actuellement plac� dans ce slot

    public void OnDrop(PointerEventData eventData)
    {
        // Si plusieurs �l�ments sont autoris�s ou si le slot est vide, place l'�l�ment d�plac� dans ce slot
        if (multipleElements || transform.childCount == 0)
        {
            PlaceItem(eventData.pointerDrag);
        }
        // Si l'�l�ment actuel doit �tre remplac� par l'�l�ment d�plac�
        else if (transform.childCount > 0 && replaceElementOnDrag)
        {
            print("child > 0");
            // Si l'�l�ment actuel doit �tre remplac� � son emplacement pr�c�dent
            if (replaceWithPreviousPlace)
            {
               
                RemoveCurrentPrevious();
                PlaceItem(eventData.pointerDrag);
            }
            // Si l'�l�ment actuel doit �tre �chang� avec l'�l�ment d�plac�
            else
            {
                print("switch");
                SwitchCurrent(eventData.pointerDrag);
            }
            // Place l'�l�ment d�plac� dans ce slot
           
        }
    }

    public void RemoveCurrentPrevious()
    {
        // D�place l'�l�ment actuel � son emplacement pr�c�dent
        CurrentItem.PlaceToSlot(CurrentItem.previousParentTransform);
        CurrentItem = null;
    }

 

    public void SwitchCurrent(GameObject dropped)
    {
        // �change l'emplacement de l'�l�ment actuel avec celui de l'�l�ment d�plac�
        Transform parent = dropped.GetComponent<DraggableItem>().parentAfterDrag;
        if (parent.GetComponent<InventorySlot>() != null)
        {
            print("current item " + CurrentItem.name + "has to move to parent" + parent.name);
            CurrentItem.PlaceToSlot(parent);
            CurrentItem = null;
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
            // R�cup�re la r�f�rence � l'�l�ment d�plac�
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            // Place l'�l�ment d�plac� dans ce slot
            draggableItem.PlaceToSlot(transform);
        }
        catch
        {
            Debug.LogWarning("Eumm attention il y a une erreur de try");
        }
    }


    public bool GetMultipleElements() { return multipleElements; }
}
