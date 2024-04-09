using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXmanager : _Scripts.Utilities.Singleton<SFXmanager>
{
    [SerializeField] private AudioSource SFX_object;
    public void PlaySFX(AudioClip audio, Transform transform, float volume)
    {
        //Se crea el gameobject
        AudioSource audioSource = Instantiate(SFX_object, transform.position, Quaternion.identity);
        //Se asigna el clip al gameobject
        audioSource.clip = audio;
        //Volumen
        audioSource.volume = volume;
        //Play clip
        audioSource.Play();
        //Duracion del clip
        float length = audio.length;
        //Destroy cuando no se usa
        Destroy(audioSource.gameObject, length);
    }
    
}
