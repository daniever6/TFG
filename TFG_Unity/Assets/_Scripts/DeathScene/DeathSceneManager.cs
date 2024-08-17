using _Scripts.Managers;
using _Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.DeathScene
{
    public class DeathSceneManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deathReason;
        
        void Start()
        {
            deathReason.text = GameManager.PlayerDeathCause.DeathReason;
        }

        /// <summary>
        /// Permite el cambio de escena, diseñado para volver al menu principal
        /// </summary>
        /// <param name="sceneName">Nombre de la escena</param>
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Reinicia el juego desde el último punto guardado antes de la muerte
        /// </summary>
        public void RepeatLevel()
        {
            var level = GameManager.PlayerDeathCause.GameLevel;

            switch (level)
            {
                case GameLevels.Level0:
                    SceneManager.LoadScene("Level_00");
                    break;
                
                case GameLevels.Laboratory:
                    SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
                    break;
                
                case GameLevels.Level1:
                    SceneManager.LoadScene("Level_01");
                    break;
                
                case GameLevels.Level2:
                    SceneManager.LoadScene("Level_02");
                    break;
                
                case GameLevels.Level3:
                    SceneManager.LoadScene("Level_03");
                    break;
            }
        }
    }
}
