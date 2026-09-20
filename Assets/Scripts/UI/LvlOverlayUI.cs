using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// TODO: remove all testing-related code

static class TimerClasses {
    public static string Emphasized => "timerEmphasized";
    public static string Regular => "timerRegular";
    public static string Flash => "timerFlash";
}

static class ScoreClasses {
    public static string Emphasized => "scoreEmphasized";
    public static string Regular => "scoreRegular";
}

[System.Serializable]
struct TestingData
{
    public bool TestTime;
    public bool TestPoints;
    public float StartTime;
    public bool isGamePaused;
    public bool isGameOver;
}
public class LvlOverlayUI : MonoBehaviour {
    [SerializeField] private UIDocument UIDoc;
    [Tooltip("Which scene to load on quit button press")]
    [SerializeField] private string sceneLoadOnQuit;
    [SerializeField] private SoundFXClip buttonPressSound;
    [SerializeField] private TestingData testingData;
    private VisualElement _timerElement, _scoreElement;
    private VisualElement _gamePausedMainContainer, _statsContainer;
    private VisualElement _gameOverOverlay, _gamePausedOverlay;
    private Label _timerLabel, _pointsLabel, _largePointsLabel;
    private Coroutine _timerFlashRoutineRef;

    private void Awake()
    {
        _timerElement = UIDoc.rootVisualElement.Q<VisualElement>("TimerElement");
        _timerLabel = UIDoc.rootVisualElement.Q<Label>("TimerLabel");
        _scoreElement = UIDoc.rootVisualElement.Q<VisualElement>("ScoreElement");
        _statsContainer = UIDoc.rootVisualElement.Q<VisualElement>("StatsContainer");
        _pointsLabel = UIDoc.rootVisualElement.Q<Label>("PointsLabel");
        _largePointsLabel = UIDoc.rootVisualElement.Q<Label>("LargePointsLabel");
        _gameOverOverlay = UIDoc.rootVisualElement.Q<VisualElement>("GameOverOverlay");
        _gamePausedOverlay = UIDoc.rootVisualElement.Q<VisualElement>("GamePausedOverlay");
        _gamePausedMainContainer = UIDoc.rootVisualElement.Q<VisualElement>("MainContainer");

        UIDoc.rootVisualElement.Query<Button>("RetryButton").ForEach(button =>
        {
            button.clicked += OnRetryButtonPress;
        });;
        UIDoc.rootVisualElement.Query<Button>("QuitButton").ForEach(button =>
        {
            button.clicked += OnQuitButtonPress;
        });;
        UIDoc.rootVisualElement.Q<Button>("ResumeButton").clicked += OnResumeButtonPress;
    }

    private void Start()
    {
        if (testingData.TestTime)
        {
            StartCoroutine(TimerTestRoutine(testingData.StartTime));
        }
        if (testingData.TestPoints)
        {
            StartCoroutine(ScoreTestRoutine(2f));
        }

        StartCoroutine(TimerEmphasizeRoutine());
        GameState.Instance.ScoreChangeEvent.AddListener(UpdatePointsVisual);
        GameState.Instance.GamePausedEvent.AddListener(OnGamePaused);
        GameState.Instance.GameUnPausedEvent.AddListener(OnGameResumed);
        GameState.Instance.GameOverEvent.AddListener(OnGameOver);
    }

    private void OnValidate()
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
    }

    private void Update()
    {
        UpdateTimerVisual(GameState.Instance.TimeRemaining);
    }


    private void UpdateTimerVisual(float time)
    {
        if (_timerFlashRoutineRef == null && time <= 10  && time > 0)
        {
            _timerFlashRoutineRef = StartCoroutine(TimerFlashRoutine(10));
        }
        else if (testingData.TestTime && time <= 0)
        {
            OnGameOver();
        }

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        string minutesText = minutes >= 10 ? minutes.ToString() : $"0{minutes}";
        string secondsText = seconds >= 10 ? seconds.ToString() : $"0{seconds}";

        _timerLabel.text = $"{minutesText}:{secondsText}";
    }

    private void UpdatePointsVisual(uint scoreIncrease, uint newScore)
    {
        StartCoroutine(ScoreEmphasizeRoutine());
        _pointsLabel.text = newScore.ToString();
        _largePointsLabel.text = _pointsLabel.text = newScore.ToString();
    }

    private void OnResumeButtonPress()
    {
        SoundFXManager.Instance.PlaySoundFXClip(buttonPressSound.audioClip, transform, buttonPressSound.volume);
        GameState.Instance.SetIsPaused(false);
    }

    private void OnQuitButtonPress()
    {
        if (testingData.TestTime)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(sceneLoadOnQuit);
        SoundFXManager.Instance.PlaySoundFXClip(buttonPressSound.audioClip, transform, buttonPressSound.volume);
        GameState.Instance.Reset();
    }

    private void OnRetryButtonPress()
    {
        if (testingData.TestTime)
        {
            Time.timeScale = 1;
            testingData.isGameOver = false;
            testingData.isGamePaused = false;
        }
        SoundFXManager.Instance.PlaySoundFXClip(buttonPressSound.audioClip, transform, buttonPressSound.volume);
        GameState.Instance.StartGame();
    }

    private void OnGameOver()
    {
        if (testingData.TestTime)
        {
            Time.timeScale = 0;
        }
        _gameOverOverlay.visible = true;
        _statsContainer.visible = false;
    }

    private void OnGamePaused()
    {
        if (testingData.TestTime)
        {
            Time.timeScale = 0;
        }
        _gamePausedMainContainer.visible = true;
        _gamePausedOverlay.visible = true;
    }

    private void OnGameResumed()
    {
        if (_gamePausedOverlay == null)
        {
            return;
        }
        if (testingData.TestTime)
        {
            Time.timeScale = 1;
            testingData.isGamePaused = false;
        }
        _gamePausedMainContainer.visible = false;
        _gamePausedOverlay.visible = false;
    }

    private IEnumerator ScoreEmphasizeRoutine()
    {
        _scoreElement.AddToClassList(ScoreClasses.Emphasized);
        _scoreElement.RemoveFromClassList(ScoreClasses.Regular);

        yield return new WaitForEndOfFrame();

        _scoreElement.AddToClassList(ScoreClasses.Regular);
        _scoreElement.RemoveFromClassList(ScoreClasses.Emphasized);
    }

    private IEnumerator TimerEmphasizeRoutine()
    {
        _timerElement.AddToClassList(TimerClasses.Emphasized);
        _timerElement.RemoveFromClassList(TimerClasses.Regular);

        yield return new WaitForEndOfFrame();

        _timerElement.AddToClassList(TimerClasses.Regular);
        _timerElement.RemoveFromClassList(TimerClasses.Emphasized);
    }

    private IEnumerator TimerFlashRoutine(float duration)
    {
        float elapsed = 0;

        // AnimationDuration corresponds to animation duration property of the visual element.
        float animationDuration = .35f;
        bool isFlashOn = false;

        while (elapsed < duration)
        {
            if (elapsed % 1 >= 0 && elapsed % 1 < animationDuration && !isFlashOn)
            {
                _timerElement.AddToClassList(TimerClasses.Flash);
                _timerElement.RemoveFromClassList(TimerClasses.Regular);
                isFlashOn = true;
            }
            else if (elapsed % 1 >= animationDuration && isFlashOn)
            {
                _timerElement.AddToClassList(TimerClasses.Regular);
                _timerElement.RemoveFromClassList(TimerClasses.Flash);
                isFlashOn = false;
            }

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _timerFlashRoutineRef = null;
    }

    private IEnumerator TimerTestRoutine(float totalTime)
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

    private IEnumerator ScoreTestRoutine(float interval)
    {
        int points = 0;
        while (Application.isPlaying)
        {
            int scoreIncrease = UnityEngine.Random.Range(1, 6) * 100;
            points += scoreIncrease;
            UpdatePointsVisual((uint)scoreIncrease, (uint)points);
            yield return new WaitForSeconds(interval);
        }
    }

}
