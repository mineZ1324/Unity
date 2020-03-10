using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dispatcher : MonoBehaviour
{
    public GameObject shipPrefab;
    public static Ship currentShip;

    string dictKey;
    bool isWorkingInstance = true;

    static Dictionary<string, int> shipsLeftToAllocate = new Dictionary<string, int>();
    static Dictionary<string, Text> lablesDict = new Dictionary<string, Text>();
    static List<Dispatcher> allShips = new List<Dispatcher>();
    static bool isAutoCreation = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isWorkingInstance = name.Contains("(Clone)");
        dictKey = name.Replace("(Clone)", null);
        if (!isAutoCreation) allShips.Add(this);

        var floorsNumStr = dictKey.Replace("Ship-", null);
        var shipsToAllocate = 5 - int.Parse(floorsNumStr);
        if (!shipsLeftToAllocate.ContainsKey(dictKey))
        {
            shipsLeftToAllocate.Add(dictKey, shipsToAllocate);
            FillLabelsDict();
        }
        RefreshLabel();
    }

    protected void OnShipClick()
    {   
        if (isWorkingInstance)
        {
            if (currentShip == null)
            {
                currentShip = GetComponentInChildren<Ship>();
            }
            else if (currentShip.IsPositionCorrect)
            {
                if (!currentShip.WAsLocatedOnse())
                {
                    shipsLeftToAllocate[dictKey]--;
                    RefreshLabel();
                }
                currentShip = null;
            }
        }
        else if (currentShip == null&&shipsLeftToAllocate[dictKey]>0) // Обычный шаблон
        {
            var shipObjToPlay = Instantiate(shipPrefab, transform.parent.transform);
            currentShip = shipObjToPlay.GetComponentInChildren<Ship>();
        }
    }
    void FillLabelsDict()
    {
        var LabelObj = GameObject.Find(dictKey+"(Label)");
        var Label = LabelObj.GetComponent<Text>();
        lablesDict.Add(dictKey, Label);
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
    void RefreshLabel()
    {
        lablesDict[dictKey].text = shipsLeftToAllocate[dictKey]+"x";
    }

    static Dispatcher[] GetAllShipsOfType(bool templateOnes)
    {
        var result = new List<Dispatcher>();
        foreach (var ship in allShips)
            if (ship.isWorkingInstance ^ templateOnes) result.Add(ship);
        return result.ToArray();
    }

    public static Dispatcher[] CreateAllShips()
    {
        isAutoCreation = true;
        var templateShips = GetAllShipsOfType(true);
        foreach (var tmplShip in templateShips) tmplShip.CreateAllClonesOfType();
        return GetAllShipsOfType(false);
    }

    void CreateAllClonesOfType()
    {
        for (int i = 0; i < shipsLeftToAllocate[dictKey]; i++)
        {
            var ship = Instantiate(shipPrefab, transform.parent.transform);
            allShips.Add(ship.GetComponent<Dispatcher>());
        }
        shipsLeftToAllocate[dictKey] = 0;
    }
}
