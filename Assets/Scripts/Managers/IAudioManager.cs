using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IAudioManager
    {
        void SetMusicVolume(float volume);
        void SetSFXVolume(float volume);
        float GetMusicVolume();
        float GetSFXVolume();
    }

}
