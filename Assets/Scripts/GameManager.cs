using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameDifficulty
{
    Easy,    // Represents easy difficulty
    Medium,  // Represents medium difficulty
    Hard     // Represents hard difficulty
}

public class GameManager : MonoBehaviour
{
    // UI Elements
    [Header("UI Elements")]
    public TextMeshProUGUI ScoreText;               // UI text for displaying the score
    public TextMeshProUGUI LivesText;               // UI text for displaying the number of lives
    public TextMeshProUGUI GameOverText;            // UI text for displaying the game over message
    public Button RestartButton;                    // Button to restart the game
    public Slider VolumeSlider;                     // Slider to control the game volume
    public GameObject StartPanel;                   // Panel displayed at the start of the game
    public GameObject PausePanel;                   // Panel displayed when the game is paused

    // Game State
    [Header("Game State")]
    [HideInInspector]
    public GameDifficulty Difficulty;               // The current difficulty level
    [HideInInspector]
    public bool isGameActive;                       // Flag to check if the game is active
    [HideInInspector]
    public int Score;                              // The current score
    [HideInInspector]
    public int Lives;                              // The current number of lives
    [HideInInspector]
    public bool Paused;                             // Flag to check if the game is paused

    // Target Management
    [Header("Target Management")]
    public List<GameObject> Targets;               // List of targets to spawn

    // Spawn Rates
    [Header("Spawn Rates")]
    private static readonly float _easySpawnRate = 4.0f;   // Spawn rate for easy difficulty
    private static readonly float _mediumSpawnRate = 2.0f; // Spawn rate for medium difficulty
    private static readonly float _hardSpawnRate = 1.0f;   // Spawn rate for hard difficulty
    private float _spawnRate = 1.0f;                // The current spawn rate

    // Audio Settings
    [Header("Audio Settings")]
    private static float _audioVolume = 1.0f;       // The current audio volume


    // This method is called when the script is initialized
    private void Start()
    {
        isGameActive = false; // Set the game to inactive at the start

        Score = 0;            // Initialize score
        Lives = 3;            // Initialize lives

        Paused = false;       // Set game to not paused initially

        UpdateScore(0);       // Update the score UI with initial value
        UpdateLives(0);       // Update the lives UI with initial value

        VolumeSlider.value = _audioVolume;  // Set the slider value to the current audio volume
        SetVolume();                      // Apply the current volume to the audio source
    }

    // Coroutine to spawn targets while the game is active
    private IEnumerator SpawnTargets()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(_spawnRate); // Wait for the spawn rate duration
            int index = Random.Range(0, Targets.Count);   // Select a random target
            var targetGameObject = Instantiate(Targets[index]); // Instantiate the target object
            var target = targetGameObject.GetComponent<Target>(); // Get the Target component
            target.GameManager = this; // Set the game manager for the target
        }
    }

    // Updates the score UI with the added score
    public void UpdateScore(int scoreToAdd)
    {
        Score += scoreToAdd; // Add score
        ScoreText.text = "Score: " + Score; // Update the score text UI
    }

    // Updates the lives UI with the added lives
    public void UpdateLives(int livesToAdd)
    {
        int newLives = Lives + livesToAdd;

        if (newLives < 0)
        {
            newLives = 0; // Ensure that lives do not go below 0
        }

        Lives = newLives; // Update the lives value

        LivesText.text = "Lives: " + Lives; // Update the lives text UI

        if (Lives == 0) // If no lives are left, the game is over
        {
            GameOver();
        }
    }

    // Decreases the number of lives by 1
    public void DecLives()
    {
        UpdateLives(-1); // Decrease lives by 1
    }

    // This method is called when the game is over
    public void GameOver()
    {
        isGameActive = false; // Set the game to inactive
        RestartButton.gameObject.SetActive(true); // Show the restart button
        GameOverText.gameObject.SetActive(true);   // Show the game over message
    }

    // This method is called when the restart button is clicked
    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Start the game with the Hard difficulty level
    public void OnHardButtonClick()
    {
        StartGame(GameDifficulty.Hard);
    }

    // Start the game with the Medium difficulty level
    public void OnMediumButtonClick()
    {
        StartGame(GameDifficulty.Medium);
    }

    // Start the game with the Easy difficulty level
    public void OnEasyButtonClick()
    {
        StartGame(GameDifficulty.Easy);
    }

    // Start the game with the selected difficulty level
    private void StartGame(GameDifficulty difficulty)
    {
        isGameActive = true; // Set the game to active
        Difficulty = difficulty; // Set the current difficulty

        // Set the spawn rate based on the selected difficulty
        switch (difficulty)
        {
            case GameDifficulty.Hard:
                _spawnRate = _hardSpawnRate;
                break;
            case GameDifficulty.Medium:
                _spawnRate = _mediumSpawnRate;
                break;
            case GameDifficulty.Easy:
                _spawnRate = _easySpawnRate;
                break;
        }

        StartPanel.gameObject.SetActive(false); // Hide the start panel

        StartCoroutine(SpawnTargets()); // Start spawning targets
    }

    // This method is called when the volume slider value changes
    public void OnVolumeChange()
    {
        SetVolume(); // Apply the new volume value
    }

    // Sets the volume of the audio source based on the volume slider
    private void SetVolume()
    {
        var audioSource = GetComponent<AudioSource>(); // Get the audio source component

        _audioVolume = audioSource.volume = VolumeSlider.value; // Set the audio volume
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGameActive)
        {
            if (Input.GetKeyDown(KeyCode.P)) // If the "P" key is pressed
            {
                TogglePause(); // Toggle pause state
            }
        }
    }

    // Toggles the pause state of the game
    private void TogglePause()
    {
        Paused = !Paused; // Toggle the paused flag

        Time.timeScale = Paused ? 0.0f : 1.0f; // Pause or resume the game time
        PausePanel.gameObject.SetActive(Paused); // Show or hide the pause panel
    }
}
