using System;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.Managers;
using _Scripts.Utilities;
using Assets._Scripts.NPCs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Interactables
{
    public class ChangeSceneTrigger : Trigger
    {
        [SerializeField] private string levelName;
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Carga la escena indicada
        /// </summary>
        public override void TriggerEvent()
        {
            if(BurnNPC.isActivated || AcidNPC.isActivate)
            {
                return;
            }

            SaveManager.SaveGameData(_player);
            SceneManager.LoadScene(levelName);
        }
    }
}