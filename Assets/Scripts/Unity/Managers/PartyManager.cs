using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    public BoardController BoardController;


    public ElementCardDisplay ElementCardPrefab;
    public SpellCardDisplay SpellCardPrefab;


    [SerializeField] GameObject _readyBlue;
    [SerializeField] GameObject _readyRed;


    [SerializeField] GameObject _spellsTitleObject;
    [SerializeField] ParticleSystem _spellsTitleParticle;
    [SerializeField] GameObject _elementTitleObject;
    [SerializeField] ParticleSystem _elementsTitleParticle;



    [SerializeField] Image _upLetterBox;
    [SerializeField] Image _bottomLetterBox;

    public bool CursorOccupied;
    public float translateSpellX;

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
        await ShowTitle(_spellsTitleObject, _spellsTitleParticle);
    }

    public async Task ShowElementsTitle()
    {
        await ShowTitle(_elementTitleObject, _elementsTitleParticle);
    }

    public void ShowLetterBox()
    {
        print(_upLetterBox.gameObject);
        print(_upLetterBox.transform.transform.localPosition);
        TransitionManager.ChangeLocalPosition(_upLetterBox.gameObject, new Vector3(0, _upLetterBox.transform.localPosition.y - 25, 0), 0.2f);
        TransitionManager.ChangeLocalPosition(_bottomLetterBox.gameObject, new Vector3(0, _bottomLetterBox.transform.localPosition.y + 25, 0), 0.2f);
    }

    public void HideLetterBox()
    {
        TransitionManager.ChangeLocalPosition(_upLetterBox.gameObject, new Vector3(0, _upLetterBox.transform.localPosition.y + 25, 0), 0.2f);
        TransitionManager.ChangeLocalPosition(_bottomLetterBox.gameObject, new Vector3(0, _bottomLetterBox.transform.localPosition.y - 25, 0), 0.2f);
    }

    private async Task ShowTitle(GameObject titleObject, ParticleSystem particle)
    {
        titleObject.SetActive(true);

        particle.Play();
        var title = titleObject.GetComponentInChildren<TMP_Text>().gameObject;
        TransitionManager.ChangeSize(title, 3.5f, 1f);
        await Task.Delay(2000);
        TransitionManager.ChangeSize(title, 0.01f, 1f);
        await Task.Delay(1000);
        titleObject.SetActive(false);
    }
}
