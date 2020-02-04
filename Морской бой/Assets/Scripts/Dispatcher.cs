﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dispatcher : MonoBehaviour
{
    static Dictionary<string, int> shipsLeftToAllocate = new Dictionary<string, int>();
    static Dictionary<string, Text> LablesDict = new Dictionary<string, Text>();
    string DictKey;
    public GameObject shipPrefab;
    public static Ship currentShip;

    // Start is called before the first frame update
    void Start()
    {
        var floorsNumStr = gameObject.name.Replace("Ship-", null);
        var shipsToAllocate = 5 - int.Parse(floorsNumStr);
        if (!shipsLeftToAllocate.ContainsKey(gameObject.name))
        {
            shipsLeftToAllocate.Add(gameObject.name, shipsToAllocate);
        }
        FillLabelsDict();
    }

    public void OnShipClick()
    {   
        if (gameObject.name.Contains("Clone"))
        {
            
            if (currentShip == null)
            {
                currentShip = GetComponentInChildren<Ship>();
            }
            else if (currentShip.IsPositionCorrect)
            {
                currentShip = null;
            }
        }
        else if (currentShip == null) // Обычный шаблон
        {
            var shipObjToPlay = Instantiate(shipPrefab, transform.parent.transform);
            currentShip = shipObjToPlay.GetComponentInChildren<Ship>();
        }
    }
    void FillLabelsDict()
    {

        /*var Labels = transform.parent.GetComponentsInChildren<Text>();
        foreach (var Label in Labels)
        {
            if (!Label.name.Contains("Label"))
            {
                continue;
            }
            Debug.Log(Label);
        }*/
        
    }
}
