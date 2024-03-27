using System;
using System.Collections.Generic;
using Dialogues;
using Managers;
using UnityEngine;
using Utilities;

public enum GameState
{
    Starting,
    Resume,
    Dialogue,
    Pause,
    Win,
    Loose
}

public class GameManager : Singleton<GameManager>
{
    private Dictionary<string, Queue<Dialogue>> dialoguesDictionary = new Dictionary<string, Queue<Dialogue>>();
    public GameState _previousGameState { get; private set; }
    public GameState _gameState { get; private set; }
    
    
    public void Start() => ChangeState(GameState.Starting);
    
    //EVENTS
    public static event Action<GameState> OnBeforeGameStateChanged;
    public static event Action<GameState> OnAfterGameStateChanged;
    
    
    /// <summary>
    /// Metodo encargado de la gestion del juego, controla los estados y ejecuta los metodos indicados
    /// </summary>
    /// <param name="newState">Estado al que se cambia</param>
    public void ChangeState(GameState newState)
    {
        OnBeforeGameStateChanged?.Invoke(newState);

        if (newState != GameState.Pause) _previousGameState = newState;
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
            case GameState.Dialogue:
                HandleDialogue();
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
        ChangeState(GameState.Dialogue);
    }

    public void HandleDialogue()
    {
        
    }

    #endregion
    

    #region Handle Starting Methods

    /// <summary>
    /// Carga los dialogos del JSON para mostrar los dialogos de la intro
    /// </summary>
    public void DeserializeDialogues()
    {
        TextAsset dialogueJson = Resources.Load<TextAsset>("JSONs/DialogueJSONs");
        if (dialogueJson == null) return;

        DialogueStages stages = JsonUtility.FromJson<DialogueStages>(dialogueJson.text);
        
        foreach (DialogueStage stage in stages.Stages)
        {
            Queue<Dialogue> dialoguesQueue = new Queue<Dialogue>(stage.Dialogues);
            dialoguesDictionary.Add(stage.Stage, dialoguesQueue);
        }

        DialogueManager.Instance.GetDialogues(dialoguesDictionary["Tutorial"].ToArray());
    }

    #endregion
   
}
