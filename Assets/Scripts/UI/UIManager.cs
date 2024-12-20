using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [FormerlySerializedAs("interaction")] [Header("Interaction")]
        public GameObject interactionPopUp;
        public Image image;

        [Header("Equipped Item")]
        public Image itemIcon;
        public Sprite defaultIcon;
    
        private RectTransform interactionRectTransform;
    

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (interactionPopUp != null)
                {
                    interactionRectTransform = interactionPopUp.GetComponent<RectTransform>();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ToggleInteractionPopUp(bool show, Vector3 popUpWorldPosition, Sprite sprite = null, Action callback = null) // DELEGATES ????
        {
            if (interactionPopUp != null)
            {
                interactionPopUp.SetActive(show);
                if (image != null && sprite != null)
                {
                    image.sprite = sprite;
                }

                if (show && interactionRectTransform != null)
                {
                    Vector3 screenPos = UnityEngine.Camera.main.WorldToScreenPoint(popUpWorldPosition);

                    interactionRectTransform.position = screenPos;
                }

                EnterButtonController buttonController = interactionPopUp.gameObject.GetComponent<EnterButtonController>();
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
