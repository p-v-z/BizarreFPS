using UnityEngine;

/// <summary>
/// This class sets the sun angle based on system time.
/// </summary>
public class Sun : MonoBehaviour
{
    [SerializeField] private float sunRise = 6;
    [SerializeField] private float sunSet = 18;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Get system current time
        var currentTime = System.DateTime.Now;
        var hour = currentTime.Hour;
        var minute = currentTime.Minute;
        
        // Calculate angle - 0 degrees at sunRise, 180 degrees at sunSet
        var angle = (hour - sunRise) / (sunSet - sunRise) * 180f;
        Debug.Log($"Current time: {hour}:{minute}");
        Debug.Log($"Angle: {angle}");

        // Set sun angle
        transform.rotation = Quaternion.Euler(angle, 0, 0);
    }
}
