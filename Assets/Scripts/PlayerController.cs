using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gokyolcu;

public class PlayerController : MonoBehaviour
{
    #region Fields and Properties

    // Related components and objects
    [SerializeField] private TripletOperations hexPicker;
    [SerializeField] private Transform idlePosition;

    // Input
    private InputController inputController;
    private bool isTouched;
    private Vector2 touchPosition;
    private Vector2 touchDelta;

    #endregion

    private void Awake()
    {
        transform.position = idlePosition.position;

        inputController = new InputController();

        inputController.Player.Touched.canceled += ctx => isTouched = true;
        inputController.Player.Position.performed += ctx => touchPosition = Utilities.ScreenToWorldPosition(ctx.ReadValue<Vector2>(), Camera.main);
        inputController.Player.Delta.performed += ctx => touchDelta = ctx.ReadValue<Vector2>();
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
        if (!isTouched) // No touch
            return;

        if (touchDelta.magnitude > 4f) // swipe
        {
            bool isClockwise;
            if (touchPosition.y > hexPicker.Center.y)
                isClockwise = touchDelta.x < 0 ? false : true;
            else
                isClockwise = touchDelta.x < 0 ? true : false;

            TurnTriplet(isClockwise);
        }
        else // touch
        {
            PickTriplet(touchPosition);
        }
        isTouched = false;
    }

    private void TurnTriplet(bool isClockwise)
    {
        hexPicker.TurnPickedTriplet(isClockwise);
    }

    private void PickTriplet(Vector3 worldPosition)
    {
        bool? foundTriplet = hexPicker.FindTriplet(worldPosition);
        if (!foundTriplet.HasValue) return;

        if (foundTriplet.Value)
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
}
