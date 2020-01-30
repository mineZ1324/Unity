using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ship : MonoBehaviour
{
    public enum Orientation
    {
        Horizontal, Vertical
    }
    public bool IsPositionCorrect;
    public Orientation orientation = Orientation.Horizontal;
    public GameObject floorButtonPref;
    Canvas canvas;
    Dispatcher dispatcher;
    bool toMove = false;
    int floorsNum;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        floorsNum = transform.childCount;
        float floorSize = 0;
        for (int i = 0; i < floorsNum; i++)
        {
            var floor = transform.GetChild(i);
            var floorPos = transform.position;
            floorSize = floor.GetComponent<SpriteRenderer>().bounds.size.x;
            if (orientation == Orientation.Horizontal) floorPos.x += i * floorSize;
            else if (orientation == Orientation.Vertical) floorPos.y += i * floorSize;
            floor.transform.position = floorPos; // Поле игры

            var floorButtonObj = Instantiate(floorButtonPref, floor.transform); //Префабы кнопок
            floorButtonObj.transform.position = floorPos; // Кнопки для кораблей
            var buttonRectTransf = floorButtonObj.GetComponent<RectTransform>(); // Задание кнопок по флуру
            buttonRectTransf.sizeDelta = new Vector2(floorSize, floorSize); // Размер кнопок
            var buttonScript = floorButtonObj.GetComponent<Button>(); // Задавание кликабельности
            buttonScript.onClick.AddListener(OnFloorClick); // Кликабельность и привязываемость.
            dispatcher = GetComponentInChildren<Dispatcher>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        toMove = Equals(Dispatcher.currentShip);
        if (!toMove) return;
        var mousePos = Input.mousePosition; // позиция мыши
        var CanvasRec = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasRec, mousePos, Camera.main, out Vector2 Daun); // Конвертация координат по канвасу с учётом камеры
        Daun = canvas.transform.TransformPoint(Daun); // Выше написанное
        transform.position = Daun;// позиция мыши
        GameField.CheckShipPosition(Daun, this);// Чекнуть корабельную позицию через Daun и это
    }

    void OnFloorClick()
    {
        dispatcher.OnShipClick();
    }

}
