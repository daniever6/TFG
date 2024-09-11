using System;
using _Scripts.Player;
using _Scripts.Utilities;
using DG.Tweening;
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

        public static event Action OnRotateToStartPos;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            
            var startRotation = transform.rotation.eulerAngles;
            _initialRotation = new Vector3(startRotation.x, startRotation.y, startRotation.z);
        }

        /// <summary>
        /// Gira el personaje hacia el jugador player
        /// </summary>
        public void RotateToPlayer()
        {
            Vector3 directionToPlayer = _player.transform.position - transform.position;
            directionToPlayer.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            transform.DORotate(lookRotation.eulerAngles, 1, RotateMode.Fast);
        }

        /// <summary>
        /// Rota hacia su posicion inicial de reposo
        /// </summary>
        public void RotateToInitial()
        {
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

            PlayerController.Instance.enabled = true;
            
            RotateToInitial();
        }
    }
}
