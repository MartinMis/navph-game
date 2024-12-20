using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    /// <summary>
    /// Singleton manager class for working with audio. Implements <c>IAudioManager</c> interface.
    /// </summary>
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private AudioMixer audioMixer;
        public static IAudioManager Instance { get; private set; }
    
        // Private variables
        private float _musicVolume = 0f;
        private float _sfxVolume = 0f;
    
        // Initialize as an Singleton
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
            _sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0f);
            audioMixer.SetFloat("MusicVolume", CalculateDecibels(_musicVolume));
            audioMixer.SetFloat("SFXVolume", CalculateDecibels(_sfxVolume));
        }

        public float GetMusicVolume() => _musicVolume;

        /// <summary>
        /// Callback for setting the music volume.
        /// </summary>
        /// <param name="volume">Volume to be set with linear scale</param>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = volume;
            audioMixer.SetFloat("MusicVolume", CalculateDecibels(volume));
        }

        public float GetSFXVolume() => _sfxVolume;
    
        /// <summary>
        /// Setter for SFX Volume.
        /// </summary>
        /// <param name="volume">Desired volume on a linear scale</param>
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = volume;
            audioMixer.SetFloat("SFXVolume", CalculateDecibels(volume));
        }

        /// <summary>
        /// Function to recalculate volume. Sliders output linear values while decibels are a logarithmic scale. This
        /// function provides conversion between the two.
        /// </summary>
        /// <param name="volume">Desired volume on linear scale</param>
        /// <returns>Volume in decibels</returns>
        private static float CalculateDecibels(float volume)
        {
            return volume > 0f ? Mathf.Log10(volume) * 20 : -80.0f;
        }
    }
}
