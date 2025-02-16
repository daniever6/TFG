using _Scripts.Dialogues;
using _Scripts.Managers;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLevel01 : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera teacherCamera;
    [SerializeField] private DialogueTrigger introDialogue;
    [SerializeField] private NpcState teacherState;

    /// <summary>
    /// Suscribe los eventos del dialogo
    /// </summary>
    private void OnEnable()
    {
        DialogueManager.OnDialogueFinish += EndIntro;
    }

    /// <summary>
    /// Desuscribe los eventos del dialogo
    /// </summary>
    private void OnDisable()
    {
        DialogueManager.OnDialogueFinish -= EndIntro;
    }

    private void Awake()
    {
        teacherCamera.enabled = true;
    }

    private void Start()
    {
        StartIntro();
    }

    /// <summary>
    /// Comienza la introduccion del nivel
    /// </summary>
    private void StartIntro()
    {
        introDialogue.TriggerEvent();

        teacherState.ChangeState(_Scripts.Utilities.NpcStates.Talking);
    }

    /// <summary>
    /// Metodo que se realiza al terminar la introduccion del nivel
    /// </summary>
    private void EndIntro()
    {
        teacherCamera.enabled = false;
        teacherState.ChangeState(_Scripts.Utilities.NpcStates.Idle);
    }
}
