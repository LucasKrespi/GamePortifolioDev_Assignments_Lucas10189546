using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assigment01GameControl : MonoBehaviour
{
    private Canvas miniGameCanvas;
    [SerializeField]
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
            GM.Instance.startedMiniGameOne.Invoke();
        }
    }

    private void ToggleCanvas()
    {
        isActive = !isActive;
        miniGameCanvas.gameObject.SetActive(isActive);
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
}
