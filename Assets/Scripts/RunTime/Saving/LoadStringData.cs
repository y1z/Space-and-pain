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
        BUNKER,
        PROJECTILE,
    }

    public static class LoadStringData
    {
        private static readonly
            Dictionary<string, TypeIdentifier> stringToType = new Dictionary<string, TypeIdentifier>
            {
                {STANDARD_ENTITY_SAVE_DATA_ID, TypeIdentifier.STANDARD_ENTITY_SAVE_DATA },
                {PLAYER_ID, TypeIdentifier.PLAYER},
                {ENEMY_ID, TypeIdentifier.ENEMY},
                {BUNKER_ID, TypeIdentifier.BUNKER},
                {PROJECTILE_ID, TypeIdentifier.PROJECTILE},
            };


        public static TypeIdentifier identify(string[] _data, int index)
        {
            TypeIdentifier result = TypeIdentifier.NONE;

            stringToType.TryGetValue(_data[index], out result);

            return result;
        }

    }
}
