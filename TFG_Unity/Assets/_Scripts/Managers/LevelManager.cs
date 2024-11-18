using System;
using System.Collections.Generic;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.UI;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public class LevelManager : Singleton<LevelManager>
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

            DeactivateAllComponents();

            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Desactiva todos los componentes y scripts relacionados con los niveles
        /// </summary>
        private void DeactivateAllComponents()
        {
            for (int i = 0; i < levelComponents.Count; i++)
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
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
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
                ChangeLevelState(LevelState.FirstLevel);
            }
        }

        /// <summary>
        /// Cambia el la escena del nivel cambiando a su vez el levelState
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="levelState"></param>
        /// <param name="changeState"></param>
        public void LoadNewScene(string sceneName, LevelState levelState, bool changeState = true)
        {
            DeactivateAllComponents();

            if (changeState)
            {
                ChangeLevelState(levelState);
            }

            SceneManager.LoadScene(sceneName);
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

                case LevelState.FirstLevelPart2:
                    HandleFirstLevelPart2();
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

        /// <summary>
        /// CONTEXTO:
        /// - El jugador tendra que interactuar con la extractora para realizar los
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
        private void HandleFirstLevelPart2()
        {
            //Desactiva los compoenentes y scripts del nivel 1
            foreach (var obj in levelComponents[0].List)
            {
                obj.SetActive(false);
            }

            foreach (var script in levelScripts[0].List)
            {
                script.enabled = false;
            }

            //Activa los componentes y scripts del nivel 2
            foreach (var obj in levelComponents[1].List)
            {
                obj.SetActive(true);
            }

            foreach (var script in levelScripts[1].List)
            {
                script.enabled = true;
            }

            InfoCanvas.Instance.ShowMessage("- Usa la extractora para usar hacer un reactivo nuevo.");
        }
        
        /// <summary>
        /// CONTEXTO:
        /// - El jugador tendrá que hablar con un NPC para ayudarle a hacer los calculos necesarios
        ///   para el correcto uso de la balanza
        ///   
        /// ACCIONES:
        /// - Desactivar scripts y componentes del nivel anterior
        /// - Activar al NPC para poder ayudarle a hacer los cálculos
        /// </summary>
        private void HandleSecondLevel()
        {
            //Desactiva los compoenentes y scripts del nivel 1
            foreach (var obj in levelComponents[1].List)
            {
                obj.SetActive(false);
            }

            foreach (var script in levelScripts[1].List)
            {
                script.enabled = false;
            }

            //Activa los componentes y scripts del nivel 2
            foreach (var obj in levelComponents[2].List)
            {
                obj.SetActive(true);
            }

            foreach (var script in levelScripts[2].List)
            {
                script.enabled = true;
            }

            InfoCanvas.Instance.ShowMessage("- Busca a NPC1 para ayudarle a hacer los cálculos necesarios " +
                                          "de los reactivos.");
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
            foreach (var obj in levelComponents[2].List)
            {
                obj.SetActive(false);
            }

            foreach (var script in levelScripts[2].List)
            {
                script.enabled = false;
            }
            
            //Activa los componentes y scripts del nivel 3
            foreach (var obj in levelComponents[3].List)
            {
                obj.SetActive(true);
            }

            foreach (var script in levelScripts[3].List)
            {
                script.enabled = true;
            }
            
            InfoCanvas.Instance.ShowMessage("- Recoge los residuos de las mesas de trabajo y tiralos en " +
                                          "sus contenedores correspondientes.");
        }
    }
}