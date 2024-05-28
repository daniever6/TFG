using System;
using _Scripts.LevelScripts;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Interactables
{
    public class Level03TableTrigger : Trigger
    {
        [SerializeField] private GameObject puntoRecogidaUI;

        private void Start()
        {
        }

        public override void TriggerEvent()
        {
            var a = LevelManager.Instance;
            if (LevelManager.Instance.GetLevelState != LevelState.ThirdLevel) return;
            
            if (Level03PlayerController.Instance.GetGarbageFromTable())
            {
                puntoRecogidaUI.SetActive(false);
                this.enabled = false;
            }
        }
    }
}