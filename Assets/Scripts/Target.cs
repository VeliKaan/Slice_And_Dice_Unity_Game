using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager GameManager; // Reference to the GameManager for score and life updates

    [Header("Particle Effects")]
    public ParticleSystem ExplosionParticle; // The explosion particle effect when the target is destroyed

    [Header("Target Settings")]
    public int PointValue = 5; // Points awarded when the target is destroyed
    private Rigidbody _rigidbody; // Reference to the Rigidbody component of the target

    [Header("Physics Settings")]
    private static readonly float _minSpeed = 12.0f;  // Minimum speed for the target's upward force
    private static readonly float _maxSpeed = 16.0f;  // Maximum speed for the target's upward force
    private static readonly float _maxTorque = 10.0f; // Maximum torque applied to the target

    [Header("Spawn Settings")]
    private static readonly float _xRange = 4.0f;     // The range for the target's horizontal spawn position
    private static readonly float _ySpawnPos = 0.0f;  // The fixed vertical spawn position for the target

    // This method is called when the target is initialized
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();    // Get the Rigidbody component attached to the target
        _rigidbody.AddForce(RandomForce(), ForceMode.Impulse); // Apply a random force upwards
        _rigidbody.AddTorque(RandomTorque(), RandomTorque(), RandomTorque()); // Apply random torque to spin the target
        transform.position = RandomSpawnPos();     // Set the target's spawn position randomly
    }

    // Generates a random upward force for the target
    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(_minSpeed, _maxSpeed); // Random upward force between min and max speed
    }

    // Generates a random torque value to spin the target
    private float RandomTorque()
    {
        return Random.Range(-_maxTorque, _maxTorque); // Random torque between negative and positive max torque
    }

    // Generates a random spawn position for the target within the specified range
    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-_xRange, _xRange), _ySpawnPos); // Random X position within the range, fixed Y position
    }

    // This method is called when the target is clicked by the player
    private void OnMouseDown()
    {
        DestroyTarget(); // Destroy the target when clicked
    }

    // This method is triggered when the target enters a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Bad") == false) // If the target is not tagged as "Bad"
        {
            GameManager.DecLives(); // Decrease player's lives
        }
        Destroy(gameObject); // Destroy the target object
    }

    // This method destroys the target, spawns an explosion effect, and updates the score
    public void DestroyTarget()
    {
        if (GameManager.isGameActive) // Ensure the game is active before destroying the target
        {
            Instantiate(ExplosionParticle, transform.position, ExplosionParticle.transform.rotation); // Spawn explosion effect
            Destroy(gameObject); // Destroy the target object
            GameManager.UpdateScore(PointValue); // Update the score with the target's point value
        }
    }
}
