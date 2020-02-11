using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameField : MonoBehaviour
{
    public enum CellState
    {
        Empty, Misdelivered, Occupied, Misplaced, Hit
    }


    public GameObject cellPrefab;
    public float bottomLeftX, bottomLeftY;
    static Bounds[,] BoundsOfCells;
    static int[,] fieldBody = new int[10, 8];
    static float cellSize;
    static Vector2 BottomLeftCorner;

    // Start is called before the first frame update
    void Start()
    {
        var sprRenderer = cellPrefab.GetComponent<SpriteRenderer>();
        cellSize = sprRenderer.bounds.size.x;
        BoundsOfCells = new Bounds[Width(), Heigth()];
        GenerateField();
    }

    void GenerateField()
    {
        for (int i = 0; i < Width(); i++)
        {
            for (int j = 0; j < fieldBody.GetLength(1); j++)
            {
                var cellPos = new Vector2(bottomLeftX + i * cellSize, bottomLeftY + j * cellSize);
                Instantiate(cellPrefab, cellPos, Quaternion.identity);
                var CellBounds = new Bounds(cellPos, new Vector2(cellSize, cellSize));
                BoundsOfCells[i, j] = CellBounds;
            }
        }
    }

    static int Width()
    {
        return fieldBody.GetLength(0);
    }

    static int Heigth()
    {
        return fieldBody.GetLength(1);
    }
    static Vector2 GetCellNormalPos(Vector2 Position)
    {
        var dx = Position.x - BottomLeftCorner.x;
        var dy = Position.y - BottomLeftCorner.y;
        int x = (int)(dx / cellSize);
        int y = (int)(dy / cellSize);
        return new Vector2(x, y);
    }
    static void CellStateUnderneathShip(Ship ship, CellState cellState)
    {
        Vector2 CellNormalPos = GetCellNormalPos(ship.CellCenterPos);
        int x = (int)CellNormalPos.x;
        int y = (int)CellNormalPos.y;
        for (int i = 0; i < ship.FloorsNum(); i++)
        {
            fieldBody[x, y] = (int)cellState;
            if (ship.orientation == Ship.Orientation.Horizontal)
            {
                x++;
            }
            else if (ship.orientation == Ship.Orientation.Vertical)
            {
                y--;
            }
        }
        for (int i = 0; i < Width(); i++)
        {
            string str = " ";
            for (int j = 0; j < Heigth(); j++)
            {
                str += fieldBody[i, j] + " ";
            }
            Debug.Log(str);
        }
    }
    public static void RegisterShip(Ship ship)
    {
        CellStateUnderneathShip(ship, CellState.Occupied);
    }

    public static void CheckShipPosition(Vector3 mousePos, Ship ship)
    {
        var CellNormalPos = GetCellNormalPos(mousePos);
        var BottomLeftCells = BoundsOfCells[0, 0];
        var UpperRightCells = BoundsOfCells[Width() - 1, Heigth() - 1];
        BottomLeftCorner = BottomLeftCells.min;
        var UpperRightCorner = UpperRightCells.max;
        bool IsOverField = mousePos.x > BottomLeftCorner.x && mousePos.y > BottomLeftCorner.y && mousePos.x < UpperRightCorner.x && mousePos.y < UpperRightCorner.y;

        if (!IsOverField)//Кораблик за пределами поля
        {
            ship.IsPositionCorrect = false;
            ship.IsWithIn = false;
            return;
        }
        int sx = (int)CellNormalPos.x;
        int sy = (int)CellNormalPos.y;
        //Debug.Log(x+" , "+y);
        ship.IsPositionCorrect = IsLocationAppropriate(ship, sx, sy);
        ship.IsWithIn = true;
        ship.CellCenterPos = BoundsOfCells[sx, sy].center;
        //Debug.Log(y);

    }
    static bool IsLocationAppropriate(Ship ship, int x, int y)
    {
        for (int i = 0; i < ship.FloorsNum(); i++)
        {
            if (!IsPointWithinMatrics(x, y))
            {
                return false;
            }
            if (ship.orientation == Ship.Orientation.Horizontal)
            {
                x++;
            }
            else //if (ship.orientation == Ship.Orientation.Vertical)
            {
                y--;
            }
        }
        return true;
    }
    static bool IsPointWithinMatrics(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width() && y < Heigth();
    }


    public static Bounds Rostislav()
    {
        var Vert = Ship.Orientation.Vertical;
        var Horiz = Ship.Orientation.Horizontal;
        var SHIP = Ship.Orientation.Vertical;
        var size = new Vector2(3, 5);
        var SHip = new Vector2(1,4);
        Bounds rectangle = new Bounds(new Vector2 (5,5), size);
        if (SHip.y<size.x&&SHip.y>size.y)
        {
            SHIP = Ship.Orientation.Horizontal;
        }
        else if (SHip.y > size.x && SHip.y < size.y)
        {
            SHIP = Ship.Orientation.Vertical;
        }
        else if (SHip.y < size.x && SHip.y < size.y)
        {
            Random.Range(Vert,Horiz);
            
        }
        return rectangle;

    }
}
