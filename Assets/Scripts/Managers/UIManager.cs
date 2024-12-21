using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Class for the UI Manager implemented as a singleton
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [Header("Interaction")]
        [Tooltip("Interaction Pop Up Prefab")]
        public GameObject interactionPopUp;
        public Image image;

        [Header("Equipped Item")]
        public Image itemIcon;
        public Sprite defaultIcon;
    
        private RectTransform _interactionRectTransform;
    

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (interactionPopUp != null)
                {
                    _interactionRectTransform = interactionPopUp.GetComponent<RectTransform>();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Toggle popup visibility
        /// </summary>
        /// <param name="show">Should the popup be visible</param>
        /// <param name="popUpWorldPosition">World position of the popup</param>
        /// <param name="sprite">Sprite</param>
        /// <param name="callback">Callback function for the popup</param>
        public void ToggleInteractionPopUp(bool show, Vector3 popUpWorldPosition, Sprite sprite = null, Action callback = null) // DELEGATES ????
        {
            if (interactionPopUp != null)
            {
                interactionPopUp.SetActive(show);
                if (image != null && sprite != null)
                {
                    image.sprite = sprite;
                }
                
                // Display the pop up
                if (show && _interactionRectTransform != null)
                {
                    Vector3 screenPos = UnityEngine.Camera.main.WorldToScreenPoint(popUpWorldPosition);

                    _interactionRectTransform.position = screenPos;
                }
                
                // Assign the callback to interaction button
                InteractButtonController buttonController = interactionPopUp.gameObject.GetComponent<InteractButtonController>();
                if (show && !buttonController.InteractionIsAssigned())
                {
                    Debug.Log("[UIManager] Assigning callback");
                    buttonController.ButtonPressed += callback;
                }
                else if (!show && buttonController.InteractionIsAssigned())
                {
                    Debug.Log("[UIManager] Unassigning callback");
                    buttonController.ResetAllListeners();
                }
            }
        }
    
    
    

        public void UpdateEquippedItemUI(Sprite itemSprite)
        {
            if (itemIcon != null)
            {
                if (itemSprite != null)
                {
                    itemIcon.sprite = itemSprite;
                }
                else
                {
                    itemIcon.sprite = defaultIcon;
                }
            }
        }
    }
}
