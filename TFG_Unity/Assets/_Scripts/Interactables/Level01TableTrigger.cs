using System;
using _Scripts.Managers;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Interactables
{
    public class Level01TableTrigger : Trigger
    {
        [SerializeField] private string levelName;

        private void Start()
        {
        }

        /// <summary>
        /// Carga la escena indicada
        /// </summary>
        public override void TriggerEvent()
        {
            if(LevelManager.Instance.GetLevelState != LevelState.FistLevel) return;
            
            SceneManager.LoadScene(levelName);
        }
    }
}