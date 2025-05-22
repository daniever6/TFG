using _Scripts.Interactables;
using _Scripts.LevelScripts.Level_01;
using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MantaIgnifugaManager : Trigger
{
    [SerializeField] private GameObject npcToBurn;
    [SerializeField] private GameObject carryPos;
    [SerializeField] private GameObject manta;

    private Interactable mantaInteractable;
    private Collider mantaCollider;
    private NpcState npcState;

    private static MantaIgnifugaManager instance;

    public static MantaIgnifugaManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new MantaIgnifugaManager();
            }
            return instance;
        }
    }

    public static bool IsCarried = false;

    private void Awake()
    {
        instance = this;
        manta.SetActive(false);
    }

    private void Start()
    {
        npcToBurn?.TryGetComponent<NpcState>(out npcState);
        TryGetComponent<Interactable>(out mantaInteractable);
        TryGetComponent<Collider>(out mantaCollider);

        mantaInteractable.enabled = false;
        mantaCollider.enabled = false;

        npcState.OnChangeStateNpc += ActivateInteractable;
    }


    /// <summary>
    /// Activa el paquete de la manta si el NPC esta en llamas
    /// </summary>
    private void ActivateInteractable()
    {
        if(npcState.State != NpcStates.Burning)
        {
            return;
        }

        mantaInteractable.enabled = true;
        mantaCollider.enabled = true;
    }

    /// <summary>
    /// Agarra el paquete de la manta y lo transporta
    /// </summary>
    public override void TriggerEvent()
    {
        IsCarried = true;

        mantaCollider.enabled = false;

        transform.parent = carryPos.transform;

        //Establece el tamaño y posicion de la manta
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale /= 1.5f;
    }

    /// <summary>
    /// Instancia la animacion de la manta encima del jugador,
    /// detiene el fuego, y 
    /// </summary>
    public async void DropMantaOnNpc()
    {
        transform.localPosition = Vector3.up * 10000; // Oculta el objeto 

        manta.SetActive(true);
        if(manta.TryGetComponent<Animator>(out var mantaAnim)) {
            mantaAnim.Play("animacionPrueba");
        }



        //var animacionManta = ParticleEffectManager.Instance.InstantiateParticleInPos("Manta", npcToBurn.transform);

        //animacionManta.transform.localRotation = Quaternion.identity;
        //animacionManta.transform.localPosition = Vector3.zero - new Vector3(0, 0, 0.5f);

        await Task.Delay(3000);

        //Destroy(animacionManta);
        Destroy(manta);

        npcToBurn.GetComponent<BurnNPC>().StopBurning();

        Destroy(this.gameObject);
    }
}
