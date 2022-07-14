using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripletOperations : MonoBehaviour
{
    [SerializeField] private HexGrid grid;
    public List<Hex> PickedHexes { get; private set; }
    public Vector3 Center { get; private set; }
    public Quaternion Rotation { get; private set; }

    public State CurrentState { get; private set; }

    private Coroutine turnCoroutine;

    public bool FindTriplet(Vector3 worldPosition)
    {
        if (CurrentState == State.Busy) return false;
        CurrentState = State.Busy;

        // if there is no hex at the given position return null
        Hex hexCell = grid.GetGridObject(worldPosition);
        if (hexCell == null) 
        {
            PickedHexes = null;
            Center = Vector3.zero;
            Rotation = Quaternion.identity;
            return false;
        }

        // if the hex doesn't have enough neighbors return null
        HexCorner nearestCorner = hexCell.GetNearestCorner(worldPosition);
        if (nearestCorner.Neighbours.Count < 2)
        {
            PickedHexes = null;
            Center = Vector3.zero;
            Rotation = Quaternion.identity;
            return false;
        }

        // Pick the triplet
        PickedHexes = new List<Hex>();
        PickedHexes.Add(hexCell);
        hexCell.transform.SetParent(transform);
        foreach (Hex neighbour in nearestCorner.Neighbours)
        {
            if (neighbour != null)
            {
                neighbour.transform.SetParent(transform);
                PickedHexes.Add(neighbour);
            }
        }

        Center = nearestCorner.WorldPosition;
        Rotation = nearestCorner.Rotation;
        CurrentState = State.Available;
        return true;
    }

    public void TurnPickedTriplet(bool isClockwise)
    {
        if (PickedHexes == null || CurrentState == State.Busy) return;
        CurrentState = State.Busy;

        if (turnCoroutine == null)
            turnCoroutine = StartCoroutine(TurnPickedTripletCoroutine(isClockwise));
    }

    private IEnumerator TurnPickedTripletCoroutine(bool isClockwise)
    {
        for (int i = 0; i < PickedHexes.Count; i++)
        {
            yield return TakeATurn(isClockwise); // Iterate
            // Check
            // Explode
        }

        turnCoroutine = null; 
        CurrentState = State.Available;
    }

    private IEnumerator TakeATurn(bool isClockwise)
    {
        if (isClockwise)
        {
            for (int i = 1; i < PickedHexes.Count; i++)
            {
                grid.SwitchHexes(PickedHexes[0], PickedHexes[i]);
            }
        }
        else
        {
            for (int i = 1; i < PickedHexes.Count; i++)
            {
                int reverseI = PickedHexes.Count - 1 - i;
                grid.SwitchHexes(PickedHexes[2], PickedHexes[reverseI]);
            }
        }

        yield return new WaitForSeconds(.5f);
    }

    public enum State
    {
        Available,
        Busy
    }
}