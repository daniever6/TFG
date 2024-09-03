using System.Collections.Generic;
using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Managers
{
    public class DeathInvoker : MonoBehaviour
    {
        [SerializeField] private GameObject fadeCanvas;
        [SerializeField] private Image fadePanel;

        private static DeathInvoker _instance;
        public static DeathInvoker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }

                return _instance;
            }
        }

        private void Start()
        {
            _instance = this;
            fadeCanvas.SetActive(false);
        }

        /// <summary>
        /// Establece la causa de la muerte y el nivel de respawn y hace una animacion de Fade a negro
        /// y carga la esccena de muerte
        /// </summary>
        /// <param name="gameLevel">Nivel de respawn</param>
        /// <param name="deathReason">Causa de la muerte</param>
        public void KillAnimation(GameLevels gameLevel, string deathReason)
        {
            GameManager.SetDeathReason(gameLevel, deathReason);
            
            if (fadePanel.IsUnityNull())
            {
                SceneManager.LoadScene("DeathScene");
            }
            else
            {
                fadeCanvas.SetActive(true);
                StartCoroutine(FadeIn());
            }
        }
        
        /// <summary>
        /// Animacion de Fade de un panel a negro
        /// </summary>
        /// <returns></returns>
        private IEnumerator<GameObject> FadeIn()
        {
            Color panelColor = fadePanel.color;
            float startAlpha = panelColor.a;

            for (float t = 0; t < 6; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(startAlpha, 1f, t / 6);
                panelColor.a = alpha;
                fadePanel.color = panelColor;
                yield return null; 
            }

            panelColor.a = 1f;
            fadePanel.color = panelColor;
            
            SceneManager.LoadScene("DeathScene");
        }
        
    }
}