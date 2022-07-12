using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HexGrid grid;

    [SerializeField] private bool showDebug;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Hex hexCell = grid.GetGridObject(Utilities.GetMouseWorldPositionWithoutZ());

            if (showDebug)
                ShowDebug(hexCell);
        }
    }

    private void ShowDebug(Hex hexCell)
    {
        List<HexCorner> hexCorners = hexCell.Corners;
        for (int cornerIndex = 0; cornerIndex < hexCorners.Count; cornerIndex++)
        {
            for (int neighbourIndex = 0; neighbourIndex < hexCorners[cornerIndex].Neighbours.Count; neighbourIndex++)
            {
                Debug.DrawLine(Utilities.GetMouseWorldPositionWithoutZ(),
                    grid.GetGridObject(hexCell.X + hexCorners[cornerIndex].Neighbours[neighbourIndex].x, hexCell.Y + hexCorners[cornerIndex].Neighbours[neighbourIndex].y).GetCenter(),
                    Color.black, 100f);

                Debug.Log($"i: {cornerIndex}");
                Debug.Log($"x1: {hexCorners[cornerIndex].Neighbours[neighbourIndex].x}, y1: {hexCorners[cornerIndex].Neighbours[neighbourIndex].y}");
            }
        }
    }
}
