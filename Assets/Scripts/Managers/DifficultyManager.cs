using UnityEngine;

namespace Managers
{
    public class DifficultyManager : MonoBehaviour
    {
        [SerializeField] private int initialDifficulty;
        [SerializeField] private float hallwaySpawnRateScaling = 0.5f;
        [SerializeField] private float hallwayLenghtScaling= 0.5f;
        [SerializeField] private float lightSizeScaling = 0.5f;
    
        public static DifficultyManager Instance { get; private set; }
        public int currentDifficulty { get; private set; }
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
        
            currentDifficulty = initialDifficulty;
            RecalculateCoeficients();
        }

        public void IncreaseDifficulty()
        {
            currentDifficulty++;
            RecalculateCoeficients();
        }

        void RecalculateCoeficients()
        {
            HallwaySpawnRateCoeficient = 1 + hallwaySpawnRateScaling * (currentDifficulty-1);
            HallwayLengthCoeficient = 1 + hallwayLenghtScaling * (currentDifficulty-1);
            LightSizeCoeficient = 1 + lightSizeScaling * (currentDifficulty-1);
        }
    }
}
