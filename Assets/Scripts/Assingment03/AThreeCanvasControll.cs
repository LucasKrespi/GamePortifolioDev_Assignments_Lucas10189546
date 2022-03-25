using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AThreeCanvasControll : MonoBehaviour
{
    private Canvas miniGameCanvas;
    [SerializeField]
    private Canvas difficultySelectCanvas;
    private bool playerIsColliding;
    private bool isActive = true;




    // Start is called before the first frame update
    void Start()
    {
        miniGameCanvas = gameObject.GetComponentInChildren<Canvas>();
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

    public void HardButtonClick()
    {
        difficultySelectCanvas.gameObject.SetActive(false);
        BoardBehavior.instance.InitilizeGame(Difficulty.HARD);
    }    
    public void MediunButtonClick()
    {
        difficultySelectCanvas.gameObject.SetActive(false);
        BoardBehavior.instance.InitilizeGame(Difficulty.MEDIUM);
    }   
    public void EasyButtonClick()
    {
        difficultySelectCanvas.gameObject.SetActive(false);
        BoardBehavior.instance.InitilizeGame(Difficulty.EASY);
    }

}
