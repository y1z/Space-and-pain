using UnityEngine;


namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "LevelGeneratorData", menuName = "Scriptable Objects/LevelGeneratorData")]
    public sealed class LevelGeneratorData : ScriptableObject
    {

        [field: Header("Player Stats")]
        [field: SerializeField, Tooltip("How many live the player will start with"), Space(5.0f)]
        public int playerLives { get; private set; } = 3;

        [field: SerializeField, Tooltip("How many shots can the player take at the same time")]
        public int playerMaxShots { get; private set; } = 2;

        [field: SerializeField, Tooltip("The starting position of the player")]
        public Vector2 playerStartPosition { get; private set; } = Vector2.zero;

        [field: Header("Grid of enemies")]
        [field: SerializeField, Tooltip("How many columns of enemies the level has"), Space(5.0f)]
        public int enemiesColumnCount { get; private set; } = 6;

        [field: SerializeField, Tooltip("How many rows of enemies the level has")]
        public int enemiesRowsCount { get; private set; } = 6;

        [field: Header("Enemy Movement")]
        [field: SerializeField, Tooltip("[lower is faster] How long it take to move 1 enemy"), Space(5.0f)]
        [Range(0f, 1f)]
        public float howLongUntilNextEnemyMovementRateStart { get; private set; } = 0.12f;

        [field: SerializeField, Tooltip("[lower is faster] How long does it take to move enemies down (relative to howLongUntilNextEnemyMovementRateStart) ")]
        [Range(0f, 1f)]
        public float moveDownFactorSpeedUpRateStart { get; private set; } = 0.4f;

        [field: Header("Next Round Speed up factor")]
        [field: SerializeField, Tooltip("[Lower is faster]How much to speed up enemy movement the next round "), Space(5.0f)]
        [Range(0f, 1f)]
        public float howLongUntilNextEnemyMovementSpeedUpRate { get; private set; } = 0.95f;

        [field: SerializeField, Tooltip("[Lower is faster]How much to speed up the enemy movement when going down next round")]
        [Range(0f, 1f)]
        public float moveDownFactorSpeedUpSpeedUpRate { get; private set; } = 1.0f;


        [Header("Bunkers ")]
        [field: SerializeField, Tooltip("The amount of bunkers"), Min(0), Space(5.0f)]
        public int bunkerAmount;

        [field: SerializeField, Tooltip("Controls were the bunker will be located")]
        public Rect bunkerArea { get; private set; }

        [field: SerializeField, Header("Define Enemy spawn area"), Tooltip("The area were enemies will spawn")]
        public Rect enemySpawnArea { get; private set; }
    }

}
