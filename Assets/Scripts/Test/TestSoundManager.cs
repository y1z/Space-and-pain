using Managers;
using UnityEngine;

namespace Test
{
    using Scriptable_Objects;

    public class TestSoundManager : MonoBehaviour
    {
        public void testBeep()
        {
            SingletonManager.inst.soundManager.playAudio(GameAudioType.SFX, "beep");
        }

        public void testBoom()
        {
            SingletonManager.inst.soundManager.playAudio(GameAudioType.SFX, "boom");
        }

        public void testGoodBeep()
        {
            SingletonManager.inst.soundManager.playAudio(GameAudioType.SFX, "good beep");
        }

        public void testAmbience()
        {
            SingletonManager.inst.soundManager.playAudio(GameAudioType.MUSIC, "ambience");
        }

        public void testGameOver()
        {
            SingletonManager.inst.soundManager.playAudio(GameAudioType.VOICE, "game over");
        }

    }
}
