using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Class controlling the settings pop up
    /// </summary>
    public class SettingsPopupController : MonoBehaviour
    {
        public GameObject settingsPopupPrefab; // Prefab pre SettingsPopup
        private GameObject _settingsPopupInstance;

        private IAudioManager _audioManager;

        private void Awake()
        {
            _audioManager = AudioManager.Instance;

            if (_audioManager == null)
            {
                Debug.LogError("[SettingsPopupController] AudioManager not found!");
            }
        }
        
        /// <summary>
        /// Method for opening the setting pop up
        /// </summary>
        public void OpenSettingsPopup()
        {
            if (_settingsPopupInstance == null)
            {
                _settingsPopupInstance = Instantiate(settingsPopupPrefab, transform);

                // find sliders in popup prefab
                var musicSlider = _settingsPopupInstance.transform.Find("popupBlock/SlidersBlock/MusicSlider")?.GetComponent<Slider>();
                var sfxSlider = _settingsPopupInstance.transform.Find("popupBlock/SlidersBlock/SFXSlider")?.GetComponent<Slider>();

                if (musicSlider != null)
                {
                    musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", _audioManager.GetMusicVolume());
                    _audioManager.SetMusicVolume(musicSlider.value);
                    musicSlider.onValueChanged.AddListener(volume => {
                        _audioManager.SetMusicVolume(volume);
                        PlayerPrefs.SetFloat("MusicVolume", volume);
                    });
                    Debug.Log("[SettingsPopupController] MusicSlider connected.");
                }
                else
                {
                    Debug.LogWarning("[SettingsPopupController] MusicSlider not found in popup.");
                }

                if (sfxSlider != null)
                {
                    sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", _audioManager.GetSFXVolume());
                    _audioManager.SetSFXVolume(sfxSlider.value);
                    sfxSlider.onValueChanged.AddListener(volume => {
                        _audioManager.SetSFXVolume(volume);
                        PlayerPrefs.SetFloat("SFXVolume", volume);
                    });
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
                _settingsPopupInstance.SetActive(true);
            }
        }
        
        /// <summary>
        /// Method for closing the setting pop up
        /// </summary>
        public void CloseSettingsPopup()
        {
            if (_settingsPopupInstance != null)
            {
                _settingsPopupInstance.SetActive(false);
            }
        }
    }
}
