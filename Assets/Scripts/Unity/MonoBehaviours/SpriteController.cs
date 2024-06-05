using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] SpriteRenderer _childrenSprite;
    private Sprite _defaultSprite;
    [SerializeField] Sprite _hideSprite;
    [SerializeField] float alphaDuration = 0.5f;
    [SerializeField] float scaleDuration = 0.5f;

    [SerializeField] List<TMP_Text> _textsToHideOnHide;

    private void Awake()
    {
        _defaultSprite = _sprite.sprite;
    }

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
        TransitionManager.ChangeSize(this.gameObject, Vector3.one*size, scaleDuration);
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
        ChangeSize(0.11f);
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

    public void Hide()
    {
        _sprite.sprite = _hideSprite;
        _textsToHideOnHide.ForEach(c => c.gameObject.SetActive(false));
    }
    public async Task Reveal()
    {
        Vector3 defaultSize = _sprite.transform.localScale;
        TransitionManager.ChangeSize(this.gameObject, new Vector3(0,defaultSize.y,defaultSize.z), 0.5f);
        await Task.Delay(500);
        _textsToHideOnHide.ForEach(c => c.gameObject.SetActive(true));
        _sprite.sprite = _defaultSprite;
        TransitionManager.ChangeSize(this.gameObject, defaultSize, 0.5f);


    }





}
