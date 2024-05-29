using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    private SpriteRenderer _sprite;
    [SerializeField] float alphaDuration = 0.5f;
    [SerializeField] float scaleDuration = 0.5f;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    public void SetTransparent()
    {
        TransitionManager.ChangeColor(_sprite.gameObject, new Color(1, 1, 1, 0.8f),alphaDuration);
       
    }

    public void SetOpaque()
    {
        TransitionManager.ChangeColor(_sprite.gameObject, new Color(1, 1, 1, 1f), alphaDuration);
    }

    void ChangeSize(float size)
    {
        TransitionManager.ChangeSize(_sprite.gameObject, size, scaleDuration);
    }
    public void SizeBig()
    {
        ChangeSize(0.12f);
    }

    public void SizeNormal()
    {
        ChangeSize(0.1f);
    }

    public void SizeSmall()
    {
        ChangeSize(0.055f);
    }

    public void SetLayerIndex(int index)
    {
        _sprite.sortingOrder = index;
    }



   

   
}
