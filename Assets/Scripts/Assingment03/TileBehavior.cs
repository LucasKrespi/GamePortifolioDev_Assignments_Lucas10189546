using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileBehavior : MonoBehaviour
{
    public TileData tileData;
    private Image image;
    private int tileType;

    public int positionX;
    public int positionY;

    BoardBehavior Board;

    private void Start()
    {
        image = GetComponent<Image>();
        Board = BoardBehavior.instance;

        this.GetComponent<Button>().onClick.AddListener(Onclick);
    }
    public void AssignData(TileData data, int pX, int pY)
    {
        tileData = data;

        positionX = pX;

        positionY = pY;

        image = GetComponent<Image>();

        image.sprite = tileData.tileImage;

        tileType = tileData.tileType;
    }

    public int ReturnTileType()
    {
        return tileType;
    }

    public void ChangeData(TileData data)
    {
        tileData = data;

        image.sprite = data.tileImage;

        tileType = data.tileType;
    }

    public void ChangeColor(Color color)
    {
        image.color = color;
    }
    public void Onclick()
    {
        if (Board.canPlay)
        {
            bool wasChanged = false;
            if (tileType == 6)
            {
                Board.Explode(this);
                return;
            }
            if (Board.selectedTile == null)
            {
                Board.setPossiblePlayTiles(this);
            }
            else
            {
                
                foreach (var t in Board.PossiblePlays)
                {
                    if (t == this)
                    {
                        Board.SwapTiles(t);
                        wasChanged = true;
                    }
                }

                if (wasChanged)
                {
                    Board.canPlay = false;
                    Board.cleanPossiblePlayList();
                    wasChanged = false;
                }
            }
        }
    }

    



}
