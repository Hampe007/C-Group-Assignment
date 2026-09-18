using UnityEngine;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [SerializeField] InputActionReference pause;

    [Header("Player Settings")]
    [SerializeField] private uint playerScore = 0;
    public uint PlayerScore => this.playerScore;

    private bool isPlaying = false;
    private bool isGameOver = false;
    private bool isPaused = false;
    public bool IsPlaying => this.isPlaying;
    public bool IsGameOver => this.isGameOver;
    public bool IsPaused => this.isPaused;

    [Header("Timer Settings")]
    [SerializeField] private float startingTime = 60f;
    private float timeRemaining;
    public float TimeRemaining => this.timeRemaining;

    private void OnEnable() {
        pause.action.Enable();
        pause.action.performed += OnPause;
    }

    private void OnDisable() {
        pause.action.performed -= OnPause;
        pause.action.Enable();
    }


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        //Time.timeScale = 0f;
        this.isPlaying = true;
        //StartGame();
    }

    private void Update() {
        if (!IsPlayerInControl()) return;

        /*if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame) {
            TogglePause();
        }*/

        CalculateTimeRemaining();
    }

    public void StartGame() {
        //spawn point (start)?
        Time.timeScale = 1f;
        this.playerScore = 0;
        this.isPlaying = true;
        this.isGameOver = false;
        RestartTimer();
    }

    private void GameOver() {
        Time.timeScale = 0f;
        this.isGameOver = true;
        Debug.Log("Game Over!");
    }

    public void EndGame() {
        Time.timeScale = 0f;
        this.isPlaying = false;
        Debug.Log("Game Session Ended.");
    }

    public bool IsPlayerInControl() {
        return (this.isPlaying && !(this.isGameOver || this.isPaused));
    }

    /*public bool IsGameOver() {
        // Add extra stuff, check if all iteams are in the drop off chest
        return this.isGameOver;
    }*/

    public void RestartTimer() {
        this.timeRemaining = this.startingTime;
    }

    public void TogglePause() {
        if (this.isGameOver) return; // Can't pause if the game is already over

        this.isPaused = !this.isPaused;
        Time.timeScale = this.isPaused ? 0f : 1f;
        Debug.Log($"isPaused: {this.isPaused}, timeScale: {Time.timeScale}");

        // Free the mouse for menus when paused, lock it back when playing
        //Cursor.lockState = this.isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible = this.isPaused;

        Debug.Log(this.isPaused ? "Game Paused" : "Game Resumed");
        // Add UI stuff here to open or close a pause menu
    }

    private void CalculateTimeRemaining() {
        if (this.timeRemaining > 0) {
            this.timeRemaining -= Time.deltaTime;

            if (this.timeRemaining <= 0) {
                this.timeRemaining = 0;
                GameOver();
            }
        }
    }

    private void OnPause(InputAction.CallbackContext context) {
        //Check if player exists?
        TogglePause();
    }

    public void AddPlayerScore(uint value) {
        this.playerScore += value;
    }

    public void AddPlayerScore(SO_InteractableCollectableData data) {
        this.playerScore += data.GetCollectableValue();
    }
}
