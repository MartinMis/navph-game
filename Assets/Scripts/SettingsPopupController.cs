using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopupController : MonoBehaviour
{
    public GameObject settingsPopupPrefab; // Prefab pre SettingsPopup
    private GameObject settingsPopupInstance;

    private IAudioManager audioManager;

    private void Awake()
    {
       
        audioManager = AudioManager.Instance;

        if (audioManager == null)
        {
            Debug.LogError("[SettingsPopupController] AudioManager not found!");
        }
    }

    public void OpenSettingsPopup()
    {
        if (settingsPopupInstance == null)
        {
            settingsPopupInstance = Instantiate(settingsPopupPrefab, transform);

            // find sliders in popup prefab
            var musicSlider = settingsPopupInstance.transform.Find("popupBlock/SlidersBlock/MusicSlider")?.GetComponent<Slider>();
            var sfxSlider = settingsPopupInstance.transform.Find("popupBlock/SlidersBlock/SFXSlider")?.GetComponent<Slider>();

            if (musicSlider != null)
            {
                musicSlider.value = audioManager.GetMusicVolume();
                musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
                Debug.Log("[SettingsPopupController] MusicSlider connected.");
            }
            else
            {
                Debug.LogWarning("[SettingsPopupController] MusicSlider not found in popup.");
            }

            if (sfxSlider != null)
            {
                sfxSlider.value = audioManager.GetSFXVolume();
                sfxSlider.onValueChanged.AddListener(audioManager.SetSFXVolume);
                Debug.Log("[SettingsPopupController] SFXSlider connected.");
            }
            else
            {
                Debug.LogWarning("[SettingsPopupController] SFXSlider not found in popup.");
            }

            // find close button in popup prefab
            var closeButton = GameObject.FindGameObjectWithTag("closeBtn")?.GetComponent<Button>();

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(CloseSettingsPopup);
                Debug.Log("[SettingsPopupController] Close Button connected.");
            }
            else
            {
                Debug.LogWarning("[SettingsPopupController] Close Button not found in popup.");
            }
        }
        // if popup is already created, just show it
        else
        {
            settingsPopupInstance.SetActive(true);
        }
    }

    public void CloseSettingsPopup()
    {
        if (settingsPopupInstance != null)
        {
            settingsPopupInstance.SetActive(false);
        }
    }
    
}
