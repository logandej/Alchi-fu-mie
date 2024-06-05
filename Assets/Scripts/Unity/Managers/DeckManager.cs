using AFM_DLL.Models.PlayerInfo;
using UnityEngine;
using Newtonsoft.Json;
using AFM_DLL.Converters;

public class DeckManager : MonoBehaviour
{
    public Deck PlayerDeck { get; private set; }

    public static DeckManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadPlayerPrefs();
        DontDestroyOnLoad(this);
    }

    private void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("DeckJSON"))
        {
            var json = PlayerPrefs.GetString("DeckJSON");
            PlayerDeck = JsonConvert.DeserializeObject<Deck>(json, new SpellCardConverter());
        }
        else
        {
            PlayerDeck = new Deck();
        }
    }

    public void SaveDeck()
    {
        var json = JsonConvert.SerializeObject(PlayerDeck);

        PlayerPrefs.SetString("DeckJSON", json);
    }
}