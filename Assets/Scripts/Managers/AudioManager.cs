using UnityEngine;
using UnityEngine.Audio;

namespace Managers
{
    /// <summary>
    /// Singleton manager class for working with audio. Implements <c>IAudioManager</c> interface.
    /// </summary>
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [Tooltip("AudioMixer to use")]
        [SerializeField] private AudioMixer audioMixer;
        public static IAudioManager Instance { get; private set; }
    
        // Private variables
        private float _musicVolume;
        private float _sfxVolume ;
    
        // Initialize as an Singleton
        public void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
            _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
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
        /// function provides conversion between the two. Inspired by https://discussions.unity.com/t/how-to-calculate-db-correct/712114/2
        /// </summary>
        /// <param name="volume">Desired volume on linear scale</param>
        /// <returns>Volume in decibels</returns>
        private static float CalculateDecibels(float volume)
        {
            return volume > 0f ? Mathf.Log10(volume) * 20 : -80.0f;
        }
    }
}
