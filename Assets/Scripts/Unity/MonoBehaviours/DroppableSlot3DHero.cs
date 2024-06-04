using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AFM_DLL;

public class DroppableSlot3DHero : DroppableSlot3D
{
    [SerializeField] SpriteRenderer heroSprite;

    [SerializeField] List<Sprite> heroSprites;

    [SerializeField] ParticleSystem looseFight;
    protected override void CheckSelection(DraggableItem3D draggableItem)
    {
        if (draggableItem.Type == GameManager.Type.Element && BoardController.Instance.Board.GetAllyBoardSide(_isBlue).Player.ManaPoints>=2)
        {
            base.CheckSelection(draggableItem);
        }
    }

    public override void Occup(DraggableItem3D draggable)
    {
        print("Card Changed " + draggable.GetComponent<ElementCardDisplay>().ElementCard.ActiveElement);
        if (!BoardController.Instance.Board.ReplacePlayerHeroWithCard(_isBlue, draggable.GetComponent<ElementCardDisplay>().ElementCard))
            Debug.LogError("Ajout du héro non réussi");
        base.Occup(draggable);

    }

    public void SwitchSprite(Element element)
    {
        switch (element)
        {
            case Element.ROCK:
                heroSprite.sprite = heroSprites[0];
                break;
            case Element.PAPER:
                heroSprite.sprite = heroSprites[1];
                break;
            case Element.SCISSORS:
                heroSprite.sprite = heroSprites[2];
                break;
        }
    }

    public override void Deactive()
    {
        if (_draggableItem != null)
        {
            print("Hero actuel = " + BoardController.Instance.Board.GetAllyBoardSide(_isBlue).Player.Deck.Hero.ActiveElement);
            BoardController.Instance.Board.CancelReplacePlayerHero(_isBlue);
            //SwitchSprite(BoardController.Instance.Board.GetAllyBoardSide(_isBlue).Player.Deck.Hero.ActiveElement);
        }
        base.Deactive();
    }

    public void ChangeHero()
    {
        if (_draggableItem != null)
        {
            SwitchSprite(_draggableItem.GetComponent<ElementCardDisplay>().ElementCard.ActiveElement);
            _draggableItem.GetComponent<ElementCardDisplay>().Delete();
        }
        DestroyDraggable();
    }

    public void StartEvaluateHero(int result)
    {
        TransitionManager.ChangeLocalPosition(this.gameObject, this.gameObject.transform.localPosition + Vector3.up/4, 0.2f);
        if (result == -1)
        {
            StartCoroutine(LoseColumn());
        }
    }

    IEnumerator LoseColumn()
    {
        looseFight.gameObject.SetActive(true);
        looseFight.Play();
        yield return new WaitForSeconds(1f);
        looseFight.gameObject.SetActive(false);



    }

    public void StopEvaluateHero()
    {
        TransitionManager.ChangeLocalPosition(this.gameObject, this.gameObject.transform.localPosition - Vector3.up/4, 0.2f);
    }
}
