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

        private void Start(){}

        /// <summary>
        /// Si el jugador coge los residuos de la mesa de trabajo, desactiva la UI de la mesa y destruye el script
        /// </summary>
        public override void TriggerEvent()
        {
            if (LevelManager.Instance.CurrentLevelState != LevelState.ThirdLevel) return;
            
            if (Level03PlayerController.Instance.GetGarbageFromTable())
            {
                puntoRecogidaUI.SetActive(false);
                Destroy(this);
            }
        }
    }
}