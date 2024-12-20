using System.Collections;
using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class UpgradePopUpController : MonoBehaviour
    {
        [Tooltip("For how many seconds should the pop-up appear")]
        [SerializeField] private float displayTime = 2;
    
        private PlayerController _playerController;
        private Text _text;
        void Start()
        {
            // Assign variables
            _text = GetComponent<Text>();
            _playerController = GameObject.FindGameObjectWithTag(Tags.Player)?.GetComponent<PlayerController>();
            // Verify the variables aren't null
            if (_text == null)
            {
                Debug.LogError("[UpgradePopUpController] Text component not found");
                return;
            }
            if (_playerController == null)
            {
                Debug.LogError("[UpgradePopUpController] Player controller not found");
                return;
            }
            // Disable the visuals and assign callback
            _text.enabled = false;
            _playerController.OnItemEquipped += AnnounceUpgrade;
        }
    
        /// <summary>
        /// Callback that is executed whenever player picks up an item.
        /// </summary>
        /// <param name="upgradeDescription">Text description of the effect of the picked up item</param>
        void AnnounceUpgrade(string upgradeDescription)
        {
            _text.enabled = true;
            _text.text = upgradeDescription;
            StartCoroutine(Disapear(displayTime));
        }
    
        IEnumerator Disapear(float time)
        {
            yield return new WaitForSeconds(time);
            _text.enabled = false;
        }

        void OnDestroy()
        {
            _playerController.OnItemEquipped -= AnnounceUpgrade;
        }
    }
}
