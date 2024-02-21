using System;
using UnityEngine;
using Utilities;

public class GameManager : Singleton<MonoBehaviour>
{
    public GameState _gameState { get; private set; }
    public void Start() => ChangeState(GameState.Starting);
    public void ChangeState(GameState newState)
    {
        _gameState = newState;
        switch (newState)
        {
            case GameState.Starting:
                break;
            case GameState.Gameplay:
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
    }
}

public enum GameState
{
    Starting,
    Gameplay,
    Pause,
    Win,
    Loose
}
