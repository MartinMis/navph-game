namespace Managers
{
    public interface IAudioManager
    {
        void SetMusicVolume(float volume);
        void SetSFXVolume(float volume);
        float GetMusicVolume();
        float GetSFXVolume();
    }

}
