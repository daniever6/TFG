using _Scripts.Dialogues;
using _Scripts.Interactables;
using _Scripts.Managers;
using _Scripts.Utilities;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EmergencyManager : GameplayMonoBehaviour<EmergencyManager>
{
    [SerializeField] private GameObject emergencyLight;
    [SerializeField] private DialogueTrigger emergencyDialgue;
    [SerializeField] private GameObject exitDoor;
    [SerializeField] private RecibidorDoorTrigger doorTrigger;
    [SerializeField] private NpcPatrol npcPatrol;

    [SerializeField] private GameObject[] npcs;

    protected override void Awake()
    {
        try
        {
            base.Awake();

            emergencyLight.SetActive(false);
            doorTrigger.enabled = false;
        }
        catch(Exception ex)
        {

        }
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueFinish += NpcsRunToExit;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueFinish -= NpcsRunToExit;
    }

    /// <summary>
    /// Activa las luces de emergencia y el dialogo de emergencia
    /// </summary>
    public void StartEmergency()
    {
        npcPatrol.enabled = false;
        npcPatrol.gameObject.TryGetComponent<NpcState>(out var npcState);

        SoundManager.Instance.Play("Alarma");
        
        if(npcState != null)
        {
            npcState.ChangeState(NpcStates.Idle);
        }

        emergencyLight.SetActive(true);

        emergencyDialgue.TriggerEvent();

        doorTrigger.enabled = true;

        //Activar el script de interactable
        exitDoor.layer = LayerMask.NameToLayer("Interactable");

        if(exitDoor.TryGetComponent<Interactable>(out var interactable))
        {
            interactable.enabled = true;
        }
    }

    /// <summary>
    /// Hace que los npcs abandonen el laboratorio por la puerta de salida
    /// </summary>
    private async void NpcsRunToExit()
    {
        if(LevelManager.Instance.CurrentLevelState != LevelState.NivelEmergencia)
        {
            return;
        }

        foreach (var npc in npcs)
        {
            await Task.Delay(200);

            if(npc.TryGetComponent<NavMeshObstacle>(out var navObstacle))
            {
                navObstacle.enabled = false;
            }

            if(!npc.TryGetComponent<Rigidbody>(out var rb))
            {
                npc.AddComponent<Rigidbody>();
            }

            if (npc.TryGetComponent<NavMeshAgent>(out var navMeshAgent))
            {
                navMeshAgent.enabled = true;

                if (npc.TryGetComponent<NpcState>(out var npcState))
                {
                    npcState.ChangeState(NpcStates.Walking);
                }
                navMeshAgent.SetDestination(exitDoor.transform.position);
            }
        }
    }
}
