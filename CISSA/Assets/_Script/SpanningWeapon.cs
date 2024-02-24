using System;
using UnityEngine;

public class SpanningWeapon : MonoBehaviour
{
    public GameObject center; // Assign your prefab to this in the Inspector

    [SerializeField] private float velocity = 0.00001f;
    [SerializeField] private float radius = 2.5f;
    private float angle = 0;
    void Start()
    {
        
    }
    private void Update()
    {
        RotateAroundCharacter();
    }

    private void RotateAroundCharacter()
    {
        // Convert speed from degrees per second to radians per second
        float speedRadiansPerSecond = 30f * Mathf.Deg2Rad;

        // Update the angle by the rotation speed adjusted by deltaTime
        angle += speedRadiansPerSecond * Time.deltaTime;

        // Ensure angle stays within 0 to 2*PI radians to keep it in a manageable range
        angle %= 2 * Mathf.PI;

        // Calculate the new position
        Vector2 centerPosition = center.transform.position;
        float x = centerPosition.x + radius * Mathf.Cos(angle);
        float y = centerPosition.y + radius * Mathf.Sin(angle);
        Vector2 newPosition = new Vector2(x, y);

        // Update the object's position, ensuring it stays in the desired Z-plane
        transform.position = new Vector3(newPosition.x, newPosition.y, 0f);
    }
}
