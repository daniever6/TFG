using System;
using _Scripts.Dialogues;
using _Scripts.Interactables;
using _Scripts.Managers;
using _Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.LevelScripts
{
    public class LevelTeacher : GameplayMonoBehaviour
    {
        [SerializeField] private LevelInteractable objectGrabbed = null;
        [SerializeField] private Transform teacherHandPosition;
        [SerializeField] private DialogueTrigger dialogueTrigger;
        [SerializeField] private GameObject interactablesParent;

        private bool _hasGrabbedObject = false;
        private Camera _camera;
        private Vector3 _cameraInitialRotation;

        protected override void Awake()
        {
            base.Awake();
            
            _camera = Camera.main;
            _cameraInitialRotation = _camera.transform.rotation.eulerAngles;
            
            DialogueManager.OnDialogueFinish += ReplaceObject;
        }
        
        private void OnDestroy()
        {
            DialogueManager.OnDialogueFinish -= ReplaceObject;
        }

        /// <summary>
        /// El profesor agarra el objeto de la alfombrilla y lo sustituye por uno nuevo
        /// </summary>
        /// <param name="interactable">Objeto de la alfombrilla</param>
        public async void GrabAndHide(LevelInteractable interactable)
        {
            _hasGrabbedObject = true;
            objectGrabbed = interactable;
            objectGrabbed.transform.parent = interactablesParent.transform;
            
            _camera.transform.DORotate(Quaternion.Euler(0, -90, 0).eulerAngles, 1.5f);
            
            await interactable.transform.DOMove(teacherHandPosition.position, 2)
                .OnComplete(() => objectGrabbed.gameObject.SetActive(false)).AsyncWaitForCompletion();
            
            dialogueTrigger.TriggerEvent();
        }

        /// <summary>
        /// El profesor coloca nuevamente el objeto en la mesa
        /// </summary>
        private void ReplaceObject()
        {
            if (!_hasGrabbedObject) return;
            
            _hasGrabbedObject = false;
            objectGrabbed.gameObject.SetActive(true);

            var cameraRotation = _camera.transform.rotation;
            
            _camera.transform.DORotate(_cameraInitialRotation, 1.5f);
            objectGrabbed.transform.DOMove(objectGrabbed.InitialPosition, 2);
            
            objectGrabbed = null;
        }
    }
}
