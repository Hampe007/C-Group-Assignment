using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] UIDocument UIDoc;
    [Tooltip("Which scene to load on start button press")]
    [SerializeField] string sceneToLoad;
    Button _startButton, _quitButton;

    void Awake()
    {
        _startButton = UIDoc.rootVisualElement.Q<Button>("StartButton");
        _quitButton = UIDoc.rootVisualElement.Q<Button>("QuitButton");

        _startButton.clicked += OnStartButtonPress;
        _quitButton.clicked += OnQuitButtonPress;
    }

    void OnStartButtonPress()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnQuitButtonPress()
    {
        Application.Quit();
    }

    private void Start() {
    }

}
