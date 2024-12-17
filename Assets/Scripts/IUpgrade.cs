using UnityEngine;

namespace Assets.Scripts
{
    public interface IUpgrade
    {
        UpgradeKey Key { get; }
        string Name { get; }
        Sprite Icon { get; }
        int BasePrice { get; }

        void ApplyEffect();
        void SetCurrentLevel(int level);
    }
}
