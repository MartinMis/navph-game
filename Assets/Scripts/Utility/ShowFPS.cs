using TMPro;
using UnityEngine;

namespace Utility
{
    public class ShowFPS : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        void Update()
        {
            _text.text = "FPS: " + ((int)(1f / Time.unscaledDeltaTime));
        }
    }
}
