using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Scripts.Interactables;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.UI;
using _Scripts.Utilities;
using Assets._Scripts.NPCs;
using JetBrains.Annotations;
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
        [SerializeField] private List<Level03TableTrigger> residuoTriggers;

        [SerializeField][CanBeNull] private BurnNPC burnNPC;
        [SerializeField][CanBeNull] private AcidNPC acidNPC;
        
        private SaveData _saveData;
        private GameObject player;
        
        private static LevelState _levelState = LevelState.NivelRecibidor;
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
                var levelState = _saveData.levelState;
                
                player.transform.position = new Vector3(_saveData.playerPosition[0], _saveData.playerPosition[1],
                                                        _saveData.playerPosition[2]);

                ResiduosDroppedManager.ResiduosDropped = _saveData.residuosTirados;

                ChangeLevelState(levelState);
            }
            else
            {
                ChangeLevelState(_levelState);
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
            _levelState = newState;

            switch (_levelState)
            {
                case LevelState.None:
                    break;
                
                case LevelState.NivelRecibidor:
                    break;
                
                case LevelState.NivelBases:
                    HandleNivelBases();
                    break;
                
                case LevelState.NivelBalanza:
                    HandleNivelBalanza();
                    break;

                case LevelState.NivelAcidos:
                    HandleNivelAcidos();
                    break;

                case LevelState.NivelResiduos:
                    HandleNivelResiduos();
                    break;

                case LevelState.NivelEmergencia:
                    HandleNivelEmergencia();
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
        private async void HandleNivelBases()
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
        private async void HandleNivelAcidos()
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

            await Task.Delay(4000);

            if (acidNPC != null)
            {
                acidNPC.StartAcid();
            }
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
        private async void HandleNivelBalanza()
        {
            //Desactiva los compoenentes y scripts del nivel 1
            foreach (var obj in levelComponents[1].List)
            {
                if(obj != null)
                    obj.SetActive(false);
            }

            foreach (var script in levelScripts[1].List)
            {
                if(script.isActiveAndEnabled)
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

            InfoCanvas.Instance.ShowMessage("- Busca al alumno 2 para ayudarle a hacer los cálculos necesarios " +
                                          "de los reactivos.");

            await Task.Delay(4000);
            
            if(burnNPC != null)
            {
                burnNPC.StartBurning();
            }
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
        /// - Guardar los residuos tirados con exito
        /// - Desactivar los residuos que ya han sido desechados
        /// 
        /// </summary>
        private void HandleNivelResiduos()
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

            _saveData = SaveManager.LoadGameData();
            ResiduosDroppedManager.ResiduosDropped = _saveData?.residuosTirados;

            // Desactiva los residuos ya tirados
            foreach(var residuo in residuoTriggers)
            {
                if (ResiduosDroppedManager.ResiduosDropped[residuo.ResiduoIdx])
                {
                    residuo.DesactivateComponent(); //Si ya ha sido tirado se desactiva
                }
            }

            if (_saveData.residuosTirados.Any(x => true))
            {
                ChangeLevelState(LevelState.NivelEmergencia);
                return;
            }

            InfoCanvas.Instance.ShowMessage("- Recoge los residuos de las mesas de trabajo y tiralos en " +
                                          "sus contenedores correspondientes.");
        }
        
        /// <summary>
        /// CONTEXTO:
        /// - Surge una emergencia en el laboratorio que hace que los personajes tengan
        ///   que huir por la puerta de salida de emergencia del laboratorio
        ///
        /// ACCIONES:
        /// - Desactivar scripts y componentes del nivel anterior
        /// - Activar los scripts y componentes de este nivel
        /// - Activar el dialogo de emergencia
        /// - Evacuar a los NPCs
        /// - Finalizar el juego al terminar
        /// 
        /// </summary>
        private void HandleNivelEmergencia()
        {
            //Desactiva los compoenentes y scripts del nivel de residuos
            foreach (var obj in levelComponents[3].List)
            {
                obj?.SetActive(false);
            }

            foreach (var script in levelScripts[3].List)
            {
                if (script == null)
                {
                    continue;
                }
                script.enabled = false;
            }
            
            //Activa los componentes y scripts del nivel de emergencia
            foreach (var obj in levelComponents[4].List)
            {
                obj?.SetActive(true);
            }

            foreach (var script in levelScripts[4].List)
            {
                if (script == null)
                {
                    continue;
                }
                script.enabled = true;
            }

            EmergencyManager.Instance.StartEmergency();
            
            InfoCanvas.Instance.ShowMessage("- Evacua el laboratorio por la puerta de emergencia.");
        }
    }
}