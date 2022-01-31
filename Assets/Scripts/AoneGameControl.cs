using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RESOURCE_TYPE
{
    NONE = 0,
    MINIMUM = 1,
    MEDIUM = 2,
    MAX = 3
}
public class AoneGameControl : MonoBehaviour
{
    //Singleton Class
    public static AoneGameControl Instance;

    //Board Variables
    public Canvas board;

    public GameObject TilePrefab;

    private int hight = 32;
    private int width = 32;

    public Tile[,] tilesList;

    private int numOfGoldenTiles = 23;

    private List<Tile> goldenTilesList;
    //UI Components
    private Toggle isDiggingToogle;
    public bool isDigging = true;
    private TextMeshProUGUI rescourceScoreText;
    private int resourceCollected = 0;
    private TextMeshProUGUI message;

    //Counters
    public int scanCounter = 6;
    public int extractCounter = 3;

    //Fill Bars
    private Slider scanBarSlider;
    private Slider extractBarSlider;

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

        isDiggingToogle = GetComponentInChildren<Toggle>();
        isDiggingToogle.onValueChanged.AddListener(delegate { OnToggleChange(); });
        rescourceScoreText = transform.Find("Resource").GetComponent<TextMeshProUGUI>();

        scanBarSlider = transform.Find("ScanBar").GetComponent<Slider>();
        extractBarSlider = transform.Find("ExtractBar").GetComponent<Slider>();
        message = transform.Find("Message Board/DialogBox/Message").GetComponent<TextMeshProUGUI>();

    }
    // Start is called before the first frame update
    void Start()
    {
        tilesList = new Tile[hight, width];
        CreateBoard();
        SetTilesValeu();
        UpdateUI();
    }
    private void Update()
    {
        CheckGameOver();
    }

    //======================= UI Management
    private void OnToggleChange()
    {
        if (isDiggingToogle.isOn)
        {
            isDigging = true;
        }
        else
        {
            isDigging = false;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        rescourceScoreText.text = "Resource Collected: " + resourceCollected;

        if (extractCounter <= 0)
        {
            message.text = "You collected a total of " + resourceCollected + " and You have no more fuel. Press E to go back to main game and reset this game.";
        }
        else if (isDigging)
        {
            message.text = "Extract Mode is active click on tile to get its resources";
        }
        else
        {
            if (scanCounter <= 0)
            {
                message.text = "No fuel left to scam.";
            }
            else
            {
                message.text = "Scam Mode is active click on tile to get a pick on its resources.";
            }
        }

        scanBarSlider.value = scanCounter;
        extractBarSlider.value = extractCounter;
    }

    // ===================== Board Creation ==================
    private void CreateBoard()
    {
        int tilecounter = 0;
        goldenTilesList = new List<Tile>();

        for (int y = 0; y < hight; y++)
        {
            for(int x = 0; x < width; x++)
            {
                RESOURCE_TYPE rTemp;
                //This is making possible to have golden/MAX tiles on x  1/7/13/19/25/31
                if (tilecounter % 6 == 1 && numOfGoldenTiles > 0)
                {
                    rTemp = (RESOURCE_TYPE)Random.Range(0, 4);
                }
                else
                {
                    rTemp = (RESOURCE_TYPE)Random.Range(0, 3);
                }
                   
                if(rTemp == RESOURCE_TYPE.MAX)
                {
                   
                    tilesList[x, y] = new Tile(Instantiate(TilePrefab, board.transform), x, y, rTemp);
                    if(CheckIfOverlaying(tilesList[x, y]))
                    {
                        //If new golden neighbour tiles are not colliding another godentile neighbours, the new goldentile is created
                        goldenTilesList.Add(tilesList[x, y]);
                        numOfGoldenTiles--;
                    }
                    else
                    {
                        //Otherwise its ressource is set at randon to a value lower then golden
                        tilesList[x, y].resource = (RESOURCE_TYPE)Random.Range(0, 3);
                    }
                    
                    tilecounter++;
                }
                else
                {
                    tilesList[x, y] = new Tile(Instantiate(TilePrefab, board.transform), x, y, rTemp);
                    tilecounter++;
                }       
            }
        }
    }

    //Will Check if neighbours of existing golden tile are not colliding with new goldentile neighbours
    private bool CheckIfOverlaying(Tile Test)
    {
        foreach(Tile t in goldenTilesList)
        {
            foreach(Tile nb in FindneighbourTilesExtract(Test))
            {
                foreach (Tile tnb in FindneighbourTilesExtract(t))
                {
                    if (tnb == nb)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    //Will accert the valeu of the neighbour tiles of a golden tile
    private void SetTilesValeu()
    {
        List<Tile> Temp = new List<Tile>();

        foreach (Tile t in goldenTilesList)
        {
            Temp = FindneighbourTilesExtract(t);
            foreach(Tile nb in Temp)
            {
                nb.resource = RESOURCE_TYPE.MINIMUM;
            }

            Temp = FindneighbourTilesScan(t);
            foreach (Tile nb in Temp)
            {
                nb.resource = RESOURCE_TYPE.MEDIUM;
            }

            t.resource = RESOURCE_TYPE.MAX;
        }
    }

    //Will Call the extract func from all tiles in a 5x5 grid arround the CallinTile
    public void ExtractNeighbordTiles(Tile CallinTile)
    {
        
        foreach(Tile t in tilesList)
        {
            foreach(Tile nb in FindneighbourTilesExtract(CallinTile))
            {
                if(t == nb)
                {
                    resourceCollected += t.Extract();
                }
            }
        }

        UpdateUI();
    }

    //Will Call the scan func from all tiles in a 5x5 grid arround the CallinTile
    public void ScanNeighbordTiles(Tile CallinTile)
    {

        foreach (Tile t in tilesList)
        {
            foreach (Tile nb in FindneighbourTilesScan(CallinTile))
            {
                if (t == nb)
                {
                   t.Scan();
                }
            }
        }

        UpdateUI();
    }

    //==================== Tiles Management ==================== 
    //Return a 5x5 Grid of Tiles arround the tile passed
    public List<Tile> FindneighbourTilesExtract(Tile tile)
    {

        List<Tile> Temp = new List<Tile>();
        int loopRangeX = (int)tile.coord.x + 3;
        int loopRangeY = (int)tile.coord.y + 3;
        int offsetX = 0;
        int offsetY = 0;


        if (tile.coord.x == 1)
        {
            offsetX = -1;
        }
        if (tile.coord.x >= 2)
        {
            offsetX = -2;    
        }
       
        if (tile.coord.y == 1)
        {
            offsetY = -1;
        }

        if (tile.coord.y >= 2)
        {
            offsetY = -2;
        }

        for (int x = (int)tile.coord.x + offsetX; x < loopRangeX; x++)
        {
            for (int y = (int)tile.coord.y + offsetY; y < loopRangeY; y++)
            {
                if(x < width && y < hight)
                    Temp.Add(tilesList[x, y]);
            }
        }

        return Temp;
    }

    //Return a 5x5 Grid of Tiles arround the tile passed
    public List<Tile> FindneighbourTilesScan(Tile tile)
    {

        List<Tile> Temp = new List<Tile>();
        int loopRangeX = (int)tile.coord.x + 2;
        int loopRangeY = (int)tile.coord.y + 2;
        int offsetX = 0;
        int offsetY = 0;


        if ((int)tile.coord.x >= 1)
        {
            offsetX = -1;
        }

        if ((int)tile.coord.y >= 1)
        {
            offsetY = -1;
        }

        for (int x = (int)tile.coord.x + offsetX; x < loopRangeX; x++)
        {
            for (int y = (int)tile.coord.y + offsetY; y < loopRangeY; y++)
            {
                if (x < width && y < hight)
                    Temp.Add(tilesList[x, y]);
            }
        }

        return Temp;
    }


    //===================== Game Management ===================

    private void CheckGameOver()
    {
        if(extractCounter <= 0)
        {
            foreach(Tile t in tilesList)
            {
                t.Scan();
            }
        }
    }
    public void ResetGame()
    {
        //Reset Variables
        scanCounter = 6;
        extractCounter = 3;
        numOfGoldenTiles = 23;
        resourceCollected = 0;
        isDigging = true;
        
        //Clean lists
        foreach(Tile t in tilesList)
        {
            t.Kill();
        }

        goldenTilesList.Clear();

        isDiggingToogle.isOn = true;
        // Generate new board
        CreateBoard();
        SetTilesValeu();
        UpdateUI();
    }
}

