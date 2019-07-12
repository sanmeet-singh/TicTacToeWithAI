﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;

namespace General
{
    public class GridHandler : MonoBehaviour
    {
        public List<Cell> cells;
        public List<int> emptyCellsIndexes;

        public void Init(Action<int> OnClickCallback)
        {
            this.emptyCellsIndexes = new List<int>(cells.Count);
            for (int i = 0; i < cells.Count; i++)
            {
                this.cells[i].Init(i, OnClickCallback);
                this.emptyCellsIndexes.Add(i);
            }
        }

        public void Reset()
        {
            this.emptyCellsIndexes.Clear();
            for (int i = 0; i < cells.Count; i++)
            {
                this.cells[i].Reset();
                this.emptyCellsIndexes.Add(i);
            }
        }

        public void UpdateCellState(int cellID, C.CellState cellState)
        {
            this.cells[cellID].UpdateCellState(cellState);
            this.emptyCellsIndexes.Remove(cellID);
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

        #region Bot AI methods

        public struct GridAnalysis
        {
            public bool isCellHasEmptySpace;
            public bool isOpponentWinning;
            public int proposedCellID;
        }

        public GridAnalysis GetRowAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysis gridAnalysis = new GridAnalysis();
            gridAnalysis.isCellHasEmptySpace = IsRowHasEmptySpace(lastUsedCellID);
            
            CheckRow(lastUsedCellID, opponentValue, ref gridAnalysis);
            
            return gridAnalysis;
        }

        public GridAnalysis GetColumnAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysis gridAnalysis = new GridAnalysis();
            gridAnalysis.isCellHasEmptySpace = IsColumnHasEmptySpace(lastUsedCellID);

            CheckColumn(lastUsedCellID, opponentValue, ref gridAnalysis);

            return gridAnalysis;
        }

        public GridAnalysis GetLeftTopToRightBottomDiagnolAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysis gridAnalysis = new GridAnalysis();
            gridAnalysis.isCellHasEmptySpace = IsLeftTopToRightBottomDiagnolHasEmptyCell(lastUsedCellID);

            CheckLeftTopToRightBottomDiagnol(lastUsedCellID, opponentValue, ref gridAnalysis);

            return gridAnalysis;
        }

        public GridAnalysis GetRightTopToLeftBottomDiagnolAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysis gridAnalysis = new GridAnalysis();
            gridAnalysis.isCellHasEmptySpace = IsRightTopToLeftBottomDiagnolHasEmptyCell(lastUsedCellID);

            CheckRightTopToLeftBottomDiagnol(lastUsedCellID, opponentValue, ref gridAnalysis);

            return gridAnalysis;
        }

        private bool IsRowHasEmptySpace(int cellID)
        {
            int rowNumber = cellID / 3;
            int start = rowNumber * 3;
            int end = start + 3;

            int counter = 0;

            for (int i = start; i < end; i++)
            {
                if (this.cells[i].CellState == C.CellState.None)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckRow(int cellID, C.CellState playerValue, ref GridAnalysis gridAnalysis)
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
                else if (this.cells[i].CellState == C.CellState.None)
                {
                    gridAnalysis.proposedCellID = i;
                }
            }

            if (counter > 1)
            {
                gridAnalysis.isOpponentWinning = true;
                return;
            }
            gridAnalysis.isOpponentWinning = false;
        }

        private bool IsColumnHasEmptySpace(int cellID)
        {
            int colNumber = cellID % 3;
            int iterationCOunter = 0;

            int counter = 0;

            for (int i = colNumber; iterationCOunter < 3; i += 3, iterationCOunter++)
            {
                if (this.cells[i].CellState == C.CellState.None)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckColumn(int cellID, C.CellState playerValue, ref GridAnalysis gridAnalysis)
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
                else if (this.cells[i].CellState == C.CellState.None)
                {
                    gridAnalysis.proposedCellID = i;
                }
            }

            if (counter > 1)
            {
                gridAnalysis.isOpponentWinning = true;
                return;
            }
            gridAnalysis.isOpponentWinning = false;
        }

        private bool IsRightTopToLeftBottomDiagnolHasEmptyCell(int cellID)
        {
            int iterationCOunter = 1;

            int counter = 0;

            for (int i = 0; iterationCOunter <= 3; i++, iterationCOunter++)
            {
                if (this.cells[(iterationCOunter * 3) - iterationCOunter].CellState == C.CellState.None)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckRightTopToLeftBottomDiagnol(int cellID, C.CellState playerValue, ref GridAnalysis gridAnalysis)
        {
            int iterationCOunter = 1;

            int counter = 0;

            for (int i = 0; iterationCOunter <= 3; i++, iterationCOunter++)
            {
                if (this.cells[(iterationCOunter * 3) - iterationCOunter].CellState == playerValue)
                {
                    counter++;
                }
                else if (this.cells[i].CellState == C.CellState.None)
                {
                    gridAnalysis.proposedCellID = i;
                }
            }

            if (counter > 1)
            {
                gridAnalysis.isOpponentWinning = true;
                return;
            }
            gridAnalysis.isOpponentWinning = false;
        }

        private bool IsLeftTopToRightBottomDiagnolHasEmptyCell(int cellID)
        {
            int iterationCOunter = 0;

            int counter = 0;

            for (int i = 0; iterationCOunter < 3; i++, iterationCOunter++)
            {
                if (this.cells[(i * 3) + i].CellState == C.CellState.None)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckLeftTopToRightBottomDiagnol(int cellID, C.CellState playerValue, ref GridAnalysis gridAnalysis)
        {
            int iterationCOunter = 0;

            int counter = 0;

            for (int i = 0; iterationCOunter < 3; i++, iterationCOunter++)
            {
                if (this.cells[(i * 3) + i].CellState == playerValue)
                {
                    counter++;
                }
                else if (this.cells[i].CellState == C.CellState.None)
                {
                    gridAnalysis.proposedCellID = i;
                }
            }

            if (counter > 1)
            {
                gridAnalysis.isOpponentWinning = true;
                return;
            }
            gridAnalysis.isOpponentWinning = false;
        }

        #endregion
    }
}
