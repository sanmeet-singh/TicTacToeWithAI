using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Players
{
    /// <summary>
    /// Human player
    /// </summary>
    public class Human : Player
    {
        private Human()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Players.Human"/> class.
        /// Init with player name, player value and default score
        /// </summary>
        /// <param name="playerName">Player name.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="score">Score.</param>
        public Human(string playerName, C.CellState playerValue, int score)
            : base(playerName, playerValue, score)
        {
            
        }
    }
}
