using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera cameraUI;

    public Texture2D cursorTexture;

    public BoardController BoardController;


    public ElementCardDisplay ElementCardPrefab;
    public SpellCardDisplay SpellCardPrefab;


    [SerializeField] GameObject _readyBlue;
    [SerializeField] GameObject _readyRed;


    [SerializeField] GameObject _spellsTitleObject;
    [SerializeField] GameObject _elementTitleObject;


    [SerializeField] GameObject _upLetterBox;
    [SerializeField] GameObject _bottomLetterBox;




    public bool CursorOccupied;
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
        HideReady(true);
        HideReady(false);
        
    }

    /// <summary>
    /// Show Ready Button when Player is ready
    /// </summary>
    /// <param name="isBlue">Blue player or Red player</param>
    public void ShowReady(bool isBlue)
    {
        if (isBlue)
        {
            _readyBlue.SetActive(true);
        }
        else
        {
            _readyRed.SetActive(true);
        }
    }


    /// <summary>
    /// Hide Ready Button when Player is not ready
    /// </summary>
    /// <param name="isBlue">Blue player or Red player</param>
    public void HideReady(bool isBlue)
    {
        if (isBlue)
        {
            _readyBlue.SetActive(false);
        }
        else
        {
            _readyRed.SetActive(false);
        }
    }


    public async Task ShowSpellsTitle()
    {
        await ShowTitle(_spellsTitleObject);
    }

    public async Task ShowElementsTitle()
    {
        await ShowTitle(_elementTitleObject);
    }

    public void ShowLetterBox()
    {
        TransitionManager.ChangeLocalPosition(_upLetterBox, new Vector3(0, 0, 0), 0.2f);
        TransitionManager.ChangeLocalPosition(_bottomLetterBox, new Vector3(0, -25, 0), 0.2f);
    }

    public void HideLetterBox()
    {
        TransitionManager.ChangeLocalPosition(_upLetterBox, new Vector3(0, 0, 0), 0.2f);
        TransitionManager.ChangeLocalPosition(_bottomLetterBox, new Vector3(0, -25, 0), 0.2f);
    }

    private async Task ShowTitle(GameObject titleObject)
    {
        titleObject.SetActive(true);
        titleObject.GetComponentInChildren<ParticleSystem>().Play();
        var title = titleObject.GetComponentInChildren<TMP_Text>().gameObject;
        TransitionManager.ChangeSize(title, 3.5f, 1f);
        await Task.Delay(2000);
        TransitionManager.ChangeSize(title, 0.01f, 1f);
        await Task.Delay(1000);
        titleObject.SetActive(false);
    }



}
