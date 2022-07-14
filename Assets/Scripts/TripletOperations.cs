using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripletOperations : MonoBehaviour
{
    #region Fields and Properties

    // Related components and objects
    [SerializeField] private HexGrid grid;

    public State CurrentState { get; private set; }

    public List<Hex> PickedHexes { get; private set; }
    public Vector3 Center { get; private set; }
    public Quaternion Rotation { get; private set; }

    private Coroutine turnCoroutine;

    #endregion

    #region Find

    public bool? FindTriplet(Vector3 worldPosition)
    {
        if (CurrentState == State.Busy) return null;
        CurrentState = State.Busy;

        // if there is no hex at the given position return null
        Hex hexCell = grid.GetGridObject(worldPosition);
        if (hexCell == null)
        {
            ResetPickedHexes();
            return false;
        }

        // if the hex doesn't have enough neighbors return null
        HexCorner nearestCorner = hexCell.GetNearestCorner(worldPosition);
        if (nearestCorner.Neighbours.Count < 2)
        {
            ResetPickedHexes();
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

    private void ResetPickedHexes()
    {
        PickedHexes = null;
        Center = Vector3.zero;
        Rotation = Quaternion.identity;
        CurrentState = State.Available;
    }

    #endregion

    #region Turn

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

            bool canExplode = CheckExplode(); // Check
            Debug.Log(canExplode);

            if (canExplode)
            {
                // Explode
                break;
            }
        }

        turnCoroutine = null; 
        CurrentState = State.Available;
    }

    private IEnumerator TakeATurn(bool isClockwise)
    {
        int pivotIndex = isClockwise ? 0 : PickedHexes.Count - 1;
        for (int i = 1; i < PickedHexes.Count; i++)
        {
            int directionalIndex = isClockwise ? i : PickedHexes.Count - 1 - i;
            grid.SwitchHexes(PickedHexes[pivotIndex], PickedHexes[directionalIndex]);
        }
        yield return new WaitForSeconds(.5f);
    }

    private bool CheckExplode()
    {
        foreach (Hex hex in PickedHexes)
        {
            if (hex.CheckExplode())
                return true;
        }
        return false;
    }

    #endregion

    public enum State
    {
        Available,
        Busy
    }
}