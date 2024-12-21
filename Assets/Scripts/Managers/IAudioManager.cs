namespace Managers
{
    /// <summary>
    /// Inteface describing the audio manager
    /// </summary>
    public interface IAudioManager
    {
        void SetMusicVolume(float volume);
        void SetSFXVolume(float volume);
        float GetMusicVolume();
        float GetSFXVolume();
    }

}
