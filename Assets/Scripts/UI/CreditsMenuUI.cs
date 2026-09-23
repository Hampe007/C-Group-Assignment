using UnityEngine;
using UnityEngine.UIElements;

public class CreditsMenuUI : MonoBehaviour {
    [SerializeField] private UIDocument UIDoc;
    private SO_SoundEffectData _buttonPressSound;
    private Button _creditsMenuButton, _returnButton;
    private VisualElement _mainContainer, _creditsMenuContainer;

    private void Awake()
    {
        _mainContainer = UIDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        _creditsMenuContainer = UIDoc.rootVisualElement.Q<VisualElement>("CreditsMenuContainer");
        _creditsMenuButton = UIDoc.rootVisualElement.Q<Button>("CreditsMenuButton");
        _returnButton = UIDoc.rootVisualElement.Q<Button>("CreditsReturnButton"); // TODO: Make overlay switching generic.

        _returnButton.clicked += OnReturnButtonPressed;
        _creditsMenuButton.clicked += OnCreditsMenuButtonPressed;
    }

    private void Start()
    {
        _buttonPressSound = AudioUtils.SoundEffects.buttonPressSound;
    }

    private void OnCreditsMenuButtonPressed()
    {
        _mainContainer.visible = false;
        _creditsMenuContainer.visible = true;
        SoundFXManager.Instance.PlaySoundFXClip(_buttonPressSound.audioClip, transform, _buttonPressSound.volume);
    }

    private void OnReturnButtonPressed()
    {
       _creditsMenuContainer.visible = false;
        _mainContainer.visible = true;
        SoundFXManager.Instance.PlaySoundFXClip(_buttonPressSound.audioClip, transform, _buttonPressSound.volume);
    }

}
