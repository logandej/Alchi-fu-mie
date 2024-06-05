using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataBar : MonoBehaviour
{
    [SerializeField] List<GameObject> _listObjects;
    [SerializeField] TMP_Text text;  
    [SerializeField] int _maxCount = 10;
    private int _currentCount;

    /// <summary>
    /// Set counter of the DataBar and update it
    /// </summary>
    /// <param name="count">New Count</param>
    public void SetCounterTo(int count)
    {
        count = count <= 0 ? 0 : count;
        if (count <= _maxCount && count>=0)
        {
            _currentCount = count;
            UpdateDataBar();
        }
        else
        {
            Debug.LogError("Compteur depassé : " + count + "/" + _maxCount);
        }
        
    }

    public void RemoveCounter(uint count)
    {
        int diff = _currentCount - (int)count;
        int decrease = diff <= 0 ? 0 : diff;
        SetCounterTo(decrease);
    }

    public void AddCounter(uint count)
    {
        SetCounterTo(_currentCount + (int)count);
    }

    private void UpdateDataBar()
    {
        text.text = _currentCount.ToString();
        for(int i=0; i < _maxCount; i++)
        {
            if (i < _currentCount)
            {
                _listObjects[i].SetActive(true);
            }
            else
            {
                _listObjects[i].SetActive(false);

            }
        }
    }

}
