using _Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla el icono de resultado de las combinaciones
/// Hay un icono por defecto, uno correcto y otro erroneo
/// </summary>
public class CombinationResultUI : GameplayMonoBehaviour<CombinationResultUI>
{
    [SerializeField] private Image resultIndicator;

    [SerializeField] private Sprite correctIcon;
    [SerializeField] private Sprite wrongIcon;
    [SerializeField] private Sprite nullIcon;
    [SerializeField] private Sprite defaultIcon;

    private float spriteDuration = 4f;

    private void Start()
    {
        resultIndicator.sprite = defaultIcon;
    }

    /// <summary>
    /// Manda la orden de empezar una corrutina para cambiar el icono
    /// al indicado
    /// </summary>
    /// <param name="icon">Tipo de Sprite a cambiar</param>
    public void ChangeResultSprite(LevelIcons icon)
    {
        Sprite sprite = defaultIcon;
        
        switch (icon)
        {
            case LevelIcons.Correct:
                SoundManager.Instance.Play("Correcto");
                sprite = correctIcon;
                break;

            case LevelIcons.Null:
                SoundManager.Instance.Play("Error");
                sprite = nullIcon;
                break;

            case LevelIcons.Wrong:
                SoundManager.Instance.Play("Error");
                sprite = wrongIcon; 
                break;

            default: 
                break;

        }
        StartCoroutine(SetCorrectSprite(sprite));
    }

    /// <summary>
    /// Cambia el icono al icono indicado
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetCorrectSprite(Sprite sprite)
    {
        resultIndicator.sprite = sprite;

        yield return new WaitForSeconds(spriteDuration);

        resultIndicator.sprite = defaultIcon;
    }

    /// <summary>
    /// Metodo que se ejecuta al pausar
    /// </summary>
    protected override void OnPostPaused()
    {
        base.OnPostPaused();
        StopAllCoroutines();
        resultIndicator.sprite = defaultIcon;
    }

    /// <summary>
    /// Metodo que se ejecuta al reanudar
    /// </summary>
    protected override void OnPostResumed()
    {
        base.OnPostResumed();
        resultIndicator.sprite = defaultIcon;
    }

}
