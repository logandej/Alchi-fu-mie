using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerDeck : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        Check();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check()
    {
        bool isValide = DeckManager.Instance.PlayerDeck.IsDeckValid;
        button.SetActive(isValide);
        text.SetActive(!isValide);
        
    }
}
