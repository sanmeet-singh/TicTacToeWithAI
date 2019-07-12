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

        protected Player()
        {
        }

        public Player(string playerName, C.PlayerValue playerValue)
        {
            this.playerName = playerName;
            this.playerValue = playerValue;   
        }
    }
}
