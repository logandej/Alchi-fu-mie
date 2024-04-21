using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuItem : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] List<MenuItem> menuItems = new List<MenuItem>(); // Liste des éléments du menu

    [Header("Views")]
    [SerializeField] List<GameObject> viewList = new List<GameObject>();

    [Header("Music")]
    [SerializeField] bool changeMusic = true;
    [SerializeField] List<AudioClip> musicList = new List<AudioClip>();
 
    [Header("Settings")]
    [SerializeField] bool defaultSelect = true; // Indique si un élément doit être sélectionné par défaut
    [SerializeField] int selectedIndex = 0; // Index de l'élément actuellement sélectionné

  
    
    private void Start()
    {
        
        selectedIndex = Mathf.Clamp(selectedIndex, 0, menuItems.Count - 1);
        // Parcourir tous les éléments du menu
        for (int i = 0; i < menuItems.Count; i++)
        {
            int index = i; // Capturer la valeur actuelle de i pour éviter les problèmes de référence
            // Ajouter un écouteur d'événement pour l'événement de sélection de l'élément
            menuItems[i].eventSelected.AddListener(() => SelectOne(index));
        }
        // Sélectionner un élément par défaut si nécessaire
        if (defaultSelect)
        {

            menuItems[selectedIndex].Select();
        }
        UpdateView();
    }

    // Méthode pour sélectionner un élément spécifique dans le menu
    public void SelectOne(int index)
    {
        
        // Désélectionner l'élément actuellement sélectionné
        menuItems[selectedIndex].Deselect();
        // Mettre à jour l'index de l'élément sélectionné
        selectedIndex = index;
        UpdateView();
    }

    public void UpdateView()
    {
        foreach(GameObject view in viewList)
        {
            view.SetActive(false);
            
        }
        if(changeMusic)
            AudioManager.Instance.PlayMusic(musicList[selectedIndex]);
        viewList[selectedIndex].SetActive(true);
    }
}
