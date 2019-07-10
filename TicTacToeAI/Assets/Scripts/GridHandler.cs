using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;

namespace General
{
    public class GridHandler : MonoBehaviour
    {
        public List<Cell> cells;

        public void Init(Action<int> OnClickCallback)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Init(i, OnClickCallback);
            }
        }

        public void UpdateCellState(int cellID, C.CellState cellState)
        {
            this.cells[cellID].UpdateCellState(cellState);
        }
    }
}
