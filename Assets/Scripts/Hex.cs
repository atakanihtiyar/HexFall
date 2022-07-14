using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu.CharacterMovement;

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
                corners.Add(new HexCorner(
                    originPosition + CornerOffsetsTo0Corner[i],
                    Quaternion.Euler(0f, 0f, 60f * i),
                    GetNeighboursAt(i)
                    ));
            }

            return corners;
        }
    }

    #endregion

    public Color ColorType { get; private set; }
    public HexGrid Grid { get; private set; }

    public IMoveToPosition movement;

    private void Start()
    {
        movement = GetComponent<IMoveToPosition>();
    }

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

    public List<Hex> GetNeighboursAt(int cornerIndex)
    {
        Vector2Int[] neighbourOffsets = X % 2 == 0 ? neighbourOffsetsEvenColumns : neighbourOffsetsOddColumns;
        int i = cornerIndex % neighbourOffsets.Length;
        int j = (cornerIndex + 1) % neighbourOffsets.Length;

        List<Hex> neighbours = new List<Hex>();
        Hex neighbour1 = Grid.GetGridObject(X + neighbourOffsets[i].x, Y + neighbourOffsets[i].y);
        if (neighbour1 != null)
            neighbours.Add(neighbour1);

        Hex neighbour2 = Grid.GetGridObject(X + neighbourOffsets[j].x, Y + neighbourOffsets[j].y);
        if (neighbour2 != null)
            neighbours.Add(neighbour2);

        return neighbours;
    }

    public HexCorner GetNearestCorner(Vector3 worldPosition)
    {
        int nearestIndex = 0;
        float nearestDistance = Vector3.Distance(worldPosition, Corners[0].WorldPosition);

        for (int i = 1; i < Corners.Count; i++)
        {
            float currDistance = Vector3.Distance(worldPosition, Corners[i].WorldPosition);
            if (currDistance < nearestDistance)
            {
                nearestIndex = i;
                nearestDistance = currDistance;
            }
        }

        return Corners[nearestIndex];
    }

    #endregion

    public void GoTo(int x, int y)
    {
        movement.SetToPosition(Grid.GetWorldPosition(x, y));
        X = x;
        Y = y;
    }

    public bool CheckExplode()
    {
        foreach (HexCorner corner in Corners)
        {
            if (corner.Neighbours.Count < 2) return false;

            int sameColoredNeighboursCount = 0;
            for (int i = 0; i < corner.Neighbours.Count; i++)
            {
                if (corner.Neighbours[i].ColorType == ColorType) 
                    sameColoredNeighboursCount++;
            }
            if (sameColoredNeighboursCount == corner.Neighbours.Count)
                return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}

public struct HexCorner
{
    public Vector3 WorldPosition { get; private set; }
    public Quaternion Rotation { get; private set; }
    public List<Hex> Neighbours { get; private set; }

    public HexCorner(Vector3 worldPosition, Quaternion rotation, List<Hex> neighbours)
    {
        WorldPosition = worldPosition;
        Rotation = rotation;
        Neighbours = neighbours;
    }
}