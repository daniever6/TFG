using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class ParticleSystemInteractable : Trigger
    {
        [SerializeField] private ParticleSystem particleSystem;
        public override void TriggerEvent()
        {
            if (particleSystem.isPlaying) return;
                
            particleSystem.Play();
        }
    }
}