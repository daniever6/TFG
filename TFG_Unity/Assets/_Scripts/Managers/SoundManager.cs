using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class AudioClipEntry
{
    public string key;
    public AudioClip clip;
}

public class SoundManager : StaticInstance<SoundManager>
{
    [SerializeField] private List<AudioClipEntry> Clips = new();
    [SerializeField] private AudioSource AudioSource;

    /// <summary>
    /// Metodo para reproducir sonidos desde el AudioSource
    /// </summary>
    /// <param name="name">Nombre del clip a reproducir</param>
    public AudioSource Play(string name)
    {
        var ClipSource = Clips.FirstOrDefault(c => c.key == name);

        if(ClipSource is null)
        {
            return null;
        }

        AudioSource.PlayOneShot(ClipSource.clip);

        return AudioSource;
    }

    /// <summary>
    /// Reproduce un sonido desde el audiosource indicado
    /// </summary>
    /// <param name="sender">Audiosource que ejecuta el sonido</param>
    /// <param name="name">Nombre del clip del sonido</param>
    public void PlayOnAudioSource(AudioSource sender, string name)
    {
        var ClipSource = Clips.FirstOrDefault(c => c.key == name);

        if (ClipSource is null)
        {
            return;
        }

        sender.PlayOneShot(ClipSource.clip);
    }
}
