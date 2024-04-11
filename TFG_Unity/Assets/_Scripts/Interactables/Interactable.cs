using System;
using _Scripts.Utilities;
using Facepunch;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class Interactable : GameplayMonoBehaviour
    {
        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
        }
        

        private void OnMouseEnter()
        {
            if(!enabled) return;
            Highlight.AddRenderer(_renderer);
            Highlight.Rebuild();
        }

        private void OnMouseExit()
        {
            Highlight.ClearAll();
            Highlight.Rebuild();
        }
    }
}