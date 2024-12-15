using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Interaction")]
    public GameObject interaction;
    public Image image;

    [Header("Equipped Item")]
    public Image itemIcon;
    public Sprite defaultIcon;

    private Camera mainCamera;
    private RectTransform interactionRectTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            mainCamera = Camera.main;

            if (interaction != null)
            {
                interactionRectTransform = interaction.GetComponent<RectTransform>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInteraction(bool show, Vector3 worldPosition, Sprite sprite = null) // DELEGATES ????
    {
        if (interaction != null)
        {
            interaction.SetActive(show);
            if (image != null && sprite != null)
            {
                image.sprite = sprite;
            }

            if (show && interactionRectTransform != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);

                interactionRectTransform.position = screenPos;
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
