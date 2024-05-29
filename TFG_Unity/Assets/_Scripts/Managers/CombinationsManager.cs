using System;
using System.Collections.Generic;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Managers
{
    public class CombinationsManager : Singleton<CombinationsManager>
    {
        [SerializeField] private GameObject teacherHand;

        public static event Action CombinationErrorEvent; 

        private Dictionary<string, CombinationResult> _combinations = new Dictionary<string, CombinationResult>();

        private void Start() {
            LoadCombinations("Combinaciones");
        }

        /// <summary>
        /// Metodo que carga el fichero con las posibles combinaciones entre interactuables, y los guarda en un
        /// diccionario cuya clave es la combinacion de nombres de los objetos, y cuyos valores son los resultados
        /// de la combinacion
        /// </summary>
        /// <param name="filePath">Archivo de texto a descomprimir</param>
        private void LoadCombinations(string filePath)
        {
            TextAsset file = Resources.Load<TextAsset>(filePath);

            if (file == null) return;
            
            string[] lines = file.text.Split('\n');
            foreach (string line in lines) {
                string[] parts = line.Split(':');
                
                string combination = parts[0].Trim();
                CombinationResult result;
                if (Enum.TryParse(parts[1].Trim(), out result)) {
                    _combinations.Add(combination, result);
                }
                
            }
        }
        
        /// <summary>
        /// Devuelve el resultado de la combinacion entre 2 interactuables
        /// </summary>
        /// <param name="combination">Nombre de la combinacion</param>
        /// <returns>Resultado de la combinacion</returns>
        public CombinationResult GetCombinationResult(string combination) {
            CombinationResult result;
            
            if (_combinations.TryGetValue(combination, out result)) {
                if(result == CombinationResult.Error) CombinationErrorEvent?.Invoke();
                return result;
            }
            
            return CombinationResult.None;
        }

    }
}