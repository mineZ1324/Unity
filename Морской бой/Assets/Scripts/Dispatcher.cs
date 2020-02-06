using System.Collections;
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
    bool IsWorkingInstance=true;

    // Start is called before the first frame update
    void Start()
    {
        IsWorkingInstance = name.Contains("Clone");
        DictKey = name.Replace("(Clone)", null);
        //Debug.Log(DictKey);
        var floorsNumStr = DictKey.Replace("Ship-", null);
        var shipsToAllocate = 5 - int.Parse(floorsNumStr);
        if (!shipsLeftToAllocate.ContainsKey(gameObject.name))
        {
            shipsLeftToAllocate.Add(DictKey, shipsToAllocate);
        }
        FillLabelsDict();
    }

    public void OnShipClick()
    {   
        if (IsWorkingInstance)
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
        var Label = GameObject.Find(DictKey+"(Label)");
        Debug.Log(Label);
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
