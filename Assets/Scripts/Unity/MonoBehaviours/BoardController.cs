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

        this.Board.GetAllyBoardSide(true).Player.AddMana(5);
        this.Board.GetAllyBoardSide(false).Player.AddMana(5);

        DrawCards();

        BoardSideBlue.UpdateHealthAndMana();
        BoardSideRed.UpdateHealthAndMana();


    }

    public void DrawCards()
    {
        var res = Board.DrawCards();
        Debug.Log("BoardController->Result of Drawn Elements count ="+res.BlueSideDrawResult.DrawnElements.Count);
        BoardSideBlue.DrawCards(res.BlueSideDrawResult);
        BoardSideRed.DrawCards(res.RedSideDrawResult);
        //Permet aux joueurs d'interagir
   
    }

    public void ShowReady(bool isBlue)
    {
        GameManager.Instance.ShowReady(isBlue);
    }
    public void HideReady(bool isBlue)
    {
        GameManager.Instance.HideReady(isBlue);
    }

    public async void SetPlayerReady(bool isBlue)
    {
        HideReady(isBlue);

        //True if 2 are ready
        if (this.Board.SetSideReady(isBlue))
        {
            //GameManager.Instance.ShowLetterBox();
            await EvaluateSpell();
            await EvaluateCardColumns();
            BoardSideBlue.UpdateHealthAndMana();
            BoardSideRed.UpdateHealthAndMana();

            this.Board.ResetBoard();

            BoardSideBlue.DestroyPlacedCards();
            BoardSideRed.DestroyPlacedCards();

            //GameManager.Instance.HideLetterBox();
            //ReDraw
            if (this.Board.GetAllyBoardSide(BoardSideBlue).Player.HealthPoints > 0 || this.Board.GetAllyBoardSide(BoardSideRed).Player.HealthPoints > 0) {
                DrawCards();
            }
            else
            {
                print("Partie Terminée !");
            }

        }
    }

    public async Task EvaluateSpell()
    {
        await Task.Delay(1000);
        var result = this.Board.EvaluateSpells();
        if (result.SpellsInOrder.Count != 0)
        {
            await GameManager.Instance.ShowSpellsTitle();
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
        await Task.Delay(1000);
        side.StopSpell();
    }

    public async Task EvaluateCardColumns()
    {
        await Task.Delay(2000);
        await GameManager.Instance.ShowElementsTitle();
        Debug.Log("Columns");
        var result = this.Board.EvaluateCardColumns();
        
        await CheckAllColumns(result);
        


    }

    public async Task CheckAllColumns(Dictionary<BoardPosition, ColumnFightResult> result)
    {
        for(int i=0; i < 3; i++)
        {
            if(i==0)
                await CheckColumn(i,result[BoardPosition.LEFT].CardFightResult);
            else if(i==1)
                await CheckColumn(i, result[BoardPosition.MIDDLE].CardFightResult);
            else if(i==2)
                await CheckColumn(i, result[BoardPosition.RIGHT].CardFightResult);

            await Task.Delay(2000);
        }
    }

    public async Task CheckColumn(int index, FightResult fightResult)
    {
        //Initalise à Draw
        int blueValue = 0;
        int redValue = 0;

        if (fightResult == FightResult.BLUE_WIN)
        {
            blueValue = 1;
            redValue = -1;
        }
        else if (fightResult == FightResult.RED_WIN)
        {
            blueValue = -1;
            redValue = 1;
        }

        BoardSideBlue.StartEvaluateSlot(index, blueValue);
        BoardSideRed.StartEvaluateSlot(index, redValue);


        await Task.Delay(2000);
        if(blueValue!=-1)
            BoardSideBlue.StopEvaluateSlot(index);
        if(redValue!=-1)
            BoardSideRed.StopEvaluateSlot(index);
    }
}
 

