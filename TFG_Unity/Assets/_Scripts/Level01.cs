using System;
using System.Collections.Generic;
using _Scripts.LevelScripts;
using Command;
using Utilities;

namespace _Scripts
{
    //Si es correcta, realiza animacion, si es none, no realiza animacion, si es incorrecta, realiza animacion + muerte
    public class Level01 : ALevel
    {
        public static int CurrentCombinationIndex = 0;
        
        private List<string> _correctCombinations = new List<string>()
        {
            "BotellaPlastico_Matraz",
            "BotellaAcido_VasoPrecision",
            "Pipeta_VasoPrecision",
            "Pipeta_Matraz",
            "BotellaPlastico_Matraz",
            "CuentaGotas_Matraz"
        };
        private Stack<string> _currentCombinations = new Stack<string>();

        public string GetCorrectCombinationAt(int idx)
        {
            return _correctCombinations[idx];
        }
    
        public override bool PerformCombination(string combination)
        {
            if(combination.Equals(GetCorrectCombinationAt(CurrentCombinationIndex)))
            {
                _currentCombinations.Push(combination);
                CurrentCombinationIndex++;
                
                if (CheckCompletion())
                {
                    //Fin Level
                }

                return true;
            }

            return false;
        }
    
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
    }
}
