using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Gokyolcu.Utilities;

public class HexGrid : MonoBehaviour
{
    #region Fields and Properties

    private const float DEBUG_DURATION = 100f;

    // Related components and objects
    [SerializeField] private ColorPicker colorPicker;
    [SerializeField] private Transform parentOfHexes;
    [SerializeField] private GameObject cellPrefab;

    // Base fields
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSizeInUnit;
    [SerializeField] private Vector3 originPosition;

    private Hex[,] gridArray;

    // Base properties
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
    public float CellSizeInUnit
    {
        get => cellSizeInUnit;
        private set => cellSizeInUnit = value;
    }

    // Generated cell sizes
    public float CellWidthInUnit
    {
        get => 2 * CellSizeInUnit;
    }
    public float CellHeightInUnit
    {
        get => Mathf.Sqrt(3) * CellSizeInUnit;
    }

    #region Debug

    [SerializeField] private bool showCellDebugTexts;
    [SerializeField] private bool showCellDebugLines;
    private TextMesh[,] debugTextArray;

    #endregion

    #endregion

    public void Start()
    {
        gridArray = new Hex[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GameObject newHexObject = Instantiate(cellPrefab, parentOfHexes);
                Hex newHex = newHexObject.GetComponent<Hex>();

                newHex.InitHex(i, j, this);
                gridArray[i, j] = newHex;

                do
                {
                    newHex.ColorType = colorPicker.GetRandomColor();
                } while (newHex.CheckExplode());

                ShowDebug(i, j);
            }
        }
    }

    #region Getters

    public Hex GetGridObject(int x, int y)
    {
        bool inRange = x < 0 || y < 0 || x >= Width || y >= Height;
        return inRange ? null : gridArray[x, y];
    }
    
    public Hex GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }

    public Vector3 GetWorldPosition(int x, int y) 
    {
        Vector3 worldPosition = Vector3.zero;
        worldPosition.x = CellSizeInUnit * (3f / 2 * x);
        worldPosition.y = CellSizeInUnit * (Mathf.Sqrt(3f) / 2 * x + Mathf.Sqrt(3f) * y) - (CellHeightInUnit * (x / 2));

        return worldPosition + OriginPosition; 
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        Vector3 offsetedWorldPosition = worldPosition - originPosition - new Vector3(CellWidthInUnit * .25f, CellHeightInUnit * .5f);

        x = Mathf.RoundToInt((offsetedWorldPosition.x / CellSizeInUnit) * 2f / 3);
        y = Mathf.RoundToInt((((offsetedWorldPosition.y + CellHeightInUnit * (x / 2)) / CellSizeInUnit) - Mathf.Sqrt(3f) / 2 * x) / Mathf.Sqrt(3f));
    }

    #endregion

    #region Misc

    public void SwitchHexes(Hex hex1, Hex hex2)
    {
        gridArray[hex1.X, hex1.Y] = hex2;
        gridArray[hex2.X, hex2.Y] = hex1;

        int tempX = hex1.X;
        int tempY = hex1.Y;
        hex1.GoTo(hex2.X, hex2.Y);
        hex2.GoTo(tempX, tempY);
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
        if (debugTextArray == null)
            debugTextArray = new TextMesh[Width, Height];

        debugTextArray[i, j] = CreateWorldText(
            message,
            parent,
            gridArray[i, j].GetCenterOffsetTo0Corner(),
            14,
            Color.black,
            TextAnchor.MiddleCenter
            );
    }

    #endregion
}
