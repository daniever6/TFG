using System;
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

        private void Start()
        {
        }

        public override void TriggerEvent()
        {
            if(LevelManager.Instance.GetLevelState != LevelState.ThirdLevel) return;
            
            Level03PlayerController.Instance.DropGarbage();
            //SceneManager.LoadScene(levelName);
        }
    }
}