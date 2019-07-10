using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Players
{
    public class Player
    {
        public C.PlayerValue playerValue;

        protected Player()
        {
        }

        public Player(C.PlayerValue playerValue)
        {
            this.playerValue = playerValue;   
        }
    }
}
