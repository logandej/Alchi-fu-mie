using AFM_DLL;
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

    [SerializeField] GameObject _titleText;
    [SerializeField] List<ParticleSystem> _particlesTitle;


    [SerializeField] GameObject _quitButton;

    [SerializeField] Image _upLetterBox;
    [SerializeField] Image _bottomLetterBox;


    [SerializeField] List<HeroScriptable> heroScriptables;

    public Transform defausse;

    public Dictionary<Element , HeroScriptable> HerosByElement;

    [SerializeField] AudioClip musicClip;

    [SerializeField] GameObject spellDescription;

    public bool CursorOccupied;
    public float translateSpellX;

    [SerializeField] AudioClip win;
    [SerializeField] AudioClip loose;
    [SerializeField] AudioClip tie;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject); }

        HerosByElement = new();
        HerosByElement.Add(Element.ROCK, heroScriptables[0]);
        HerosByElement.Add(Element.PAPER, heroScriptables[1]);
        HerosByElement.Add(Element.SCISSORS, heroScriptables[2]);
    }


    private void Start()
    {
        HideReady();
        _quitButton.SetActive(false);
        AudioManager.Instance.PlayMusic(musicClip);
    }

    /// <summary>
    /// Show Ready Button when Player is ready
    /// </summary>
    public void ShowReady()
    {
            _readyBlue.SetActive(true);
       
    }


    /// <summary>
    /// Hide Ready Button when Player is not ready
    /// </summary>
    public void HideReady()
    {
            _readyBlue.SetActive(false);
    }

    public async Task ShowHeroTitle()
    {
        await ShowTitle("Hero", _particlesTitle[1], true);
    }

    public async Task ShowSpellsTitle()
    {
        await ShowTitle("Spells", _particlesTitle[0], true);
    }

    public async Task ShowElementsTitle()
    {
        await ShowTitle("Elements", _particlesTitle[1], true);
    }

    public async Task ShowVictory()
    {
        await ShowTitle("Victory", _particlesTitle[2], false);
        AudioManager.Instance.PlayMusic(win);
        _quitButton.SetActive(true);
    }

    public async Task ShowDefeat()
    {
        await ShowTitle("Defeat", _particlesTitle[3], false);
        AudioManager.Instance.PlayMusic(loose);
        _quitButton.SetActive(true);
    }

    public async Task ShowTie()
    {
        await ShowTitle("Tie", _particlesTitle[4], false);
        AudioManager.Instance.PlayMusic(tie);
        _quitButton.SetActive(true);
    }

    public void ShowLetterBox()
    {
        TransitionManager.ChangeLocalPosition(_upLetterBox.gameObject, new Vector3(0, _upLetterBox.transform.localPosition.y - 25, 0), 0.2f);
        TransitionManager.ChangeLocalPosition(_bottomLetterBox.gameObject, new Vector3(0, _bottomLetterBox.transform.localPosition.y + 25, 0), 0.2f);
    }

    public void HideLetterBox()
    {
        TransitionManager.ChangeLocalPosition(_upLetterBox.gameObject, new Vector3(0, _upLetterBox.transform.localPosition.y + 25, 0), 0.2f);
        TransitionManager.ChangeLocalPosition(_bottomLetterBox.gameObject, new Vector3(0, _bottomLetterBox.transform.localPosition.y - 25, 0), 0.2f);
    }

    private async Task ShowTitle(string newtext, ParticleSystem particle,bool end)
    {
        _titleText.SetActive(true);
        particle.gameObject.SetActive(true);
        particle.Play();
        var title = _titleText.GetComponentInChildren<TMP_Text>();
        title.text = newtext;
        TransitionManager.ChangeSize(title.gameObject, Vector3.one*3.5f, 1f);
        await Task.Delay(2000);
        if (end)
        {
            TransitionManager.ChangeSize(title.gameObject, Vector3.one * 0.01f, 1f);
            await Task.Delay(1000);
            _titleText.SetActive(false);
            particle.gameObject.SetActive(false);

        }
    }

    public void ShowSpellDescription(string text)
    {
        spellDescription.SetActive(true);
        spellDescription.GetComponentInChildren<TMP_Text>().text = text;
    }

    public void HideSpellDescription()
    {
        spellDescription.SetActive(false);
    }
}
