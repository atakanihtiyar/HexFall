using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gokyolcu.CharacterMovement
{
    public class MoveToPositionDirect : MonoBehaviour, IMoveToPosition
    {
        private const float MIN_VALUE = .02f;
        private IMoveVelocity moveVelocity;
        private Vector3 toPosition;
        private bool canMove;

        private void Awake()
        {
            moveVelocity = GetComponent<IMoveVelocity>();
        }

        private void Update()
        {
            Vector3 direction = toPosition - transform.position;
            if (canMove && direction.magnitude > MIN_VALUE)
            {
                moveVelocity.SetVelocity(direction.normalized);
            }
            else
            {
                transform.position = toPosition;
                moveVelocity.SetVelocity(Vector3.zero);
                canMove = false;
            }
        }

        public void SetToPosition(Vector3 toPosition)
        {
            this.toPosition = toPosition;
            this.canMove = true;
        }
    }
}
