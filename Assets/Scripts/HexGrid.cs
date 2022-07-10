using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    private const float DEBUG_DURATION = 100f;

    #region Grid Fields
    
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 originPosition;
    private Hex[,] gridArray;

    #endregion

    #region Grid Properties

    public int Width
    {
        get => width;
        private set => width = value;
    }
    public int Height
    {
        get => height;
        private set => height = value;
    }
    public Vector3 OriginPosition 
    { 
        get => originPosition; 
        private set => originPosition = value; 
    }

    // Cell Sizing
    public float CellSizeUnit
    {
        get => cellSize;
        private set => cellSize = value;
    }
    public float CellWidthInUnit
    {
        get => 2 * CellSizeUnit;
    }
    public float CellHeightInUnit
    {
        get => Mathf.Sqrt(3) * CellSizeUnit;
    }

    #endregion
    

    public void Start()
    {
        InitGrid();
    }

    private void InitGrid()
    {
        gridArray = new Hex[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                gridArray[i, j] = new Hex(i, j, this); // Instantiate yapmak daha uygun gibi
            }
        }

        ShowDebug();
    }

    #region Getters

    public Hex GetGridObject(int x, int y) 
    { 
        return (x < 0 || y < 0 || x >= Width || y >= Height) ? null : gridArray[x, y]; 
    }

    public Vector3 GetWorldPosition(int x, int y) 
    {
        Hex hex = GetGridObject(x, y);

        Vector3 worldPosition = Vector3.zero;
        worldPosition.x = CellSizeUnit * (3f / 2 * hex.X);
        worldPosition.y = CellSizeUnit * (Mathf.Sqrt(3f) / 2 * hex.X + Mathf.Sqrt(3f) * hex.Y) - (CellHeightInUnit * (x / 2));

        return worldPosition + OriginPosition; 
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = (int)((2f / 3 * worldPosition.x) / CellSizeUnit);
        y = (int)((-1f / 3 * worldPosition.x + Mathf.Sqrt(3) / 3 * worldPosition.y  ) / CellSizeUnit);
    }

    #endregion

    #region Debug

    public void ShowDebug()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                DrawAHex(i, j);
            }
        }
    }

    private void DrawAHex(int i, int j)
    {
        // Find corners
        Vector3 hexWorldPosition0 = GetWorldPosition(i, j);
        Vector3 hexWorldPosition1 = hexWorldPosition0 + new Vector3(CellWidthInUnit / 2, 0f);
        Vector3 hexWorldPosition2 = hexWorldPosition1 + new Vector3(CellWidthInUnit / 4, CellHeightInUnit / 2);
        Vector3 hexWorldPosition3 = hexWorldPosition2 + new Vector3(-CellWidthInUnit / 4, CellHeightInUnit / 2);
        Vector3 hexWorldPosition4 = hexWorldPosition3 + new Vector3(-CellWidthInUnit / 2, 0f);
        Vector3 hexWorldPosition5 = hexWorldPosition4 + new Vector3(-CellWidthInUnit / 4, -CellHeightInUnit / 2);

        // Draw lines between neghbour corners
        Debug.DrawLine(hexWorldPosition0, hexWorldPosition1, Color.white, DEBUG_DURATION);
        Debug.DrawLine(hexWorldPosition1, hexWorldPosition2, Color.white, DEBUG_DURATION);
        Debug.DrawLine(hexWorldPosition2, hexWorldPosition3, Color.white, DEBUG_DURATION);
        Debug.DrawLine(hexWorldPosition3, hexWorldPosition4, Color.white, DEBUG_DURATION);
        Debug.DrawLine(hexWorldPosition4, hexWorldPosition5, Color.white, DEBUG_DURATION);
        Debug.DrawLine(hexWorldPosition5, hexWorldPosition0, Color.white, DEBUG_DURATION);
    }

    #endregion
}
