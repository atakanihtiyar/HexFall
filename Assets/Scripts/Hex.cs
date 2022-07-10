using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex //: MonoBehaviour
{
    // Positioning
    public int X { get; private set; }
    public int Y { get; private set; }
    
    public HexGrid Grid { get; private set; }

    public Hex(int x, int y, HexGrid grid)
    {
        X = x;
        Y = y;
        Grid = grid;
    }
}
