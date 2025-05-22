using _Scripts.LevelScripts.SaveManager;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace _Scripts.LevelScripts.Level_03
{
    public class Level03Manager : MonoBehaviour
    {
        [SerializeField] private Vector3[] reactivosInitalPositions; //Posiciones por defecto en la caja
        [SerializeField] private List<GameObject> reactivos; //Prefabs de los reactivos a tirar
        [SerializeField] private GameObject interactablesParent;
        [SerializeField] private TextMeshProUGUI reactivoText;
        private int _reactivosCount = 3;

        private void OnEnable()
        {
            _reactivosCount = 3;
            InteractableLevel03.OnReactivoCorrectDropped += OnReactivoDropped;
        }

        private void OnDisable()
        {
            InteractableLevel03.OnReactivoCorrectDropped -= OnReactivoDropped;
        }

        private void Start()
        {
            LevelDBManager.OnStartTimeCount?.Invoke();

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

        }

        /// <summary>
        /// Al tirar todos los reactivos, se vuelve al nivel del laboratorio
        /// </summary>
        private void OnReactivoDropped()
        {
            _reactivosCount--;
            if (_reactivosCount == 0)
            {
                ResiduosDroppedManager.ResiduosDropped[ResiduosDroppedManager.CurrentResiduoIdx] = true;
                SaveManager.SaveManager.SaveResiduosData();

                var _saveData = SaveManager.SaveManager.LoadGameData();
                ResiduosDroppedManager.ResiduosDropped = _saveData?.residuosTirados;

                int residuosTirados = 0;
                foreach (var residuoTirado in _saveData.residuosTirados)
                {
                    if (residuoTirado == true)
                    {
                        residuosTirados++;
                    }
                }

                if(residuosTirados == 3)
                {
                    LevelDBManager.OnLevelCompleted?.Invoke();
                }

                SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
            }
        }
    }
}
