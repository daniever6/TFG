using _Scripts.Utilities;
using Assets._Scripts.NPCs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Clase para agarrar o soltar NPCs
/// </summary>
public class CarryNPC : Singleton<CarryNPC>
{
    [SerializeField] private GameObject carryPosition;
    [SerializeField] private GameObject NpcParent;

    [SerializeField] private Transform duchaPos;

    private GameObject currentNpc = null;
    private Vector3 initialNpcPos = Vector3.zero;
    private bool isCarrying = false;

    public bool IsCarrying { get => isCarrying; }

    /// <summary>
    /// El jugador coge al jugador en brazos
    /// </summary>
    /// <param name="npc">NPC al que coger en brazos</param>
    public void Carry(GameObject npc)
    {
        if (isCarrying)
        {
            return;
        }

        isCarrying = true;
        currentNpc = npc;

        initialNpcPos = npc.transform.position;

        npc.TryGetComponent<Animator>(out Animator npcAnimator);

        if(npcAnimator == null)
        {
            npcAnimator = npc.GetComponentInChildren<Animator>();
        }

        //Desactiva los componentes conflictivos del NPC
        try
        {
            npc.GetComponent<Collider>().enabled = false;
            npc.GetComponent<NavMeshObstacle>().enabled = false;
        }
        catch(Exception ex)
        {
            //Fallo en coger componente
        }
        

        // Mueve el npc a la posicion de carry
        npc.transform.SetParent(carryPosition.transform);
        npc.transform.localPosition = Vector3.zero;
        npc.transform.localRotation = Quaternion.identity;

        // Ejecuta la animacion de Carry
        npcAnimator.Play("Carry");
    }

    /// <summary>
    /// Suelta al NPC en la posicion indicada
    /// </summary>
    /// <param name="dropPosition">Posicion en la que soltar al npc</param>
    public void DropNPC(Vector3 dropPosition)
    {
        if (!isCarrying)
        {
            return;
        }

        isCarrying = false;

        currentNpc.TryGetComponent<Animator>(out Animator npcAnimator);

        if (npcAnimator == null)
        {
            npcAnimator = currentNpc.GetComponentInChildren<Animator>();
        }

        // Establece la nueva posicion del NPC
        currentNpc.gameObject.transform.SetParent(NpcParent.transform);
        currentNpc.transform.position = new Vector3(dropPosition.x, initialNpcPos.y, dropPosition.z);

        //Activamos los componentes conflictivos del NPC
        try
        {
            currentNpc.GetComponent<Collider>().enabled = true;
            currentNpc.GetComponent<NavMeshObstacle>().enabled = true;
        }
        catch (Exception ex)
        {
            //Fallo en coger componente
        }

        npcAnimator.Play("Idle");

        currentNpc = null;
    }

    /// <summary>
    /// Suelta al NPC en la posicion de la ducha
    /// </summary>
    public void DropOnDucha()
    {
        Vector3 newPos = new Vector3(duchaPos.position.x, duchaPos.position.y, duchaPos.position.z);
        
        DropNPC(newPos);

        BurnNpc.Instance.StopBurning();
    }
}
