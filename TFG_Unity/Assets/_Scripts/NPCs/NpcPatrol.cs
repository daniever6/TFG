using _Scripts.Player;
using _Scripts.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NpcPatrol : GameplayMonoBehaviour<NpcPatrol>
{
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private NpcState npcState;
    [SerializeField] private NavMeshAgent npcNavMeshAgent;
    [SerializeField] private Animator npcAnimator;

    private int targetIdx = -1;

    private void Awake()
    {
        if(patrolPoints.Length > 0)
        {
            targetIdx = 0;
        }
        else
        {
            Destroy(this);
        }
    }

    private async void Update()
    {
        if(npcState.IsDying())
        {
            return;
        }
        
        if(npcNavMeshAgent.hasPath == false)
        {
            MoveToTarget();
        }

        if (Vector3.Distance(transform.position, patrolPoints[targetIdx].transform.position) < 1f)
        {
            IncreaseTargetIdx();

            npcState?.ChangeState(NpcStates.Idle);

            await Task.Delay(5000);

            MoveToTarget();

        }
    }

    private void Start()
    {
        npcState.OnChangeStateNpc += CheckNpcHealth;

        MoveToTarget();

        npcAnimator.CrossFade("Walk", 0f);
    }

    /// <summary>
    /// Metodo que se llama cada vez que el npc cambia de estado,
    /// si el npc tiene algun problema detiene su movimiento
    /// </summary>
    private void CheckNpcHealth()
    {
        if (npcState.IsDying()) 
        {
            npcNavMeshAgent.isStopped = true;
        }
        else
        {
            npcNavMeshAgent.isStopped = false;
        }
    }

    /// <summary>
    /// Mueve al NPC al siguiente punto de patruya
    /// </summary>
    private void MoveToTarget()
    {
        try
        {
            npcState?.ChangeState(NpcStates.Walking);

            npcNavMeshAgent?.SetDestination(patrolPoints[targetIdx].transform.position);
        }
        catch(Exception Ex)
        {

        }
    }

    /// <summary>
    /// Pasa al siguiente punto de patruya
    /// </summary>
    private void IncreaseTargetIdx()
    {
        targetIdx++;
        targetIdx %= (patrolPoints.Length);
    }

    /// <summary>
    /// Detiene el movimiento del NPC
    /// </summary>
    protected override void OnPostPaused()
    {
        npcNavMeshAgent.isStopped = true;
    }

    /// <summary>
    /// Despues de la pausa devuelve el recorrido al NPC
    /// </summary>
    protected override void OnPostResumed()
    {
        if (npcNavMeshAgent.hasPath && npcState.IsDying() == false)
        {
            npcNavMeshAgent.isStopped = false;
            npcNavMeshAgent.SetDestination(npcNavMeshAgent.pathEndPosition);
        }
            
    }
}
