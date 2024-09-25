using System;
using System.Threading;
using System.Threading.Tasks;
using _Scripts.Dialogues;
using _Scripts.Managers;
using _Scripts.Utilities;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace _Scripts.LevelScripts.Level_02
{
    public class CheckResults : GameplayMonoBehaviour<CheckResults>
    {
        [SerializeField] private GameObject papelPesaje;
        [SerializeField] private ReactivoPesaje _reactivoPesaje;
        [SerializeField] private CinemachineVirtualCamera npcCamera;
        [SerializeField] private OpenBalanza balanzaAnimator;
        [SerializeField] private Vector3 papelPesajeNewPos;
        [SerializeField] private DialogueTrigger npcDialogue;

        private bool HasBeenPaused = false;
        private bool HasBeenPlayed = false;
        private Vector3 initialRotation;
        private Sequence mySequence;

        private void Start()
        {
            initialRotation = papelPesaje.transform.rotation.eulerAngles;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueManager.OnDialogueFinish -= NpcMakeCombination;
        }

        /// <summary>
        /// Metodo para llamar a entregar el pesaje
        /// </summary>
        public void EntregarPesaje()
        {
            DialogueManager.OnDialogueFinish += NpcMakeCombination;
            
            if (!BalanzaManager.IsBalanzaOpen && papelPesaje.transform.parent.name == "PositionPesa")
            {
                balanzaAnimator.OpenBalanzaAnimation();
            }

            var tpapel = papelPesaje.transform;
            OnPesajeDone(tpapel);
        }

        /// <summary>
        /// Metodo que se ejecuta para entregar la mezcla al NPC
        /// </summary>
        private void OnPesajeDone(Transform transform)
        {
            transform.DOMove(papelPesajeNewPos, 4).OnComplete(async ()=>
            {
                await Task.Delay(500);
                npcDialogue.TriggerEvent();
            });
            npcCamera.enabled = true;
        }

        /// <summary>
        /// Animacion de cuando el NPC mezcla la base pesada con el matraz
        /// </summary>
        private void NpcMakeCombination()
        {
            mySequence = DOTween.Sequence();
            mySequence.Append(papelPesaje.transform.DOMove(new Vector3(-2.06f,2.59f,-0.80f), 1.5f));
            mySequence.Append(papelPesaje.transform.DORotate(new Vector3(327.16f,270f,90f), 0.5f));
            mySequence.AppendInterval(1f);
            mySequence.AppendCallback(() => _reactivoPesaje.SetReactivoCero());
            mySequence.Append(papelPesaje.transform.DORotate(initialRotation, 0.5f));
            mySequence.Append(papelPesaje.transform.DOMove(papelPesajeNewPos, 1.5f));
            mySequence.OnComplete(() => GetLevelResult());
            mySequence.Play();
            HasBeenPlayed = true;
        }

        /// <summary>
        /// Si el peso esta bien hecho se acaba el nivel con un aviso de bien hecho
        /// Si el peso esta mal, explota y mueren
        /// </summary>
        private void GetLevelResult()
        {
            Destroy(this);
        }

        /// <summary>
        /// Paramos la animacion si se para el juego
        /// </summary>
        protected override void OnPostPaused()
        {
            base.OnPostPaused();

            if (HasBeenPlayed)
            {
                mySequence.Pause();
                HasBeenPaused = true;
            }
        }

        /// <summary>
        /// Continuamos la animacion si se Reanuda el juego
        /// </summary>
        protected override void OnPostResumed()
        {
            base.OnPostResumed();
            if (HasBeenPaused)
            {
                mySequence.Play();
                HasBeenPaused = false;
            }
        }
    }
}
