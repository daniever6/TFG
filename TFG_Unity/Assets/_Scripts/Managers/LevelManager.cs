using System;
using _Scripts.UI;
using _Scripts.Utilities;

namespace _Scripts.Managers
{
    public enum LevelState
    {
        None,
        Tutorial,
        FistLevel,
        SecondLevel,
        ThirdLevel
    }
    
    /// <summary>
    /// Esta clase controla los eventos y niveles a nivel global, a diferencia del GameManager,
    /// esta clase gestiona los niveles y el estado del juego durante el nivel activo
    ///
    /// LevelState indica el nivel activo
    /// </summary>
    public class LevelManager : GameplayMonoBehaviour
    {
        private LevelState _levelState = LevelState.None;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            ChangeLevelState(LevelState.ThirdLevel);
        }

        public void ChangeLevelState(LevelState newState)
        {
            _levelState = newState;

            switch (_levelState)
            {
                case LevelState.None:
                    break;
                
                case LevelState.Tutorial:
                    break;
                
                case LevelState.FistLevel:
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

        private void HandleFirstLevel()
        {
            
        }
        
        private void HandleSecondLevel()
        {
            
        }
        
        /// <summary>
        /// CONTEXTO:
        /// - El jugador tendra que ir recogiendo los residuos de las mesas de trabajo y
        ///   depositarlas en los contenedores correspondientes para pasarse el nivel.
        ///
        /// ACCIONES
        /// - Mostrar iconos animados encima de las mesas de trabajo con residuos todavia.
        /// - Mostrar la direccion a los contenedores cuando transporta residuos.
        /// - Permitir al jugador coger residuos visualmente(Una caja mientras los transporta).
        /// - Cambiar entre primera y tercera persona cuando sea necesario.
        /// - Llevar a cabo la gestion de residuos.
        /// 
        /// </summary>
        private void HandleThirdLevel()
        {
            InfoCanvas.Instance.ShowMessage("- Recoge los residuos de las mesas de trabajo y tiralos en " +
                                          "sus contenedores correspondientes.");
        }
    }
}