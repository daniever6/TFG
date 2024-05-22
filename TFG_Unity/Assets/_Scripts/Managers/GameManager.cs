using System;
using System.Collections.Generic;
using _Scripts.Dialogues;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Managers
{
    public enum GameState
    {
        Starting,
        Resume,
        Dialogue,
        Pause,
        Win,
        Loose
    }

    public class GameManager : _Scripts.Utilities.Singleton<GameManager>
    {
        private Dictionary<string, Queue<Dialogue>> _dialoguesDictionary = new Dictionary<string, Queue<Dialogue>>();
        public GameState PreviousGameState { get; private set; }
        public static GameState GameState { get; private set; }
        
        //EVENTS
        public static event Action<GameState> OnBeforeGameStateChanged;
        public static event Action<GameState> OnAfterGameStateChanged;

        protected override void Awake()
        {
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

    }
}