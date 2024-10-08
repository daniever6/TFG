using System;
using System.Threading.Tasks;
using _Scripts.LevelScripts.Level_01;
using _Scripts.Managers;
using _Scripts.Utilities;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.LevelScripts.Level_03
{
    public class InteractableLevel03 : MonoBehaviour
    {
        public static event Action OnReactivoCorrectDropped;
        
        [SerializeField] private string reactivoName;
        
        private TextMeshProUGUI _reactivoName;
        private Vector3 _initialPosition;
        private static BidonOnHover _lastContainerInteracted;
        private string _lastContainerName = "";
        
        void Start()
        {
            _initialPosition = transform.position;
            name = reactivoName;
        }

        public void SetTextMeshPro(TextMeshProUGUI textMeshPro)
        {
            _reactivoName = textMeshPro;
        }

        private void OnMouseUp()
        {
            _reactivoName.text = String.Empty;

            RaycastHit hit;

            Vector3 rayOrigin = transform.position;
            Vector3 cameraPos = Camera.main!.transform.position;
            Vector3 rayDirection = (rayOrigin-cameraPos).normalized;
            
            // Accion entre objetos interactables de la escena
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                Enum.TryParse(hit.collider.tag, out parsedEnum);
                switch (parsedEnum)
                {
                    case Iteractables.Contenedor:
                        transform.DOMove(hit.transform.position + Vector3.up, 2)
                                 .SetEase(Ease.InOutSine)
                                 .OnComplete(DropOnContainer);
                        break;
                    
                    default:
                        GoToInitialPos();
                        break;
                }
            }
            else
            {
                GoToInitialPos();
            }
        }

        /// <summary>
        /// Una vez que se haya tirado el objeto en el contenedor, lo cierra y destruye el objeto
        /// </summary>
        private async void DropOnContainer()
        {
            string combination = $"{name}_{_lastContainerName}";
            CombinationResult result = CombinationsManager.Instance.GetCombinationResult(combination);

            switch (result)
            {
                case CombinationResult.Correct:
                    OnReactivoCorrectDropped?.Invoke();
                    break;
                
                default:
                    Transform ParticlePos = _lastContainerInteracted.transform;
                    
                    ParticleEffectManager.Instance.InstantiateParticleInPos("Fuego", ParticlePos);
                    ParticleEffectManager.Instance.InstantiateParticleInPos("Explosion", ParticlePos);
                    _lastContainerInteracted.CloseContenedor();
                    Destroy(this.gameObject);
                    await Task.Delay(2000);
                    DeathInvoker.Instance.KillAnimation(GameLevels.Level3, "El residuo ha reaccionado y explotado", 2f);
                    return;
            }
            
            _lastContainerInteracted.CloseContenedor();
            _lastContainerInteracted = null;
            Destroy(this.gameObject);
        }
        
        /// <summary>
        /// Cuando el jugador este arrastrando este gameobject, este metodo lanza un raycast que si colisiona
        /// con un contenedor, abrirá su tapa o la cerrará
        /// </summary>
        private void OnMouseDrag()
        {
            RaycastHit hit;

            Vector3 rayOrigin = transform.position;
            Vector3 cameraPos = Camera.main!.transform.position;
            Vector3 rayDirection = (rayOrigin-cameraPos).normalized;

            _reactivoName.text = this.name;
            
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                Enum.TryParse(hit.collider.tag, out parsedEnum);

                // Si no es contenedor cierra el ultimo abierto
                if (parsedEnum != Iteractables.Contenedor)
                {
                    _lastContainerName = "";
                    if (_lastContainerInteracted.IsUnityNull()) return;
                    _lastContainerInteracted.CloseContenedor();
                    return;
                }
                
                if (hit.collider.name == _lastContainerName) return;
                
                _lastContainerName = hit.collider.name;

                // Coge el nuevo bidon onHover
                BidonOnHover bidon = hit.collider.GetComponent<BidonOnHover>();

                if (_lastContainerInteracted.IsUnityNull())
                {
                    _lastContainerInteracted = bidon;
                }
                else
                {
                    // Si ya se habia abierto un contenedor, se cierra
                    _lastContainerInteracted.CloseContenedor();
                    _lastContainerInteracted = bidon;
                }
                
                bidon.OpenContenedor();
            }
        }

        /// <summary>
        /// Metodo que mueve el objeto a su posicion inicial antes de haberle cogido
        /// </summary>
        private void GoToInitialPos()
        {
            transform.DOMove(_initialPosition, 1).SetEase(Ease.InOutSine);
        }

    }
}
