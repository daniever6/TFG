using _Scripts.LevelScripts.SaveManager;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Managers
{
    /// <summary>
    /// Manager que controla el menu de pausa del juego
    /// </summary>
    public class PauseManager : Singleton<PauseManager>
    {
        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private GameObject panelPausa;
        [SerializeField] private GameObject panelOpcion;

        private GameObject player;

        protected override void Awake()
        {
            base.Awake();
            GameManager.OnAfterGameStateChanged += HandlePauseGameState;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnDestroy()
        {
            GameManager.OnAfterGameStateChanged -= HandlePauseGameState;
        }

        /// <summary>
        /// Metodo que se ejecuta cuando hay un cambio en el estado del juego, y que se encarga de
        /// activar o desactivar las UIs correspondientes
        /// </summary>
        /// <param name="gameState">Nuevo estado del juego</param>
        private void HandlePauseGameState(GameState gameState)
        {
            mainCanvas.SetActive(gameState != GameState.Pause);
            pauseCanvas.SetActive(gameState == GameState.Pause);
        }
        
        /// <summary>
        /// Metodo que se encarga de pausar o resumir el juego mediante los botones de la UI
        /// </summary>
        public void PauseAndResume()
        {
            GameState gameState = GameManager.GameState != GameState.Pause
                ? GameState.Pause
                : GameManager.Instance.PreviousGameState;

            if (GameManager.GameState == GameState.Starting && SceneManager.GetActiveScene().name != "Intro")
            {
                GameManager.Instance.ChangeState(GameState.Resume);
            }

            GameManager.Instance.ChangeState(gameState);
        }

        /// <summary>
        /// Cambia la escena del juego a la escena mediante el nombre de la escena
        /// </summary>
        /// <param name="sceneName">Nombre de la escena a la que cambiar</param>
        public void ChangeLevelScene(string sceneName)
        {
            SaveManager.SaveGameData(player); // Guarda la posicion del jugador

            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Cambia la escena del juego sin guardar
        /// </summary>
        /// <param name="sceneName"></param>
        public void ChangeSceneNoSave(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        ///  Evento donde si se hace clic en el boton 
        /// </summary>
        public void Options()
        {
            if (panelOpcion.active == false)
            {
                panelPausa.SetActive(false);
                panelOpcion.SetActive(true);
            }
            else
            {
                panelPausa.SetActive(true);
                panelOpcion.SetActive(false);
            }
        }

    }
}
