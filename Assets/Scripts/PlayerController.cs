using System;
using UnityEngine;

/// <summary>
/// This is the main class that handles player movement and weapon firing.
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static Action<Weapon> OnFire = delegate { };

    [SerializeField] private float lookSpeedX = 2;
    [SerializeField] private float lookSpeedY = 2;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private Weapon weapon;
    
    private Camera fpsCamera;
    private Rigidbody rb;

    private void Awake()
    {
        fpsCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        HUD.Instance.InitHud(weapon);
        Cursor.visible = false;
    }

    private void Update()
    {
        // Disable movement if no camera
        if (fpsCamera == null)
        {
            return;
        }
        
        // Handle player input (if any)
        HandleMovement();
        HandleWeapon();
    }

    private void HandleMovement()
    {
        // Move
        var inputX = Input.GetAxis("Horizontal");
        var inputY = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(inputX, 0, inputY) * 0.1f);
        
        // Rotate only on X and Y axis
        var h = lookSpeedX * Input.GetAxis("Mouse X");
        var v = lookSpeedY * Input.GetAxis("Mouse Y");
        transform.RotateAround(transform.position, Vector3.up, h); // Y axis
        fpsCamera.transform.RotateAround(transform.position, transform.right, -v); // X axis
        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleWeapon()
    {
        // Shoot if player clicked
        if (Input.GetMouseButtonDown(0))
        {
            var canShoot = weapon.TryShoot();
            if (canShoot)
            {
                FireWeapon();
            }
        }
        
        // Reload if player pressed R
        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }
    }
    
    private void FireWeapon()
    {
        // Cast a ray that looks for enemy tag on hit from camera forward
        var fpsCam = fpsCamera.transform;
        var ray = new Ray(fpsCam.position, fpsCam.forward);
        if (Physics.Raycast(ray, out var hit))
        {
            var enemyTag = hit.collider.CompareTag("Enemy");
            
            // If enemy, destroy it
            if (enemyTag)
            {
                Destroy(hit.collider.gameObject);
            } 
            else if (hit.collider.CompareTag("Obstacle"))
            {
                // If cinder block, crack it
                hit.collider.GetComponent<CinderBlock>().Crack();
            }
        }
        
        // Fire weapon and invoke event
        weapon.Fire();
        OnFire(weapon);
    }
}
