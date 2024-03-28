using _Scripts.Managers;
using UnityEngine;
using Utilities;

namespace _Scripts.Utilities
{
    /// <summary>
    /// Esta es una clase abstracta que se encarga de desactivar los componentes hijos cuando el juego esta pausado
    /// </summary>
    public abstract class GameplayMonoBehaviour : Singleton<GameplayMonoBehaviour>
    {
        private void Awake()
        {
            GameManager.OnBeforeGameStateChanged += HandleGameStatedChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnBeforeGameStateChanged -= HandleGameStatedChanged;
        }

        /// <summary>
        /// Detiene los componentes que hijos cuando gameState es Pause o Dialogue
        /// </summary>
        /// <param name="gameState"></param>
        private void HandleGameStatedChanged(GameState gameState)
        {
            enabled = (gameState != GameState.Pause && gameState != GameState.Dialogue);
            if (gameState == GameState.Pause || gameState == GameState.Dialogue)
            {
                OnPostPaused();
            }
            else
            {
                OnPostResumed();
            }
        }
    
        protected virtual void OnPostPaused(){}
        protected virtual void OnPostResumed(){}
    }
}
