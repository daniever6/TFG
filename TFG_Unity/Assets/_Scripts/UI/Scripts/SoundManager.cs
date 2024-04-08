using UnityEngine;

namespace _Scripts.UI.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        [Header("------------- Audio Source -------------")]
        [SerializeField] AudioSource BGM;
        [SerializeField] AudioSource SFXsource;

        [Header("------------- Audio clip -------------")]
        public AudioClip Chocar;
        public AudioClip Recoger;
        public AudioClip Retirar;
        public AudioClip Exito;
        public AudioClip Fallo;
        public AudioClip Warning;
        public AudioClip Cristal;
        public AudioClip Liquido;
        public AudioClip AbrirTapa;

    }
}
