using Assets.Scripts;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager
{
    public static IAudioManager Instance { get; private set; }

    private float musicVolume = 1f;
    private float sfxVolume = 1f;

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

    public float GetMusicVolume() => musicVolume;

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        Debug.Log($"[AudioManager] Music volume set to {volume}");
    }

    public float GetSFXVolume() => sfxVolume;

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        Debug.Log($"[AudioManager] SFX volume set to {volume}");
    }
}
