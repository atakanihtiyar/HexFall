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

        inputController.Player.Position.performed += ctx => position = ctx.ReadValue<Vector2>();

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
        if (touched) // Input started
        {
            if (delta.magnitude > 4f)
            {
                // Turn the picked triplet
                Debug.Log("turn: " + delta);
            }
            else
            {
                PickTriplet();
            }
            touched = false;
        }

        ShowDebug();
    }

    private void PickTriplet()
    {
        Vector3 touchPosition = Utilities.ScreenToWorldPosition(position, Camera.main);
        bool foundTriplet = hexPicker.FindTriplet(touchPosition);
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
