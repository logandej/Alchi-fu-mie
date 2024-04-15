using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListObjects
{
    public static void Show(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(true); // Activer ou désactiver l'objet selon le paramètre 'show'
        }
    }

    public static void Hide(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            obj.SetActive(false); // Activer ou désactiver l'objet selon le paramètre 'show'
        }
    }
}
