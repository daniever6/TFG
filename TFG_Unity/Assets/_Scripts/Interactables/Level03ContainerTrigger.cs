using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class Level03ContainerTrigger : Trigger
    {
        [SerializeField] private string levelName;

        private void Start() {}

        /// <summary>
        /// Suelta la caja de residuos y abre la escena del nivel 3
        /// </summary>
        public override void TriggerEvent()
        {
            if(LevelManager.Instance.GetLevelState != LevelState.ThirdLevel) return;
            
            Level03PlayerController.Instance.DropGarbage();
            //SceneManager.LoadScene(levelName);
        }
    }
}