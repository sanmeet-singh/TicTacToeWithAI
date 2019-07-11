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

        public void EvaluateResult(int cellID, C.CellState playerValue)
        {
//            CheckRow(cellID, playerValue);
//            CheckColumn(cellID, playerValue);
//            CheckLeftTopToRightBottomDiagnol(cellID, playerValue);
//            CheckRightTopToLeftBottomDiagnol(cellID, playerValue);
            
            switch (cellID)
            {
                case 0:
                case 8:
                    CheckRow(cellID, playerValue);
                    CheckColumn(cellID, playerValue);
                    CheckLeftTopToRightBottomDiagnol(cellID, playerValue);
                    break;
                    
                case 1:
                case 3:
                case 5:
                case 7:
                    CheckRow(cellID, playerValue);
                    CheckColumn(cellID, playerValue);
                    break;
                    
                case 2:
                case 6:
                    Debug.Log("id : " + cellID);
                    CheckRow(cellID, playerValue);
                    CheckColumn(cellID, playerValue);
                    CheckRightTopToLeftBottomDiagnol(cellID, playerValue);
                    break;
                    
                case 4:
                    CheckRow(cellID, playerValue);
                    CheckColumn(cellID, playerValue);
                    CheckRightTopToLeftBottomDiagnol(cellID, playerValue);
                    CheckLeftTopToRightBottomDiagnol(cellID, playerValue);
                    break;
            }
        }

        private void CheckRightTopToLeftBottomDiagnol(int cellID, C.CellState playerValue)
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

            if (counter == 3)
            {
                Debug.Log("WIN! R to L Diagnol");
            }
        }

        private void CheckLeftTopToRightBottomDiagnol(int cellID, C.CellState playerValue)
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

            if (counter == 3)
            {
                Debug.Log("WIN L to R diagnol");
            }
        }

        private void CheckRow(int cellID, C.CellState playerValue)
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
            
            if (counter == 3)
            {
                Debug.Log("WIN Row");
            }
        }

        private void CheckColumn(int cellID, C.CellState playerValue)
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

            if (counter == 3)
            {
                Debug.Log("WIN Col");
            }
        }
    }
}
