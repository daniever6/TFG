using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace _Scripts.Interactables
{
    public class InteractableTrigger : Trigger
    {
        [SerializeField] private string _levelName;
        public override void TriggerEvent()
        {
            //Guardar
            
            //Cambiar de escena
            SceneManager.LoadScene(_levelName);
        }
    }
}