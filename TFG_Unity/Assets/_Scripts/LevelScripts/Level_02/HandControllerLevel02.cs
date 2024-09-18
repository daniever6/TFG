using System;
using System.Collections.Generic;
using _Scripts.Interactables;
using _Scripts.Utilities;
using _Scripts.Player;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;
using Task = System.Threading.Tasks.Task;

namespace _Scripts.LevelScripts.Level_02
{
    public class HandControllerLevel02: MonoBehaviour, IHand
    {
        [SerializeField] private GameObject BalanzaSetPosition;
        [SerializeField] private Vector3 PosInteraccionPapelPesaje;
        [SerializeField] private Vector3 PosInteraccionBase;
        
        private Vector3 initialPosition;
        private bool isTweening = false;
        private static LevelInteractable ObjectSelected;

        

        public Vector3 HandInitialPosition
        {
            get => initialPosition;
        }

        private void Start()
        {
            initialPosition = transform.position;
        }

        /// <summary>
        /// Controla la accion de la mano una vez soltado sobre un objeto
        /// </summary>
        public async void OnMouseUp()
        {
            if (PlayerGrab.IsTweening) return;
            
            RaycastHit hit;

            Vector3 rayOrigin = transform.position;
            Vector3 cameraPos = Camera.main.transform.position;
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
                            await GoToInitialPosition();
                        }
                        else
                        {
                            EspatulaCombinations(ObjectSelected.gameObject, hit.collider.gameObject);
                        }
                        
                        return;
                    
                    case Iteractables.Alfombrilla: //Balanza
                        bool alfombrillaIsEmpty = BalanzaSetPosition.transform.childCount <= 0;
                        bool handHasObject = !ObjectSelected.IsUnityNull();

                        if (!alfombrillaIsEmpty && !handHasObject) //Coger objeto de balanza
                        {
                            Grab(hit.collider.gameObject.GetComponent<LevelInteractable>());
                            BalanzaManager.Instance.SetPesoBalanza(0);
                            await GoToInitialPosition();
                            break;
                        }
                        
                        if (!alfombrillaIsEmpty && handHasObject) //Hacer combinacion
                        {
                            EspatulaCombinations(ObjectSelected.gameObject, hit.collider.gameObject);
                            break;
                        }

                        if (alfombrillaIsEmpty && !handHasObject) //Soltar en alfombrilla
                        {
                            await GoToInitialPosition();
                            break;
                        } 
                        
                        if (alfombrillaIsEmpty && handHasObject)//Dejar en balanza
                        {
                            if (BalanzaManager.IsBalanzaOpen)
                            {
                                Vector3 newPos = BalanzaSetPosition.transform.position;
                                ObjectSelected.gameObject.tag = "Alfombrilla";
                                
                                //Establecer peso de la balanza
                                ObjectSelected.gameObject.TryGetComponent(out PesoObjetos peso);
                                if (!peso.IsUnityNull())
                                {
                                    BalanzaManager.Instance.SetPesoBalanza(peso.GetPesoTotal());
                                }
                                
                                DropObject(BalanzaSetPosition, newPos);
                            }
                            
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
        /// Devuelve la mano hacia hacia su posicion inicial
        /// </summary>
        public async Task GoToInitialPosition()
        {
            await transform.DOMove(HandInitialPosition, 1).AsyncWaitForCompletion();;
        }

        /// <summary>
        /// Metodo para hacer una animacion para coger el objeto con la mano
        /// </summary>
        /// <param name="objectToGrab">Objeto a coger</param>
        /// <param name="newParent">Nuevo padre del interactable</param>
        public void GrabObject(LevelInteractable objectToGrab, GameObject newParent)
        {
            if (objectToGrab.IsUnityNull())
            {
                return;
            }
            
            transform.DOMove(objectToGrab.gameObject.GetComponent<LevelInteractable>().GrabPos, 1)
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
        /// Coge un objeto con la mano fisicamente
        /// </summary>
        /// <param name="objectToGrab">Objeto a coger</param>
        public void Grab(LevelInteractable objectToGrab)
        {
            if (objectToGrab.IsUnityNull())
            {
                return;
            }
            
            ObjectSelected = objectToGrab;

            var objectTransform = objectToGrab.transform;
            
            objectTransform.SetParent(transform);
            objectTransform.localPosition = objectToGrab.GrabLocalPosition;
            
            objectToGrab.gameObject.tag = "Interactable";    
            objectToGrab.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

            if (BalanzaSetPosition.transform.childCount <= 0 && BalanzaManager.IsBalanzaReady)
            {
                BalanzaManager.Instance.SetPesoBalanza(0);
            }
        }

        /// <summary>
        /// Suelta un objeto en la posicion inicial o en un posicion indicada
        /// </summary>
        /// <param name="newParent">Nuevo gameobject padre de la jerarquia</param>
        /// <param name="position">Posicion en la que colocarlo</param>
        public void DropObject(GameObject newParent, Vector3 position = default)
        {
            try
            {
                if (ObjectSelected.IsUnityNull()) return;

                Vector3 newPos = position.Equals(default) ? ObjectSelected.InitialPosition : position;

                //Evita poner mas de 2 objetos dentro de la balanza
                if (newPos.Equals(position) && BalanzaSetPosition.transform.childCount > 0) return; 
                
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
        /// Maneja las interacciones entre los objetos de la escena usando la Espatula como herramienta
        /// </summary>
        /// <param name="objectGrabbed">Objeto que tenemos en la mano</param>
        /// <param name="secondaryObject">Objeto con el que interactuamos</param>
        private void EspatulaCombinations(GameObject objectGrabbed, GameObject secondaryObject)
        {
            //Si no sujetamos la espatula no hacemos nada
            if(objectGrabbed.name != "Espatula") return;
            
            //Si esta cerrada no puede añadir o quitar pesos
            if(!BalanzaManager.IsBalanzaOpen) return;

            //Si el papel de pesaje no esta colocado en la balanza
            if (BalanzaSetPosition.transform.childCount <= 0 ||
                BalanzaSetPosition.transform.GetChild(0).gameObject.name != "PapelPesaje")
            {
                return;
            }

            Enum.TryParse(secondaryObject.name, out Level02Interactables resul);
            switch (resul)
            {
                case Level02Interactables.None:
                    break;
                case Level02Interactables.Base:
                    //Añadir peso al papel de pesaje
                    EspatulaAnimation(PosInteraccionBase, PosInteraccionPapelPesaje, 0.25f);
                    break;
                case Level02Interactables.Espatula:
                    break;
                case Level02Interactables.PapelPesaje:
                    //Retirar peso del papel del pesaje
                    EspatulaAnimation(PosInteraccionPapelPesaje, PosInteraccionBase, -0.25f);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Logica de la espatula con los interactables.
        /// Se mueve a la posicion 1, luego a la 2, y luego le suma el peso "acumPeso" a la balanza
        /// </summary>
        /// <param name="position1">Posicion del primer interactable</param>
        /// <param name="position2">Posicion del segundo interactable</param>
        /// <param name="acumPeso">Peso a acumular</param>
        private async Task EspatulaAnimation(Vector3 position1, Vector3 position2, float acumPeso)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOLocalMove(position1, 2));
            mySequence.AppendInterval(2f);
            mySequence.Append(transform.DOLocalMove(position2, 2));
            mySequence.Play();

            mySequence.OnComplete(async () =>
            {
                BalanzaManager.Instance.AddSubPeso(acumPeso);
                await GoToInitialPosition();
            });
        }
    }
}