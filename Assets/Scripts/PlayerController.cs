using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TripletOperations hexPicker;
    [SerializeField] private Transform idlePosition;

    private InputController inputController;
    private bool touched;
    private Vector2 position;
    private Vector2 delta;

    [SerializeField] private bool showDebug;

    private void Awake()
    {
        inputController = new InputController();

        inputController.Player.Touched.canceled += ctx => touched = true;

        inputController.Player.Position.performed += ctx => position = Utilities.ScreenToWorldPosition(ctx.ReadValue<Vector2>(), Camera.main);

        inputController.Player.Delta.performed += ctx => delta = ctx.ReadValue<Vector2>();
    }

    private void Start()
    {
        transform.position = idlePosition.position;
    }

    private void OnEnable()
    {
        inputController.Player.Enable();
    }

    private void OnDisable()
    {
        inputController.Player.Disable();
    }

    private void Update()
    {
        if (!touched)
            return;

        if (delta.magnitude > 4f)
        {
            if (position.y > hexPicker.Center.y) // if touch above picked hexes
            {
                TurnTriplet(delta.x < 0 ? false : true); // clockwise if delta.x positive otherwise counter clockwise
            }
            else // if touch under picked hexes
            {
                TurnTriplet(delta.x < 0 ? true : false); // counter clockwise if delta.x positive otherwise clockwise
            }
        }
        else
        {
            PickTriplet();
        }
        touched = false;

        ShowDebug();
    }

    private void TurnTriplet(bool isClockwise)
    {
        hexPicker.TurnPickedTriplet(isClockwise);
    }

    private void PickTriplet()
    {
        if (hexPicker.CurrentState == TripletOperations.State.Busy) return;

        bool foundTriplet = hexPicker.FindTriplet(position);
        if (foundTriplet)
        {
            transform.rotation = hexPicker.Rotation;
            transform.position = hexPicker.Center;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.position = idlePosition.position;
        }
    }

    private void ShowDebug()
    {
        if (showDebug)
        {

        }
    }
}
