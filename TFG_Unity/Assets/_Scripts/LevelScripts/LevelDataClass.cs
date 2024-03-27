using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.LevelScripts
{
    [System.Serializable]
    public class LevelCombinations
    {
        public List<Combinations> LevelCombinationsList;
    }

    [System.Serializable]
    public class Combinations
    {
        public List<string> CombinationList;
    }
}