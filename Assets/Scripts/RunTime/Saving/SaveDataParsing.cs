using System;
using System.Collections.Generic;

namespace Saving
{
    using static SavingConstants;

    public enum TypeIdentifier
    {
        NONE = 0,
        UNKNOWN,
        STANDARD_ENTITY_SAVE_DATA,
        PLAYER,
        ENEMY,
        ENEMY_SPAWNER,
        BUNKER,
        PROJECTILE,
        SCORE_MANAGER,
        GAME_MANAGER,
        ENEMY_MANAGER,
    }

    public static class SaveDataParsing
    {
        private static readonly
            Dictionary<string, TypeIdentifier> stringToType = new Dictionary<string, TypeIdentifier>
            {
                {STANDARD_ENTITY_SAVE_DATA_ID, TypeIdentifier.STANDARD_ENTITY_SAVE_DATA },
                {PLAYER_ID, TypeIdentifier.PLAYER},
                {ENEMY_ID, TypeIdentifier.ENEMY},
                {ENEMY_SPAWNER_ID, TypeIdentifier.ENEMY_SPAWNER},
                {BUNKER_ID, TypeIdentifier.BUNKER},
                {PROJECTILE_ID, TypeIdentifier.PROJECTILE},
                {SCORE_MANAGER_ID, TypeIdentifier.SCORE_MANAGER},
                {GAME_MANAGER_ID, TypeIdentifier.GAME_MANAGER},
                {ENEMY_MANAGER_ID, TypeIdentifier.ENEMY_MANAGER},
            };


        public static TypeIdentifier identify(string[] _data, int index)
        {
            TypeIdentifier result = TypeIdentifier.NONE;

            string identifyerData = _data[index];

            int dividerIndex = identifyerData.IndexOf(SavingConstants.DIVIDER);

            stringToType.TryGetValue(identifyerData[0..dividerIndex], out result);

            return result;
        }

    }
}
