using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Player
{
    public class Level03PlayerController : GameplayMonoBehaviour<Level03PlayerController>
    {
        [SerializeField] private GameObject levelComponents;
        [SerializeField] private GameObject indicator;
        private bool _hasGarbage = false;

        /// <summary>
        /// Coge la caja de residuos de la mesa de trabajo para cargarla hacia los contenedores
        /// </summary>
        /// <returns>True si coge una caja y false si no</returns>
        public bool GetGarbageFromTable()
        {
            if (_hasGarbage) return false;
            
            levelComponents.SetActive(true);
            indicator.SetActive(true);
            _hasGarbage = true;
            return true;
        }

        public void DropGarbage()
        {
            if(!_hasGarbage) return;
            
            levelComponents.SetActive(false);
            indicator.SetActive(false);
            _hasGarbage = false;
        }
    }
}