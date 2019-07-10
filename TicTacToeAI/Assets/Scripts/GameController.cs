using System;
using System.Collections;
using UnityEngine;
using Players;
using Utils;

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
        
        private Player[] players;
        
        private int playerTurnIndex;

        public void Init()
        {
            InitPlayers();
            this.gridHandler.Init(OnCellClick);
        }

        private void InitPlayers()
        {
            players = new Player[C.MAX_PLAYERS];
            
            bool isEven = UnityEngine.Random.Range(1, 10) % 2 == 0;
            
            players[0] = new Human(isEven ? C.PlayerValue.Cross : C.PlayerValue.Circle);
            players[1] = new Human(players[0].playerValue == C.PlayerValue.Cross ? C.PlayerValue.Circle : C.PlayerValue.Cross);
            
            this.playerTurnIndex = isEven ? 0 : 1;
        }

        private void OnCellClick(int cellID)
        {
            Debug.Log("cell ID : " + cellID);
            this.gridHandler.UpdateCellState(cellID, GetCellStateFromPlayerValue(this.players[this.playerTurnIndex].playerValue));
        }

        private C.CellState GetCellStateFromPlayerValue(C.PlayerValue playerValue)
        {
            return playerValue == C.PlayerValue.Circle ? C.CellState.Circle : C.CellState.Cross;
        }

        #endregion

    }
}