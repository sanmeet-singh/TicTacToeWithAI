using System;
using System.Collections;
using UnityEngine;
using Players;
using Utils;
using UnityEngine.UI;

namespace General
{
    /// <summary>
    /// Game controller keeps all the logic of game play from initiating players to showing result.
    /// It also restarts the game on Game over.
    /// Singleton Monobehaviour to keep one instance of controller through out.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        #region Singleton

        public static GameController instance { get; protected set; }

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

        #region Utils

        public GridHandler gridHandler;
        
        public GameObject resultPopup;
        
        public Text resultText;
        public Text scoreText;
        
        private Player[] players;
        
        private int playerTurnIndex;
        private int totalTurns;

        /// <summary>
        /// Entry point Initiating game.
        /// Inits players and Grid.
        /// </summary>
        public void Init()
        {
            //Initialize Players, ie , Human and Bot.
            InitPlayers();
            //Initialize all the cells inside the Grid.
            this.gridHandler.Init(OnCellClick);
        }

        /// <summary>
        /// Inits the players, ie, Human and Bot with all the default settings.
        /// </summary>
        private void InitPlayers()
        {
            players = new Player[C.MAX_PLAYERS];
            
            bool isEven = UnityEngine.Random.Range(1, 10) % 2 == 0;
            
            players[0] = new Human("Player ", C.CellState.Cross, C.DEFAULT_SCORE);
            players[1] = new Bot("Bot ", players[0].playerValue == C.CellState.Cross ? C.CellState.Circle : C.CellState.Cross, C.DEFAULT_SCORE);

            //Player Turn
//            this.playerTurnIndex = 0;
            this.playerTurnIndex = isEven ? 0 : 1;
            
            //if its Bot turn then tell Bot to play
            CheckForBotTurn();
        }

        /// <summary>
        /// Call back when any cell in the Grid is clicked.
        /// </summary>
        /// <param name="cellID">Cell ID</param>
        private void OnCellClick(int cellID)
        {
            C.CellState tempState = this.players[this.playerTurnIndex].playerValue;
            //Update cell with appropriate Image. 
            this.gridHandler.UpdateCellState(cellID, tempState);
            this.totalTurns += 1;
            
            //After every turn check for result
            EvaluateResult(cellID, tempState);

            ChangePlayerTurn();
            
            //if result is not declared tell bot to play.
            if (!this.resultPopup.activeSelf)
            {
                BotTurn(cellID);
            }
        }

        /// <summary>
        /// Let Bot Play the next turn/=.
        /// </summary>
        /// <param name="cellID">Cell I.D which was last used by the user.</param>
        private void BotTurn(int cellID)
        {
            //CHeck if its actually Bot's turn.
            if (this.players[this.playerTurnIndex] is Bot)
            {
                //Cell ID which is selected by Bot.
                int turnCellID = ((Bot)this.players[this.playerTurnIndex]).PlayTurn(this.gridHandler, this.totalTurns, cellID);
                if (turnCellID != -1)
                {
                    C.CellState tempState = this.players[this.playerTurnIndex].playerValue;
                    //Update cell with appropriate Image. 
                    this.gridHandler.UpdateCellState(turnCellID, tempState);
                    this.totalTurns += 1;

                    //Evaluate result
                    EvaluateResult(turnCellID, tempState);
                    //change player turn
                    ChangePlayerTurn();
                }
            }
        }

        /// <summary>
        /// Evaluates the result.
        /// </summary>
        /// <param name="cellID">Cell ID from last turn</param>
        /// <param name="playerValue">Player value.</param>
        private void EvaluateResult(int cellID, C.CellState playerValue)
        {
            bool isAnyPlayerWon = this.gridHandler.EvaluateResult(cellID, playerValue);
            
            //if any player won then decalre result
            if (isAnyPlayerWon)
            {
                //Declare result
                DeclareResult(true, this.players[this.playerTurnIndex].playerName + " Won!!!");
                //Update score
                this.players[this.playerTurnIndex].score += 1;
                //Update score text
                UpdateScoreText();
            }
            else if (this.totalTurns == C.DEFAULT_SCORE)
            {
                //If Max turn is achieved, declare game is draw.
                DeclareResult(true, "Draw!! Try Again!!");
            }
        }

        /// <summary>
        /// Updates the score text. SHow it to On Screen
        /// </summary>
        private void UpdateScoreText()
        {
            this.scoreText.text = this.players[0].score + " - " + this.players[1].score;
        }

        /// <summary>
        /// Changes the player turn.
        /// </summary>
        private void ChangePlayerTurn()
        {
            this.playerTurnIndex = this.playerTurnIndex == 0 ? 1 : 0;
        }

        //        private C.CellState GetCellStateFromPlayerValue(C.PlayerValue playerValue)
        //        {
        //            return playerValue == C.PlayerValue.Circle ? C.CellState.Circle : C.CellState.Cross;
        //        }

        /// <summary>
        /// Declares the result. Update the textBox and update active status of pop up
        /// </summary>
        /// <param name="isActive">Active Status</param>
        /// <param name="resultstring">Resultstring.</param>
        private void DeclareResult(bool isActive, string resultstring)
        {
            this.resultText.text = resultstring;
            this.resultPopup.SetActive(isActive);
        }

        
        /// <summary>
        /// Restart the game. Disable result pop up, reset variables and reset's grid.
        /// </summary>
        public void Restart()
        {
            DeclareResult(false, string.Empty);
            Reset();
            this.gridHandler.Reset();
            CheckForBotTurn();
        }

        /// <summary>
        /// Checks for bot turn.
        /// </summary>
        private void CheckForBotTurn()
        {
            if (this.players[this.playerTurnIndex] is Bot)
            {
                BotTurn(-1);
            }
        }

        /// <summary>
        /// Reset variables
        /// </summary>
        private void Reset()
        {
            this.totalTurns = 0;
            this.playerTurnIndex = UnityEngine.Random.Range(1, 10) % 2 == 0 ? 0 : 1;
        }

        #endregion

    }
}