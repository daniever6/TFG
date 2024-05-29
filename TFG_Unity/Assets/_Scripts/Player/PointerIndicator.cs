using System;
using UnityEngine;

namespace _Scripts.Player
{
    public class PointerIndicator : MonoBehaviour
    {
        [SerializeField] private Transform target;

        /// <summary>
        /// Gira la UI que indica la posicion del target
        /// </summary>
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
