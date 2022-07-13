using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripletOperations : MonoBehaviour
{
    [SerializeField] private HexGrid grid;
    public List<Hex> PickedHexes { get; private set; }

    public void GetTriplet(Vector3 worldPosition, out Vector3? center, out Quaternion? centerRotation)
    {
        Hex hexCell = grid.GetGridObject(worldPosition);

        if (hexCell == null) // if there is no hex at the given position return null
        {
            centerRotation = null;
            center = null;
            return;
        }

        HexCorner nearestCorner = hexCell.GetNearestCorner(worldPosition);
        if (nearestCorner.Neighbours.Count < 2) // if the hex doesn't have enough neighbors return null
        {
            centerRotation = null;
            center = null;
            return;
        }

        PickedHexes = new List<Hex>();
        PickedHexes.Add(hexCell);
        foreach (Hex neighbour in nearestCorner.Neighbours)
        {
            if (neighbour != null)
            {
                PickedHexes.Add(neighbour);
            }
        }

        centerRotation = nearestCorner.Rotation;
        center = nearestCorner.WorldPosition;
    }
}
