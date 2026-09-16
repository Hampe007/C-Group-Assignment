using System.Collections;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// TODO: remove all testing-related code

static class TimerClasses {
    public static string Emphasized => "timerEmphasized";
    public static string Regular => "timerRegular";
    public static string Flash => "timerFlash";
}

[System.Serializable]
struct TestingData
{
    public bool IsTest;
    public float StartTime;
    public bool isGamePaused;
    public bool isGameOver;
}
public class LvlOverlayUI : MonoBehaviour {
    [SerializeField] UIDocument UIDoc;
    [Tooltip("Which scene to load on quit button press")]
    [SerializeField] string mainMenuScene;
    [SerializeField] TestingData testingData;
    VisualElement _timerElement;
    VisualElement _gameOverOverlay, _gamePausedOverlay;
    Label _timerLabel;
    Coroutine _timerFlashRoutineRef;

    void Awake()
    {
        _timerLabel = UIDoc.rootVisualElement.Q<Label>("TimerLabel");
        _timerElement = UIDoc.rootVisualElement.Q<VisualElement>("TimerElement");
        _gameOverOverlay = UIDoc.rootVisualElement.Q<VisualElement>("GameOverOverlay");
        _gamePausedOverlay = UIDoc.rootVisualElement.Q<VisualElement>("GamePausedOverlay");

        UIDoc.rootVisualElement.Query<Button>("RetryButton").ForEach(button =>
        {
            button.clicked += OnRetryButtonPress;
        });;
        UIDoc.rootVisualElement.Query<Button>("QuitButton").ForEach(button =>
        {
            button.clicked += OnQuitButtonPress;
        });;
        UIDoc.rootVisualElement.Q<Button>("ResumeButton").clicked += OnGameResumed; // TODO: invoke ResumeGame method from public class

        // TODO: subscribe UpdateTimerVisual to UnityEvent/class which returns the actual timer
        // TODO: subscribe OnGamePaused to UnityEvent/game state class which tells when the game is paused
        // TODO: subscribe OnGameResumed to UnityEvent/game state class which tells when the game is resumed/not paused
        // TODO: subscribe OnGameOver to UnityEvent/game state class which tells when the game is over
    }

    void Start()
    {
        if (testingData.IsTest)
        {
            StartCoroutine(TimerTestRoutine(testingData.StartTime));
        }
        StartCoroutine(TimerEmphasizeRoutine());
    }

    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (testingData.isGameOver)
        {
            OnGameOver();
        }
        else if (testingData.isGamePaused)
        {
            OnGamePaused();
        }
        else
        {
            OnGameResumed();
        }
    }


    void UpdateTimerVisual(float time)
    {
        if (_timerFlashRoutineRef == null && time <= 10  && time > 0)
        {
            _timerFlashRoutineRef = StartCoroutine(TimerFlashRoutine(10));
        }
        else if (testingData.IsTest && time <= 0)
        {
            OnGameOver();
        }

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        string minutesText = minutes >= 10 ? minutes.ToString() : $"0{minutes}";
        string secondsText = seconds >= 10 ? seconds.ToString() : $"0{seconds}";

        _timerLabel.text = $"{minutesText}:{secondsText}";
    }

    void OnQuitButtonPress()
    {
        if (testingData.IsTest)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(mainMenuScene);
    }

    void OnRetryButtonPress()
    {
        if (testingData.IsTest)
        {
            Time.timeScale = 1;
            testingData.isGameOver = false;
            testingData.isGamePaused = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnGameOver()
    {
        if (testingData.IsTest)
        {
            Time.timeScale = 0;
        }
        _gameOverOverlay.visible = true;
    }

    void OnGamePaused()
    {
        if (testingData.IsTest)
        {
            Time.timeScale = 0;
        }
        _gamePausedOverlay.visible = true;
    }

    void OnGameResumed()
    {
        if (_gamePausedOverlay == null)
        {
            return;
        }
        if (testingData.IsTest)
        {
            Time.timeScale = 1;
            testingData.isGamePaused = false;
        }
        _gamePausedOverlay.visible = false;
    }

    IEnumerator TimerEmphasizeRoutine()
    {
        _timerElement.RemoveFromClassList(TimerClasses.Regular);
        _timerElement.AddToClassList(TimerClasses.Emphasized);

        yield return new WaitForEndOfFrame();

        _timerElement.RemoveFromClassList(TimerClasses.Emphasized);
        _timerElement.AddToClassList(TimerClasses.Regular);
    }

    IEnumerator TimerFlashRoutine(float duration)
    {
        float elapsed = 0;

        // animationDuration corresponds to animation duration property of the visual element
        float animationDuration = .35f;
        bool isFlashOn = false;

        while (elapsed < duration)
        {
            if (elapsed % 1 >= 0 && elapsed % 1 < animationDuration && !isFlashOn)
            {
                _timerElement.RemoveFromClassList(TimerClasses.Regular);
                _timerElement.AddToClassList(TimerClasses.Flash);
                isFlashOn = true;
            }
            else if (elapsed % 1 >= animationDuration && isFlashOn)
            {
                _timerElement.RemoveFromClassList(TimerClasses.Flash);
                _timerElement.AddToClassList(TimerClasses.Regular);
                isFlashOn = false;
            }

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _timerFlashRoutineRef = null;
    }

    // timer for testing purposes
    IEnumerator TimerTestRoutine(float totalTime)
    {
        float startTime = Time.time;
        float elapsed = 0;

        while (elapsed < totalTime)
        {
            elapsed = Time.time - startTime;
            UpdateTimerVisual(totalTime - elapsed);
            yield return new WaitForEndOfFrame();
        }
    }

}
