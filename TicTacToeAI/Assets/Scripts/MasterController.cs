using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class MasterController : MonoBehaviour
    {
        #region Singleton

        public static MasterController instance { get; protected set; }

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

        public void StartGame()
        {
            ScreensHandler.instance.SwitchToPlayScreen();
            GameController.instance.Init();
        }

        #endregion
        
    }
}
