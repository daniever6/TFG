using System;
using UnityEngine;
using Utilities;

public class GameManager : Singleton<GameManager>
{
    public GameState _gameState { get; private set; }
    
    public void Start() => ChangeState(GameState.Starting);
    
    public static event Action<GameState> OnBeforeGameStateChanged;
    public static event Action<GameState> OnAfterGameStateChanged;
    
    public void ChangeState(GameState newState)
    {
        OnBeforeGameStateChanged?.Invoke(newState);

        _gameState = newState;

        switch (newState)
        {
            case GameState.Starting:
                break;
            case GameState.Resume:
                break;
            case GameState.Pause:
                break;
            case GameState.Win:
                break;
            case GameState.Loose:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        OnAfterGameStateChanged?.Invoke(newState);
    }
}

public enum GameState
{
    Starting,
    Resume,
    Pause,
    Win,
    Loose
}
