using _Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcState : MonoBehaviour
{
    private NpcStates state = NpcStates.Burning;

    public event Action OnChangeStateNpc;


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
        state = newState;

        OnChangeStateNpc?.Invoke();
    }
}
