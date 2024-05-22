using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Interactables
{
    public class InteractableTrigger : Trigger
    {
        [SerializeField] private string levelName;
        public override void TriggerEvent()
        {
            SceneManager.LoadScene(levelName);
        }
    }
}