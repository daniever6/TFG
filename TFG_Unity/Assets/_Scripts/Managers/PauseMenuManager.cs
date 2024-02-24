using System;
using UnityEngine;
using Utilities;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    [SerializeField] private GameObject _pauseCanvas;

    private void Awake()
    {
        GameManager.OnAfterGameStateChanged += HandlePauseGameState;
    }

    private void OnDestroy()
    {
        GameManager.OnAfterGameStateChanged -= HandlePauseGameState;
    }

    private void HandlePauseGameState(GameState gameState)
    {
        _pauseCanvas.SetActive(gameState == GameState.Pause);
    }
}
