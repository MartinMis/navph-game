using System.Collections;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    /// <summary>
    /// Class for the item pop up
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ItemPopUpController : MonoBehaviour
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
        
        /// <summary>
        /// Coroutine to disappear after some time. Inspired by https://discussions.unity.com/t/how-to-wait-a-certain-amount-of-seconds-in-c/192244
        /// </summary>
        /// <param name="time">How long to wait until disapperaing</param>
        IEnumerator Disapear(float time)
        {
            yield return new WaitForSeconds(time);
            _text.enabled = false;
        }
        
        // Unsubscribe from event when destroyed
        void OnDestroy()
        {
            _playerController.OnItemEquipped -= AnnounceUpgrade;
        }
    }
}
