using _Scripts.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Interactables
{
    /// <summary>
    /// Clase que se utiliza para mostrar el nombre del interactable
    /// </summary>
    public class ShowInteractableName : GameplayMonoBehaviour<ShowInteractableName>
    {
        [SerializeField] private Canvas NameTextCanvas; //Canvas para mostrar el nombre del interactable

        private void Start()
        {
            if (NameTextCanvas.IsUnityNull())
            {
                Destroy(this);
                return;
            }

            NameTextCanvas.enabled = false;
        }

        /// <summary>
        /// Muestra el canvas con el nombre
        /// </summary>
        private void OnMouseEnter()
        {
            NameTextCanvas.enabled = true;
        }

        /// <summary>
        /// Oculta el canvas con el nombre
        /// </summary>
        private void OnMouseExit()
        {
            NameTextCanvas.enabled = false;
        }
    }
}