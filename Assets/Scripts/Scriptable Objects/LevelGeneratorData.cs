using UnityEngine;


namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "LevelGeneratorData", menuName = "Scriptable Objects/LevelGeneratorData")]
    public sealed class LevelGeneratorData : ScriptableObject
    {

        [Header("Grid of enemies")]
        [Tooltip("How many columns of enemies the level has")]
        public int enemiesColumnCount = 6;
        [Tooltip("How many rows of enemies the level has")]
        public int enemiesRowsCount = 6;

        [Header("Enemy Movement")]
        [Tooltip("[lower is faster] How long it take to move 1 enemy")]
        [Range(0f, 1f)]
        public float howLongUntilNextEnemyMovementRateStart = 0.12f;
        [Tooltip("[lower is faster] How long does it take to move enemies down (relative to howLongUntilNextEnemyMovementRateStart) ")]
        [Range(0f, 1f)]
        public float moveDownFactorSpeedUpRateStart = 0.4f;

        [Header("Next Round Speed up")]
        [Tooltip("[Lower is faster]How much to speed up enemy movement the next round ")]
        [Range(0f, 1f)]
        public float howLongUntilNextEnemyMovementSpeedUpRate = 0.95f;
        [Tooltip("[Lower is faster]How much to speed up the enemy movement when going down next round")]
        [Range(0f, 1f)]
        public float moveDownFactorSpeedUpSpeedUpRate = 1.0f;


        [Header("Bunkers ")]
        [Tooltip("The amount of bunkers"), Min(0)]
        public int bunkersAmount;
        [field: SerializeField, Tooltip("Controls were the bunker will be located")]
        public Rect bunkerArea { get; private set; }


        [field: SerializeField, Header("Define Enemy spawn area"), Tooltip("The area were enemies will spawn")]
        public Rect enemySpawnArea { get; private set; }
    }

}
