using UnityEngine;

namespace Entities
{
    public static class DefaultEntities
    {
        const string PATH_TO_DEFAULT_PROJECTILE = "Prefabs/Entities/Projectile";

        public static readonly Projectile defaultProjectile = Resources.Load<Projectile>(PATH_TO_DEFAULT_PROJECTILE);
    }

}
