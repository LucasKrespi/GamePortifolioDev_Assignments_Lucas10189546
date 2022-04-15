using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFourCanvasControl : MonoBehaviour
{
    private Canvas miniGameCanvas;
    private AFourGameControl fourGameControl;
    [SerializeField]
    private Canvas difficultySelectCanvas;
    private bool playerIsColliding;
    private bool isActive = true;

    // Start is called before the first frame update

    private void Awake()
    {
        miniGameCanvas = gameObject.GetComponentInChildren<Canvas>();
        fourGameControl = miniGameCanvas.GetComponent<AFourGameControl>();
    }
    void Start()
    {
        
        ToggleCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsColliding)
        {
            ToggleCanvas();
            GM.Instance.startedMiniGame.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ToggleCanvas()
    {
        isActive = !isActive;
        miniGameCanvas.gameObject.SetActive(isActive);

        difficultySelectCanvas.gameObject.SetActive(isActive);

        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            
            fourGameControl.ResetGame();

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsColliding = false;
        }
    }


    public void EasyButtonClick()
    {
        AFourGameControl.Instance.EasyButtonClick();
        difficultySelectCanvas.gameObject.SetActive(false);
    }
    public void MediumButtonClick()
    {
        AFourGameControl.Instance.MediumButtonClick();
        difficultySelectCanvas.gameObject.SetActive(false);
    }
    public void HardButtonClick()
    {
        AFourGameControl.Instance.HardButtonClick();
        difficultySelectCanvas.gameObject.SetActive(false);
    }

}
