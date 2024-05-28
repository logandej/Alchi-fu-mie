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

        DrawCards();


    }

    public void DrawCards()
    {
        var res = Board.DrawCards();
        BoardSideBlue.DrawCards(res.BlueSideDrawResult);

        print("DRAWRESULTBLUE=" + res.BlueSideDrawResult.DrawnElements.Count);

        BoardSideRed.DrawCards(res.RedSideDrawResult);

        print("DRAWRESULTRED=" + res.RedSideDrawResult.DrawnElements.Count);
    }

    public void SetPlayerReady(bool isBlue)
    {
        //True if 2 are ready
        if (this.Board.SetSideReady(isBlue))
        {
            print("HandCountBlue = "+this.Board.GetAllyBoardSide(true).Player.Hand.Elements.Count);
            print("HandCountRed = "+this.Board.GetAllyBoardSide(false).Player.Hand.Elements.Count);

            EvaluateSpell();
            EvaluateCardColumns();
            

        }
    }

    public void EvaluateSpell()
    {
        this.Board.EvaluateSpells();
        Debug.Log("Spells");
    }

    public void EvaluateCardColumns()
    {
        Debug.Log("Columns");
        var result = this.Board.EvaluateCardColumns();

        BoardSideBlue.UpdateHealthAndMana();
        BoardSideRed.UpdateHealthAndMana();

        print("Left" + result[BoardPosition.LEFT].CardFightResult);
        print("Middle" + result[BoardPosition.MIDDLE].CardFightResult);
        print("Right" + result[BoardPosition.RIGHT].CardFightResult);

        BoardSideBlue.DestroyPlacedCards();
        BoardSideRed.DestroyPlacedCards();

        DrawCards();
    }
}
 

