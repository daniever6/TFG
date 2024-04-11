using System;
using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Transform interactionPoint;
        [SerializeField] private float interactionRadius = 1.5f;
        [SerializeField] private LayerMask interactableMask;
        
        private Collider[] _collidersBuffer = new Collider[5];
        private int _numCollisions = 0;

        [SerializeField] private Canvas interactorUI;

        private void Update()
        {
            _numCollisions = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionRadius, 
                _collidersBuffer, interactableMask);


            interactorUI.enabled = _numCollisions > 0 ? true : false;
        }

        /// <summary>
        /// Metodo que se encarga de interactuar con el objeto interactable mas cercano, y que responde al
        /// inputActionReference interact del UserInput
        /// </summary>
        public void Interact()
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestObject = null;

            for(int i = 0; i < _numCollisions; i++)
            {
                Collider colliderObject = _collidersBuffer[i];
                float distance = Vector3.Distance(interactionPoint.position, colliderObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = colliderObject.gameObject;
                }
            }

            if (!closestObject.IsUnityNull())
            {
                closestObject.GetComponent<Trigger>().TriggerEvent();
            }
        }
    }
}