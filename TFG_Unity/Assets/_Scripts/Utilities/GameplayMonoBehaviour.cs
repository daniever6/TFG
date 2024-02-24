using UnityEngine;

public abstract class GameplayMonoBehaviour : MonoBehaviour
{
    private void Awake()
    {
        GameManager.OnAfterGameStateChanged += HandleGameStatedChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnAfterGameStateChanged += HandleGameStatedChanged;
    }

    private void HandleGameStatedChanged(GameState gameState)
    {
        enabled = gameState != GameState.Pause;
        if (gameState == GameState.Pause)
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
