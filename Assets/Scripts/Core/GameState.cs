using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameState : Singleton<GameState>
{
    [SerializeField] InputActionReference pause;
    [SerializeField] private int startScene = 1;

    [Header("Player Settings")]
    [SerializeField] private uint playerScore = 0;

    [Header("Timer Settings")]
    [SerializeField] private float startingTime = 60f;
    private float timeRemaining;
    private Coroutine timerCoroutine;

    private bool isPlaying = false;
    private bool isGameOver = false;
    private bool isPaused = false;

    public float TimeRemaining => this.timeRemaining;
    public uint PlayerScore => this.playerScore;
    public bool IsGameOver => this.isGameOver;
    public bool IsPaused => this.isPaused;
    public bool IsPlaying => this.isPlaying;

    public UnityEvent<uint, uint> ScoreChangeEvent { get; } = new(); // The first parameter is the point increase, the second parameter is the new score.
    public UnityEvent GameOverEvent { get; } = new();
    public UnityEvent GamePausedEvent { get; } = new();
    public UnityEvent GameUnPausedEvent { get; } = new();
    public UnityEvent GameResetEvent { get; } = new();
    public UnityEvent<float> TimerTickEvent { get; } = new(); // Sends the time remaining every whole second.

    /* PUBLIC METHODS */

    /// <summary>
    /// Loads the main scene and resets the game state. Effectively reloads the level.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
        Reset();
    }

    public void Reset()
    {
        Time.timeScale = 1f;

        this.playerScore = 0;
        this.isPlaying = true;
        this.isGameOver = false;
        this.isPaused = false;

        RestartTimer();

        GameResetEvent.Invoke();
    }

    /// <summary>
    /// Sets Time.timeScale to 0f and notifies other components that the game is over.
    /// </summary>
    public void GameOver()
    {
        Time.timeScale = 0f;
        this.isGameOver = true;

        StopTimer();

        GameOverEvent.Invoke();
    }

    /// <summary>
    /// Increments the player score and notifies other components by what value and the resultant score.
    /// </summary>
    /// <param name="value">The number of points to increase the score by.</param>
    public void AddPlayerScore(uint value)
    {
        this.playerScore += value;

        ScoreChangeEvent.Invoke(value, this.playerScore);
    }

    /// <summary>
    /// Increments the player score based off the SO_InteractableCollectableData and notifies other components by what value and the resultant score.
    /// </summary>
    /// <param name="data">The interactable from which to extract points.</param>
    public void AddPlayerScore(SO_InteractableCollectableData data)
    {
        uint value = data.GetCollectableValue();
        this.playerScore += value;

        ScoreChangeEvent.Invoke(value, this.playerScore);
    }

    /// <summary>
    /// Pauses the game if set to true or else resumes the game if set to false,
    /// while also notifying other components.
    /// </summary>
    public void SetIsPaused(bool isPaused)
    {
        if (this.isGameOver)
        {
            return; // Can't pause if the game is already over
        }

        this.isPaused = isPaused;

        if (this.isPaused)
        {
            Time.timeScale = 0;
            GamePausedEvent.Invoke();
        }
        else
        {
            Time.timeScale = 1;
            GameUnPausedEvent.Invoke();
        }

        Debug.Log($"isPaused: {this.isPaused}, timeScale: {Time.timeScale}");
        Debug.Log(this.isPaused ? "Game Paused" : "Game Resumed");

        // Free the mouse for menus when paused, lock it back when playing
        // Cursor.lockState = this.isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        // Cursor.visible = this.isPaused;
        // Made private to prevent from being used in multiple places.
    }

    /// <summary>
    /// Returns true if the game is ongoing AND the game is NOT over or NOT paused and vice versa.
    /// </summary>
    public bool IsPlayerInControl()
    {
        return (this.isPlaying && !(this.isGameOver || this.isPaused));
    }

    /* MONOBEHAVIOR LIFECYCLE METHODS */

    private void OnEnable()
    {
        pause.action.Enable();
        pause.action.performed += OnPause;
    }

    private void OnDisable()
    {
        pause.action.performed -= OnPause;
        pause.action.Disable();

        StopTimer();
    }

    private void Start()
    {
        Reset();
    }

    /* OTHER PRIVATE METHODS */

    private void EndGame()
    {
        Time.timeScale = 0f;
        this.isPlaying = false;

        StopTimer();

        // Should probably either return to main menu or quit the application.
        // Replace with something akin to QuitToMain() (which could set isPlaying false) and Application.Quit();
    }

    private void RestartTimer()
    {
        StopTimer();

        this.timeRemaining = this.startingTime;
        this.timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    private void StopTimer()
    {
        if (this.timerCoroutine != null)
        {
            StopCoroutine(this.timerCoroutine);
            this.timerCoroutine = null;
        }
    }

    private IEnumerator TimerCoroutine()
    {
        while (this.timeRemaining > 0 && this.isPlaying && !this.isGameOver)
        {
            yield return new WaitForSeconds(1f);

            if (!IsPlayerInControl())
            {
                continue;
            }

            this.timeRemaining -= 1f;

            if (this.timeRemaining < 0)
            {
                this.timeRemaining = 0;
            }

            TimerTickEvent.Invoke(this.timeRemaining);

            if (this.timeRemaining <= 0)
            {
                GameOver();
                yield break;
            }
        }

        this.timerCoroutine = null;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        SetIsPaused(!this.isPaused);
    }
}
