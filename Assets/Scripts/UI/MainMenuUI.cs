using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private UIDocument UIDoc;
    [SerializeField] private SoundFXClip buttonPressSound;
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
        SoundFXManager.Instance.PlaySoundFXClip(buttonPressSound.audioClip, transform, buttonPressSound.volume);
    }

    private void OnQuitButtonPress()
    {
        Application.Quit();
        SoundFXManager.Instance.PlaySoundFXClip(buttonPressSound.audioClip, transform, buttonPressSound.volume);
    }

}
