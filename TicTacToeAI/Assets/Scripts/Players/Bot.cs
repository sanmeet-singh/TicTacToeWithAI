using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using General;

namespace Players
{

    public class Bot : Player
    {
        private int lastUsedCellID;

        private Bot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Players.Bot"/> class.
        /// </summary>
        /// <param name="playerName">Player name.</param>
        /// <param name="playerValue">Player value.</param>
        /// <param name="score">Score.</param>
        public Bot(string playerName, C.CellState playerValue, int score)
            : base(playerName, playerValue, score)
        {
        }

        /// <summary>
        /// Plaies the turn.
        /// </summary>
        /// <returns>The turn.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="totalTurns">Total turns.</param>
        /// <param name="opponentUsedCellID">Opponent used cell ID</param>
        public int PlayTurn(GridHandler gridHandler, int totalTurns, int opponentUsedCellID)
        {
            if (totalTurns < 3)
            {
                this.lastUsedCellID = gridHandler.emptyCellsIndexes[Random.Range(0, gridHandler.emptyCellsIndexes.Count)];
                return this.lastUsedCellID;
            }
            
            //defence
            int defendCellID = Defend(gridHandler, totalTurns, opponentUsedCellID);
            //if cell id == -1 then attack otherwise defend.
            if (defendCellID != -1)
            {
                this.lastUsedCellID = defendCellID;
                return defendCellID;
            }

            //attack
            int attackCellID = Attack(gridHandler);
            if (attackCellID != -1)
            {
                this.lastUsedCellID = attackCellID;
                return attackCellID;
            }
            
            //if this is returning then there is some error
            return -1;
        }

        #region defend

        /// <summary>
        /// Defend the specified gridHandler, totalTurns and opponentUsedCellID.
        /// </summary>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="totalTurns">Total turns.</param>
        /// <param name="opponentUsedCellID">Opponent used cell ID.</param>
        private int Defend(GridHandler gridHandler, int totalTurns, int opponentUsedCellID)
        {
            C.CellState tempOpponentValue = this.playerValue == C.CellState.Circle ? C.CellState.Cross : C.CellState.Circle;
            
            int proposedCellID = -1;
            switch (opponentUsedCellID)
            {
                case 0:
                case 8:
                    proposedCellID = CheckRowForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckColumnForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckLeftTopToRightBottomDiagnolForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    break;

                case 1:
                case 3:
                case 5:
                case 7:
                    proposedCellID = CheckRowForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckColumnForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    break;

                case 2:
                case 6:
                    proposedCellID = CheckRowForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckColumnForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckRightTopToLeftBottomDiagnolForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    break;

                case 4:
                    proposedCellID = CheckRowForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckColumnForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckRightTopToLeftBottomDiagnolForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    
                    proposedCellID = CheckLeftTopToRightBottomDiagnolForDefence(gridHandler, totalTurns, opponentUsedCellID, tempOpponentValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    break;
            }
            
            return proposedCellID;
        }

        /// <summary>
        /// Checks the row for defence.
        /// </summary>
        /// <returns>The row for defence.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="totalTurns">Total turns.</param>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        private int CheckRowForDefence(GridHandler gridHandler, int totalTurns, int lastUsedCellID, C.CellState opponentValue)
        {
            GridHandler.GridAnalysisForDefence analysis = gridHandler.GetRowAnalysis(lastUsedCellID, opponentValue); 
            if (analysis.isCellHasEmptySpace)
            {
                if (analysis.isOpponentWinning)
                {
                    return analysis.proposedCellID;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks the column for defence.
        /// </summary>
        /// <returns>The column for defence.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="totalTurns">Total turns.</param>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        private int CheckColumnForDefence(GridHandler gridHandler, int totalTurns, int lastUsedCellID, C.CellState opponentValue)
        {
            GridHandler.GridAnalysisForDefence analysis = gridHandler.GetColumnAnalysis(lastUsedCellID, opponentValue); 
            if (analysis.isCellHasEmptySpace)
            {
                if (analysis.isOpponentWinning)
                {
                    return analysis.proposedCellID;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks the left top to right bottom diagnol for defence.
        /// </summary>
        /// <returns>The left top to right bottom diagnol for defence.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="totalTurns">Total turns.</param>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        private int CheckLeftTopToRightBottomDiagnolForDefence(GridHandler gridHandler, int totalTurns, int lastUsedCellID, C.CellState opponentValue)
        {
            GridHandler.GridAnalysisForDefence analysis = gridHandler.GetLeftTopToRightBottomDiagnolAnalysis(lastUsedCellID, opponentValue); 
            if (analysis.isCellHasEmptySpace)
            {
                if (analysis.isOpponentWinning)
                {
                    return analysis.proposedCellID;
                }
            }
            return -1;
        }

        /// <summary>
        /// Checks the right top to left bottom diagnol for defence.
        /// </summary>
        /// <returns>The right top to left bottom diagnol for defence.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="totalTurns">Total turns.</param>
        /// <param name="lastUsedCellID">Last used cell I.</param>
        /// <param name="opponentValue">Opponent value.</param>
        private int CheckRightTopToLeftBottomDiagnolForDefence(GridHandler gridHandler, int totalTurns, int lastUsedCellID, C.CellState opponentValue)
        {
            GridHandler.GridAnalysisForDefence analysis = gridHandler.GetRightTopToLeftBottomDiagnolAnalysis(lastUsedCellID, opponentValue); 
            if (analysis.isCellHasEmptySpace)
            {
                if (analysis.isOpponentWinning)
                {
                    return analysis.proposedCellID;
                }
            }
            return -1;
        }

        #endregion

        
        #region attack

        /// <summary>
        /// Attack the specified gridHandler.
        /// </summary>
        /// <param name="gridHandler">Grid handler.</param>
        private int Attack(GridHandler gridHandler)
        {
            C.CellState tempPlayerValue = this.playerValue == C.CellState.Circle ? C.CellState.Circle : C.CellState.Cross;

            int proposedCellID = -1;
            switch (this.lastUsedCellID)
            {
                case 0:
                case 8:
                    proposedCellID = CheckRowForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckColumnForForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckLeftTopToRightBottomDiagnolForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    break;

                case 1:
                case 3:
                case 5:
                case 7:
                    proposedCellID = CheckRowForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckColumnForForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    break;

                case 2:
                case 6:
                    proposedCellID = CheckRowForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckColumnForForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckRightTopToLeftBottomDiagnolForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    break;

                case 4:
                    proposedCellID = CheckRowForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckColumnForForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckRightTopToLeftBottomDiagnolForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }

                    proposedCellID = CheckLeftTopToRightBottomDiagnolForAttack(gridHandler, tempPlayerValue);
                    if (proposedCellID != -1)
                    {
                        return proposedCellID;
                    }
                    break;
            }

            proposedCellID = gridHandler.emptyCellsIndexes[Random.Range(0, gridHandler.emptyCellsIndexes.Count)];
            return proposedCellID;
        }

        /// <summary>
        /// Checks the row for attack.
        /// </summary>
        /// <returns>The row for attack.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="playerValue">Player value.</param>
        private int CheckRowForAttack(GridHandler gridHandler, C.CellState playerValue)
        {
            GridHandler.GridAnalysisForAttack analysis = gridHandler.GetRowAnalysisForAttack(this.lastUsedCellID, playerValue); 
            if (analysis.isCellHasTwoEmptySpace || analysis.playerValueCounter == 2)
            {
                return analysis.proposedCellID;
            }
            return -1;
        }

        /// <summary>
        /// Checks the column for for attack.
        /// </summary>
        /// <returns>The column for for attack.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="playerValue">Player value.</param>
        private int CheckColumnForForAttack(GridHandler gridHandler, C.CellState playerValue)
        {
            GridHandler.GridAnalysisForAttack analysis = gridHandler.GetColumnAnalysisForAttack(this.lastUsedCellID, playerValue); 
            if (analysis.isCellHasTwoEmptySpace || analysis.playerValueCounter == 2)
            {
                return analysis.proposedCellID;
            }
            return -1;
        }

        /// <summary>
        /// Checks the left top to right bottom diagnol for attack.
        /// </summary>
        /// <returns>The left top to right bottom diagnol for attack.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="playerValue">Player value.</param>
        private int CheckLeftTopToRightBottomDiagnolForAttack(GridHandler gridHandler, C.CellState playerValue)
        {
            GridHandler.GridAnalysisForAttack analysis = gridHandler.GetLeftTopToRightBottomDiagnolAnalysisForAttack(this.lastUsedCellID, playerValue); 
            if (analysis.isCellHasTwoEmptySpace || analysis.playerValueCounter == 2)
            {
                return analysis.proposedCellID;
            }
            return -1;
        }

        /// <summary>
        /// Checks the right top to left bottom diagnol for attack.
        /// </summary>
        /// <returns>The right top to left bottom diagnol for attack.</returns>
        /// <param name="gridHandler">Grid handler.</param>
        /// <param name="playerValue">Player value.</param>
        private int CheckRightTopToLeftBottomDiagnolForAttack(GridHandler gridHandler, C.CellState playerValue)
        {
            GridHandler.GridAnalysisForAttack analysis = gridHandler.GetRightTopToLeftBottomDiagnolAnalysisForAttack(this.lastUsedCellID, playerValue); 
            if (analysis.isCellHasTwoEmptySpace || analysis.playerValueCounter == 2)
            {
                return analysis.proposedCellID;
            }
            return -1;
        }

        #endregion
    }
}

