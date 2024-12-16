using Assets.Scripts;
using UnityEngine;

public class UpgradeUsageExample : MonoBehaviour
{
    public float playerSpeed = 10f;

    private void Start()
    {
        var upgradeManager = UpgradeManager.Instance;
        var speedUpgrade = upgradeManager.GetUpgradeByKey(UpgradeKey.Slippers);
        if (speedUpgrade != null)
        {
            
            speedUpgrade.ApplyEffect();

            if (speedUpgrade is PlayerSpeedUpgrade psu)
            {
                playerSpeed *= psu.SpeedMultiplier;
                Debug.Log($"New player speed: {playerSpeed}");
            }
        }
    }
}