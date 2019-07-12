using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Players
{
    public class Human : Player
    {
        private Human()
        {
        }

        public Human(string playerName, C.PlayerValue playerValue, int score)
            : base(playerName, playerValue, score)
        {
            
        }
    }
}
