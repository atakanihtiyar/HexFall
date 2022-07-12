using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    // Positioning
    public int X { get; private set; }
    public int Y { get; private set; }

    #region Corners

    // Corner position offsets
    private Vector3[] cornerOffsetsTo0Corner = null;
    private Vector3[] CornerOffsetsTo0Corner
    {
        get
        {
            if (cornerOffsetsTo0Corner != null)
                return cornerOffsetsTo0Corner;

            cornerOffsetsTo0Corner = new Vector3[6]
            {
                new Vector3(0, 0),
                new Vector3(Grid.CellWidthInUnit * .5f, 0),
                new Vector3(Grid.CellWidthInUnit * .75f, Grid.CellHeightInUnit * .5f),
                new Vector3(Grid.CellWidthInUnit * .5f, Grid.CellHeightInUnit),
                new Vector3(0, Grid.CellHeightInUnit),
                new Vector3(-Grid.CellWidthInUnit * .25f, Grid.CellHeightInUnit * .5f),
            };

            return cornerOffsetsTo0Corner;
        }
    }

    // Neighbour XY offsets
    private readonly Vector2Int[] neighbourOffsetsEvenColumns = new Vector2Int[6]
    {
            new Vector2Int(-1, -1), // Bottom Left
            new Vector2Int(0, -1), // Bottom
            new Vector2Int(1, -1), // Bottom Right
            new Vector2Int(1, 0), // Upper Right
            new Vector2Int(0, 1), // Up
            new Vector2Int(-1, 0), // Upper Left
    };
    private readonly Vector2Int[] neighbourOffsetsOddColumns = new Vector2Int[6]
    {
            new Vector2Int(-1, 0), // Bottom Left
            new Vector2Int(0, -1), // Bottom
            new Vector2Int(1, 0), // Bottom Right
            new Vector2Int(1, 1), // Upper Right
            new Vector2Int(0, 1), // Up
            new Vector2Int(-1, 1), // Upper Left
    };

    // Corners
    List<HexCorner> corners = null;
    public List<HexCorner> Corners
    {
        get
        {
            Vector3 originPosition = Grid.GetWorldPosition(X, Y);
            if (corners != null && corners[0].WorldPosition == originPosition) return corners;

            corners = new List<HexCorner>();
            int cornerCount = CornerOffsetsTo0Corner.Length;
            for (int i = 0; i < cornerCount; i++)
            {
                if (X % 2 == 0)
                {
                    List<Vector2Int> neighbours = GetNeighboursAt(i, neighbourOffsetsEvenColumns);

                    corners.Add(new HexCorner(
                        originPosition + CornerOffsetsTo0Corner[i],
                        neighbours
                        ));
                }
                else
                {
                    List<Vector2Int> neighbours = GetNeighboursAt(i, neighbourOffsetsOddColumns);

                    corners.Add(new HexCorner(
                        originPosition + CornerOffsetsTo0Corner[i],
                        neighbours
                        ));
                }
            }

            return corners;
        }
    }

    #endregion

    public Color ColorType { get; private set; }
    public HexGrid Grid { get; private set; }

    public void InitHex(int x, int y, HexGrid grid, Color color)
    {
        X = x;
        Y = y;
        Grid = grid;

        ColorType = color;
        GetComponentInChildren<SpriteRenderer>().color = ColorType;

        transform.position = Grid.GetWorldPosition(x, y);
    }

    #region Getters

    public Vector3 GetCenter()
    {
        return transform.position + new Vector3(Grid.CellWidthInUnit * .25f, Grid.CellHeightInUnit * .5f);
    }

    public Vector3 GetCenterOffset()
    {
        return new Vector3(Grid.CellWidthInUnit * .25f, Grid.CellHeightInUnit * .5f);
    }

    private List<Vector2Int> GetNeighboursAt(int cornerIndex, Vector2Int[] neighbourOffsets)
    {
        int i = cornerIndex % neighbourOffsets.Length;
        int j = (cornerIndex + 1) % neighbourOffsets.Length;

        List<Vector2Int> neighbours = new List<Vector2Int>();
        Hex neighbour1 = Grid.GetGridObject(X + neighbourOffsets[i].x, Y + neighbourOffsets[i].y);
        if (neighbour1 != null)
            neighbours.Add(neighbourOffsets[i]);

        Hex neighbour2 = Grid.GetGridObject(X + neighbourOffsets[j].x, Y + neighbourOffsets[j].y);
        if (neighbour2 != null)
            neighbours.Add(neighbourOffsets[j]);

        return neighbours;
    }

    #endregion

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

public struct HexCorner
{
    public Vector3 WorldPosition { get; private set; }
    public List<Vector2Int> Neighbours { get; private set; }

    public HexCorner(Vector3 worldPosition, List<Vector2Int> neighbours)
    {
        WorldPosition = worldPosition;
        Neighbours = neighbours;
    }
}