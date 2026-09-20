using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private UIDocument UIDoc;
    private Button _startButton, _quitButton;

    private void Awake()
    {
        _startButton = UIDoc.rootVisualElement.Q<Button>("StartButton");
        _quitButton = UIDoc.rootVisualElement.Q<Button>("QuitButton");

        _startButton.clicked += OnStartButtonPress;
        _quitButton.clicked += OnQuitButtonPress;
    }

    private void OnStartButtonPress()
    {
        GameState.Instance.StartGame();
    }

    private void OnQuitButtonPress()
    {
        Application.Quit();
    }

}
