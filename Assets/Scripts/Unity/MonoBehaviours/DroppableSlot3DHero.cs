using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AFM_DLL;
using System.Threading.Tasks;

public class DroppableSlot3DHero : DroppableSlot3D
{
    [SerializeField] SpriteRenderer heroSprite;

    [SerializeField] ParticleSystem looseFight;

    [SerializeField] GameObject cardSlot;

    public HeroScriptable heroScriptable { get; private set; }
    protected override void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _visualIndicator.SetActive(false);
        //Définition de la hauteur de base de la pierre
        _defaultHeight = cardSlot.transform.localPosition.y;
    }

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

    public bool HasCard()
    {
        return _draggableItem != null;
    }

    /// <summary>
    /// To call only at the beginning of the 
    /// </summary>
    /// <param name="element"></param>
    public void SetStartHero(Element element)
    {
        print(PartyManager.Instance.HerosByElement[element].elementType);
        heroScriptable = PartyManager.Instance.HerosByElement[element];
        heroSprite.sprite = heroScriptable.sprite;
    }

    private void SwitchHero(Element element)
    {
        heroScriptable = PartyManager.Instance.HerosByElement[element];
        heroSprite.sprite = heroScriptable.sprite;
        AudioManager.Instance.PlayEffect(heroScriptable.onAppearingClip);
        Instantiate(heroScriptable.onAppearingParticle,this.transform.position,Quaternion.Euler(0,0,0));
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
            SwitchHero(_draggableItem.GetComponent<ElementCardDisplay>().ElementCard.ActiveElement);
            

            _draggableItem.GetComponent<ElementCardDisplay>().Delete();
        }
        DestroyDraggable();
    }

    public void StartEvaluateHero(int result)
    {
        StartCoroutine(Attack(result));
    }

    public void DestroyParticle(GameObject particle)
    {
        Destroy(particle);
    }

    IEnumerator Attack(int result)
    {
        //TransitionManager.ChangeLocalPosition(cardSlot, cardSlot.transform.localPosition + Vector3.up/4, 0.2f);
        var attackParticle = Instantiate(heroScriptable.attackParticle, transform.position, Quaternion.Euler(0,0,0));
        yield return new WaitForSeconds(1);
        TransitionManager.ChangePosition(attackParticle.gameObject, BoardController.Instance.GetBoardSideController(!_isBlue).GetHeroSlotPosition(), 2);
        if (result == 1)
        {
            yield return new WaitForSeconds(3);
            DestroyParticle(attackParticle.gameObject);
        }
        else
        {
            float time = result == 0 ? 2 : 1;
            yield return new WaitForSeconds(time);
            DestroyParticle(attackParticle.gameObject);
            if(result==-1)
                yield return new WaitForSeconds(1f);
            StartCoroutine(LoseColumn());

        }
    }

    

    IEnumerator LoseColumn()
    {
        looseFight.gameObject.SetActive(true);
        looseFight.Play();
        AudioManager.Instance.PlayEffectPitch("mexplosion");
        yield return new WaitForSeconds(3f);
        looseFight.gameObject.SetActive(false);
    }

    public void StopEvaluateHero()
    {
        //TransitionManager.ChangeLocalPosition(cardSlot, cardSlot.transform.localPosition - Vector3.up/4, 0.2f);
    }

    /// <summary>
    ///Monte la pierre 
    /// </summary>
    protected override void MoveUp()
    {
        TransitionManager.ChangeLocalPosition(cardSlot.gameObject, new Vector3(cardSlot.transform.localPosition.x, _targetHeight, cardSlot.transform.localPosition.z), 0.7f);
        _audioSource.clip = _moveUpClip;
        _audioSource.Play();
    }
    /// <summary>
    /// Descend la pierre
    /// </summary>
    protected override void MoveDown()
    {
        TransitionManager.ChangeLocalPosition(cardSlot.gameObject, new Vector3(cardSlot.transform.localPosition.x, _defaultHeight, cardSlot.transform.localPosition.z), 0.7f);
        _audioSource.clip = _moveDownClip;
        _audioSource.Play();
    }
}
