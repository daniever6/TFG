using UnityEngine;

namespace _Scripts.UI.Scripts
{
    public class SfxManager : Utilities.Singleton<SfxManager>
    {
        [SerializeField] private AudioSource SFX_object;
        
        /// <summary>
        /// Crea un audioSource en la posicion indicada, lo ejecuta, y lo destruye al acabar
        /// </summary>
        /// <param name="audio">Audio del clip a tocar</param>
        /// <param name="transform">Posicion del audio</param>
        /// <param name="volume">Volumen del audio</param>
        public void PlaySFX(AudioClip audio, Transform transform, float volume)
        {
            AudioSource audioSource = Instantiate(SFX_object, transform.position, Quaternion.identity);
            audioSource.clip = audio;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource.gameObject, audio.length);
        }
    
    }
}
