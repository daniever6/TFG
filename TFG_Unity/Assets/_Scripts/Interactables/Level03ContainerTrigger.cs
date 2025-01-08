using _Scripts.LevelScripts.SaveManager;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Interactables
{
    public class Level03ContainerTrigger : Trigger
    {
        [SerializeField] private string levelName;

        private GameObject player = null;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Suelta la caja de residuos y abre la escena del nivel 3
        /// </summary>
        public override void TriggerEvent()
        {
            if(LevelManager.Instance.CurrentLevelState != LevelState.NivelResiduos) return;

            SaveManager.SaveGameData(player);

            Level03PlayerController.Instance.DropGarbage();
            SceneManager.LoadScene(levelName);
        }
    }
}