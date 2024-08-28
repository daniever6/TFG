using System;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.Managers;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Interactables
{
    public class Level01TableTrigger : Trigger
    {
        [SerializeField] private string levelName;
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Carga la escena indicada
        /// </summary>
        public override void TriggerEvent()
        {
            SaveManager.SaveGameData(_player);
            SceneManager.LoadScene(levelName);
        }
    }
}