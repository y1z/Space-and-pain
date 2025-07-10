using interfaces;
using System.Text;

namespace Saving
{

    using static SavingConstants;

    public static partial class SaveStringifyer
    {

        public static string StringifyEntitySaveData(StandardEntitySaveData _saveData)
        {
            StringBuilder sb = new();
            sb.Append(STANDARD_ENTITY_SAVE_DATA_ID);
            sb.Append(DIVIDER);

            sb.Append(_saveData.position.x);
            sb.Append(DIVIDER);

            sb.Append(_saveData.position.y);
            sb.Append(DIVIDER);

            sb.Append(_saveData.speed.x);
            sb.Append(DIVIDER);

            sb.Append(_saveData.speed.y);
            sb.Append(DIVIDER);

            sb.Append(_saveData.direction.x);
            sb.Append(DIVIDER);

            sb.Append(_saveData.direction.y);
            sb.Append(DIVIDER);

            sb.Append(_saveData.isActive ? 1 : 0);
            sb.Append(DIVIDER);

            sb.Append(_saveData.prefabName);
            sb.Append(DIVIDER);

            return sb.ToString();
        }

    }

}
