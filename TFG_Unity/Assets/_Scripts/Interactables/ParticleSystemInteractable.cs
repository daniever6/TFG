using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class ParticleSystemInteractable : Trigger
    {
        [SerializeField] private ParticleSystem particles;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Ejecuta la animacion del sistema de particulas indicado
        /// </summary>
        public override void TriggerEvent()
        {
            if (particles.isPlaying) return;

            SoundManager.Instance.PlayOnAudioSource(audioSource, "Ducha");
                
            particles.Play();
        }
    }
}