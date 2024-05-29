using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class ParticleSystemInteractable : Trigger
    {
        [SerializeField] private ParticleSystem particles;
        
        /// <summary>
        /// Ejecuta la animacion del sistema de particulas indicado
        /// </summary>
        public override void TriggerEvent()
        {
            if (particles.isPlaying) return;
                
            particles.Play();
        }
    }
}