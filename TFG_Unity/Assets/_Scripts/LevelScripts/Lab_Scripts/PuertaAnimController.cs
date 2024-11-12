using UnityEngine;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    /// <summary>
    /// Clase para abrir o cerrar la puerta del laboratorio mediante animacion
    /// </summary>
    public class PuertaAnimController : MonoBehaviour
    {
        private Animator doorAnimator;
        private bool isDoorOpen = false;
        private int triggersActivated = 0;

        private void Start()
        {
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
                doorAnimator.Play("AbrirPuertaLab");
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
                doorAnimator.Play("CerrarPuertaLab");
                isDoorOpen = false;
            }
        }
    }
}

