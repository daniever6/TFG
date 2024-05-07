using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Scripts.UI.Scripts
{
    public class VolumeSettings: MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider volumeSlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                LoadVolume();
            }
            else
            {
                SetMusicVolume();
            }
        }
        
        /// <summary>
        /// Carga el valor del volumen del PlayerPrefs y lo asigna al slider
        /// </summary>
        public void LoadVolume()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
            SetMusicVolume();
        }
        
        /// <summary>
        /// Guarda el valor del volumen del slider de musica en el PlayerPrefs
        /// </summary>
        public void SetMusicVolume()
        {
            float volume = volumeSlider.value;
            audioMixer.SetFloat("music", Mathf.Log10(volume) * 20 );
            PlayerPrefs.SetFloat("musicVolume", volume);
        }
        
    }
}

