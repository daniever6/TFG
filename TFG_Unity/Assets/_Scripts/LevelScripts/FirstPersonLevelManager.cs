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
        
        public static int CurrentCombinationIndex = 0;

        private List<List<string>> _levelCorrectCombinations = new ();
        private List<string> _correctCombinations = new ();
        private Stack<string> _currentCombinations = new ();

        #endregion

        #region Level Methods

        protected override async void Start()
        {
            base.Start();
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

            _currentCombinations.Push(combination);
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
                    SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
                }
            }
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
