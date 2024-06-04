using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AFM_DLL;
using AFM_DLL.Models.BoardData;
using AFM_DLL.Models.PlayerInfo;
using AFM_DLL.Models.Cards;
using AFM_DLL.Models.Enum;
using AFM_DLL.Helpers;
using AFM_DLL.Models.UnityResults;
using System.Threading.Tasks;

public class BoardController : MonoBehaviour
{

    public static BoardController Instance;

    public Board Board;

    public BoardSideController BoardSideBlue;
    public BoardSideController BoardSideRed;

    [SerializeField] AIPlayer _aiPlayer;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }
    void Start()
    {
        SetupBoard();
    }

    void SetupBoard()
    {
        Board = new Board(new PlayerGame(BoardSideBlue.Deck), new PlayerGame(BoardSideRed.Deck));

        this.BoardSideBlue.BoardSide = this.Board.GetAllyBoardSide(true);
        this.BoardSideRed.BoardSide = this.Board.GetAllyBoardSide(false);

        DrawCards();

    }

    public BoardSideController GetBoardSideController(bool isBlue)
    {
        return isBlue ? BoardSideBlue : BoardSideRed;
    }

    public void DrawCards()
    {
        var res = Board.DrawCards();
        Debug.Log("BoardController->Result of Drawn Elements count ="+res.BlueSideDrawResult.DrawnElements.Count);
        BoardSideBlue.DrawCards(res.BlueSideDrawResult);
        BoardSideRed.DrawCards(res.RedSideDrawResult);
        //Permet aux joueurs d'interagir
        if (_aiPlayer != null)
        {
            _aiPlayer.StartRound();
        }
        BoardSideBlue.UpdateHealthAndMana();
        BoardSideRed.UpdateHealthAndMana();

    }

    public void ShowReady()
    {
        PartyManager.Instance.ShowReady();
    }
    public void HideReady()
    {
        PartyManager.Instance.HideReady();
    }

    /// <summary>
    /// Set Player Ready
    /// </summary>
    /// <param name="isBlue">Is Blue</param>
    public async void SetPlayerReady(bool isBlue)
    {
        if(isBlue)
            HideReady();

        //True if 2 are ready
        if (this.Board.SetSideReady(isBlue))
        {
            PartyManager.Instance.ShowLetterBox();

            await BoardSideRed.RevealCards();

            if(Board.GetAllyBoardSide(true).Player.Deck.Hero.OverrideCard !=null || Board.GetAllyBoardSide(false).Player.Deck.Hero.OverrideCard != null)
            {
                await PartyManager.Instance.ShowHeroTitle();
                await Task.Delay(500);

                BoardSideBlue.ChangeHero();
                BoardSideRed.ChangeHero();

                await Task.Delay(500);

            }

            await EvaluateSpell();
            await EvaluateCardColumns();



            BoardSideBlue.DiscardPlacedCards();
            BoardSideRed.DiscardPlacedCards();

            //ReDraw
            if (this.Board.GetAllyBoardSide(true).Player.HealthPoints > 0 && this.Board.GetAllyBoardSide(false).Player.HealthPoints > 0) {
                this.Board.ResetBoard();

                PartyManager.Instance.HideLetterBox();
                DrawCards();
            }
            else
            {
                print("Partie Terminée !");
                BoardSideBlue.UpdateHealthAndMana();
                BoardSideRed.UpdateHealthAndMana();
                if(this.Board.GetAllyBoardSide(true).Player.HealthPoints > 0)
                {
                    await PartyManager.Instance.ShowVictory();
                }
                else if(this.Board.GetAllyBoardSide(false).Player.HealthPoints > 0)
                {
                    await PartyManager.Instance.ShowDefeat();
                }
                else
                {
                    await PartyManager.Instance.ShowTie();
                }
            }

        }
    }

    public async Task EvaluateSpell()
    {
        await Task.Delay(1000);
        var result = this.Board.EvaluateSpells();
        if (result.SpellsInOrder.Count != 0)
        {
         
            await PartyManager.Instance.ShowSpellsTitle();
            if (result.SpellsInOrder.Count == 1)
            {
                await ExecuteSpell(result.SpellsInOrder[0].isBlueSide ? BoardSideBlue : BoardSideRed);
                
            }
            else if (result.SpellsInOrder[0].isBlueSide)
            {
                await ExecuteSpell(BoardSideBlue);
                await Task.Delay(1000);
                await ExecuteSpell(BoardSideRed);
            }
            else
            {
                await ExecuteSpell(BoardSideRed);
                await Task.Delay(1000);
                await ExecuteSpell(BoardSideBlue);
            }
        }

    }

    public async Task ExecuteSpell(BoardSideController side)
    {
        side.StartSpell();
        await Task.Delay(1000);
        await BoardSideRed.UpdateOverrideElements();
        await BoardSideBlue.UpdateOverrideElements();
        BoardSideBlue.UpdateHealthAndMana();
        BoardSideRed.UpdateHealthAndMana();
        await Task.Delay(1000);
        side.StopSpell();
    }

    public async Task EvaluateCardColumns()
    {
        await Task.Delay(500);
        await PartyManager.Instance.ShowElementsTitle();
        Debug.Log("Columns");
        var result = this.Board.EvaluateCardColumns();
        
        await CheckAllColumns(result);
        


    }

    public async Task CheckAllColumns(Dictionary<BoardPosition, ColumnFightResult> result)
    {
        for(int i=0; i < 3; i++)
        {
            
            if(i==0)
                await CheckColumn(i,result[BoardPosition.LEFT]);
            else if(i==1)
                await CheckColumn(i, result[BoardPosition.MIDDLE]);
            else if(i==2)
                await CheckColumn(i, result[BoardPosition.RIGHT]);

            await Task.Delay(1000);
        }
    }

    public async Task CheckColumn(int index, ColumnFightResult fightResult)
    {
        //Initalise à Draw
        int blueValue = 0;
        int redValue = 0;

        if (fightResult.CardFightResult == FightResult.BLUE_WIN)
        {
            blueValue = 1;
            redValue = -1;
        }
        else if (fightResult.CardFightResult == FightResult.RED_WIN)
        {
            blueValue = -1;
            redValue = 1;
        }
        

        BoardSideBlue.StartEvaluateSlot(index, blueValue);
        BoardSideRed.StartEvaluateSlot(index, redValue);

        if (blueValue == 0)
        {
            int heroBlueValue = 0;
            int heroRedValue = 0;

            if (fightResult.HeroFightResult == FightResult.BLUE_WIN)
            {
                heroBlueValue = 1;
                heroRedValue = -1;
            }
            else if(fightResult.HeroFightResult == FightResult.RED_WIN)
            {
                heroBlueValue = -1;
                heroRedValue = 1;
            }

            await Task.Delay(500);

            BoardSideBlue.StartEvalutateHero(heroBlueValue);
            BoardSideRed.StartEvalutateHero(heroRedValue);

            await Task.Delay(4000);

            BoardSideBlue.StopEvalutateHero();
            BoardSideRed.StopEvalutateHero();
        }
 



        BoardSideBlue.UpdateHealthAndMana();
        BoardSideRed.UpdateHealthAndMana();
        await Task.Delay(1000);
        if(blueValue!=-1)
            BoardSideBlue.StopEvaluateSlot(index);
        if(redValue!=-1)
            BoardSideRed.StopEvaluateSlot(index);
    }
}
 

