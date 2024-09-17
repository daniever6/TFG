using System.Collections.Generic;
using _Scripts.UI;
using _Scripts.Utilities;
using Facepunch;
using UnityEngine;

namespace _Scripts.Interactables
{
    /// <summary>
    /// Clase que utiliza un outline para aquellos objetos que implementen esta clase,
    /// se activa cuando el ratón se posiciona encima del objeto
    /// </summary>
    public class Interactable : GameplayMonoBehaviour<Interactable>
    {
        private Renderer _renderer;
        [SerializeField] private List<GameObject> extraMeshes = new();
        private List<Renderer> extraRenderers = new();

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            foreach (var gameobject in extraMeshes)
            {
                extraRenderers.Add(gameobject.GetComponent<Renderer>());
            }
        }
        
        /// <summary>
        /// Pinta el outline del objeto
        /// </summary>
        private void OnMouseEnter()
        {
            if(!enabled) return;
            Highlight.AddRenderer(_renderer);
            foreach (var renderer in extraRenderers)
            {
                Highlight.AddRenderer(renderer);
            }
            Highlight.Rebuild();
        }

        /// <summary>
        /// Desactiva el outline del objeto
        /// </summary>
        private void OnMouseExit()
        {
            Highlight.ClearAll();
            Highlight.Rebuild();
        }
    }
}