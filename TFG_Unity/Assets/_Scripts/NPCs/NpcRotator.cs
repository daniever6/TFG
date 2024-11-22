using System;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.NPCs
{
    /// <summary>
    /// Esta clase se encarga de girar al NPC hacia el jugador, cuando habla con el
    /// </summary>
    public class NpcRotator : GameplayMonoBehaviour<NpcRotator>
    {
        private GameObject _player; //Referencia del jugador
        private Vector3 _initialRotation; //Rotacion de reposo
        private Animator animator;

        public static event Action OnRotateToStartPos;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");

            if(TryGetComponent<Animator>(out var anim))
            {
                animator = anim;
            }

            var startRotation = transform.rotation.eulerAngles;
            _initialRotation = new Vector3(startRotation.x, startRotation.y, startRotation.z);
        }

        /// <summary>
        /// Gira el personaje hacia el jugador player
        /// </summary>
        public void RotateToPlayer()
        {
            if (!animator.IsUnityNull() && animator.HasState(0, Animator.StringToHash("Talk")))
            {
                animator.Play("Talk");
            }

            Vector3 directionToPlayer = _player.transform.position - transform.position;
            directionToPlayer.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            transform.DORotate(lookRotation.eulerAngles,1);
        }

        /// <summary>
        /// Rota hacia su posicion inicial de reposo
        /// </summary>
        public void RotateToInitial()
        {
            if (!animator.IsUnityNull() && animator.HasState(0, Animator.StringToHash("Idle")))
            {
                animator.Play("Idle");
            }

            transform.DORotate(_initialRotation, 1, RotateMode.Fast);
            OnRotateToStartPos?.Invoke();
        }

        /// <summary>
        /// Si se pausa el juego, el npc volverá a mirar hacia su posicion
        /// inicial y desactivara el canvas
        /// </summary>
        protected override void OnPostPaused()
        {
            base.OnPostPaused();

            if(GameManager.GameState == GameState.Pause)
            {
                animator.speed = 0f;
            }
            else if(GameManager.GameState == GameState.Dialogue)
            {
                animator.speed = 1f;
            }

            PlayerController.Instance.enabled = true;
        }

        /// <summary>
        /// Si se pausa el juego, el npc volverá a mirar hacia su posicion
        /// inicial y desactivara el canvas
        /// </summary>
        protected override void OnPostResumed()
        {
            base.OnPostResumed();

            if(GameManager.GameState != GameState.Pause)
            {
                animator.speed = 1f;
            }

            PlayerController.Instance.enabled = true;
        }
    }
}
