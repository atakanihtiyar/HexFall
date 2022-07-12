using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gokyolcu.Utilities;

public class HexGrid : MonoBehaviour
{
    private const float DEBUG_DURATION = 100f;

    #region Grid Fields
    
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 originPosition;

    [SerializeField] private ColorPicker colorPicker;
    [SerializeField] private GameObject cellPrefab;
    private Hex[,] gridArray;
    [SerializeField] private Transform parentOfHexes;

    [SerializeField] private bool showCellDebugTexts;
    [SerializeField] private bool showCellDebugLines;
    private TextMesh[,] debugTextArray;

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
        debugTextArray = new TextMesh[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GameObject newHexObject = Instantiate(cellPrefab, parentOfHexes);
                Hex newHex = newHexObject.GetComponent<Hex>();

                newHex.InitHex(i, j, this, colorPicker.GetRandomColor());
                gridArray[i, j] = newHex;

                ShowDebug(i, j);
            }
        }
    }

    #region Getters

    public Hex GetGridObject(int x, int y) 
    { 
        return (x < 0 || y < 0 || x >= Width || y >= Height) ? null : gridArray[x, y];
    }
    
    public Hex GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y) 
    {
        Vector3 worldPosition = Vector3.zero;
        worldPosition.x = CellSizeUnit * (3f / 2 * x);
        worldPosition.y = CellSizeUnit * (Mathf.Sqrt(3f) / 2 * x + Mathf.Sqrt(3f) * y) - (CellHeightInUnit * (x / 2));

        return worldPosition + OriginPosition; 
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 offsetedWorldPosition = worldPosition - originPosition - new Vector3(CellWidthInUnit * .25f, CellHeightInUnit * .5f);

        x = Mathf.RoundToInt((offsetedWorldPosition.x / CellSizeUnit) * 2f / 3);
        y = Mathf.RoundToInt((((offsetedWorldPosition.y + CellHeightInUnit * (x / 2)) / CellSizeUnit) - Mathf.Sqrt(3f) / 2 * x) / Mathf.Sqrt(3f));
    }

    #endregion

    #region Debug

    public void ShowDebug(int i, int j)
    {
        if (showCellDebugTexts)
            WriteDebugText(i, j, gridArray[i, j].transform, gridArray[i, j]?.ToString());

        if (showCellDebugLines)
            DrawAHex(gridArray[i, j]);
    }

    private void DrawAHex(Hex hex)
    {
        for (int i = 0; i < hex.Corners.Count; i++)
        {
            int from = i;
            int to = i == hex.Corners.Count - 1 ? 0 : i + 1;
            Debug.DrawLine(hex.Corners[from].WorldPosition, hex.Corners[to].WorldPosition, Color.white, DEBUG_DURATION);
        }
    }

    private void WriteDebugText(int i, int j, Transform parent, string message = "")
    {
        debugTextArray[i, j] = CreateWorldText(
            message,
            parent,
            gridArray[i, j].GetCenterOffset(),
            14,
            Color.black,
            TextAnchor.MiddleCenter
            );
    }

    #endregion
}
