using UnityEngine;

namespace _Scripts.Interactables
{
    public class InteractionPrompt : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        /// <summary>
        /// Sigue al jugador a una distancia fija
        /// </summary>
        private void LateUpdate()
        {
            Quaternion rotation = _camera.transform.rotation;
            
            transform.LookAt(transform.position + rotation * Vector3.forward, 
                rotation * Vector3.up);
        }
    }
}
