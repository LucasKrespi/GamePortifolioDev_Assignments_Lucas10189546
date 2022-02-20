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
    LOCK_BROCKEN
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
    private int minutes = 1;
    private float seconds = 30;

    //UI Varibles
    public TextMeshProUGUI ClockText;

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
        Cursor.visible = false;

        initialRotationLeverOne = leverOnePivot.transform.eulerAngles;
        initialRotationLeverTwo = leverTwoPivot.transform.eulerAngles;
        initialRotationLock = lockGameObject.transform.eulerAngles;

        UpdateStage(LockPicking_State.STAGE_ONE);

        goalOneAngle = Random.Range(0.0f , 360.0f);
        goalTwoAngle = Random.Range(0.0f, 360.0f);
        goalThreeAngle = 270.0f;

        Debug.Log("ANGLE ONE = " + goalOneAngle);
        Debug.Log("ANGLE TWO = " + goalTwoAngle);
    }

    // Update is called once per frame
    void Update()
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

                if (lockGameObject.transform.eulerAngles.z > 10 &&
                   lockGameObject.transform.eulerAngles.z < 350)
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
        }

        Timer();
        UpdateUI();
    }

    private void Timer()
    {
        seconds -= Time.deltaTime;

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

    public void UpdateUI()
    {
        ClockText.text = "0" + minutes.ToString() + ":" + ((int)seconds).ToString();
    }
    public void UpdateStage( LockPicking_State newState)
    {
        currentState = newState;
        UpdateMarkers(newState);
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
                if (!isReseting)
                {
                    StartCoroutine(Resetdelay());
                }
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

        minutes = 1;
        seconds = 30;

        UpdateStage(LockPicking_State.STAGE_ONE);

        isReseting = false;
    }

    public IEnumerator Resetdelay()
    {
        isReseting = true;

        yield return new WaitForSeconds(2);

        ResetGame();
    }
}
