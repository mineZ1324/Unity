using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dispetcher : MonoBehaviour
{
    static Dictionary<string, int> ShipsLeftToLocate = new Dictionary<string, int>();
    static Dictionary<string, Text> LabelsDict = new Dictionary<string, Text>();
    public GameObject shipprefab;
    public static SingleShip CurrentShip;
    string DictKey;
    public void OnShipClick()
    {
        
        if (gameObject.name.Contains("Clone"))
        {
            CurrentShip = null;
            if (CurrentShip==null)
            {
                CurrentShip = GetComponentInChildren<SingleShip>();
            }
            else if(CurrentShip.IsPositionCorrect)
            {
                CurrentShip = null;
            }        
        }
        else if (CurrentShip == null)
        {
            var shipToPlay = Instantiate(shipprefab, transform.parent.transform);
            var cloneContoller = shipToPlay.GetComponent<SingleShip>();
            CurrentShip = cloneContoller;
        }
    }
    void FillLabelsDict()
    {
        if(LabelsDict.Count>0)
        {
            return;
        }
        var Labels = transform.parent.GetComponentsInChildren<Text>();
        foreach (var Label in Labels)
        {
            
            if (!Label.name.Contains("Label"))
            {
                continue;
            }
            LabelsDict.Add(Label.name.Replace("-Label",null), Label);
        }
    }
    // Use this for initialization
    void Start()
    {
        DictKey = gameObject.name.Replace("(Clone)", null);
        FillLabelsDict();
        
        var ships = DictKey.Replace("Ship", null);
      
        var shipsnumtolocate = 5 - int.Parse(ships);
        if (!ShipsLeftToLocate.ContainsKey(gameObject.name))
        {
            ShipsLeftToLocate.Add(DictKey, shipsnumtolocate);
            RefreshLabels();
        }
        
    }
    void RefreshLabels()
    {
        LabelsDict[DictKey].text = ShipsLeftToLocate[DictKey] + "x";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
