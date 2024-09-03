using System;
using System.Collections.Generic;
using System.IO;
using _Scripts.Dialogues;
using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

public struct DeathReasonAndLevel
{
    public GameLevels GameLevel { get; set; } //Nivel donde reaparecerá
    public string DeathReason { get; set; } //Causa de la muerte

    public DeathReasonAndLevel(GameLevels gameLevel, string deathReason)
    {
        GameLevel = gameLevel;
        DeathReason = deathReason;
    }
}

namespace _Scripts.Managers
{
    public class GameManager : Utilities.Singleton<GameManager>
    {
        public static DeathReasonAndLevel PlayerDeathCause = new(GameLevels.Level0, "");
        private Dictionary<string, Queue<Dialogue>> _dialoguesDictionary = new ();
        public GameState PreviousGameState { get; private set; }
        public static GameState GameState { get; private set; }
        
        //EVENTS
        public static event Action<GameState> OnBeforeGameStateChanged;
        public static event Action<GameState> OnAfterGameStateChanged;

        protected override void Awake()
        {
            try
            {
                var a = Instance?.name;
                
                if (Instance != null)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    var savePath = Application.persistentDataPath + "/gameState.save";
                    var clothPath = Application.persistentDataPath + "/clothingIndex.json";
                    
                    if(File.Exists(savePath)) 
                        File.Delete(savePath);
                    if(File.Exists(clothPath))
                        File.Delete(clothPath);
                }
            }
            catch (Exception ex)
            {
                Destroy(this.gameObject);
            }
            
            
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }
        
        public void Start()
        {
            ChangeState(GameState.Starting);
        }

        /// <summary>
        /// Metodo encargado de la gestion del juego, controla los estados y ejecuta los metodos indicados
        /// </summary>
        /// <param name="newState">Estado al que se cambia</param>
        public void ChangeState(GameState newState)
        {
            OnBeforeGameStateChanged?.Invoke(newState);

            if (newState != GameState.Pause) PreviousGameState = newState;
            GameState = newState;

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

        private void HandleStarting()
        {
            if (File.Exists(Application.persistentDataPath + "/gameState.save")) return;
            
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
        private void DeserializeDialogues()
        {
            TextAsset dialogueJson = Resources.Load<TextAsset>("JSONs/DialogueJSONs");
            if (UnityObjectUtility.IsUnityNull(dialogueJson)) return;

            DialogueStages stages = JsonUtility.FromJson<DialogueStages>(dialogueJson.text);
        
            foreach (DialogueStage stage in stages.Stages)
            {
                Queue<Dialogue> dialoguesQueue = new Queue<Dialogue>(stage.Dialogues);
                _dialoguesDictionary.Add(stage.Stage, dialoguesQueue);
            }

            DialogueManager.Instance.GetDialogues(_dialoguesDictionary["Tutorial"].ToArray());
        }
        
        #endregion

        public static void SetDeathReason(GameLevels gameLevel, string deathReason)
        {
            PlayerDeathCause.GameLevel = gameLevel;
            PlayerDeathCause.DeathReason = deathReason;
        }
    }
}