using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TripletOperations hexPicker;
    [SerializeField] private Transform idlePosition;

    private InputController inputController;
    private bool picked;
    private Vector2 pickPosition;
    private Vector2 direction;

    [SerializeField] private bool showDebug;

    private void Awake()
    {
        inputController = new InputController();

        inputController.Player.Picked.performed += ctx => picked = true;
        inputController.Player.Picked.canceled += ctx => picked = false;

        inputController.Player.PickPosition.performed += ctx => pickPosition = ctx.ReadValue<Vector2>();

        inputController.Player.Direction.performed += ctx => direction = ctx.ReadValue<Vector2>();
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
        if (picked)
        {
            Debug.Log(direction);
            Vector3 mousePosition = Utilities.ScreenToWorldPosition(pickPosition, Camera.main);
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
