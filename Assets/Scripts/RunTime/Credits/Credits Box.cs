using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Credits
{
    public sealed class CreditsBox : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Image background;

        private void Awake()
        {
            if (text is null)
            {
                text = GetComponentInChildren<TextMeshProUGUI>();
                DDebug.Assert(text is not null, "text is null// FIX THAT", this); ;
            }

            if (background is null)
            {
                background = GetComponentInChildren<Image>();
                DDebug.Assert(background is not null, " background is null// FIX THAT", this); ;

            }

        }

        public void Move(Vector2 dir, float speed)
        {
            Vector2 pos = transform.position;
            pos += dir * speed;
            transform.position = pos;
        }
    }

}
