using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image image;
    
    public Sprite spriteCircle;
    public Sprite spriteCross;
    
    private Action<int> OnClickCallback;
    
    private int cellID;
    
    private C.CellState cellState;

    public C.CellState CellState
    {
        get
        {
            return cellState;
        }
        set
        {
            cellState = value;
            CellStateUpdated();
        }
    }

    public void Init(int cellID, Action<int> OnClickCallback)
    {
        this.cellID = cellID;
        this.OnClickCallback = OnClickCallback;
        Reset();
    }

    public void Reset()
    {
        this.CellState = C.CellState.None;
    }

    public void OnClick()
    {
        if (this.CellState != C.CellState.None)
        {
            return;
        }
        
        if (this.OnClickCallback != null)
        {
            this.OnClickCallback(this.cellID);
        }
    }

    public void UpdateCellState(C.CellState cellState)
    {
        this.CellState = cellState;
    }

    private void CellStateUpdated()
    {
        switch (this.cellState)
        {
            case C.CellState.None:
                this.image.sprite = null;
                break;
                
            case C.CellState.Circle:
                this.image.sprite = this.spriteCircle;
                break;
                
            case C.CellState.Cross:
                this.image.sprite = this.spriteCross;
                break;
        }
    }
}
