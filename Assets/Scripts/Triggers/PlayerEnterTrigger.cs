using System;
using Utility;
using UnityEngine;


namespace Triggers
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerEnterTrigger : MonoBehaviour
    {
        private AudioSource _audioSource;
        public event Action OnTriggered;
    
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _audioSource.Play();
                OnTriggered?.Invoke();
            }
        }
    }
}

