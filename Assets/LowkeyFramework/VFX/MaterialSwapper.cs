using Sirenix.OdinInspector;
using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Material materialToSwap;

    private bool isCurrentMaterialTheDefault = true;

    private void OnValidate()
    {
        if(meshRenderer && !defaultMaterial)
        {
            defaultMaterial = meshRenderer.sharedMaterial;
        }
    }

    [Button]
    public void SwapMaterials() => SwapMaterials(!isCurrentMaterialTheDefault);

    [Button(Expanded = true)]
    public void SwapMaterials(bool toDefault)
    {
        Material materialToSet = toDefault ? defaultMaterial : materialToSwap;
        isCurrentMaterialTheDefault = toDefault;
        meshRenderer.material = materialToSet;
    }
}
