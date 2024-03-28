using System;
using System.Collections.Generic;
using Dialogues;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

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

    public class GameManager : Singleton<GameManager>
    {
        private Dictionary<string, Queue<Dialogue>> _dialoguesDictionary = new Dictionary<string, Queue<Dialogue>>();
        public GameState PreviousGameState { get; private set; }
        public GameState GameState { get; private set; }

        private Button Pause;
        
        //EVENTS
        public static event Action<GameState> OnBeforeGameStateChanged;
        public static event Action<GameState> OnAfterGameStateChanged;

        public void Start()=>ChangeState(GameState.Starting);

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
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

        public void HandleStarting()
        {
            DeserializeDialogues();
            ChangeState(GameState.Dialogue);
        }

        public void HandleDialogue()
        {
        
        }
        //Metodo para invocar el canvas del menu de pausa.

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
                _dialoguesDictionary.Add(stage.Stage, dialoguesQueue);
            }

            DialogueManager.Instance.GetDialogues(_dialoguesDictionary["Tutorial"].ToArray());
        }

        #endregion

    }
}