using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    // Positioning
    public int X { get; private set; }
    public int Y { get; private set; }
    
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

    public override string ToString()
    {
        return $"{X}, {Y}";
    }
}
