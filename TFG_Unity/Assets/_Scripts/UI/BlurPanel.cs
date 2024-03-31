using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/Blur Panel")]
public class BlurPanel : Image
{
    public bool animate;
    public float time = 0.5f;
    public float delay = 0f;

    CanvasGroup canvasGroup;

    protected override void Reset()
    {
        color = Color.black * 0.1f;
    }

    protected override void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable()
    {
        if (Application.isPlaying)
        {
            material.SetFloat("_Size", 0);
            canvasGroup.alpha = 0;
            LeanTween.value(gameObject, updateBlur, 0, 1, time).setDelay(delay);

        }
    }
    void updateBlur(float value)
    {
        material.SetFloat("_Size", value);
        canvasGroup.alpha = value;
    }
}
