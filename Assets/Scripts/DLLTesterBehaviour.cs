using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AFM_DLL;
using System;
using AFM_DLL.Helpers;

public class DLLTesterBehaviour : MonoBehaviour
{
    private void Start()
    {
        var res = FightHelper.ElementFight(Element.ROCK, Element.SCISSORS);
        Debug.Log(res);
    }
}
