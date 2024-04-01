using System;
using System.Threading.Tasks;
using _Scripts.Interactables;
using _Scripts.LevelScripts;
using _Scripts.Managers;
using DG.Tweening;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts.Player
{
    /// <summary>
    /// Clase que maneja define las manos del personaje en los niveles del juego
    /// </summary>
    public class PlayerHand:MonoBehaviour
    {
        #region Definicion de la clase

        [SerializeField] private GameObject hand;
        [SerializeField] private PlayerHand otherHand;
        [SerializeField] private GameObject alfombrillaContainer;
        private Camera _camera;

        [SerializeField]private Vector3 handActionOffset;
        [SerializeField] private Vector3 handActionRotation;

        public Vector3 HandInitialPosition { get; private set; }

        public Vector3? ObjectInitialPosition => ObjectSelected.InitialPosition;

        public ObjectInteractable ObjectSelected { get; set; }

        private void Start()
        {
            HandInitialPosition = hand.transform.position;
            _camera = Camera.main;
        }

        #endregion
        
        #region Acciones de la mano

        /// <summary>
        /// Controla la accion de la mano una vez soltado sobre un objeto
        /// </summary>
        public async void OnMouseUp()
        {
            if (PlayerGrab.IsTweening) return;
            
            RaycastHit hit;

            Vector3 rayOrigin = hand.transform.position;
            Vector3 cameraPos = _camera.transform.position;
            Vector3 rayDirection = (rayOrigin-cameraPos).normalized;

            // Interaccion entre las dos manos
            if (Vector3.Distance(hand.transform.position, otherHand.transform.position) < 0.1f)
            {
                UseObjects(otherHand.ObjectSelected);
                if(otherHand.ObjectSelected.IsUnityNull() || ObjectSelected.IsUnityNull()) await GoToInitialPosition();
            }
            // Accion entre objetos interactables de la escena
            else if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                Enum.TryParse(hit.collider.tag, out parsedEnum);
                switch(parsedEnum)
                {
                    case Iteractables.Interactable:
                        if (ObjectSelected.IsUnityNull())
                        {
                            Grab(hit.collider.gameObject.GetComponent<ObjectInteractable>()); 
                            break;
                        }
                        
                        UseObjects(hit.collider.gameObject.GetComponent<ObjectInteractable>());
                        return;
                    
                    case Iteractables.Alfombrilla:
                        Vector3 newPos = alfombrillaContainer.transform.position;
                        newPos.y = ObjectInitialPosition.GetValueOrDefault().y;
                        DropObject(alfombrillaContainer, newPos);
                        break;
                }
                
                await GoToInitialPosition();
            }
        }

        /// <summary>
        /// Mueve la mano hacia el objeto y coloca el objeto como hijo de la mano
        /// </summary>
        /// <param name="objectToGrab">Objeto a coger</param>
        /// <param name="newParent">Gameobject padre en el que soltar el objeto</param>
        public void GrabObject(ObjectInteractable objectToGrab, GameObject newParent)
        {
            transform.DOMove(objectToGrab.transform.position, 1)
                .OnComplete(async () =>
                {
                    if (!ObjectSelected.IsUnityNull())
                    {
                        DropObject(newParent);
                    }
                    Grab(objectToGrab);
                
                    await GoToInitialPosition();
                });
        }

        /// <summary>
        /// Metodo para agarrar un objeto con la mano
        /// </summary>
        /// <param name="objectToGrab">Objeto a agarrar</param>
        public void Grab(ObjectInteractable objectToGrab)
        {
            ObjectSelected = objectToGrab;

            objectToGrab.transform.SetParent(transform);
            objectToGrab.transform.localPosition = Vector3.zero;
                
            objectToGrab.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        
        /// <summary>
        /// Devuelve la mano hacia hacia su posicion inicial
        /// </summary>
        public async Task GoToInitialPosition()
        {
            await transform.DOMove(HandInitialPosition, 1).AsyncWaitForCompletion();;
        }

        /// <summary>
        /// Lleva la logica del soltado de objetos. Deja el objetos de la mano en su posicion original
        /// y establece sus valores por defecto.
        /// </summary>
        /// <param name="newParent">Gameobject padre al que se le suelta el objeto</param>
        /// <param name="position"></param>
        public void DropObject(GameObject newParent, Vector3 position = default)
        {
            try
            {
                if (ObjectSelected.IsUnityNull()) return;

                Vector3 newPos = position.Equals(default) ? ObjectInitialPosition.GetValueOrDefault() : position;

                //Evita poner mas de 2 objetos en la alfombrilla
                if (newPos.Equals(position) && alfombrillaContainer.transform.childCount > 0) return; 
                
                var droppedObject = ObjectSelected;

                droppedObject.transform.position = newPos;
                droppedObject.transform.SetParent(newParent.transform);
                droppedObject.gameObject.layer = LayerMask.NameToLayer("Default");
            
                ObjectSelected = null;
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Realiza la animación de la interaccion de objetos
        /// </summary>
        /// <param name="secondaryObject">Objeto secundario</param>
        private Task UseObjectsAnimation(ObjectInteractable secondaryObject)
        {
            TaskCompletionSource<bool> animationTask = new TaskCompletionSource<bool>();
            
            Sequence animationSecuence = DOTween.Sequence();
            animationSecuence.Append(transform.DOMove(secondaryObject.transform.position + handActionOffset, 1.5f));
            animationSecuence.Append(transform.DORotate(handActionRotation, 2));
            animationSecuence.Append(transform.DORotate(Vector3.zero, 2));
            animationSecuence.Append(transform.DOMove(HandInitialPosition, 1.5f));
            animationSecuence.OnComplete(()=>
            {
                animationTask.SetResult(true);
                PlayerGrab.IsTweening = false;
            });
            
            animationSecuence.Play();

            return animationTask.Task;
        }
        
        /// <summary>
        /// Lleva la accion resultante de juntar el Objeto que sujeta la mano arrastrada (PrimaryObject), al
        /// interactuar con el otro objeto (secondaryObject)
        /// </summary>
        /// <param name="secondaryObject">Objeto secundario</param>
        private async void UseObjects(ObjectInteractable secondaryObject)
        {
            if (ObjectSelected.IsUnityNull() || secondaryObject.IsUnityNull() || PlayerGrab.IsTweening) return;
            PlayerGrab.IsTweening = true;
            
            string combinationName = ObjectSelected.name + "_" + secondaryObject.name;

            // Resultado de la accion
            CombinationResult result = CombinationsManager.Instance.GetCombinationResult(combinationName);

            switch (result)
            {
                case CombinationResult.None:
                    PlayerGrab.IsTweening = false;
                    await GoToInitialPosition();
                    break;
                case CombinationResult.Error:
                    Debug.Log("Error");
                    PlayerGrab.IsTweening = false;
                    await GoToInitialPosition();
                    break;
                case CombinationResult.Correct:
                    Debug.Log("Correcto");
                    var resul = ALevel.PerformCombinationCommand.Execute(combinationName);
                    if (resul)
                    {
                        await UseObjectsAnimation(secondaryObject);
                        LevelManager.Instance.PostPerformCombination();
                    }
                    else
                    {
                        PlayerGrab.IsTweening = false;
                        await GoToInitialPosition();
                    }
                    break;
                case CombinationResult.Explosion:
                    await UseObjectsAnimation(secondaryObject);
                    Debug.Log("Explosion");
                    break;
                case CombinationResult.Corrosion:
                    await UseObjectsAnimation(secondaryObject);
                    Debug.Log("Corrosion");
                    break;
            }
            
        }
        
        #endregion
    }
}