using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSwitcher : Singleton<MusicSwitcher>
{
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource emergencyMusic;

    private static int intancesCount = 0;

    /// <summary>
    /// Se asegura de que la instancia sea unica
    /// </summary>
    private void Awake()
    {
        base.Awake();

        intancesCount++;
        if(intancesCount > 1)
        {
            intancesCount--;
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private AudioSource currentAudioSourcePlaying;
    private void Update()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "Intro" || sceneName == "MainMenuScene" || sceneName == "DeathScene")
        {
            if (!backgroundMusic.isPlaying && !emergencyMusic.isPlaying) 
            {
                return;
            }

            if (backgroundMusic.isPlaying)
            {
                currentAudioSourcePlaying = backgroundMusic.isPlaying ? backgroundMusic : emergencyMusic;
            }
            backgroundMusic.Stop();
            emergencyMusic.Stop();
        }
        else if (!backgroundMusic.isPlaying && !emergencyMusic.isPlaying)
        {
            currentAudioSourcePlaying?.Play();
        }
    }

    /// <summary>
    /// Toca la musica de fondo normal
    /// </summary>
    public void SetBackground()
    {
        backgroundMusic.Play();
        emergencyMusic.Stop();
    }

    /// <summary>
    /// Toca la musica de emergencia
    /// </summary>
    public void SetEmergency()
    {
        backgroundMusic.Stop();
        emergencyMusic.Play();
    }
}
