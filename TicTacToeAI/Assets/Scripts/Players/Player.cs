using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Players
{
    public class Player
    {
        public C.PlayerValue playerValue;
        
        public string playerName;
        
        public int score;

        protected Player()
        {
        }

        public Player(string playerName, C.PlayerValue playerValue, int score)
        {
            this.playerName = playerName;
            this.playerValue = playerValue; 
            this.score = score;
        }
    }
}
