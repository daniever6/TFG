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
        private static bool isPaused = false;

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
                    BalanzaManager.Instance.CalculateCurrentPesos();
                    break;
                case BalanzaButtons.Set:
                    break;
                case BalanzaButtons.Zero:
                    SetPesoCero();
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
            if (!BalanzaManager.IsBalanzaOn)
            {
                BalanzaManager.IsBalanzaOn = true;
                await TurnOnBehaviour();
            }
            else
            {
                if (!BalanzaManager.IsBalanzaReady)
                {
                    return;
                }
                
                await TurnOffBehaviour();
                BalanzaManager.IsBalanzaOn = false;
                BalanzaManager.IsBalanzaReady = false;
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

                BalanzaManager.Instance.SetBalanzaText(new string('.', cont));
                cont = (cont + 1) % 4;
                await Task.Delay(500);
                elapsedTime += 0.5f;
                await Task.Yield();
            }

            BalanzaManager.Instance.SetBalanzaText("On");
            await Task.Delay(2500);
            BalanzaManager.IsBalanzaReady = true;
            BalanzaManager.Instance.CalculateCurrentPesos();
            
            
        }

        /// <summary>
        /// Muestra el texto cuando se apaga la balanza
        /// </summary>
        private async Task TurnOffBehaviour()
        {
            BalanzaManager.Instance.SetBalanzaText("0ff");
            await Task.Delay(1000);
            BalanzaManager.Instance.SetBalanzaText(string.Empty);
        }

        
        /// <summary>
        /// Nivela el peso actual de la balanza a cero
        /// </summary>
        private void SetPesoCero()
        {
            if (!BalanzaManager.IsBalanzaReady || !BalanzaManager.IsBalanzaOn)
            {
                return;
            }

            BalanzaManager.Instance.SetPesoBalanza(0);
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

