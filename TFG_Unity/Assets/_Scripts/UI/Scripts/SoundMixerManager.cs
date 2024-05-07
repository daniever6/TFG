using UnityEngine;
using UnityEngine.Audio;

namespace _Scripts.UI.Scripts
{
    public class SoundMixerManager : Utilities.Singleton<SoundMixerManager>
    {
        [SerializeField] private AudioMixer audioMixer;

        public void SetMasterVolume(float level)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(level) * 20);
        }
        public void SetSfxVolume(float level)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(level) * 20);
        }
        public void SetMusicVolume(float level)
        {
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
        }
    }
}
