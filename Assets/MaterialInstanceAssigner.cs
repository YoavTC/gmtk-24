using UnityEngine;

public class MaterialInstanceAssigner : MonoBehaviour
{
    [SerializeField] private Material baseMaterial; // The base material to duplicate

    [Header("Shader Properties")]
    [SerializeField] private Color originalColor = Color.white;
    [SerializeField] private Color targetColor = Color.white;
    [SerializeField] private float tolerance = 0.001f;

    private void Start()
    {
        if (baseMaterial == null)
        {
            Debug.LogError("Base material is not assigned!");
            return;
        }

        // Loop through all child objects with a SpriteRenderer to find the first texture
        Texture2D mainTexture = null;
        foreach (Transform child in transform.GetChild(0))
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                mainTexture = spriteRenderer.sprite.texture;
                break; // We only need to get the texture from the first valid child
            }
        }

        if (mainTexture == null)
        {
            Debug.LogError("No SpriteRenderer with a texture found in children!");
            return;
        }

        // Create a new instance of the material
        Material newMaterialInstance = new Material(baseMaterial);

        // Set the shader properties
        newMaterialInstance.SetColor("_OriginalColor", originalColor);
        newMaterialInstance.SetColor("_TargetColor", targetColor);
        newMaterialInstance.SetFloat("_Tolerance", tolerance);
        newMaterialInstance.SetTexture("_MainTex", mainTexture);

        // Assign the new material instance to all SpriteRenderers
        foreach (Transform child in transform.GetChild(0))
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Assign the new material instance to the SpriteRenderer
                spriteRenderer.material = newMaterialInstance;
            }
        }
    }
}
