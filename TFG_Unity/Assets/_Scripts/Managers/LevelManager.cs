using System;
using System.Collections.Generic;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.UI;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Managers
{
    [System.Serializable]
    public class ComponentList<T> 
    {
        public List<T> List;
    }
    
    /// <summary>
    /// Esta clase controla los eventos y niveles a nivel global, a diferencia del GameManager,
    /// esta clase gestiona los niveles y el estado del juego durante el nivel activo
    ///
    /// LevelState indica el nivel activo
    /// </summary>
    public class LevelManager : GameplayMonoBehaviour<LevelManager>
    {
        [SerializeField] private List<ComponentList<GameObject>> levelComponents;
        [SerializeField] private List<ComponentList<MonoBehaviour>> levelScripts;
        
        private SaveData _saveData;
        private GameObject player;
        
        private LevelState _levelState = LevelState.None;
        public LevelState CurrentLevelState => _levelState;
        protected override void Awake()
        {
            base.Awake();
            // DontDestroyOnLoad(this);
            
            //Desactiva todos los componentes y scripts relacionados con los niveles
            for (int i = 0; i < 3; i++)
            {
                foreach (var component in levelComponents[i].List)
                {
                    component.SetActive(false);
                }
                foreach (var script in levelScripts[i].List)
                {
                    script.enabled = false;
                }
            }

            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Establece el estado inicial del juego
        /// </summary>
        private void Start()
        {
            _saveData = SaveManager.LoadGameData();

            if (_saveData != null)
            {
                ChangeLevelState(_saveData.levelState);
                 player.transform.position = new Vector3(_saveData.playerPosition[0], _saveData.playerPosition[1],
                                                        _saveData.playerPosition[2]);
            }
            else
            {
                ChangeLevelState(LevelState.ThirdLevel);
            }
        }

        /// <summary>
        /// Establece el estado del juego y maneja los eventos de cada estado
        /// </summary>
        /// <param name="newState">Estado nuevo</param>
        public void ChangeLevelState(LevelState newState)
        {
            if (newState == _levelState) return;
            
            _levelState = newState;

            switch (_levelState)
            {
                case LevelState.None:
                    break;
                
                case LevelState.Tutorial:
                    break;
                
                case LevelState.FirstLevel:
                    HandleFirstLevel();
                    break;
                
                case LevelState.SecondLevel:
                    HandleSecondLevel();
                    break;
                
                case LevelState.ThirdLevel:
                    HandleThirdLevel();
                    break;
            }
        }

        /// <summary>
        /// CONTEXTO:
        /// - El jugador tendra que interactuar con las mesas de trabajo para realizar los
        ///   reactivos correspondientes
        ///
        /// ACCIONES:
        /// - Desactivar scripts y componentes del nivel anterior
        /// - Al interactuar con las mesas de trabajo, no abrir niveles diferentes.
        /// - Llevar a cabo la elaboracion de reactivos y combinaciones.
        /// - Gestionar las combinaciones entre elementos
        /// - Finalizar el nivel una vez terminado
        /// 
        /// </summary>
        private void HandleFirstLevel()
        {
            //Activa los componentes y scripts del nivel 1
            foreach (var obj in levelComponents[0].List)
            {
                obj.SetActive(true);
            }

            foreach (var script in levelScripts[0].List)
            {
                script.enabled = true;
            }
            
            InfoCanvas.Instance.ShowMessage("- Prepara los reactivos en las mesas de trabajo.");
        }
        
        private void HandleSecondLevel()
        {
            
        }
        
        /// <summary>
        /// CONTEXTO:
        /// - El jugador tendra que ir recogiendo los residuos de las mesas de trabajo y
        ///   depositarlas en los contenedores correspondientes para pasarse el nivel.
        ///
        /// ACCIONES:
        /// - Desactivar scripts y componentes del nivel anterior
        /// - Mostrar iconos animados encima de las mesas de trabajo con residuos todavia.
        /// - Mostrar la direccion a los contenedores cuando transporta residuos.
        /// - Al interactuar con las mesas de trabajo, no abrir niveles diferentes.
        /// - Permitir al jugador coger residuos visualmente(Una caja mientras los transporta).
        /// - Cambiar entre primera y tercera persona cuando sea necesario.
        /// - Llevar a cabo la gestion de residuos.
        /// - Finalizar el nivel una vez terminado
        /// 
        /// </summary>
        private void HandleThirdLevel()
        {
            //Desactiva los compoenentes y scripts del nivel 2
            foreach (var obj in levelComponents[1].List)
            {
                obj.SetActive(false);
            }

            foreach (var script in levelScripts[1].List)
            {
                script.enabled = false;
            }
            
            //Activa los componentes y scripts del nivel 3
            foreach (var obj in levelComponents[2].List)
            {
                obj.SetActive(true);
            }

            foreach (var script in levelScripts[2].List)
            {
                script.enabled = true;
            }
            
            InfoCanvas.Instance.ShowMessage("- Recoge los residuos de las mesas de trabajo y tiralos en " +
                                          "sus contenedores correspondientes.");
        }
    }
}