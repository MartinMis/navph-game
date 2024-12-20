using UnityEngine;

namespace Upgrades
{
    public interface IUpgrade
    {
        UpgradeKey Key { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        int BasePrice { get; }

        void ApplyEffect();
        void SetCurrentLevel(int level);
    }
}
