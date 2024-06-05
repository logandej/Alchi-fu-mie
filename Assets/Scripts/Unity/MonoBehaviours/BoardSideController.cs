using AFM_DLL;
using AFM_DLL.Models.BoardData;
using AFM_DLL.Models.Cards;
using AFM_DLL.Models.PlayerInfo;
using AFM_DLL.Models.UnityResults;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BoardSideController : MonoBehaviour
{
    public BoardSide BoardSide;
    public Transform HandTransform;
    public Transform HandSpellTransform;

    public float translateX;


    [SerializeField] List<ElementCardScriptable> _elementDeckList;
    [SerializeField] List<SpellCardScriptable> _spellDeckList;

    public List<DroppableSlot3D> DroppableSlotList;

    [SerializeField] DroppableSlot3DHero _heroSlot;



    public Deck Deck;


    [SerializeField] DataBar _healtBar;
    [SerializeField] DataBar _manaBar;


    public bool IsBlue;
    public bool CanInteract { get; set; }

    private void Awake()
    {
        Deck = new Deck()
        {
            Hero = new Hero("LeafMan", Element.PAPER)
        };

        foreach (ElementCardScriptable element in _elementDeckList)
        {
            Deck.Elements.Add(new ElementCard(element.Element));
        }

        foreach (SpellCardScriptable spell in _spellDeckList)
        {
            Deck.Spells.Add(SpellCard.FromType(spell.SpellType));
        }

        _heroSlot.SetStartHero(Deck.Hero.ActiveElement);
    }

    private void Start()
    {
        ReplaceHand(HandTransform);
        ReplaceHand(HandSpellTransform);
        _healtBar.SetCounterTo(10);
        _manaBar.SetCounterTo(1);
    }

    public void DiscardPlacedCards()
    {
        foreach(var droppable in DroppableSlotList)
        {
            droppable.Discard();
        }
    }

    public async Task RevealCards()
    {
        foreach (var droppable in DroppableSlotList)
        {
            await droppable.RevealCard();
            await Task.Delay(1000);
        }
        await _heroSlot.RevealCard();
    }

    public void PlaceToHand(DraggableItem3D draggableItem3D)
    {
        if (draggableItem3D.Type == GameManager.Type.Element)
        {
            PlaceToParent(draggableItem3D, HandTransform);
        }
        else
        {
            PlaceToParent(draggableItem3D, HandSpellTransform);
        }
    }

    //InterpolationPlace
    void PlaceToParent(DraggableItem3D draggableItem3D,Transform parent)
    {
        draggableItem3D.transform.SetParent(parent);
        MoveToPosition(draggableItem3D.transform, Vector3.zero+draggableItem3D.HandPosition, 0.3f);
       
    }

    public void UpdateMana()
    {
        _manaBar.SetCounterTo(BoardController.Instance.Board.GetAllyBoardSide(IsBlue).Player.ManaPoints);
    }

    public void UpdateHealth()
    {
        _healtBar.SetCounterTo(BoardController.Instance.Board.GetAllyBoardSide(IsBlue).Player.HealthPoints);
    }

    public void RemoveMana(uint mana)
    {
        _manaBar.RemoveCounter(mana);
    }

    public void RemoveHealth(uint health)
    {
        _healtBar.RemoveCounter(health);
    }

    public void AddHealth(uint health)
    {
        _healtBar.AddCounter(health);
    }

    public void AddMana(uint mana)
    {
        _manaBar.AddCounter(mana);
    }

    public void UpdateHealthAndMana()
    {
        UpdateHealth();
        UpdateMana();
    }

    public HeroScriptable GetHeroScriptable()
    {
        return _heroSlot.heroScriptable;
    }



    void ReplaceHands()
    {
        ReplaceHand(HandTransform);
        ReplaceHand(HandSpellTransform);
    }
    void ReplaceHand(Transform handTransform)
    {
        for (int i = 0; i < handTransform.childCount; i++)
        {
            var vector = new Vector3(i * translateX, 0, 0);
            handTransform.GetChild(i).GetComponent<DraggableItem3D>().HandPosition = vector;
            handTransform.GetChild(i).GetComponentInChildren<SpriteController>().SetLayerIndex(i*2-100);
            handTransform.GetChild(i).transform.localPosition = vector;
        }
    }


    public void DrawCards(DrawResult drawResult)
    {

        //Instanciate Element Cards
        Debug.Log("DRAWN ELEMENTS NUMBER = " + drawResult.DrawnElements.Count);
        drawResult.DrawnElements.ForEach(c =>
        {
            string elementName = System.Enum.GetName(typeof(Element), c.ActiveElement);
            string resourcePath = $"ScriptableObjects/Elements/{elementName}";
            ElementCardScriptable data = Resources.Load<ElementCardScriptable>(resourcePath);
            if (data == null)
            {
                throw new System.Exception($"Resource not found at path: {resourcePath}");
            }

            PartyManager.Instance.ElementCardPrefab.Data = data;

            PartyManager.Instance.ElementCardPrefab.GetComponent<DraggableItem3D>().IsBlue = IsBlue;
            var ele = Instantiate(PartyManager.Instance.ElementCardPrefab, HandTransform);
            ele.ElementCard = c;
           
          
        }
       );

        if (drawResult.DrawnSpell != null)
        {
            //Instanciate Spell Card
            PartyManager.Instance.SpellCardPrefab.Data = Resources.Load<SpellCardScriptable>("ScriptableObjects/Spells/" + drawResult.DrawnSpell.SpellType);
            PartyManager.Instance.SpellCardPrefab.GetComponent<DraggableItem3D>().IsBlue = IsBlue;
            var ele = Instantiate(PartyManager.Instance.SpellCardPrefab, HandSpellTransform);
            ele.SpellCard = drawResult.DrawnSpell;
        }
       
        ReplaceHands();
    }


    // Fonction pour commencer l'animation de déplacement
    public void MoveToPosition(Transform card, Vector3 targetPosition, float duration)
    {
        StartCoroutine(MoveOverTime(card,targetPosition, duration));
    }

    // Coroutine pour animer la position
    private IEnumerator MoveOverTime(Transform card,Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = card.transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            card.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        card.transform.localPosition = targetPosition;
        ReplaceHands();
    }




    public void StartEvaluateSlot(int index, int result)
    {
        DroppableSlot3DElement elementSlot = (DroppableSlot3DElement)DroppableSlotList[index];
        elementSlot.StartEvaluateElementCard(result);
    }

    public void ChangeHero()
    {
        _heroSlot.ChangeHero();
    }

    public void StartEvalutateHero(int result)
    {
       _heroSlot.StartEvaluateHero(result);
    }
    public void StopEvalutateHero()
    {
        _heroSlot.StopEvaluateHero();
    }

    public void StopEvaluateSlot(int index)
    {

        DroppableSlot3DElement elementSlot = (DroppableSlot3DElement)DroppableSlotList[index];
        elementSlot.StopEvaluateElementCard();
    }

    public void StartSpell()
    {
        //3 Should be the index of the spellSlot
        DroppableSlot3DSpell spellSlot = (DroppableSlot3DSpell)DroppableSlotList[3];
        spellSlot.StartSpell();
    }

    public void StopSpell()
    {
        //3 Should be the index of the spellSlot
        DroppableSlot3DSpell spellSlot = (DroppableSlot3DSpell)DroppableSlotList[3];
        spellSlot.StopSpell();

    }

    public Vector3 GetHeroSlotPosition()
    {
        return _heroSlot.transform.position;
    }

}
