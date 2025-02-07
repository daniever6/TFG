using UnityEngine;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    /// <summary>
    /// Clase para abrir o cerrar la puerta del laboratorio mediante animacion
    /// </summary>
    public class PuertaAnimController : MonoBehaviour
    {
        private AudioSource? doorAudioSource;
        private Animator doorAnimator;
        private bool isDoorOpen = false;
        private int triggersActivated = 0;

        private void Start()
        {
            doorAudioSource = GetComponent<AudioSource>();
            doorAnimator = GetComponent<Animator>();
        }

        /// <summary>
        /// Abre la puerta del laboratorio
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            ++triggersActivated;

            if(triggersActivated > 0 && isDoorOpen == false)
            {
                SoundManager.Instance.PlayOnAudioSource(doorAudioSource, "AbrirPuerta");

                doorAnimator.CrossFade("AbrirPuertaLab", 1f);
                isDoorOpen = true;
            }
        }

        /// <summary>
        /// Cierra la puerta del laboratorio
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            --triggersActivated;

            if(triggersActivated <= 0 && isDoorOpen)
            {
                SoundManager.Instance.PlayOnAudioSource(doorAudioSource,"CerrarPuerta");

                doorAnimator.CrossFade("CerrarPuertaLab", 1f);
                isDoorOpen = false;
            }
        }
    }
}

