using _Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Scripts.DeathScene
{
    public class DeathSceneManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deathReason;
        
        void Start()
        {
            deathReason.text = GameManager.PlayerDeathCause.DeathReason;
        }
    }
}
