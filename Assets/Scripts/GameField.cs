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

    public static void CheckShipPosition(Vector3 mousePos, Ship ship)
    {
        var BottomLeftCells = BoundsOfCells[0, 0];
        var UpperRightCells = BoundsOfCells[Width() - 1, Heigth() - 1];
        var BottomLeftCorner = BottomLeftCells.min;
        var UpperRightCorner = UpperRightCells.max;
        bool IsOverField = mousePos.x>BottomLeftCorner.x && mousePos.y>BottomLeftCorner.y && mousePos.x<UpperRightCorner.x && mousePos.y<UpperRightCorner.y;

        if (!IsOverField)
        {
            ship.IsPositionCorrect = false;
            return;
        }
        var dx = mousePos.x-BottomLeftCorner.x;
        var dy = mousePos.y - BottomLeftCorner.y;
        
        int x = (int)(dx/cellSize);
        int y = (int)(dy / cellSize);
        Debug.Log(x+" , "+y);
        //Debug.Log(y);

    }

}
