using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URPOutline : MonoBehaviour
{
    public int originalMatIdx = 0;

    [SerializeField] private Material outlineMaterial;
    private Material originalMaterial;
    private Renderer objRenderer;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        originalMaterial = objRenderer.materials[originalMatIdx];
    }

    private void OnMouseOver()
    {
        objRenderer.material = outlineMaterial;
    }

    private void OnMouseExit()
    {
        objRenderer.material = originalMaterial;
    }
}
