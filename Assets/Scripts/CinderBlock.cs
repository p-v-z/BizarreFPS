using UnityEngine;

/// <summary>
/// This class allows the player to damage obstacles by switching their textures.
/// </summary>
public class CinderBlock : MonoBehaviour
{
    [SerializeField] private Texture crackedTexture;
    [SerializeField] private Texture brokenTexture;
    
    private Material material;
    private bool cracked = false;
    private bool broken = false;
    
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    
    public void Crack()
    {
        var damaged = false;
        var targetTexture = crackedTexture;
        
        if (!broken && !cracked)
        {
            damaged = true;
            cracked = true;
        } 
        else if (cracked && !broken)
        {
            targetTexture = brokenTexture;
            damaged = true;
            broken = true;
        }

        if (damaged)
        {
            material.mainTexture = targetTexture;
            material.color = Color.white;
        }
    }
}
