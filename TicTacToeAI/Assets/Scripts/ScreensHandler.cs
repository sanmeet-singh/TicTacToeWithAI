using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Screens handler is responsible for switching screens. eg- start screen to game screen.
    /// Singleton Monobehaviour to keep one instance of controller through out.
    /// </summary>
    public class ScreensHandler : MonoBehaviour
    {
        #region Singleton

        public static ScreensHandler instance { get; protected set; }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                instance = this;
            }
            else
            {
                instance = this;
            }
        }

        #endregion

        #region Util methods

        public GameObject playScreen;
        public GameObject startGameScreen;

        /// <summary>
        /// Switchs to play screen.
        /// </summary>
        public void SwitchToPlayScreen()
        {
            this.startGameScreen.SetActive(false);
            this.playScreen.SetActive(true);
        }

        /// <summary>
        /// Switchs to start game screen.
        /// </summary>
        public void SwitchToStartGameScreen()
        {
            this.startGameScreen.SetActive(true);
            this.playScreen.SetActive(false);
        }

        #endregion
    }
}
