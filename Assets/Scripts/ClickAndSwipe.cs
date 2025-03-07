using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]

public class ClickAndSwipe : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager GameManager; // Reference to the GameManager for game logic

    [Header("Components")]
    private Camera _camera;          // Reference to the Camera component
    private TrailRenderer _trail;    // Reference to the TrailRenderer component
    private BoxCollider _collider;   // Reference to the BoxCollider component

    [Header("Game State")]
    private bool _swiping = false;   // Flag to check if the player is swiping

    // This method is called when the script is initialized
    private void Awake()
    {
        _camera = Camera.main; // Get the main camera in the scene

        _trail = GetComponent<TrailRenderer>(); // Get the TrailRenderer component
        _trail.enabled = false; // Initially, the trail is not visible

        _collider = GetComponent<BoxCollider>(); // Get the BoxCollider component
        _collider.enabled = false; // Initially, the collider is not enabled
    }

    // Updates the position of the object based on the mouse position
    private void UpdateMousePosition()
    {
        // Convert mouse position to world space
        var mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos; // Set the object's position to the mouse position
    }

    // Updates the TrailRenderer and BoxCollider components based on whether the user is swiping
    private void UpdateComponents()
    {
        _trail.enabled = _swiping; // Enable or disable the trail based on swiping state
        _collider.enabled = _swiping; // Enable or disable the collider based on swiping state
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isGameActive) // Check if the game is active
        {
            if (Input.GetMouseButtonDown(0)) // If the left mouse button is pressed
            {
                _swiping = true; // Set swiping to true
                UpdateComponents(); // Update the components
            }
            else if (Input.GetMouseButtonUp(0)) // If the left mouse button is released
            {
                _swiping = false; // Set swiping to false
                UpdateComponents(); // Update the components
            }

            if (_swiping) // If swiping is active
            {
                UpdateMousePosition(); // Update the position of the object based on the mouse
            }
        }
    }

    // This method is called when the object collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        Target target = collision.gameObject.GetComponent<Target>(); // Check if the collided object has a Target component

        if (target)
        {
            target.DestroyTarget(); // Destroy the target if it exists
        }
    }
}
