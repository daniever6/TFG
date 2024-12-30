using _Scripts.Utilities;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcState : MonoBehaviour
{
    private NpcStates state = NpcStates.None;
    [SerializeField][CanBeNull] private Animator npcAnimator;

    public event Action OnChangeStateNpc;

    private void Start()
    {
        if(npcAnimator == null)
        {
            npcAnimator = GetComponentInChildren<Animator>();
        }
    }

    /// <summary>
    /// Devuelve true si el npc tiene algun problema y false en caso contrario
    /// </summary>
    /// <returns></returns>
    public bool IsDying()
    {
        return state == NpcStates.Burning || state == NpcStates.Acid || state == NpcStates.Carry;
    }


    public NpcStates State
    {
        get => state;
    }

    /// <summary>
    /// Cambia el estado del NPC
    /// </summary>
    /// <param name="newState">Nuevo estado del npc</param>
    public void ChangeState(NpcStates newState)
    {
        // Evita que si se esta muriendo cambie su estado si no se salva
        if(IsDying() && (newState != NpcStates.Save || newState == NpcStates.Carry)) 
        {
            return;
        }

        if(newState == NpcStates.Save)
        {
            newState = NpcStates.Idle;
        }

        state = newState;

        // Si tiene animator controlar sus animaciones
        if (!npcAnimator.IsUnityNull())
        {
            switch (newState)
            {
                case NpcStates.None:
                    npcAnimator.CrossFade("Idle", 0.2f);
                    break;

                case NpcStates.Idle:
                    npcAnimator.CrossFade("Idle", 0.2f);
                    break;

                case NpcStates.Walking:
                    npcAnimator.CrossFade("Walk", 0f);
                    break;

                case NpcStates.Talking:
                    npcAnimator.CrossFade("Talk", 0.2f);
                    break;

                case NpcStates.Burning:
                    npcAnimator.CrossFade("Fire", 0.2f);
                    break;

                case NpcStates.Acid:
                    npcAnimator.CrossFade("Fire", 0.2f);
                    break;

                case NpcStates.Carry:
                    npcAnimator.CrossFade("Carry", 0.2f);
                    break;

                case NpcStates.Die:
                    npcAnimator.CrossFade("Die", 0.2f);
                    break;

                default:
                    break;

            }
        }

        OnChangeStateNpc?.Invoke();
    }
}
