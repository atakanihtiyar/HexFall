using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TripletOperations hexPicker;
    [SerializeField] private Transform idlePosition;

    [SerializeField] private bool showDebug;

    private void Start()
    {
        transform.position = idlePosition.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Utilities.GetMouseWorldPositionWithoutZ();
            hexPicker.GetTriplet(mousePosition, out Vector3? center, out Quaternion? rotation);
            if (center.HasValue && rotation.HasValue && hexPicker.PickedHexes.Count == 3)
            {
                transform.rotation = rotation.Value;
                transform.position = center.Value;
            }
            else
            {
                transform.rotation = Quaternion.identity;
                transform.position = idlePosition.position;
            }
        }

        ShowDebug();
    }

    private void ShowDebug()
    {
        if (showDebug)
        {

        }
    }
}
