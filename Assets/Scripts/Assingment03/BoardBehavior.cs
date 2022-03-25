using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public enum Dificulty
{
    Easy,
    Medium,
    Hard
}
public class BoardBehavior : MonoBehaviour
{
    public static BoardBehavior instance;

    public GameObject TilePrefab;
    public TileBehavior[,] tileSpotListB;
    public List<TileData> tileDataList;
    public int width;
    public int height;
    public int numOfImages;
    public TileBehavior selectedTile;
    private AudioSource sound;

    public List<TileBehavior> PossiblePlays;

    private int minutes;
    private float seconds;
    public TextMeshProUGUI ClockText;

    private bool isBomb = false;
    public bool canPlay = true;
    private bool GameOver = false;

    public TileBehavior Bomb;

    public int points;
    public int goalPoints;

    public Slider pointsSlider;
    public TextMeshProUGUI Message;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        tileSpotListB = new TileBehavior[height, width];
        sound = gameObject.GetComponent<AudioSource>();

    }

    public void InitilizeGame(Difficulty difficulty)
    {
        setDificulty(difficulty);
        CreateBoard();
        TestBoard();
    }

    private void setDificulty(Difficulty difficulty)
    {
    
        points = 0;
        canPlay = true;
        GameOver = false;
        Message.text = "Match 3 Or More to make points!";
   
        switch (difficulty)
        {
            case Difficulty.EASY:
                numOfImages = 3;
                minutes = 2;
                goalPoints = 1000;
                pointsSlider.maxValue = goalPoints;
                break; 
            case Difficulty.MEDIUM:
                numOfImages = 4;
                minutes = 1;
                seconds = 30;
                goalPoints = 2000;
                pointsSlider.maxValue = goalPoints;
                break; 
            case Difficulty.HARD:
                numOfImages = 5;
                minutes = 1;
                seconds = 0;
                goalPoints = 4000;
                pointsSlider.maxValue = goalPoints;
                break;
        }
    }
    // Update is called once per frame
    void Update()   
    {
        if(PossiblePlays != null)
        {
            foreach (var t in PossiblePlays)
            {
                t.ChangeColor(Color.green);
            }
        }

        if (!GameOver)
        {
            Timer();

            pointsSlider.value = points;

            if (seconds <= 0 && minutes <= 0)
            {
                EndGame();
            }
            else if (points >= goalPoints)
            {
                WinGame();
            }
        }
        
    }

    public void CreateBoard()
    {
        if(tileSpotListB[0,0] != null)
        {
            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    int tempInt = Random.Range(0, numOfImages);
                    tileSpotListB[i, k].AssignData(tileDataList[tempInt], i, k);
                }
            }
        }
        else
        {
            for (int i = 0; i < height; i++)
            {
                for (int k = 0; k < width; k++)
                {
                    int tempInt = Random.Range(0, numOfImages);

                    var temp = Instantiate(TilePrefab, transform);
                    tileSpotListB[i, k] = temp.GetComponent<TileBehavior>();
                    tileSpotListB[i, k].AssignData(tileDataList[tempInt], i, k);
                }
            }
        }
       
    }

    public TileBehavior retunTileSpotAt(int x, int y)
    {
        return tileSpotListB[x, y];
    }

    public void setPossiblePlayTiles(TileBehavior tile)
    {
        selectedTile = tile;

        int referenceX = selectedTile.positionX;
        int referenceY = selectedTile.positionY;

        if(referenceX + 1 < width)
            PossiblePlays.Add(tileSpotListB[referenceX + 1, referenceY]);
        if(referenceX - 1 >= 0)
            PossiblePlays.Add(tileSpotListB[referenceX - 1, referenceY]);
        if (referenceY + 1 < height)
            PossiblePlays.Add(tileSpotListB[referenceX, referenceY + 1]);
        if (referenceY - 1 >= 0)
            PossiblePlays.Add(tileSpotListB[referenceX, referenceY - 1]);
    }

    public void cleanPossiblePlayList()
    {
        foreach(var t in PossiblePlays)
        {
            t.ChangeColor(Color.white);
        }

        TestBoard();
        PossiblePlays.Clear();
        selectedTile = null;
    }

    public void SwapTiles(TileBehavior tile)
    {

        TileData tempData = tile.tileData;

        TileBehavior tempTB = selectedTile.GetComponent<TileBehavior>();

        tile.GetComponent<TileBehavior>().ChangeData(tempTB.tileData);

        tempTB.ChangeData(tempData);
    }

    public void TestBoard()
    {
        if (!GameOver)
        {
            List<bool> hasMachtedList = new List<bool>();
            foreach (var t in tileSpotListB)
            {
                hasMachtedList.Add(TestTile(t));
            }

            if (TestBoolList(hasMachtedList))
            {
                StartCoroutine(waitforAnimation());
            }
            else
            {
                canPlay = true;
            }


            if (isBomb)
            {
                isBomb = false;
            }
        }

    }

    public bool TestBoolList(List<bool> list)
    {
        foreach (bool b in list)
        {
            if(b == true)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator waitforAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        TestBoard();

        if (Bomb != null)
        {
            tileSpotListB[Bomb.positionX, Bomb.positionY].ChangeData(tileDataList[5]);
            Bomb = null;
        }
    }

    public bool TestTile(TileBehavior t)
    {
        List<TileBehavior> listToBeConsumed = new List<TileBehavior>();

        int X = t.positionX;
        int Y = t.positionY;
        List<TileBehavior> combinationListHorizional = new List<TileBehavior>();
        List<TileBehavior> combinationListVertical = new List<TileBehavior>();

        if (Y >= 0)
        {

            for (int i = Y; i < height; i++)
            {
                if (tileSpotListB[X, i].ReturnTileType() == t.ReturnTileType())
                {
                    combinationListHorizional.Add(tileSpotListB[X, i]);
                }
                else
                {
                    break;
                }
            }

        }

        if (X >= 0)
        {

            for (int i = X; i < width; i++)
            {
                if (tileSpotListB[i, Y].ReturnTileType() == t.ReturnTileType())
                {
                    combinationListVertical.Add(tileSpotListB[i, Y]);
                }
                else
                {
                    break;
                }
            }

        }

        if(combinationListHorizional.Count >= 3)
        {
           for(int i = 0; i < combinationListHorizional.Count; i++)
           {
                if(i != 0)
                {
                    TestTile(combinationListHorizional[i]);
                }

                listToBeConsumed.Add(combinationListHorizional[i]);
           }
        }  

        if(combinationListVertical.Count >= 3)
        {
           for(int i = 0; i < combinationListVertical.Count; i++)
           {
                if(i != 0)
                {
                    TestTile(combinationListVertical[i]);
                }

                listToBeConsumed.Add(combinationListVertical[i]);
           }
        }

        if(listToBeConsumed.Count > 0)
        {
            if(listToBeConsumed.Count > 4 && !isBomb)
            {
                Bomb = listToBeConsumed[3];
            }

            ConsumeList(listToBeConsumed);
            return true;
        }
        else
        {
            return false;
        }
    }
    private void ConsumeList(List<TileBehavior> list)
    {
        foreach (var i in list)
        {
            ConsumeTile(i);
        }

        list.Clear();

    }

    public IEnumerator WaitBeforeColapse(TileBehavior t)
    {
        yield return new WaitForSeconds(0.5f);

        for (int i = t.positionX; i >= 0; i--)
        {
            if (i == 0)
            {
                int tempInt = Random.Range(0, numOfImages);
                tileSpotListB[i, t.positionY].ChangeData(tileDataList[tempInt]);
            }
            else if(i > 0)
            {
                tileSpotListB[i, t.positionY].ChangeData(tileSpotListB[i - 1, t.positionY].tileData);
            }


            t.ChangeColor(Color.white);

        }
      
        sound.Play();

    }

    public void ConsumeTile(TileBehavior t)
    {
        t.ChangeColor(Color.blue);
        points += 5;
        StartCoroutine(WaitBeforeColapse(t));
    }

    private void Timer()
    {
        seconds -= Time.deltaTime;

        if (minutes <= 0 && seconds <= 0)
        {
            return;
        }

        if (seconds <= 0)
        {
            if (minutes < 0)
            {
                Debug.Log("Game Over");
            }
            else
            {
                seconds = 60;

                minutes -= 1;
            }
        }

        ClockText.text = "0" + minutes.ToString() + ":" + ((int)seconds).ToString();

    }


    public void Explode(TileBehavior t)
    {
        int referenceX = t.positionX;
        int referenceY = t.positionY;
        List<TileBehavior> BombedTiles = new List<TileBehavior>();

        BombedTiles.Add(tileSpotListB[referenceX, referenceY]);

        if (referenceX + 1 < width)
        {
            BombedTiles.Add(tileSpotListB[referenceX + 1, referenceY]);

            //Diagonals top
            if (referenceY + 1 < height)
                BombedTiles.Add(tileSpotListB[referenceX + 1, referenceY + 1]);
            if (referenceY - 1 >= 0)
                BombedTiles.Add(tileSpotListB[referenceX + 1, referenceY - 1]);
        }
        if (referenceX - 1 >= 0)
        {
            BombedTiles.Add(tileSpotListB[referenceX - 1, referenceY]);

            //Diagonals botton
            if (referenceY + 1 < height)
                BombedTiles.Add(tileSpotListB[referenceX - 1, referenceY + 1]);
            if (referenceY - 1 >= 0)
                BombedTiles.Add(tileSpotListB[referenceX - 1, referenceY - 1]);
        }
       
        if (referenceY + 1 < height)
            BombedTiles.Add(tileSpotListB[referenceX, referenceY + 1]);
        if (referenceY - 1 >= 0)
            BombedTiles.Add(tileSpotListB[referenceX, referenceY - 1]);

        

        isBomb = true;

        ConsumeList(BombedTiles);

        StartCoroutine(waitforAnimation());

    }

    public void EndGame()
    {
        canPlay = false;
        GameOver = true;
        Message.text = "You Lose Press E to return to the Main Game";
    }

    public void WinGame()
    {
        canPlay = false;
        GameOver = true;
        Message.text = "You Win!! Press E to return to the Main Game";
    }
        
}
