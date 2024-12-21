using System;

namespace Upgrades
{
    /// <summary>
    /// Interface for the upgrade blocks
    /// </summary>
    public interface IUpgradeBlock
    {
        string UpgradeName { get; } 
        int CurrentLevel { get; }   
        int MaxLevel { get; }       
        int CurrentPrice { get; }   

        Action OnUpgradeApplied { get; set; } 

        void ApplyUpgrade(); 
        void ResetUpgrade();       
    }
}
