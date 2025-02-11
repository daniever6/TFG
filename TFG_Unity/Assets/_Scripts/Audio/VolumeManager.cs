using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixer masterMixer;

    [Header("Audio source")]
    [SerializeField][CanBeNull] private AudioSource clickAudioSource;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;

    private void Start()
    {
        LoadState();
    }

    /// <summary>
    /// Carga el valor guardado de los sliders
    /// </summary>
    public void LoadState()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        fxSlider.value = PlayerPrefs.GetFloat("FxVolume", 0f);
    }

    /// <summary>
    /// Guarda los valores de los sliders de sonido
    /// </summary>
    public void SaveState()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("FxVolume", fxSlider.value);
    }

    /// <summary>
    /// Establece el volumen del sonido en general
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        if (volume <= -39f) volume = -80f;

        masterMixer.SetFloat("MasterVolume", volume);
        SaveState();
    }

    /// <summary>
    /// Establece el volumen de la musica
    /// </summary>
    /// <param name="volume"></param>
    public void SetMusicVolume(float volume)
    {
        if (volume <= -39f) volume = -80f;

        masterMixer.SetFloat("MusicVolume", volume);
        SaveState();
    }

    /// <summary>
    /// Establece el volumen de los efectos de sonido
    /// </summary>
    /// <param name="volume"></param>
    public void SetFxVolume(float volume)
    {
        if (volume <= -39f) volume = -80f;

        if(clickAudioSource != null)
        {
            clickAudioSource.Play();
        }

        masterMixer.SetFloat("FXVolume", volume);
        SaveState();
    }
}
