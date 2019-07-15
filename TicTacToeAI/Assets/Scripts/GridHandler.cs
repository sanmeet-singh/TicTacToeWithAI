using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;

namespace General
{
    /// <summary>
    /// Grid handler.
    /// </summary>
    public class GridHandler : MonoBehaviour
    {
        public List<Cell> cells;
        public List<int> emptyCellsIndexes;

        /// <summary>
        /// Init the specified OnClickCallback.
        /// </summary>
        /// <param name="OnClickCallback">On click callback.</param>
        public void Init(Action<int> OnClickCallback)
        {
            this.emptyCellsIndexes = new List<int>(cells.Count);
            for (int i = 0; i < cells.Count; i++)
            {
                this.cells[i].Init(i, OnClickCallback);
                this.emptyCellsIndexes.Add(i);
            }
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            this.emptyCellsIndexes.Clear();
            for (int i = 0; i < cells.Count; i++)
            {
                this.cells[i].Reset();
                this.emptyCellsIndexes.Add(i);
            }
        }

        /// <summary>
        /// Updates the state of the cell.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="cellState">Cell state.</param>
        public void UpdateCellState(int cellID, C.CellState cellState)
        {
            this.cells[cellID].UpdateCellState(cellState);
            this.emptyCellsIndexes.Remove(cellID);
        }

        /// <summary>
        /// Evaluates the result.
        /// </summary>
        /// <returns><c>true</c>, if result was evaluated, <c>false</c> otherwise.</returns>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
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

        /// <summary>
        /// Checks the right top to left bottom diagnol.
        /// </summary>
        /// <returns><c>true</c>, if right top to left bottom diagnol was checked, <c>false</c> otherwise.</returns>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
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

        /// <summary>
        /// Checks the left top to right bottom diagnol.
        /// </summary>
        /// <returns><c>true</c>, if left top to right bottom diagnol was checked, <c>false</c> otherwise.</returns>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
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

        /// <summary>
        /// Checks the row.
        /// </summary>
        /// <returns><c>true</c>, if row was checked, <c>false</c> otherwise.</returns>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
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

        
        /// <summary>
        /// Checks the column.
        /// </summary>
        /// <returns><c>true</c>, if column was checked, <c>false</c> otherwise.</returns>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
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

        #region Bot Defence AI methods

        /// <summary>
        /// Grid analysis for defence. Purpose of this struct is to data in single go, so that Bot can decide its best move.
        /// isCellHasEmptySpace - is there any empty space in Row/Col
        /// isOpponentWinning - is true when row/col/diagnol has an empty space and 2 cells contain player value (x or o)
        /// proposedCellID - if above conditions are true, then to defend this cell ID is used.
        /// </summary>
        public struct GridAnalysisForDefence
        {
            public bool isCellHasEmptySpace;
            public bool isOpponentWinning;
            public int proposedCellID;
        }

        /// <summary>
        /// Gets the row analysis.
        /// Information it will return is whether row/col/diagnol has empty space, has 2 cells which have player value and a proposed cell ID
        /// </summary>
        /// <returns>The row analysis.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        public GridAnalysisForDefence GetRowAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysisForDefence gridAnalysis = new GridAnalysisForDefence();
            gridAnalysis.isCellHasEmptySpace = IsRowHasEmptySpace(lastUsedCellID);
            
            CheckRow(lastUsedCellID, opponentValue, ref gridAnalysis);
            
            return gridAnalysis;
        }

        /// <summary>
        /// Gets the column analysis.
        /// Information it will return is whether row/col/diagnol has empty space, has 2 cells which have player value and a proposed cell ID
        /// </summary>
        /// <returns>The column analysis.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        public GridAnalysisForDefence GetColumnAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysisForDefence gridAnalysis = new GridAnalysisForDefence();
            gridAnalysis.isCellHasEmptySpace = IsColumnHasEmptySpace(lastUsedCellID);

            CheckColumn(lastUsedCellID, opponentValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Gets the left top to right bottom diagnol analysis.
        ///  Information it will return is whether row/col/diagnol has empty space, has 2 cells which have player value and a proposed cell ID
        /// </summary>
        /// <returns>The left top to right bottom diagnol analysis.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        public GridAnalysisForDefence GetLeftTopToRightBottomDiagnolAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysisForDefence gridAnalysis = new GridAnalysisForDefence();
            gridAnalysis.isCellHasEmptySpace = IsLeftTopToRightBottomDiagnolHasEmptyCell(lastUsedCellID);

            CheckLeftTopToRightBottomDiagnol(lastUsedCellID, opponentValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Gets the right top to left bottom diagnol analysis.
        ///  Information it will return is whether row/col/diagnol has empty space, has 2 cells which have player value and a proposed cell ID
        /// </summary>
        /// <returns>The right top to left bottom diagnol analysis.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        public GridAnalysisForDefence GetRightTopToLeftBottomDiagnolAnalysis(int lastUsedCellID, C.CellState opponentValue)
        {
            GridAnalysisForDefence gridAnalysis = new GridAnalysisForDefence();
            gridAnalysis.isCellHasEmptySpace = IsRightTopToLeftBottomDiagnolHasEmptyCell(lastUsedCellID);

            CheckRightTopToLeftBottomDiagnol(lastUsedCellID, opponentValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Determines whether this instance is row has empty space the specified cellID.
        /// </summary>
        /// <returns><c>true</c> if this instance is row has empty space the specified cellID; otherwise, <c>false</c>.</returns>
        /// <param name="cellID">Cell I.</param>
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

        /// <summary>
        /// Checks the row.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysis">Grid analysis.</param>
        private void CheckRow(int cellID, C.CellState playerValue, ref GridAnalysisForDefence gridAnalysis)
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

        /// <summary>
        /// Determines whether this instance is column has empty space the specified cellID.
        /// </summary>
        /// <returns><c>true</c> if this instance is column has empty space the specified cellID; otherwise, <c>false</c>.</returns>
        /// <param name="cellID">Cell I.</param>
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

        
        /// <summary>
        /// Checks the column.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysis">Grid analysis.</param>
        private void CheckColumn(int cellID, C.CellState playerValue, ref GridAnalysisForDefence gridAnalysis)
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

        /// <summary>
        /// Determines whether this instance is right top to left bottom diagnol has empty cell the specified cellID.
        /// </summary>
        /// <returns><c>true</c> if this instance is right top to left bottom diagnol has empty cell the specified cellID;
        /// otherwise, <c>false</c>.</returns>
        /// <param name="cellID">Cell I.</param>
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

        /// <summary>
        /// Checks the right top to left bottom diagnol.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysis">Grid analysis.</param>
        private void CheckRightTopToLeftBottomDiagnol(int cellID, C.CellState playerValue, ref GridAnalysisForDefence gridAnalysis)
        {
            int iterationCOunter = 1;

            int counter = 0;
            int index;
            for (int i = 0; iterationCOunter <= 3; i++, iterationCOunter++)
            {
                index = (iterationCOunter * 3) - iterationCOunter;
                if (this.cells[index].CellState == playerValue)
                {
                    counter++;
                }
                else if (this.cells[index].CellState == C.CellState.None)
                {
                    gridAnalysis.proposedCellID = index;
                }
            }

            if (counter > 1)
            {
                gridAnalysis.isOpponentWinning = true;
                return;
            }
            gridAnalysis.isOpponentWinning = false;
        }

        /// <summary>
        /// Determines whether this instance is left top to right bottom diagnol has empty cell the specified cellID.
        /// </summary>
        /// <returns><c>true</c> if this instance is left top to right bottom diagnol has empty cell the specified cellID;
        /// otherwise, <c>false</c>.</returns>
        /// <param name="cellID">Cell I.</param>
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

        /// <summary>
        /// Checks the left top to right bottom diagnol.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysis">Grid analysis.</param>
        private void CheckLeftTopToRightBottomDiagnol(int cellID, C.CellState playerValue, ref GridAnalysisForDefence gridAnalysis)
        {
            int iterationCOunter = 0;

            int counter = 0;
            int index;
            for (int i = 0; iterationCOunter < 3; i++, iterationCOunter++)
            {
                index = (i * 3) + i;
                if (this.cells[index].CellState == playerValue)
                {
                    counter++;
                }
                else if (this.cells[index].CellState == C.CellState.None)
                {
                    gridAnalysis.proposedCellID = index;
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

        #region Bot AI for attack

        /// <summary>
        /// Grid analysis for attack.
        ///  Purpose of this struct is to data in single go, so that Bot can decide its best move.
        /// isCellHasEmptySpace - is there are 2 empty space in Row/Col/Diagnal
        /// playerValueCounter - number of player value in row/col/diagnol.
        /// proposedCellID - if above conditions are true, then to defend this cell ID is used.
        /// </summary>
        public struct GridAnalysisForAttack
        {
            public bool isCellHasTwoEmptySpace;
            public int proposedCellID;
            public int playerValueCounter;
        }

        /// <summary>
        /// Gets the row analysis for attack.
        /// </summary>
        /// <returns>The row analysis for attack.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="playerValue">Player value.</param>
        public GridAnalysisForAttack GetRowAnalysisForAttack(int lastUsedCellID, C.CellState playerValue)
        {
            GridAnalysisForAttack gridAnalysis = new GridAnalysisForAttack();
            gridAnalysis.proposedCellID = -1;
            CheckRowForAttack(lastUsedCellID, playerValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Gets the column analysis for attack.
        /// </summary>
        /// <returns>The column analysis for attack.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="playerValue">Player value.</param>
        public GridAnalysisForAttack GetColumnAnalysisForAttack(int lastUsedCellID, C.CellState playerValue)
        {
            GridAnalysisForAttack gridAnalysis = new GridAnalysisForAttack();
            gridAnalysis.proposedCellID = -1;
            CheckColumnForAttack(lastUsedCellID, playerValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Gets the left top to right bottom diagnol analysis for attack.
        /// </summary>
        /// <returns>The left top to right bottom diagnol analysis for attack.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="playerValue">Player value.</param>
        public GridAnalysisForAttack GetLeftTopToRightBottomDiagnolAnalysisForAttack(int lastUsedCellID, C.CellState playerValue)
        {
            GridAnalysisForAttack gridAnalysis = new GridAnalysisForAttack();
            gridAnalysis.proposedCellID = -1;
            CheckLeftTopToRightBottomDiagnolForAttack(lastUsedCellID, playerValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Gets the right top to left bottom diagnol analysis for attack.
        /// </summary>
        /// <returns>The right top to left bottom diagnol analysis for attack.</returns>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="playerValue">Player value.</param>
        public GridAnalysisForAttack GetRightTopToLeftBottomDiagnolAnalysisForAttack(int lastUsedCellID, C.CellState playerValue)
        {
            GridAnalysisForAttack gridAnalysis = new GridAnalysisForAttack();
            gridAnalysis.proposedCellID = -1;
            CheckRightTopToLeftBottomDiagnolForAttack(lastUsedCellID, playerValue, ref gridAnalysis);

            return gridAnalysis;
        }

        /// <summary>
        /// Checks the row for attack.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysisForAttack">Grid analysis for attack.</param>
        private void CheckRowForAttack(int cellID, C.CellState playerValue, ref GridAnalysisForAttack gridAnalysisForAttack)
        {
            int rowNumber = cellID / 3;
            int start = rowNumber * 3;
            int end = start + 3;

            int counter = 0;
            int playerValueCounter = 0;

            for (int i = start; i < end; i++)
            {
                if (this.cells[i].CellState == C.CellState.None)
                {
                    counter++;
                    gridAnalysisForAttack.proposedCellID = i;
                }
                else if (this.cells[i].CellState == playerValue)
                {
                    gridAnalysisForAttack.playerValueCounter += 1;
                }
            }

            gridAnalysisForAttack.isCellHasTwoEmptySpace = counter > 1; 
        }

        /// <summary>
        /// Checks the column for attack.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysisForAttack">Grid analysis for attack.</param>
        private void CheckColumnForAttack(int cellID, C.CellState playerValue, ref GridAnalysisForAttack gridAnalysisForAttack)
        {
            int colNumber = cellID % 3;
            int iterationCOunter = 0;

            int counter = 0;

            for (int i = colNumber; iterationCOunter < 3; i += 3, iterationCOunter++)
            {
                if (this.cells[i].CellState == C.CellState.None)
                {
                    counter++;
                    gridAnalysisForAttack.proposedCellID = i;
                }
                else if (this.cells[i].CellState == playerValue)
                {
                    gridAnalysisForAttack.playerValueCounter += 1;
                }
            }

            gridAnalysisForAttack.isCellHasTwoEmptySpace = counter > 1; 
        }

        /// <summary>
        /// Checks the right top to left bottom diagnol for attack.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysisForAttack">Grid analysis for attack.</param>
        private void CheckRightTopToLeftBottomDiagnolForAttack(int cellID, C.CellState playerValue, ref GridAnalysisForAttack gridAnalysisForAttack)
        {
            int iterationCOunter = 1;

            int counter = 0;
            int index;
            for (int i = 0; iterationCOunter <= 3; i++, iterationCOunter++)
            {
                index = (iterationCOunter * 3) - iterationCOunter;
                if (this.cells[index].CellState == C.CellState.None)
                {
                    counter++;
                    gridAnalysisForAttack.proposedCellID = index;
                }
                else if (this.cells[index].CellState == playerValue)
                {
                    gridAnalysisForAttack.playerValueCounter += 1;
                }
            }

            gridAnalysisForAttack.isCellHasTwoEmptySpace = counter > 1; 
        }

        /// <summary>
        /// Checks the left top to right bottom diagnol for attack.
        /// </summary>
        /// <param name="cellID">Cell I.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="gridAnalysisForAttack">Grid analysis for attack.</param>
        private void CheckLeftTopToRightBottomDiagnolForAttack(int cellID, C.CellState playerValue, ref GridAnalysisForAttack gridAnalysisForAttack)
        {
            int iterationCOunter = 0;

            int counter = 0;
            int index;
            for (int i = 0; iterationCOunter < 3; i++, iterationCOunter++)
            {
                index = (i * 3) + i;
                if (this.cells[index].CellState == C.CellState.None)
                {
                    counter++;
                    gridAnalysisForAttack.proposedCellID = index;
                }
                else if (this.cells[index].CellState == playerValue)
                {
                    gridAnalysisForAttack.playerValueCounter += 1;
                }
            }

            gridAnalysisForAttack.isCellHasTwoEmptySpace = counter > 1; 
        }

        #endregion
    }
}
