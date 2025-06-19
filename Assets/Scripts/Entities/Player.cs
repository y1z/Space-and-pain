using System.Collections;
using UnityEngine;

namespace Entities
{
    [SelectionBase]
    public sealed class Player : MonoBehaviour
    {
        public int live { get; set; } = 3;
        public int maxShots { get; set; } = 2;

        public Vector2 startingPosition;

        private void Start()
        {
            startingPosition = transform.position;
        }

        /// <summary>
        /// TODO : PLAY DEATH ANIMATION WITH THIS FUNCTION
        /// </summary>
        /// <returns></returns>
        public void dies()
        {
            StartCoroutine(deathAnmation());
        }

        private IEnumerator deathAnmation()
        {
            yield return null;
            gameObject.SetActive(false);
        }


    }

}
