using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Master controller - currently plays the role of starting the game. As the game evolves it will hold other game related settings
    /// which needs to be initiated when game starts like sound on/off , game theme etc
    /// Singleton Monobehaviour to keep one instance of controller through out.
    /// </summary>
    public class MasterController : MonoBehaviour
    {
        #region Singleton

        public static MasterController instance { get; protected set; }

        void Awake()
        {
            //Check for instance
            if (instance != null && instance != this)
            {
                //Destroy other instance and reassigns it to latest Gameobject/instance
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

        /// <summary>
        /// Starts the game by calling Screen handler to switch the screen and initiating game controller.
        /// </summary>
        public void StartGame()
        {
            ScreensHandler.instance.SwitchToPlayScreen();
            GameController.instance.Init();
        }

        #endregion
        
    }
}
