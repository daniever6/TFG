using System;
using System.Collections.Generic;
using Dialogues;
using Managers;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine;
using Utilities;

public enum GameState
{
    Starting,
    Resume,
    Pause,
    Win,
    Loose
}

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, Queue<DialogueSerializable>> dialoguesDictionary = new Dictionary<string, Queue<DialogueSerializable>>();

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
                HandleStarting();
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

    #region Game State Handlers

    public void HandleStarting()
    {
        DeserializeDialogues();
        ChangeState(GameState.Resume);
    }

    #endregion
    

    #region Handle Starting Methods

    public void DeserializeDialogues()
    {
        TextAsset dialogueJson = Resources.Load<TextAsset>("JSONs/DialogueJSONs");
        if (dialogueJson == null) return;

        DialogueStages stages = JsonUtility.FromJson<DialogueStages>(dialogueJson.text);
        
        foreach (DialogueStage stage in stages.Stages)
        {
            Queue<DialogueSerializable> dialoguesQueue = new Queue<DialogueSerializable>(stage.Dialogues);
            dialoguesDictionary.Add(stage.Stage, dialoguesQueue);
        }
        
        DialogueManager.Instance.CinematicDialogue(dialoguesDictionary["Tutorial"]);
    }

    #endregion
   
}
