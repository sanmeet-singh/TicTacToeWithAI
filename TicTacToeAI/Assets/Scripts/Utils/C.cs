using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// All the constants and enums.
    /// </summary>
    public static class C
    {
        public enum CellState
        {
            None,
            Cross,
            Circle
        }

        //        public enum PlayerValue
        //        {
        //            Cross,
        //            Circle
        //        }

        public const int MAX_PLAYERS = 2;
        public const int DEFAULT_SCORE = 0;
        public const int MAX_TURNS_PER_GAME = 9;
    }
}
