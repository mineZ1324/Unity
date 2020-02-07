using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SingleShip : MonoBehaviour
{
    public bool IsPositionCorrect;
    public bool IsWidthCell;
    public Vector2 Cellcenter;
    bool vosallocate = false;
    Animator[] animators;
    int floorsnum;
    Canvas canvas;
    bool toMove = false;
    dispetcher Dispetcher;
    public GameObject ButtonPref;
    Bounds[] BoundsOfFloors;

    public enum Oreintation
    {
        vertical, horizontal
    }
    public Oreintation oreintation = Oreintation.vertical;

    public int Floorsnum()
    {
        return floorsnum;
    }
    // Use this for initialization
    void Start()
    {
        Dispetcher = GetComponentInChildren<dispetcher>();
        canvas = GetComponentInParent<Canvas>();
        floorsnum = transform.childCount;
        BoundsOfFloors = new Bounds[floorsnum];
        animators = new Animator[floorsnum];
        for (int i = 0; i < floorsnum; i++)
        {
            var floor = transform.GetChild(i);
            var Sr = floor.GetComponent<SpriteRenderer>();
            var floorSize = Sr.bounds.size.y;
            var vector = transform.position;

            if (oreintation == Oreintation.vertical)
            {
                vector.y -= floorSize * i;

            }
            else if (oreintation == Oreintation.horizontal)
            {
                vector.x += floorSize * i;
            }
            floor.transform.position = vector;
            BoundsOfFloors[i] = Sr.bounds;

            var animator = floor.GetComponent<Animator>();
            animators[i] = animator;

            var ButtonObj = Instantiate(ButtonPref, floor.transform);
            ButtonObj.transform.position = floor.position;
            var Rt = ButtonObj.GetComponent<RectTransform>();
            Rt.sizeDelta = new Vector2(floorSize, floorSize);
            ButtonObj.GetComponent<Button>().onClick.AddListener(OnFloorClick);
        }
    }

    void OnFloorClick()
    {
        if (!Input.GetMouseButtonUp(0))
        {
            return;
        }
        Dispetcher.OnShipClick();
        if (!vosallocate)
        {
            vosallocate = true;
        }

    }
    public bool VosAllocate()
    {
        return (vosallocate);
    }

    void Roteid()
    {
        var engle = -90f;
        if (oreintation == Oreintation.vertical)
        {
            oreintation = Oreintation.horizontal;
            engle = -engle;
        }
        else
        {
            oreintation = Oreintation.vertical;
        }
        transform.Rotate(new Vector3(0,0,engle),Space.Self);
    }
    void SwitchErrorAnimation()
    {
        foreach (var animation in animators)
        {
            animation.SetBool("IsMissPlays", !IsPositionCorrect);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        toMove = Equals(dispetcher.CurrentShip);
        if (!toMove) return;
        var canvasRect = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, Camera.main, out Vector2 position);
        position = canvas.transform.TransformPoint(position);
        transform.position = position;


        GameField.ChekShipLocation(position, this);
        if(IsWidthCell)
        {
            transform.position = Cellcenter;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Roteid();
        }

        
        SwitchErrorAnimation();
        //Debug.Log(transform.position);


        //var mousePos = Input.mousePosition;
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //transform.position = mousePos;

        //Debug.Log(mousePos);
        //Debug.Log(transform.position);

    }
}
