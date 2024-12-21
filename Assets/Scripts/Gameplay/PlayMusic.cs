using Triggers;
using UnityEngine;

namespace Gameplay
{
    /// <summary>
    /// Class to play music when player enters the room
    /// </summary>
    public class PlayMusic: MonoBehaviour
    {
        [Tooltip("Player enter trigger")]
        [SerializeField] private PlayerEnterTrigger playerEnterTrigger;
        
        private AudioSource _audioSource;
    
        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            playerEnterTrigger.OnTriggered += Play;
        }
        
        /// <summary>
        /// Method to start playing music
        /// </summary>
        void Play()
        {
            _audioSource.Play();
        }
    }
}