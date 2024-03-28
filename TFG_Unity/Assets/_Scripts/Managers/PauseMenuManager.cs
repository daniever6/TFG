using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace _Scripts.Managers
{
    /// <summary>
    /// Manager que controla el menu de pausa del juego
    /// </summary>
    public class PauseMenuManager : Singleton<PauseMenuManager>
    {
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private GameObject Button;


        protected override void Awake()
        {
            base.Awake();
            GameManager.OnAfterGameStateChanged += HandlePauseGameState;
        }

        private void OnDestroy()
        {
            GameManager.OnAfterGameStateChanged -= HandlePauseGameState;
        }

        public void HandlePauseGameState(GameState gameState)
        {
            pauseCanvas.SetActive(gameState == GameState.Pause);
            Button.SetActive(gameState == GameState.Pause ? false : true);
        }
        //Pausar la partida
        public void pause()
        {
            pauseCanvas.SetActive(true);
            GameManager.Instance.ChangeState(GameState.Pause);
        }
        //Seguir con la partida
        public void resume()
        {
            pauseCanvas.SetActive(false);
            GameManager.Instance.ChangeState(GameState.Resume);
        }
        //Cambiar de escena a la escena de menu principal
        public void StartMenu()
        {
            SceneManager.LoadScene(0);
        }

    }
}
