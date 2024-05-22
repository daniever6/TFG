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

        [SerializeField] private LevelTeacher _levelTeacher;
        
        [SerializeField] private GameObject hand;
        [SerializeField] private GameObject alfombrillaContainer;
        private Camera _camera;

        [SerializeField]private Vector3 handActionOffset;
        [SerializeField] private Vector3 handActionRotation;

        public Vector3 HandInitialPosition { get; private set; }

        public Vector3? ObjectInitialPosition => ObjectSelected.InitialPosition;

        public LevelInteractable ObjectSelected { get; set; }

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
            // if (Vector3.Distance(hand.transform.position, otherHand.transform.position) < 0.1f)
            // {
            //     //UseObjects(otherHand.ObjectSelected);
            //     //if(otherHand.ObjectSelected.IsUnityNull() || ObjectSelected.IsUnityNull()) await GoToInitialPosition();
            // }
            
            // Accion entre objetos interactables de la escena
            if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity))
            {
                Iteractables parsedEnum;
                Enum.TryParse(hit.collider.tag, out parsedEnum);
                switch(parsedEnum)
                {
                    case Iteractables.Interactable:
                        if (ObjectSelected.IsUnityNull())
                        {
                            Grab(hit.collider.gameObject.GetComponent<LevelInteractable>());
                        }
                        
                        //UseObjects(hit.collider.gameObject.GetComponent<LevelInteractable>()); Quitar await si se descomenta
                        await GoToInitialPosition();
                        return;
                    
                    case Iteractables.Alfombrilla:
                        bool alfombrillaIsEmpty = alfombrillaContainer.transform.childCount <= 0;
                        bool handHasObject = !ObjectSelected.IsUnityNull();

                        if (!alfombrillaIsEmpty && !handHasObject) //Coger objeto de alfombrilla
                        {
                            Grab(hit.collider.gameObject.GetComponent<LevelInteractable>());
                            await GoToInitialPosition();
                            break;
                        }
                        if (!alfombrillaIsEmpty && handHasObject) //Hacer combinacion
                        {
                            UseObjects(hit.collider.gameObject.GetComponent<LevelInteractable>());
                            break;
                        }

                        if (alfombrillaIsEmpty && !handHasObject) //Soltar en alfombrilla
                        {
                            await GoToInitialPosition();
                            break;
                        } 
                        
                        if (alfombrillaIsEmpty && handHasObject)//Dejar en alfombrilla
                        {
                            Vector3 newPos = alfombrillaContainer.transform.position;
                            newPos.y = ObjectInitialPosition.GetValueOrDefault().y;
                            ObjectSelected.gameObject.tag = "Alfombrilla";
                            DropObject(alfombrillaContainer, newPos);
                            await GoToInitialPosition();
                            break;
                        }
                        break;
                    
                    default:
                        await GoToInitialPosition();
                        break;
                }
            }
        }

        /// <summary>
        /// Mueve la mano hacia el objeto y coloca el objeto como hijo de la mano
        /// </summary>
        /// <param name="objectToGrab">Objeto a coger</param>
        /// <param name="newParent">Gameobject padre en el que soltar el objeto</param>
        public void GrabObject(LevelInteractable objectToGrab, GameObject newParent)
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
        public void Grab(LevelInteractable objectToGrab)
        {
            ObjectSelected = objectToGrab;

            var objectTransform = objectToGrab.transform;
            
            objectTransform.SetParent(transform);
            objectTransform.localPosition = Vector3.zero;

            if (ObjectSelected.name == "Pipeta")
            {
                objectTransform.localRotation = objectToGrab.GrabRotation;
                objectTransform.localPosition = objectToGrab.GrabLocalPos;
            }
            
            objectToGrab.gameObject.tag = "Interactable";    
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

                var droppedTransform = droppedObject.transform;
                
                droppedTransform.position = newPos;
                droppedTransform.localRotation = droppedObject.InitialRotation;
                droppedTransform.SetParent(newParent.transform);
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
        private Task UseObjectsAnimation(LevelInteractable secondaryObject)
        {
            TaskCompletionSource<bool> animationTask = new TaskCompletionSource<bool>();
            
            Sequence animationSecuence = DOTween.Sequence();
            if (ObjectSelected.name != "Pipeta")
            {
                animationSecuence.Append(transform.DOMove(secondaryObject.transform.position + handActionOffset, 1.5f));
                animationSecuence.Append(transform.DORotate(handActionRotation, 2));
                animationSecuence.Append(transform.DORotate(Vector3.zero, 2));
            }
            else
            {
                animationSecuence.Append(transform.DOMove(secondaryObject.transform.position + new Vector3(0,0.3f,0), 1.5f));
            }
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
        private async void UseObjects(LevelInteractable secondaryObject)
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
                    PlayerGrab.IsTweening = false;
                    await UseObjectsAnimation(secondaryObject);
                    await GoToInitialPosition();
                    Debug.Log("Error");
                    
                    _levelTeacher.GrabAndHide(secondaryObject);
                    
                    break;
                
                case CombinationResult.Correct:
                    Debug.Log("Correcto");
                    var resul = ALevel.PerformCombinationCommand.Execute(combinationName);
                    if (resul)
                    {
                        await UseObjectsAnimation(secondaryObject);
                        FirstPersonLevelManager.Instance.PostPerformCombination();
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