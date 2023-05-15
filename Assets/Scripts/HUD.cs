using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Main class to handle HUD elements.
/// </summary>
public class HUD : MonoBehaviour
{
    public static HUD Instance;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private RectTransform bulletContainer;
    [SerializeField] private TextMeshProUGUI ammoText;
    
    private readonly List<GameObject> bullets = new List<GameObject>();
    private int maxAmmo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

    public void InitHud(Weapon playerWeapon)
    {
        // Create bullets
        FillBullets(playerWeapon.currentAmmo);
        maxAmmo = playerWeapon.maxAmmo;
        
        // Subscribe to player fire event and weapon reload event
        PlayerController.OnFire += HandlePlayerFire;
        Weapon.OnReload += HandlePlayerReload;
    }
    
    private void FillBullets(int amount)
    {
        bullets.Clear();
        for (var i = 0; i < amount; i++)
        {
            bullets.Add(Instantiate(bulletPrefab, bulletContainer.transform));
        }
    }
    
    private void HandlePlayerReload(bool finished)
    {
        ammoText.text = finished ? "Ammo" : "Reloading...";

        if (finished)
        {
            FillBullets(maxAmmo);
        }
        else
        {
            // Destroy all bullets
            foreach (var bullet in bullets)
            {
                Destroy(bullet);
            }
        }
    }

    private void HandlePlayerFire(Weapon obj)
    {
        // Remove first bullet
        var bullet = bullets[0];
        bullets.Remove(bullet);
        Destroy(bullet);
    }
}
