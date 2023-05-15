using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class to handle weapon firing and reloading logic.
/// </summary>
public class Weapon : MonoBehaviour
{
	public static Action<bool> OnReload = delegate { };
	
	public int currentAmmo;
	public int maxAmmo;
	public float reloadTime;
	public float fireRate;
	
	[SerializeField] private GameObject muzzleFlash;
	
	private float lastFireTime;
	private bool reloading = false;
		
	public bool TryShoot()
	{
		// Check if we can shoot
		var bulletLoaded = currentAmmo == maxAmmo || Time.time - lastFireTime > fireRate;
		return !reloading && currentAmmo != 0 && bulletLoaded;
	}

	public void Reload()
	{
		if (reloading)
		{
			return;
		}
		
		StartCoroutine(ReloadWeapon());
	}

	public void Fire()
	{
		lastFireTime = Time.time;
		currentAmmo--;
		StartCoroutine(ShowMuzzleFlash());
	}
	
	private IEnumerator ShowMuzzleFlash()
	{
		muzzleFlash.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		muzzleFlash.SetActive(false);
	}
	
	private IEnumerator ReloadWeapon()
	{
		reloading = true;
		OnReload(false);
		
		yield return new WaitForSeconds(reloadTime);
		currentAmmo = maxAmmo;
		reloading = false;
		OnReload(true);
	}
}
