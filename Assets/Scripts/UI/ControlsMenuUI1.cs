using UnityEngine;
using UnityEngine.UIElements;

public class ControlsMenuUI : MonoBehaviour {
    [SerializeField] private UIDocument UIDoc;
    private SO_SoundEffectData _buttonPressSound;
    private Button _controlsMenuButton, _returnButton;
    private VisualElement _mainContainer, _controlsMenuContainer;

    private void Awake()
    {
        _mainContainer = UIDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        _controlsMenuContainer = UIDoc.rootVisualElement.Q<VisualElement>("ControlsMenuContainer");
        _controlsMenuButton = UIDoc.rootVisualElement.Q<Button>("ControlsMenuButton");
        _returnButton = UIDoc.rootVisualElement.Q<Button>("ControlsReturnButton"); // TODO: Make overlay switching generic.

        _returnButton.clicked += OnReturnButtonPressed;
        _controlsMenuButton.clicked += OncontrolsMenuButtonPressed;
    }

    private void Start()
    {
        _buttonPressSound = AudioUtils.SoundEffects.buttonPressSound;
    }

    private void OncontrolsMenuButtonPressed()
    {
        _mainContainer.visible = false;
        _controlsMenuContainer.visible = true;
        SoundFXManager.Instance.PlaySoundFXClip(_buttonPressSound.audioClip, transform, _buttonPressSound.volume);
    }

    private void OnReturnButtonPressed()
    {
       _controlsMenuContainer.visible = false;
        _mainContainer.visible = true;
        SoundFXManager.Instance.PlaySoundFXClip(_buttonPressSound.audioClip, transform, _buttonPressSound.volume);
    }

}
