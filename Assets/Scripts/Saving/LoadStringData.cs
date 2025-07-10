
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

        public static TypeIdentifier identify(string _data)
        {
            TypeIdentifier result = TypeIdentifier.NONE;

            return result;
        }
    
    }
}
