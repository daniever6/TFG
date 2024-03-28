using UnityEngine;
using Utilities;

namespace _Scripts.Managers
{
    /// <summary>
    /// Manager que controla el menu de pausa del juego
    /// </summary>
    public class PauseMenuManager : Singleton<PauseMenuManager>
    {
        [SerializeField] private GameObject pauseCanvas;

        protected override void Awake()
        {
            base.Awake();
            GameManager.OnAfterGameStateChanged += HandlePauseGameState;
        }

        private void OnDestroy()
        {
            GameManager.OnAfterGameStateChanged -= HandlePauseGameState;
        }

        private void HandlePauseGameState(GameState gameState)
        {
            pauseCanvas.SetActive(gameState == GameState.Pause);
        }
    }
}
