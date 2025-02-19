using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class URPOutline : MonoBehaviour
{
    public int originalMatIdx = 0;

    [SerializeField] private Material outlineMaterial;
    private Material originalMaterial;
    private Renderer objRenderer;
    private bool rendererActive = true;

    private List<Material> originalList = new();
    private List<Material> outlineList = new();

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        originalMaterial = objRenderer.materials[originalMatIdx];
        rendererActive = objRenderer.enabled;

        originalList = objRenderer.materials.ToList();
        outlineList = objRenderer.materials.ToList();
        outlineList[originalMatIdx] = outlineMaterial;
    }

    private void OnMouseOver()
    {
        if (!rendererActive) objRenderer.enabled = true;

        objRenderer?.SetMaterials(outlineList);
    }

    private void OnMouseExit()
    {
        if (!rendererActive) objRenderer.enabled = false;

        objRenderer?.SetMaterials(originalList);

    }
}
