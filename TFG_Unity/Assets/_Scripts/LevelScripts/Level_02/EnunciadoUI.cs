using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnunciadoUI : GameplayMonoBehaviour<EnunciadoUI>
{
    [SerializeField] private GameObject canvasEnunciado;

    /// <summary>
    /// Desactiva el canvas al inicio por defecto
    /// </summary>
    private void Start()
    {
        canvasEnunciado.SetActive(false);
    }

    /// <summary>
    /// Muestra el canvas del enunciado al pulsar
    /// </summary>
    private void OnMouseUp()
    {
        canvasEnunciado?.SetActive(true);
    }

    /// <summary>
    /// Cierra el canvas del enunciado
    /// </summary>
    public void CloseEnunciado()
    {
        canvasEnunciado.SetActive(false);
    }

    /// <summary>
    /// Desactiva el canvas en caso de pausa
    /// </summary>
    protected override void OnPostPaused()
    {
        base.OnPostPaused();
        canvasEnunciado.SetActive(false);
    }

}
