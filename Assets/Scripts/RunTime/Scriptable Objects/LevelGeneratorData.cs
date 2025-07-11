using UnityEngine;
using TMPro;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "LevelGeneratorData", menuName = "Scriptable Objects/LevelGeneratorData")]
    public sealed class LevelGeneratorData : ScriptableObject
    {

        [field: Header("Player Stats"), Space(5.0f)]
        [field: SerializeField]
        public int playerLives { get; private set; } = 3;

        [field: SerializeField, Tooltip("How many shots can the player take at the same time")]
        public int playerMaxShots { get; private set; } = 2;

        [field: SerializeField, Tooltip("The starting position of the player")]
        public Vector2 playerStartPosition { get; private set; } = Vector2.zero;

        [field: Header("Grid of enemies"), Space(5.0f)]
        [field: SerializeField, Tooltip("How many columns of enemies the level has")]
        public int enemiesColumnCount { get; private set; } = 6;

        [field: SerializeField, Tooltip("How many rows of enemies the level has")]
        public int enemiesRowsCount { get; private set; } = 6;

        [field: Header("Enemy Movement"), Space(5.0f)]
        [field: SerializeField, Tooltip("[lower is faster] How long it take to move 1 enemy")]
        [field: Range(0f, 1f)]
        public float howLongUntilNextEnemyMovementRateStart { get; private set; } = 0.12f;

        [field: SerializeField, Tooltip("[lower is faster] How long does it take to move enemies down (relative to howLongUntilNextEnemyMovementRateStart) ")]
        [field: Range(0f, 1f)]
        public float moveDownFactorSpeedUpRateStart { get; private set; } = 0.4f;

        [field: Header("Next Round Speed up factor"), Space(5.0f)]
        [field: SerializeField, Tooltip("[Lower is faster]How much to speed up enemy movement the next round ")]
        [field: Range(0f, 1f)]
        public float howLongUntilNextEnemyMovementSpeedUpRate { get; private set; } = 0.95f;

        [field: SerializeField, Tooltip("[Lower is faster]How much to speed up the enemy movement when going down next round")]
        [field: Range(0f, 1f)]
        public float moveDownFactorSpeedUpSpeedUpRate { get; private set; } = 1.0f;


        [field: Header("Bunkers "), Space(5.0f)]
        [field: SerializeField, Tooltip("The amount of bunkers"), Min(0)]
        public int bunkerAmount;

        [field: SerializeField, Tooltip("Controls were the bunker will be located")]
        public Rect bunkerArea { get; private set; }

        [field: SerializeField, Header("Define Enemy spawn area"), Tooltip("The area were enemies will spawn")]
        public Rect enemySpawnArea { get; private set; }

        [field: Header("UI Properties"), Space(10.0f)]
        [field: SerializeField, Tooltip("The offset of the UI from the top left of the canvas")]
        public Vector2 scoreOffsetFromTopLeft { get; private set; }
        [field: SerializeField, Tooltip("The width and height of the UI")]
        public Vector2 scoreSizeDelta { get; private set; }
        [field: SerializeField, Tooltip("The alignment of the text")]
        public TextAlignmentOptions scoreAlignment { get; private set; } = TextAlignmentOptions.Top | TextAlignmentOptions.Left;


        [field: SerializeField, Tooltip("The width and height of the UI"), Space(10.0f)]
        public Vector2 liveOffsetFromTopLeft { get; private set; }
        [field: SerializeField, Tooltip("The width and height of the UI for the lives display")]
        public Vector2 livesSizeDelta { get; private set; }

        [field: SerializeField, Tooltip("The alignment of the text")]
        public TextAlignmentOptions livesAlignment { get; private set; } = TextAlignmentOptions.Top | TextAlignmentOptions.Left;

    }

}
