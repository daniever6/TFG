using System;
using _Scripts.Managers;
using Unity.VisualScripting;

namespace _Scripts.Utilities
{
    /// <summary>
    /// Esta es una clase abstracta que se encarga de desactivar los componentes hijos cuando el juego esta pausado
    /// </summary>
    public abstract class GameplayMonoBehaviour<T> : StaticInstance<T> where T : GameplayMonoBehaviour<T>
    {
        private void OnEnable()
        {
            GameManager.OnBeforeGameStateChanged += HandleGameStatedChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            GameManager.OnBeforeGameStateChanged -= HandleGameStatedChanged;
        }

        /// <summary>
        /// Detiene los componentes que hijos cuando gameState es Pause o Dialogue
        /// </summary>
        /// <param name="gameState"></param>
        private void HandleGameStatedChanged(GameState gameState)
        {
            if (this.IsUnityNull()) return;
            if (gameState == null) return;
            
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
