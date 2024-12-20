using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "NewButtonConfig", menuName = "UI/ButtonConfig")]
    public class ButtonConfig : ScriptableObject
    {
        public Sprite normalSprite;  // normal state
        public Sprite activeSprite;  // active state, when clicked
        public string targetScene;   // target scene to load after click
    }
}
