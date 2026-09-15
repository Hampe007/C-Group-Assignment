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
struct TimerData
{
    [Header("These fields are meant for testing purposes")]
    public bool IsTest;
    public float StartTime;
}
public class LvlOverlayUI : MonoBehaviour {
    [SerializeField] UIDocument UIDoc;
    [Tooltip("Which scene to load on quit button press")]
    [SerializeField] string mainMenuScene;
    [SerializeField] TimerData timerData;
    VisualElement _timerElement;
    VisualElement _gameOverOverlay;
    Label _timerLabel;
    Button _retryButton;
    Coroutine _timerFlashRoutineRef;

    void Awake()
    {
        _timerLabel = UIDoc.rootVisualElement.Q<Label>("TimerLabel");
        _timerElement = UIDoc.rootVisualElement.Q<VisualElement>("TimerElement");
        _gameOverOverlay = UIDoc.rootVisualElement.Q<VisualElement>("GameOverOverlay");
        _retryButton = UIDoc.rootVisualElement.Q<Button>("RetryButton");

        // there are two quit buttons: one in the pause menu and one in the game over
        UIDoc.rootVisualElement.Query<Button>("QuitButton").ForEach(button =>
        {
            button.clicked += OnQuitButtonPress;
        });;
        _retryButton.clicked += OnRetryButtonPress;
    }

    void Start()
    {
        if (timerData.IsTest)
        {
            StartCoroutine(TimerTestRoutine(timerData.StartTime));
        }
        StartCoroutine(TimerEmphasizeRoutine());
    }


    void SetTime(float time)
    {
        if (_timerFlashRoutineRef == null && time <= 10  && time > 0)
        {
            _timerFlashRoutineRef = StartCoroutine(TimerFlashRoutine(10));
        }
        else if (time <= 0)
        {
            // TODO: invoke GameOver() method which will probably be created at some point
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
        if (timerData.IsTest)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(mainMenuScene);
    }

    void OnRetryButtonPress()
    {
        if (timerData.IsTest)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnGameOver()
    {
        if (timerData.IsTest)
        {
            Time.timeScale = 0;
        }
        _gameOverOverlay.visible = true;
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

    IEnumerator TimerTestRoutine(float totalTime)
    {
        float startTime = Time.time;
        float elapsed = 0;

        while (elapsed < totalTime)
        {
            elapsed = Time.time - startTime;
            SetTime(totalTime - elapsed);
            yield return new WaitForEndOfFrame();
        }
    }

}
