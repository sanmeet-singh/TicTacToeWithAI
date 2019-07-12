using System;
using System.Collections;
using UnityEngine;
using Players;
using Utils;
using UnityEngine.UI;

namespace General
{
    public class GameController : MonoBehaviour
    {
        #region Singleton

        public static GameController instance { get; protected set; }

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

        #region Utils

        public GridHandler gridHandler;
        
        public GameObject resultPopup;
        
        public Text resultText;
        
        private Player[] players;
        
        private int playerTurnIndex;
        private int totalTurns;

        public void Init()
        {
            InitPlayers();
            this.gridHandler.Init(OnCellClick);
        }

        private void InitPlayers()
        {
            players = new Player[C.MAX_PLAYERS];
            
            bool isEven = UnityEngine.Random.Range(1, 10) % 2 == 0;
            
//            players[0] = new Human("Player ", isEven ? C.PlayerValue.Cross : C.PlayerValue.Circle);
//            players[1] = new Bot("Bot ", players[0].playerValue == C.PlayerValue.Cross ? C.PlayerValue.Circle : C.PlayerValue.Cross);
//            this.playerTurnIndex = isEven ? 0 : 1;
            
            players[0] = new Human("Player ", C.PlayerValue.Cross);
            players[1] = new Bot("Bot ", players[0].playerValue == C.PlayerValue.Cross ? C.PlayerValue.Circle : C.PlayerValue.Cross);
            
            this.playerTurnIndex = 0;
            
        }

        private void OnCellClick(int cellID)
        {
            C.CellState tempState = GetCellStateFromPlayerValue(this.players[this.playerTurnIndex].playerValue);
            this.gridHandler.UpdateCellState(cellID, tempState);
            this.totalTurns += 1;
            
            EvaluateResult(cellID, tempState);
            ChangePlayerTurn();
            
            if (!this.resultPopup.activeSelf)
            {
                BotTurn(cellID);
            }
        }

        private void BotTurn(int cellID)
        {
            if (this.players[this.playerTurnIndex] is Bot)
            {
                int turnCellID = ((Bot)this.players[this.playerTurnIndex]).PlayTurn(this.gridHandler, this.totalTurns, cellID);
                if (turnCellID != -1)
                {
                    C.CellState tempState = GetCellStateFromPlayerValue(this.players[this.playerTurnIndex].playerValue);
                    this.gridHandler.UpdateCellState(turnCellID, tempState);
                    this.totalTurns += 1;

                    EvaluateResult(turnCellID, tempState);
                    ChangePlayerTurn();
                }
            }
        }

        private void EvaluateResult(int cellID, C.CellState playerValue)
        {
            bool isPlayerWon = this.gridHandler.EvaluateResult(cellID, playerValue);
            
            if (isPlayerWon)
            {
                DeclareResult(true, this.players[this.playerTurnIndex].playerName + " Won!!!");
            }
            else if (this.totalTurns == 9)
            {
                DeclareResult(true, "Draw!! Try Again!!");
            }
            
        }

        private void ChangePlayerTurn()
        {
            this.playerTurnIndex = this.playerTurnIndex == 0 ? 1 : 0;
        }

        private C.CellState GetCellStateFromPlayerValue(C.PlayerValue playerValue)
        {
            return playerValue == C.PlayerValue.Circle ? C.CellState.Circle : C.CellState.Cross;
        }

        private void DeclareResult(bool isActive, string resultstring)
        {
            this.resultText.text = resultstring;
            this.resultPopup.SetActive(isActive);
        }

        public void Restart()
        {
            DeclareResult(false, string.Empty);
            Reset();
            this.gridHandler.Reset();
            BotTurn(-1);
        }

        private void Reset()
        {
            this.totalTurns = 0;
            this.playerTurnIndex = UnityEngine.Random.Range(1, 10) % 2 == 0 ? 0 : 1;
        }

        #endregion

    }
}