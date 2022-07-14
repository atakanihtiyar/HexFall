using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TripletOperations hexPicker;
    [SerializeField] private Transform idlePosition;

    private InputController inputController;
    private bool? touched;
    private Vector2 position;
    private Vector2 delta;
    private float inputDeltaTime;
    private const float limitInputDeltaTime = .11f;

    [SerializeField] private bool showDebug;

    private void Awake()
    {
        inputController = new InputController();

        inputController.Player.Touched.performed += ctx =>
        {
            touched = true;
            inputDeltaTime = 0f;
        };
        inputController.Player.Touched.canceled += ctx => touched = false;

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
        if (!touched.HasValue) return;
        
        if (touched.Value) // Input started
        {
            inputDeltaTime += Time.deltaTime;
        }
        else // Input end
        {
            if (inputDeltaTime < limitInputDeltaTime) 
            {
                PickTriplet();
            }
            else
            {
                // Turn the picked triplet
                hexPicker.TurnPickedTriplet();
            }
            touched = null;
        }

        ShowDebug();
    }

    private void PickTriplet()
    {
        inputDeltaTime += Time.deltaTime;
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
