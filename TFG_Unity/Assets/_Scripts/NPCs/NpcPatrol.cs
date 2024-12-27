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
        MoveToTarget();

        npcAnimator.CrossFade("Walk", 0f);
    }

    private void MoveToTarget()
    {
        npcState?.ChangeState(NpcStates.Walking);

        npcNavMeshAgent.SetDestination(patrolPoints[targetIdx].transform.position);
    }

    private void IncreaseTargetIdx()
    {
        targetIdx++;
        targetIdx %= (patrolPoints.Length);
    }

    protected override void OnPostPaused()
    {
        npcNavMeshAgent.isStopped = true;
    }

    protected override void OnPostResumed()
    {
        npcNavMeshAgent.isStopped = false;

        if (npcNavMeshAgent.hasPath) 
            npcNavMeshAgent.SetDestination(npcNavMeshAgent.pathEndPosition);
    }
}
