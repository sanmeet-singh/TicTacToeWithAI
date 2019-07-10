using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
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

        public void SwitchToPlayScreen()
        {
            this.startGameScreen.SetActive(false);
            this.playScreen.SetActive(true);
        }

        public void SwitchToStartGameScreen()
        {
            this.startGameScreen.SetActive(true);
            this.playScreen.SetActive(false);
        }

        #endregion
    }
}
