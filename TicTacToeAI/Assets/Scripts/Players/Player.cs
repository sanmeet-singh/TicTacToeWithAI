using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Players
{
    /// <summary>
    /// Parent class for all the Players (Human and Bot)
    /// </summary>
    public class Player
    {
        public C.CellState playerValue;
        
        public string playerName;
        
        public int score;

        protected Player()
        {
        }

        public Player(string playerName, C.CellState playerValue, int score)
        {
            this.playerName = playerName;
            this.playerValue = playerValue; 
            this.score = score;
        }
    }
}
