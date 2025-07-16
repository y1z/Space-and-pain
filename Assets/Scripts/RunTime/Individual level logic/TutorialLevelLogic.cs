using Entities;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace IndividualLevelLogic
{

    public sealed class TutorialLevelLogic : MonoBehaviour
    {
        internal enum TutorialSates
        {
            NONE,
            Start,
            TeachMovement,
            TeachShoot,
            TeachPlayerLives,
            TeachBunker,
            TeachBunkerEnemyShoot,
            TeachPlayerShootBunkerShoot
        }

        public Enemy tutorialEnemy;
        public Bunker tutorialBunker;
        public Player tutorialPlayer;

        private TutorialSates tutoriaStates = TutorialSates.NONE;

        public void loadStartScreen()
        {
            SceneManager.LoadScene("Scenes/Game/StartScreen");
        }

        /// <summary>
        /// TODO : REMOVE THIS UPDATE
        /// </summary>
        public void Update()
        {
            if (Managers.SingletonManager.inst.inputManager.isPauseActionPressedThisFrame())
            {
                loadStartScreen();
            }
        }
    }
}
