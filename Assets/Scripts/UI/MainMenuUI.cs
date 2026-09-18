using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] UIDocument UIDoc;
    [Tooltip("Which scene to load on start button press")]
    [SerializeField] string sceneLoadOnStart;
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
        if (sceneLoadOnStart.Length == 0)
        {
            Debug.LogError($"Empty string. Enter a scene to load in the inspector for {name}.");
            return;
        }
        SceneManager.LoadScene(sceneLoadOnStart);
    }

    void OnQuitButtonPress()
    {
        Application.Quit();
    }

}
