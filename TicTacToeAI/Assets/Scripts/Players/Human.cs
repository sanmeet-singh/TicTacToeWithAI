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

        public Human(C.PlayerValue playerValue)
            : base(playerValue)
        {
            
        }
    }
}
