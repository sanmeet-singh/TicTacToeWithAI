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

        public void Reset()
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Reset();
            }
        }

        public void UpdateCellState(int cellID, C.CellState cellState)
        {
            this.cells[cellID].UpdateCellState(cellState);
        }

        public bool EvaluateResult(int cellID, C.CellState playerValue)
        {
            switch (cellID)
            {
                case 0:
                case 8:
                    if (CheckRow(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckColumn(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckLeftTopToRightBottomDiagnol(cellID, playerValue))
                    {
                        return true;
                    }
                    break;
                    
                case 1:
                case 3:
                case 5:
                case 7:
                    if (CheckRow(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckColumn(cellID, playerValue))
                    {
                        return true;
                    }
                    break;
                    
                case 2:
                case 6:
                    if (CheckRow(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckColumn(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckRightTopToLeftBottomDiagnol(cellID, playerValue))
                    {
                        return true;
                    }
                    break;
                    
                case 4:
                    if (CheckRow(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckColumn(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckRightTopToLeftBottomDiagnol(cellID, playerValue))
                    {
                        return true;
                    }
                    if (CheckLeftTopToRightBottomDiagnol(cellID, playerValue))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private bool CheckRightTopToLeftBottomDiagnol(int cellID, C.CellState playerValue)
        {
            int iterationCOunter = 1;

            int counter = 0;

            for (int i = 0; iterationCOunter <= 3; i++, iterationCOunter++)
            {
                if (this.cells[(iterationCOunter * 3) - iterationCOunter].CellState == playerValue)
                {
                    counter++;
                }
            }

            return counter == 3;
        }

        private bool CheckLeftTopToRightBottomDiagnol(int cellID, C.CellState playerValue)
        {
            int iterationCOunter = 0;

            int counter = 0;

            for (int i = 0; iterationCOunter < 3; i++, iterationCOunter++)
            {
                if (this.cells[(i * 3) + i].CellState == playerValue)
                {
                    counter++;
                }
            }

            return counter == 3;
        }

        private bool CheckRow(int cellID, C.CellState playerValue)
        {
            int rowNumber = cellID / 3;
            int start = rowNumber * 3;
            int end = start + 3;
            
            int counter = 0;
            
            for (int i = start; i < end; i++)
            {
                if (this.cells[i].CellState == playerValue)
                {
                    counter++;
                }
            }
            
            return counter == 3;
        }

        private bool CheckColumn(int cellID, C.CellState playerValue)
        {
            int colNumber = cellID % 3;
            int iterationCOunter = 0;

            int counter = 0;

            for (int i = colNumber; iterationCOunter < 3; i += 3, iterationCOunter++)
            {
                if (this.cells[i].CellState == playerValue)
                {
                    counter++;
                }
            }

            return counter == 3;
        }
    }
}
