using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera cameraUI;

    public Texture2D cursorTexture;

    public BoardController boardController;

    public Transform handTransform;
    public float translateX;

    public Transform handSpellTransform;
    public float translateSpellX;

    public enum Type
    {
        Element,
        Spell
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture,Vector2.zero,CursorMode.Auto);
        ReplaceHand(handTransform);
        ReplaceHand(handSpellTransform);
    }


    public void PlaceToHand(DraggableItem3D draggableItem3D)
    {
        if (draggableItem3D.type == Type.Element)
        {
            draggableItem3D.transform.SetParent(handTransform);
            draggableItem3D.transform.localPosition = Vector3.zero;
            ReplaceHand(handTransform);
        }
        else
        {
            draggableItem3D.transform.SetParent(handSpellTransform);
            draggableItem3D.transform.localPosition = Vector3.zero;
            ReplaceHand(handSpellTransform);
        }


    }

    void ReplaceHand(Transform handTransform)
    {
        for (int i= 0; i < handTransform.childCount; i++){
            handTransform.GetChild(i).transform.localPosition = new Vector3(i * translateX, 0, 0);
        }
    }
}
