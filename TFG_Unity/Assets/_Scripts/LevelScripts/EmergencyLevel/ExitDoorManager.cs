using _Scripts.Managers;
using UnityEngine;

public class ExitDoorManager : MonoBehaviour
{
    /// <summary>
    /// Cuando el Npc llegue a la puerta de salida destruye el Gameobject
    /// para dar la sensacion de que ha salido por la puerta
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(LevelManager.Instance.CurrentLevelState != _Scripts.Utilities.LevelState.NivelEmergencia)
        {
            return;
        }

        if(other.gameObject.tag == "Npc")
        {
            Destroy(other.gameObject);
        }
    }
}
