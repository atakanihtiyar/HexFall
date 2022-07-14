using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gokyolcu.CharacterMovement
{
    public class MoveVelocityTransform : MonoBehaviour, IMoveVelocity
    {
        [SerializeField] private float moveSpeed;

        private Vector3 velocity;

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        private void Update()
        {
            transform.position += velocity * moveSpeed * Time.deltaTime;
        }
    }
}
