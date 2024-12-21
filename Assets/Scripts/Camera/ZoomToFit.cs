using UnityEngine;

namespace Camera
{
    /// <summary>
    /// Zooms the camera so that the game always fills the whole width of the screen
    /// </summary>
    public class ZoomToFit : MonoBehaviour
    {
        [Tooltip("Object that should fill the screen")]
        [SerializeField] private Transform targetObject;
        
        [Tooltip("Optional padding")]
        [SerializeField] private float padding;
        void Start()
        {
            var screenAspect = Screen.width / (float)Screen.height;
            var targetWidth = targetObject.GetComponent<Renderer>().bounds.size.x + padding;
            GetComponent<UnityEngine.Camera>().orthographicSize = targetWidth / (2*screenAspect);
        }
    }
}
