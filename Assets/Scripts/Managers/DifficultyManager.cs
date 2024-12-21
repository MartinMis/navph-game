using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manager of the difficulty implemented as a singleton
    /// </summary>
    public class DifficultyManager : MonoBehaviour
    {
        [Tooltip("Difficulty in the first level")]
        [SerializeField] private int initialDifficulty;
        
        [Tooltip("How many more things should spawn with each diificulty level")]
        [SerializeField] private float hallwaySpawnRateScaling = 0.5f;
        
        [Tooltip("How much should the hallway extend with each difficulty level")]
        [SerializeField] private float hallwayLenghtScaling= 0.5f;
        
        [Tooltip("How much longer should the lights be with each difficulty level")]
        [SerializeField] private float lightSizeScaling = 0.5f;
    
        public static DifficultyManager Instance { get; private set; }
        public int CurrentDifficulty { get; private set; }
        public float HallwaySpawnRateCoeficient {get; private set;}
        public float HallwayLengthCoeficient {get; private set;}
        public float LightSizeCoeficient {get; private set;}
    
        void Awake()
        {
            DontDestroyOnLoad(this);
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        
            CurrentDifficulty = initialDifficulty;
            RecalculateCoeficients();
        }
        
        /// <summary>
        /// Method for increasing the difficulty level
        /// </summary>
        public void IncreaseDifficulty()
        {
            CurrentDifficulty++;
            RecalculateCoeficients();
        }
        
        /// <summary>
        /// Method for recalculating the difficulty coeficients
        /// </summary>
        void RecalculateCoeficients()
        {
            HallwaySpawnRateCoeficient = 1 + hallwaySpawnRateScaling * (CurrentDifficulty-1);
            HallwayLengthCoeficient = 1 + hallwayLenghtScaling * (CurrentDifficulty-1);
            LightSizeCoeficient = 1 + lightSizeScaling * (CurrentDifficulty-1);
        }
    }
}
