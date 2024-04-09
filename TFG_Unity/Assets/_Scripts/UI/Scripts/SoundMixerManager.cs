using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;
using UnityEngine.Audio;

public class SoundMixerManager : _Scripts.Utilities.Singleton<SoundMixerManager>
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(level) * 20);
    }
    public void setSFXvolume(float level)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(level) * 20);
    }
    public void setMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20);
    }
}
