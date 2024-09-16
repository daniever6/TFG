using System.Collections;
using System.Threading.Tasks;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_01
{
    public class MakeReactiveVisible : GameplayMonoBehaviour<MakeReactiveVisible>
    {
        [SerializeField] private GameObject mezcla;
        private Renderer objectRenderer;
        private Color initialColor;
        
        public bool isMezclaVisible = false;
        private bool isPaused = false;

        private void Start()
        {
            mezcla.SetActive(false);
            
            objectRenderer = mezcla.GetComponent<Renderer>();
            initialColor = objectRenderer.material.color;
        }

        /// <summary>
        /// Hace la mezcla del vaso visible
        /// </summary>
        public void MakeMezclaVisible()
        {
            isMezclaVisible = true;
            mezcla.SetActive(true);
        }

        /// <summary>
        /// Hace la mezcla del vaso invisible
        /// </summary>
        public void HideMezcla()
        {
            isMezclaVisible = false;
            mezcla.SetActive(false);
        }

        /// <summary>
        /// Ejecuta la corrutina ChangeColorOverTime para cambiar el color de la mezcla
        /// </summary>
        /// <param name="newColor">Color al que cambiar la mezcla</param>
        /// <param name="duration">Duracion de la interpolacion</param>
        public async void ChangeMezclaColor(Color newColor, float duration = 2f)
        {
            initialColor = objectRenderer.material.color;

            await ChangeColorOverTime(newColor, duration);
        }
        
        /// <summary>
        /// Cambia el color de la mezcla progresivamente de su color inicial a un nuevo color
        /// </summary>
        /// <param name="toColor">Color al que cambiar</param>
        /// <param name="duration">Duracion de cambio de color</param>
        /// <returns></returns>
        private async Task ChangeColorOverTime(Color toColor, float duration)
        {
            //Color inicial
            Color currentColor = objectRenderer.material.color;
            float alpha = currentColor.a;

            float elapsedTime = 0f;

            //Interpola el color y lo aplica al material
            while (elapsedTime < duration)
            {
                if (isPaused)
                {
                    await Task.Yield();
                    continue;
                }
                
                elapsedTime += Time.deltaTime;

                Color newColor = Color.Lerp(initialColor, toColor, elapsedTime / duration);
                newColor.a = alpha;

                objectRenderer.material.color = newColor;
                
                await Task.Yield();
            }

            //Color final
            Color finalColor = toColor;
            finalColor.a = alpha;
            objectRenderer.material.color = finalColor;
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
