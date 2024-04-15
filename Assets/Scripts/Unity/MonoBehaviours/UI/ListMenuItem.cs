using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMenuItem : MonoBehaviour
{
    [SerializeField] List<MenuItem> menuItems = new List<MenuItem>(); // Liste des �l�ments du menu
    [SerializeField] bool defaultSelect = true; // Indique si un �l�ment doit �tre s�lectionn� par d�faut
    [SerializeField] int selectedIndex = 0; // Index de l'�l�ment actuellement s�lectionn�
    private void Start()
    {
        // Parcourir tous les �l�ments du menu
        for (int i = 0; i < menuItems.Count; i++)
        {
            int index = i; // Capturer la valeur actuelle de i pour �viter les probl�mes de r�f�rence
            // Ajouter un �couteur d'�v�nement pour l'�v�nement de s�lection de l'�l�ment
            menuItems[i].eventSelected.AddListener(() => SelectOne(index));
        }
        // S�lectionner un �l�ment par d�faut si n�cessaire
        if (defaultSelect)
            menuItems[selectedIndex].Select();
    }

    // M�thode pour s�lectionner un �l�ment sp�cifique dans le menu
    public void SelectOne(int index)
    {
        // D�s�lectionner l'�l�ment actuellement s�lectionn�
        menuItems[selectedIndex].Deselect();
        // Mettre � jour l'index de l'�l�ment s�lectionn�
        selectedIndex = index;
    }
}
