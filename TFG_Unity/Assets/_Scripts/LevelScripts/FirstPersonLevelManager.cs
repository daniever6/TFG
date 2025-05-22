using _Scripts.Dialogues;
using _Scripts.LevelScripts.Level_02._1;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.Managers;
using _Scripts.Utilities;
using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.LevelScripts
{
    
    public class FirstPersonLevelManager : ALevel
    {
        #region Class definition

        [SerializeField] private TextAsset levelCombinationsTextAsset;

        [SerializeField] private CinemachineVirtualCamera teacherCamera; 
        [SerializeField] private DialogueTrigger EndDialogue;
        [SerializeField] private NpcState teacherState;

        public static event Action<string> OnCombinationPerformed;
        
        public static int CurrentCombinationIndex = 0;
        public bool isAcidLevel = false;

        private List<List<string>> _levelCorrectCombinations = new ();
        private List<string> _correctCombinations = new ();
        private List<string> _currentCombinations = new ();

        #endregion

        #region Level Methods

        protected override async void Start()
        {
            base.Start();
            CurrentCombinationIndex = 0;
            await LoadCorrectCombinations();
            _correctCombinations = _levelCorrectCombinations[0].ToList();
        }

        /// <summary>
        /// Devuelve la combinacion correcta en la posicion del indice indicado
        /// </summary>
        /// <param name="idx">Posicion a comprobar</param>
        /// <returns>Nombre de la combinacion</returns>
        public string GetCorrectCombinationAt(int idx)
        {
            if(idx > _correctCombinations.Count)
            {
                SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
            }
            return _correctCombinations[idx];
        }
    
        /// <summary>
        /// Realiza la combinacion si la combinacion pasada es igual a la combinacion esperada
        /// </summary>
        /// <param name="combination">Combinacion realizada</param>
        /// <returns>True si se realiza la combinacion</returns>
        public override bool PerformCombination(string combination)
        {
            if(!combination.Equals(GetCorrectCombinationAt(CurrentCombinationIndex))) return false;

            if(SceneManager.GetActiveScene().name == "Level_02.1")
            {
                if (SubirVentanaExtractora.IsTooHigh)
                {
                    DeathInvoker.Instance.KillAnimation(GameLevels.LevelAcidos, "Te has intoxicado con los ácidos " +
                        "por tener la ventana demasiado subida");
                    return false;
                }
            }

            OnCombinationPerformed?.Invoke(combination);

            _currentCombinations.Add(combination);
            CurrentCombinationIndex++;
            return true;
        }

        /// <summary>
        /// Metodo que se ejecuta despues de una combinacion correcta, lleva la orden de las combinaciones
        /// </summary>
        public override void PostPerformCombination()
        {
            if (CheckCompletion())
            {
                _levelCorrectCombinations.RemoveAt(0);
                if (_levelCorrectCombinations.Count > 0)
                {
                    CurrentCombinationIndex = 0;
                    _currentCombinations.Clear();
                    _correctCombinations = _levelCorrectCombinations[0];
                    
                    //Nuevo proceso quimico
                }
                else
                {
                    //Fin Level
                    ShowEndDialogue();
                }
            }
        }

        /// <summary>
        /// Activa el dialogo del final del nivel
        /// </summary>
        private void ShowEndDialogue()
        {
            teacherCamera.enabled = true;
            teacherState.ChangeState(NpcStates.Talking);
            EndDialogue.TriggerEvent();

            LevelDBManager.OnLevelCompleted?.Invoke();

            DialogueManager.OnDialogueFinish += ExitLevel;
        }

        /// <summary>
        /// Sale del nivel y guarda el progreso
        /// </summary>
        private void ExitLevel()
        {
            SaveData saveData = SaveManager.SaveManager.LoadGameData();

            LevelState nextLevelState = LevelState.NivelBalanza;

            try
            {
                if (isAcidLevel)
                {
                    nextLevelState = LevelState.NivelResiduos;
                    SaveManager.SaveManager.SaveGameData(saveData.playerPosition, GameState.Resume, LevelState.NivelResiduos);
                }
                else
                {
                    nextLevelState = LevelState.NivelBalanza;
                    SaveManager.SaveManager.SaveGameData(saveData.playerPosition, GameState.Resume, LevelState.NivelBalanza);
                }
            }
            finally
            {
                SaveManager.SaveManager.SaveGameData(saveData.playerPosition, GameState.Resume, nextLevelState);
                ResiduosDroppedManager.ResiduosDropped = new[] { false, false, false };
                SaveManager.SaveManager.SaveResiduosData();

                
                SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
            }

            DialogueManager.OnDialogueFinish -= ExitLevel;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            DialogueManager.OnDialogueFinish -= ExitLevel;
        }

        /// <summary>
        /// Comprueba si se han acabado las combinaciones a realizar, comprobando
        /// si currentCombination = correctCombination
        /// </summary>
        /// <returns>True si se han acabado las combinaciones</returns>
        private bool CheckCompletion()
        {
            if (_correctCombinations.Count != _currentCombinations.Count)
            {
                return false;
            }

            int i = 0;
            foreach (string combination in _currentCombinations)
            {
                if (combination != _correctCombinations[i])
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        /// <summary>
        /// Carga la lista de combinaciones a la pila de combinaciones correctas desde el JsonIndicado
        /// </summary>
        private async Task LoadCorrectCombinations()
        {
            if (levelCombinationsTextAsset == null) return;

            LevelCombinations combinationList = JsonUtility.FromJson<LevelCombinations>(levelCombinationsTextAsset.text);

            foreach (Combinations combination in combinationList.LevelCombinationsList)
            {
                _levelCorrectCombinations.Add(combination.CombinationList);
            }
        }
        
        #endregion

    }
}
