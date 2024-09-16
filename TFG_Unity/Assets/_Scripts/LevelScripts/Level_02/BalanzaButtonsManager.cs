using System;
using System.Collections;
using System.Threading.Tasks;
using _Scripts.Utilities;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class BalanzaButtonsManager : GameplayMonoBehaviour<BalanzaButtonsManager>
    {
        [SerializeField] private TextMeshProUGUI textoPantalla;
        
        private bool isPaused = false;
        private bool isBalanzaOn = false;
        private bool isBalanzaReady = false;

        private void Start()
        {
            textoPantalla.text = string.Empty;
        }

        /// <summary>
        /// Captura los eventos en los botones
        /// </summary>
        private void OnMouseUp()
        {
            Enum.TryParse(this.name, out BalanzaButtons button);

            if (button.IsUnityNull())
            {
                button = BalanzaButtons.None;
            }
            
            ButtonBehaviour(button);
        }

        /// <summary>
        /// Gestion de los botones de la balanza
        /// </summary>
        /// <param name="button">Boton pulsado</param>
        private void ButtonBehaviour(BalanzaButtons button)
        {
            switch (button)
            {
                case BalanzaButtons.None:
                    break;
                case BalanzaButtons.OnOff:
                    OnOffButton();
                    break;
                case BalanzaButtons.Get:
                    break;
                case BalanzaButtons.Set:
                    break;
                case BalanzaButtons.Zero:
                    break;
                case BalanzaButtons.Print:
                    break;
                
                default:
                    break;
            }
        }

        /// <summary>
        /// Enciende o apaga la balanza
        /// </summary>
        private async void OnOffButton()
        {
            if (!isBalanzaOn)
            {
                isBalanzaOn = true;
                await TurnOnBehaviour();
            }
            else
            {
                if (!isBalanzaReady)
                {
                    return;
                }
                
                await TurnOffBehaviour();
                isBalanzaOn = false;
                isBalanzaReady = false;
            }
        }

        /// <summary>
        /// Muestra un texto mientras se va encendiendo la balanza
        /// </summary>
        private async Task TurnOnBehaviour()
        {
            float elapsedTime = 0f;
            float duration = 5f;

            int cont = 0;

            //Establece el texto mientras carga la balanza
            while (elapsedTime < duration)
            {
                if (isPaused)
                {
                    await Task.Yield();
                    continue;
                }
                
                elapsedTime += Time.deltaTime;

                textoPantalla.text = new string('.', cont);
                cont = (cont + 1) % 4;
                await Task.Delay(500);
                elapsedTime += 0.5f;
                await Task.Yield();
            }

            textoPantalla.text = "On";
            await Task.Delay(2500);
            textoPantalla.text = "---";
            
            isBalanzaReady = true;
        }

        /// <summary>
        /// Muestra el texto cuando se apaga la balanza
        /// </summary>
        private async Task TurnOffBehaviour()
        {
            textoPantalla.text = "0ff";
            await Task.Delay(1000);
            textoPantalla.text = string.Empty;
        }

        protected override void OnPostPaused()
        {
            base.OnPostPaused();
            isPaused = true;
        }

        protected override void OnPostResumed()
        {
            base.OnPostResumed();
            isPaused = false;
        }
    }
}

