using System;
using System.Threading;
using System.Threading.Tasks;
using _Scripts.Dialogues;
using _Scripts.LevelScripts.Level_01;
using _Scripts.LevelScripts.SaveManager;
using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private DialogueTrigger npcDialogueEntrega;
        [SerializeField] private DialogueTrigger finishDialogue;
        [SerializeField] private Animator npcAnimator;

        [SerializeField] private PesoObjetos pesoReactivo;  // Script del peso del reactivo
        [SerializeField] private Transform posicionMatraz;
        [SerializeField] private float finalResult = 0.75f; // Resultado final del peso para pasar el nivel

        [SerializeField] private PlayerGrab playerGrab;
        [SerializeField] private Collider pesajeCollider;

        [SerializeField] private GameObject entregarButton;

        private float finalPeso = 0f;           //Peso final entregado
        private Transform npcTransform;         //Transform del NPC
        private bool HasBeenPaused = false;     
        private bool HasBeenPlayed = false;
        private Vector3 initialRotation;
        private Sequence mySequence;

        private void Start()
        {
            initialRotation = papelPesaje.transform.rotation.eulerAngles;
            npcTransform = npcAnimator.gameObject.transform;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DialogueManager.OnDialogueFinish -= NpcMakeCombination;
            DialogueManager.OnDialogueFinish -= FinishLevel;
        }

        /// <summary>
        /// Metodo para llamar a entregar el pesaje
        /// </summary>
        public void EntregarPesaje()
        {
            entregarButton.SetActive(false);
            playerGrab.enabled = false;
            pesajeCollider.enabled = false;
            
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
                npcDialogueEntrega.TriggerEvent();
            });
            npcCamera.enabled = true;
        }

        /// <summary>
        /// Animacion de cuando el NPC mezcla la base pesada con el matraz
        /// </summary>
        private void NpcMakeCombination()
        {
            DialogueManager.OnDialogueFinish -= NpcMakeCombination;

            npcAnimator.Play("Idle");
            

            mySequence = DOTween.Sequence();
            mySequence.Append(npcTransform.DORotate(new Vector3(0, 0), 1f));
            mySequence.AppendCallback(()=> npcAnimator.CrossFade("Search", 0.5f));
            mySequence.Append(papelPesaje.transform.DOMove(new Vector3(-2.06f,2.59f,-0.80f), 2f));
            mySequence.Append(papelPesaje.transform.DORotate(new Vector3(327.16f,270f,90f), 1f));
            mySequence.AppendInterval(1f);
            mySequence.AppendCallback(() => 
            { 
                finalPeso = _reactivoPesaje.CurrentPesoReactivo();
                _reactivoPesaje.SetReactivoCero();
                SoundManager.Instance.Play("Burbujas");
            });
            mySequence.Append(papelPesaje.transform.DORotate(initialRotation, 1f));
            mySequence.Append(papelPesaje.transform.DOMove(papelPesajeNewPos, 2f));
            mySequence.AppendCallback(() => npcAnimator.CrossFade("Sorpresa", 0.5f));
            mySequence.AppendInterval(3f);
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
            if (Math.Abs(finalPeso - finalResult) > 0.1)
            {
                ParticleEffectManager.Instance.InstantiateParticleInPos("Explosion", posicionMatraz);
                ParticleEffectManager.Instance.InstantiateParticleInPos("Fuego", posicionMatraz); 
                
                npcAnimator.Play("Die");

                DeathInvoker.Instance.KillAnimation(GameLevels.LevelBalanza, 
                    "Has muerto por un accidente al no pesar bien el reactivo", 
                    3f);
            }
            else
            {
                CongratulatePlayer();
            }
        }

        /// <summary>
        /// Animacion y activacion del dialogo para felicitar al jugador por completar el nivel
        /// </summary>
        private void CongratulatePlayer()
        {
            npcAnimator.CrossFade("Clap", 0.5f);


            mySequence = DOTween.Sequence();
            mySequence.Append(npcTransform.DORotate(new Vector3(0f, 90f), 1f));
            mySequence.AppendCallback(() => npcAnimator.CrossFade("Talk", 0.5f));
            mySequence.OnComplete(() => finishDialogue.TriggerEvent());
            mySequence.Play();

            DialogueManager.OnDialogueFinish += FinishLevel;
        }

        private void FinishLevel()
        {
            SaveData saveData = SaveManager.SaveManager.LoadGameData();

            if(saveData != null)
            {
                SaveManager.SaveManager.SaveGameData(saveData.playerPosition, GameState.Resume, LevelState.NivelAcidos);
            }

            SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
        }

        /// <summary>
        /// Paramos la animacion si se para el juego
        /// </summary>
        protected override void OnPostPaused()
        {
            base.OnPostPaused();

            if (GameManager.GameState == GameState.Pause)
            {
                npcAnimator.speed = 0f;
            }

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

            if(GameManager.GameState != GameState.Pause)
            {
                npcAnimator.speed = 1f;
            }

            if (HasBeenPaused)
            {
                mySequence.Play();
                HasBeenPaused = false;
            }
        }
    }
}
