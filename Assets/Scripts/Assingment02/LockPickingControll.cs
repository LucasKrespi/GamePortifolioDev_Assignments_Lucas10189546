using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public enum LockPicking_State
{
    STAGE_ONE,
    STAGE_TWO,
    STAGE_THREE,
    LOCK_OPEN,
    LOCK_BROCKEN,
    GAME_OVER_LIVE,
    GAME_OVER_TIME
}
public class LockPickingControll : MonoBehaviour
{
    public static LockPickingControll instance;

    public Image stageOneMarker;
    public Image stageTwoMarker;
    public Image stageThreeMarker;

    [SerializeField]
    private GameObject leverOnePivot;
    [SerializeField]
    private GameObject leverTwoPivot;
    [SerializeField]
    private GameObject lockGameObject;

    //Initial Positions for reset
    private Vector3 initialRotationLeverOne;
    private Vector3 initialRotationLeverTwo;
    private Vector3 initialRotationLock;

    public float rotationSpeed;

    private LockPicking_State currentState;

    private float goalOneAngle;
    private float goalTwoAngle;
    private float goalThreeAngle;
    private float offsetAsseptance = 10.0f;

    private bool isReseting = false;

    private int lives = 3;
    private int minutes;
    private float seconds;

    public bool pause = true;
    //UI Varibles
    public TextMeshProUGUI ClockText;
    public TextMeshProUGUI LivesText;
    public TextMeshProUGUI InstructionsText;
    public TextMeshProUGUI DificultyText;


    public Difficulty currentdifficulty;
    

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

        initialRotationLeverOne = leverOnePivot.transform.eulerAngles;
        initialRotationLeverTwo = leverTwoPivot.transform.eulerAngles;
        initialRotationLock = lockGameObject.transform.eulerAngles;

        UpdateStage(LockPicking_State.STAGE_ONE);

        goalOneAngle = Random.Range(0.0f , 360.0f);
        goalTwoAngle = Random.Range(0.0f, 360.0f);
        goalThreeAngle = 270.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            switch (currentState)
            {
            case LockPicking_State.STAGE_ONE:
                    ControllerLeverOne();
               
                    if (leverOnePivot.transform.eulerAngles.z > goalOneAngle - offsetAsseptance &&
                        leverOnePivot.transform.eulerAngles.z < goalOneAngle + offsetAsseptance)
                    {
                        UpdateStage(LockPicking_State.STAGE_TWO);
                    }
                break;

            case LockPicking_State.STAGE_TWO:
                    ControllLockRotation();
                    ControllerLeverTwo();

                    if(leverTwoPivot.transform.eulerAngles.z > goalTwoAngle - offsetAsseptance &&
                       leverTwoPivot.transform.eulerAngles.z < goalTwoAngle + offsetAsseptance)
                    {
                        UpdateStage(LockPicking_State.STAGE_THREE);
                    }

                    if (lockGameObject.transform.eulerAngles.z > offsetAsseptance * 3 &&
                       lockGameObject.transform.eulerAngles.z < 360 - offsetAsseptance * 3)
                    {
                        UpdateStage(LockPicking_State.LOCK_BROCKEN);
                    }


                    break;

            case LockPicking_State.STAGE_THREE:
                    ControllLockRotation();
                

                    if (lockGameObject.transform.eulerAngles.z < goalThreeAngle + offsetAsseptance &&
                        lockGameObject.transform.eulerAngles.z > goalThreeAngle - offsetAsseptance)
                    {
                        UpdateStage(LockPicking_State.LOCK_OPEN);
                    }

                    break;

            case LockPicking_State.LOCK_OPEN:
                    return;  
            case LockPicking_State.GAME_OVER_LIVE:
                    return;
            case LockPicking_State.GAME_OVER_TIME:
                    return;
            }

            Timer(); 

            if(lives <= 0)
            {
                UpdateStage(LockPicking_State.GAME_OVER_LIVE);
            }
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentdifficulty = difficulty;

        switch (difficulty)
        {
            case Difficulty.EASY:
                offsetAsseptance = 10.0f;
                minutes = 1;
                seconds = 30;
                break;
            case Difficulty.MEDIUM:
                offsetAsseptance = 5.0f;
                minutes = 0;
                seconds = 60;
                break;
            case Difficulty.HARD:
                offsetAsseptance = 2.5f;
                minutes = 0;
                seconds = 30;
                break;
        }
        DificultyText.text = currentdifficulty.ToString();
    }
    private void Timer()
    {
        seconds -= Time.deltaTime;

        if(minutes <= 0 && seconds <= 0)
        {
            UpdateStage(LockPicking_State.GAME_OVER_TIME);
            return;
        }

        if(seconds <= 0)
        {
            if(minutes < 0)
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

    public void UpdateUI(LockPicking_State newState)
    {
        LivesText.text = "Lives: " + lives;

        switch (newState)
        {
            case LockPicking_State.STAGE_ONE:
                InstructionsText.text = " Press A and D to move the botton lever and find the right angle, but go slow if you rotate to fast you may turn the lock and break it.";
                break;
            case LockPicking_State.STAGE_TWO:
                InstructionsText.text = " Rotate the the second lever with the mouse to find the right angle, do not move with A and D it may brack the lock.";
                break;
            case LockPicking_State.STAGE_THREE:
                InstructionsText.text = " Press A and D to rotate the lock. Tip: right opens it.";
                break;
            case LockPicking_State.LOCK_OPEN:
                InstructionsText.text = " LOCK OPEN !!!! Press E to go back to the main game. ";
                break;
            case LockPicking_State.LOCK_BROCKEN:
                InstructionsText.text = " You brock the lock. :(";
                break;
            case LockPicking_State.GAME_OVER_LIVE:
                InstructionsText.text = " You Have no more lives, press E to go back to the main game. ";
                break;
            case LockPicking_State.GAME_OVER_TIME:
                InstructionsText.text = " You Have no more time, press E to go back to the main game. ";
                break;

        }

    }
    public void UpdateStage(LockPicking_State newState)
    {
        currentState = newState;
        UpdateMarkers(newState);
        UpdateUI(newState);
    }

    public void UpdateMarkers(LockPicking_State newState)
    {
        stageOneMarker.color = Color.blue;
        stageTwoMarker.color = Color.blue;
        stageThreeMarker.color = Color.blue;

        switch (newState)
        {
        case LockPicking_State.STAGE_TWO:
                stageOneMarker.color = Color.green;
                break;
        case LockPicking_State.STAGE_THREE:
                stageOneMarker.color = Color.green;
                stageTwoMarker.color = Color.green;
                break;
        case LockPicking_State.LOCK_OPEN:
                stageOneMarker.color = Color.green;
                stageTwoMarker.color = Color.green;
                stageThreeMarker.color = Color.green;
                break;
        case LockPicking_State.LOCK_BROCKEN:
                stageOneMarker.color = Color.red;
                stageTwoMarker.color = Color.red;
                stageThreeMarker.color = Color.red;
                if (!isReseting)
                {
                    StartCoroutine(Resetdelay());
                }
                break;
        case LockPicking_State.GAME_OVER_LIVE:
                stageOneMarker.color = Color.red;
                stageTwoMarker.color = Color.red;
                stageThreeMarker.color = Color.red;
                break;
         case LockPicking_State.GAME_OVER_TIME:
                stageOneMarker.color = Color.red;
                stageTwoMarker.color = Color.red;
                stageThreeMarker.color = Color.red;
                break;
        }
       
    }

    public void ControllerLeverOne()
    {

        if (Input.GetKey(KeyCode.D))
        {
            leverOnePivot.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f * rotationSpeed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            leverOnePivot.transform.Rotate(new Vector3(0.0f, 0.0f, -1.0f * rotationSpeed * Time.deltaTime));
        }
    }

    public void ControllerLeverTwo()
    {
        
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(leverTwoPivot.transform.position);

        
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        leverTwoPivot.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen)));

    }

    float AngleBetweenTwoPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2((b.y - a.y), (b.x - a.x)) * Mathf.Rad2Deg;
    }

    public void ControllLockRotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            
            lockGameObject.transform.Rotate(new Vector3(0.0f, 0.0f, -1.0f * rotationSpeed * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            
            lockGameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f * rotationSpeed * Time.deltaTime));
        }

        if(lockGameObject.transform.eulerAngles.z >= 95 && lockGameObject.transform.eulerAngles.z <= 180)
        {
            lockGameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90);
        }
        if(lockGameObject.transform.eulerAngles.z <= 265 && lockGameObject.transform.eulerAngles.z >= 180)
        {
            lockGameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 270);
        }
    }


    public void ResetGame()
    {
        leverOnePivot.transform.eulerAngles = initialRotationLeverOne;
        leverTwoPivot.transform.eulerAngles = initialRotationLeverTwo;
        lockGameObject.transform.eulerAngles = initialRotationLock;


        goalOneAngle = Random.Range(0.0f, 360.0f);
        goalTwoAngle = Random.Range(0.0f, 360.0f);

        SetDifficulty(currentdifficulty);
        UpdateStage(LockPicking_State.STAGE_ONE);

        isReseting = false;

    }

    public IEnumerator Resetdelay()
    {
        isReseting = true;

        lives--;

        yield return new WaitForSeconds(2);

        ResetGame();

    }

    public void setLives(int lives)
    {
        this.lives = lives;
    }
}
