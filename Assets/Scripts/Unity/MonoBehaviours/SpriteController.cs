using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] SpriteRenderer _childrenSprite;
    [SerializeField] float alphaDuration = 0.5f;
    [SerializeField] float scaleDuration = 0.5f;

    public void SetTransparent()
    {     
           TransitionManager.ChangeColor(_sprite.gameObject, new Color(1, 1, 1, 0.8f), alphaDuration);
           TransitionManager.ChangeColor(_childrenSprite.gameObject, new Color(1, 1, 1, 0.8f), alphaDuration);     
    }

    public void SetOpaque()
    {
            TransitionManager.ChangeColor(_sprite.gameObject, new Color(1, 1, 1, 1f), alphaDuration);
            TransitionManager.ChangeColor(_childrenSprite.gameObject, new Color(1, 1, 1, 1f), alphaDuration);       
    }

    void ChangeSize(float size)
    {
        TransitionManager.ChangeSize(this.gameObject, size, scaleDuration);
    }
    public void SizeBig()
    {
        ChangeSize(0.22f);
    }

    public void SizeNormal()
    {
        ChangeSize(0.18f);
    }

    public void SizeSmall()
    {
        ChangeSize(0.10f);
    }

    public void AddLayerIndex(uint number)
    {
        SetLayerIndex(_childrenSprite.sortingOrder+(int)number);

    }

    public void RemoveLayerIndex(uint number)
    {
        SetLayerIndex(_childrenSprite.sortingOrder - (int)number);
    }

    public void SetLayerIndex(int index)
    {
        _sprite.sortingOrder = index+1;
        _childrenSprite.sortingOrder = index;
    }

    public void SetChildSprite(Sprite sprite)
    {
        _childrenSprite.sprite = sprite;
    }



   

   
}
