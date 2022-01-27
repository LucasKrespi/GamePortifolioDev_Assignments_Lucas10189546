using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile 
    
{
    public GameObject tile;

    private Image image;

    public Button button;

    public RESOURCE_TYPE resource;

    public Vector2 coord;

    private Color Orange = new Color(0.96f, 0.52f, 0.0f);

    public Tile(GameObject prefab, int x, int y, RESOURCE_TYPE resourcetype)
    {
        tile = prefab;
        button = tile.GetComponent<Button>();
        image = tile.GetComponent<Image>();
        button.onClick.AddListener(OnClick);

        coord.x = x;
        coord.y = y;

        resource = resourcetype;
    }

    public void OnClick()
    {

        InteractWithTile();

    }

    public void InteractWithTile()
    {
      
        if (AoneGameControl.Instance.isDigging)
        {
            if (AoneGameControl.Instance.extractCounter < 1) return;
            AoneGameControl.Instance.extractCounter--;
            AoneGameControl.Instance.ExtractNeighbordTiles(this);
        }
        else
        {
            if (AoneGameControl.Instance.scanCounter < 1) return;

            Scan();
            AoneGameControl.Instance.scanCounter--;
            AoneGameControl.Instance.ScanNeighbordTiles(this);
        }
    }

    public void Scan()
    {

        switch (resource)
        {
            case RESOURCE_TYPE.NONE:
                image.color = Color.green;
                break;
            case RESOURCE_TYPE.MINIMUM:
                image.color = Color.red;
                break;
            case RESOURCE_TYPE.MEDIUM:
                image.color = Orange;
                break;
            case RESOURCE_TYPE.MAX:
                image.color = Color.yellow;
                break;
        }


    }

    public int Extract()
    {
        int temp = 0;
        switch (resource)
        {
            case RESOURCE_TYPE.NONE:

                image.color = Color.green;
                
                break;
            case RESOURCE_TYPE.MINIMUM:
                image.color = Color.green;
                resource = RESOURCE_TYPE.NONE;
                temp = 100;
                break;
            case RESOURCE_TYPE.MEDIUM:
                image.color = Color.red;
                resource = RESOURCE_TYPE.MINIMUM;
                temp = 200;
                break;
            case RESOURCE_TYPE.MAX:
                image.color = Orange;
                resource = RESOURCE_TYPE.MEDIUM;
                temp = 400;
                break;


        }

        return temp;

    }

    //For Debug
    public void resetColor()
    {
        image.color = Color.white;
    }

    public void Kill()
    {
        GameObject.Destroy(tile);
    }

}

