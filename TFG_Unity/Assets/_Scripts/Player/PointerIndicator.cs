using System;
using UnityEngine;

namespace _Scripts.Player
{
    public class PointerIndicator : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float rotationSpeed;

        private void Update()
        {
            Vector3 direction = new Vector3
            (
                target.position.x - transform.position.x,
                0,
                target.position.z - transform.position.z

            );
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
