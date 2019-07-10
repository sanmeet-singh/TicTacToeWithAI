using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class C
    {
        public enum CellState
        {
            None,
            Cross,
            Circle
        }

        public enum PlayerValue
        {
            Cross,
            Circle
        }

        public const int MAX_PLAYERS = 2;
    }
}
