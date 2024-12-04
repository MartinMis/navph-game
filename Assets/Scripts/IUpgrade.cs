﻿using UnityEngine;

namespace Assets.Scripts
{
    public interface IUpgrade
    {
        string Key { get; }           
        string Name { get; }          
        Sprite Icon { get; }          
        int BasePrice { get; }
        
        void ApplyEffect();
    }
}