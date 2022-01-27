using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AoneGameControl : MonoBehaviour
{
    //Singleton Class
    public static AoneGameControl Instance;


    public Canvas board;

    public GameObject TilePrefab;

    private int hight = 32;
    private int width = 32;

    public Tile[,] tilesList;

    private Toggle isDiggingToogle;
    public bool isDigging = true;
    private TextMeshProUGUI rescourceScoreText;
    private int rescourceCollected = 0;

    //Counters
    public int scanCounter = 6;
    public int extractCounter = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        tilesList = new Tile[hight, width];

        isDiggingToogle = GetComponentInChildren<Toggle>();
        isDiggingToogle.onValueChanged.AddListener(delegate { OnToggleChange(); });
        rescourceScoreText = GetComponentInChildren<TextMeshProUGUI>();
      

        CreateBoard();
    }

    private void OnToggleChange()
    {
        isDigging = !isDigging;
    }
    void CreateBoard()
    {
        for(int y = 0; y < hight; y++)
        {
            for(int x = 0; x < width; x++)
            {
                tilesList[x, y] = new Tile(Instantiate(TilePrefab, board.transform), x, y);
               
            }
        }
    }

    public void ExtractNeighbordTiles(Tile CallinTile)
    {
        
        foreach(Tile t in tilesList)
        {
            foreach(Vector2 pos in CallinTile.FindneighbourTilesExtract())
            {
                if(t.coord == pos)
                {
                    rescourceCollected += t.Extract();
                }
            }
        }

        UpdateScore();
    }

    public void ScanNeighbordTiles(Tile CallinTile)
    {

        foreach (Tile t in tilesList)
        {
            foreach (Vector2 pos in CallinTile.FindneighbourTilesScan())
            {
                if (t.coord == pos)
                {
                   t.Scan();
                }
            }
        }

        UpdateScore();
    }



    private void UpdateScore()
    {
        rescourceScoreText.text = "Resource Collected: " + rescourceCollected;
    }

}

public class Tile
{
    public GameObject tile;

    private Image image;

    public Button button;

    public int resource;

    public Vector2 coord;

    public Tile(GameObject prefab, int x, int y)
    {
        tile = prefab;
        button = tile.GetComponent<Button>();
        image = tile.GetComponent<Image>();
        button.onClick.AddListener(OnClick);

        coord.x = x;
        coord.y = y;

        resource = Random.RandomRange(0, 3);
    }

    public void OnClick()
    {

       InteractWithTile();
      
    }

    public int InteractWithTile()
    {
        int temp = 0;
        if (AoneGameControl.Instance.isDigging)
        {
            if (AoneGameControl.Instance.extractCounter < 1) return temp;

            temp = Extract();
           // AoneGameControl.Instance.extractCounter--;
            AoneGameControl.Instance.ExtractNeighbordTiles(this);
        }
        else
        {
            if (AoneGameControl.Instance.scanCounter < 1) return temp;

            Scan();
           // AoneGameControl.Instance.scanCounter--;
            AoneGameControl.Instance.ScanNeighbordTiles(this);
        }


        return temp;
    }

    public void Scan()
    {

        switch (resource)
        {
            case 0:
                image.color = Color.green;
                break;
            case 1:
                image.color = Color.blue;
                break;
            case 2:
                image.color = Color.red;
                break;
            case 3:
                image.color = Color.black;
                break;
        }

       
    }

    public int Extract()
    {
        int temp;
        switch (resource)
        {
            case 0:
                image.color = Color.green;
                break;
            case 1:
                image.color = Color.blue;
                break;
            case 2:
                image.color = Color.red;
                break;
            case 3:
                image.color = Color.black;
                break;


        }

        temp = resource;
        resource = (int)(resource * 0.5f);
       
        return temp;
 
    }

    //Returns an array with a vector 2 with coodenates x and y
    public List<Vector2> FindneighbourTilesScan()
    {

        List<Vector2> Temp = new List<Vector2>();
        int loopRangeX = (int)coord.x + 2;
        int loopRangeY = (int)coord.y + 2;
        int offsetX = 0;
        int offsetY = 0;

    
        if (coord.x >= 1)
        {
            offsetX = -1;
        }

        if (coord.y >= 1)
        {
            offsetY = -1;
        }

        for (int x = (int)coord.x + offsetX; x < loopRangeX; x++)
        {
            for (int y = (int)coord.y + offsetY; y < loopRangeY; y++)
            {
                Temp.Add(new Vector2(x, y));
            }
        }

        return Temp;
    }

    public List<Vector2> FindneighbourTilesExtract()
    {

        List<Vector2> Temp = new List<Vector2>();
        int loopRangeX = (int)coord.x + 3;
        int loopRangeY = (int)coord.y + 3;
        int offsetX = 0;
        int offsetY = 0;


        if (coord.x == 1)
        {
            offsetX = -1;
        }
        if (coord.x >= 2)
        {
            offsetX = -2;
        }


        if (coord.y == 1)
        {
            offsetY = -1;
        }

        if (coord.y >= 2)
        {
            offsetY = -2;
        }

        for (int x = (int)coord.x + offsetX; x < loopRangeX; x++)
        {
            for (int y = (int)coord.y + offsetY; y < loopRangeY; y++)
            {
                Temp.Add(new Vector2(x, y));
            }
        }

        return Temp;
    }
}
