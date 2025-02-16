using _Scripts.LevelScripts;
using _Scripts.Managers;
using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtractoraManager : MonoBehaviour
{
    [SerializeField] private AudioSource extractoraAudioSource;

    private bool isOn = false;

    private void OnEnable()
    {
        FirstPersonLevelManager.OnCombinationPerformed += CheckIsOn;
    }

    private void OnDisable()
    {
        FirstPersonLevelManager.OnCombinationPerformed -= CheckIsOn;
    }

    /// <summary>
    /// Enciende o apaga la extractora
    /// </summary>
    private void OnMouseDown()
    {
        isOn = !isOn;

        if (isOn)
        {
            extractoraAudioSource.Play();
        }
        else
        {
            extractoraAudioSource.Stop();
        }
    }

    /// <summary>
    /// Si la extractora no esta encendida te intoxicas con el aire
    /// </summary>
    /// <param name="combination"></param>
    private void CheckIsOn(string combination)
    {
        if (isOn) return;

        DeathInvoker.Instance.KillAnimation(GameLevels.LevelAcidos, "Te has intoxicado con el humo", 5);
    }
}
