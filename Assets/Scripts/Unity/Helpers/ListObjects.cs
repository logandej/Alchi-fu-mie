using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListObjects
{
    public static void Show(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(true); // Activer ou d�sactiver l'objet selon le param�tre 'show'
        }
    }

    public static void Hide(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(false); // Activer ou d�sactiver l'objet selon le param�tre 'show'
        }
    }
}
