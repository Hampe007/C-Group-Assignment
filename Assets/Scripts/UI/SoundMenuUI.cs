using UnityEngine;
using UnityEngine.UIElements;

public class SoundMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument UIDoc;
    private Slider _masterSlider, _musicSlider, _soundFXSlider;
    private Button _soundOptionsButton, _returnButton;
    private VisualElement _mainContainer, _soundOptionsContainer;

    private void Awake()
    {
        _mainContainer = UIDoc.rootVisualElement.Q<VisualElement>("MainContainer");
        _soundOptionsContainer = UIDoc.rootVisualElement.Q<VisualElement>("SoundOptionsContainer");
        _masterSlider = UIDoc.rootVisualElement.Q<Slider>("MasterSlider");
        _musicSlider = UIDoc.rootVisualElement.Q<Slider>("MusicSlider");
        _soundFXSlider = UIDoc.rootVisualElement.Q<Slider>("SoundFXSlider");
        _soundOptionsButton = UIDoc.rootVisualElement.Q<Button>("SoundOptionsButton");
        _returnButton = UIDoc.rootVisualElement.Q<Button>("ReturnButton");

        _masterSlider.RegisterValueChangedCallback(evt => OnMasterSliderValueChanged(evt.newValue));
        _musicSlider.RegisterValueChangedCallback(evt => OnMusicSliderValueChanged(evt.newValue));
        _soundFXSlider.RegisterValueChangedCallback(evt => OnSoundFXSliderValueChanged(evt.newValue));
        _soundOptionsButton.clicked += OnSoundOptionsButtonPressed;
        _returnButton.clicked += OnReturnButtonPressed;
    }

    private void Start()
    {
        _masterSlider.value = SoundMixerManager.Instance.MasterVolume;
        _musicSlider.value = SoundMixerManager.Instance.MusicVolume;
        _soundFXSlider.value = SoundMixerManager.Instance.SoundFXVolume;
    }

    private void OnMasterSliderValueChanged(float volume)
    {
        SoundMixerManager.Instance.SetMasterVolume(volume);
    }

    private void OnMusicSliderValueChanged(float volume)
    {
        SoundMixerManager.Instance.SetMusicVolume(volume);
    }

    private void OnSoundFXSliderValueChanged(float volume)
    {
        SoundMixerManager.Instance.SetSoundFXVolume(volume);
    }

    private void OnSoundOptionsButtonPressed()
    {
        _mainContainer.visible = false;
        _soundOptionsContainer.visible = true;
    }

    private void OnReturnButtonPressed()
    {
        _soundOptionsContainer.visible = false;
        _mainContainer.visible = true;
    }
}
