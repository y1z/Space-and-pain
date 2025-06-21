using Managers;
using UnityEngine;

namespace Entities
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float speed = 1.0f;
        [SerializeField] CharacterController cc;
        [SerializeField] private Player referenceToPlayer;

        void Start()
        {
            if (cc == null)
            {
                cc = GetComponent<CharacterController>();
            }
            EDebug.Assert(cc != null, $"This Script needs a {typeof(CharacterController)} to work", this);

            if (referenceToPlayer == null)
            {
                referenceToPlayer = GetComponent<Player>();
            }
            EDebug.Assert(referenceToPlayer != null, $"This Script needs a {typeof(Player)} to work", this);
        }

        void Update()
        {
            InputManager im = SingletonManager.inst.inputManager;

            InputInfo info = im.getInputedDirection();

            Vector2 final_movement = new Vector2(im.movement.x * speed, 0.0f);

            switch (info)
            {
                case InputInfo.LEFT:
                    cc.Move(final_movement * Time.deltaTime);
                    break;

                case InputInfo.RIGHT:
                    cc.Move(final_movement * Time.deltaTime);
                    break;
            }

        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Enemy"))
            {
                EDebug.Log("Enemy hit");
                cc.enabled = false;
                transform.position = referenceToPlayer.startingPosition;
                cc.enabled = true;
            }
            if (hit.gameObject.CompareTag("Enemy Projectile"))
            {
                referenceToPlayer.gameObject.SetActive(false);
            }

            EDebug.Log($"{nameof(OnControllerColliderHit)}");
        }

    }
}
