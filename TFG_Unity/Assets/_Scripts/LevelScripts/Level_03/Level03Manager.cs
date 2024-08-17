using System;
using System.Collections.Generic;
using _Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace _Scripts.LevelScripts.Level_03
{
    public class Level03Manager : Singleton<Level03Manager>
    {
        [SerializeField] private Vector3[] reactivosInitalPositions; //Posiciones por defecto en la caja
        [SerializeField] private List<GameObject> reactivos; //Prefabs de los reactivos a tirar
        [SerializeField] private GameObject interactablesParent;
        [SerializeField] private TextMeshProUGUI reactivoText;
        private int _reactivosCount = 3;

        private void Start()
        {
            Random random = new Random();
            foreach (var position in reactivosInitalPositions)
            {
                int randIdx = random.Next(0, reactivos.Count);
                var reactivo = Instantiate(reactivos[randIdx], position, Quaternion.identity, interactablesParent.transform);
                reactivo.GetComponent<InteractableLevel03>().SetTextMeshPro(reactivoText);
                try
                {
                    reactivos.RemoveAt(randIdx);
                }
                catch (Exception ex)
                {
                    
                }
            }

            InteractableLevel03.OnReactivoCorrectDropped += OnReactivoDropped;
        }

        /// <summary>
        /// Al tirar todos los reactivos, se vuelve al nivel del laboratorio
        /// </summary>
        private void OnReactivoDropped()
        {
            _reactivosCount--;
            if (_reactivosCount == 0)
            {
                InteractableLevel03.OnReactivoCorrectDropped -= OnReactivoDropped;
                SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
            }
        }
    }
}
