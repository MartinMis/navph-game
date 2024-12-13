using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
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
