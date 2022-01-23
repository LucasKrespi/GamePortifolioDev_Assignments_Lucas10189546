using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    [Header("Player Camera properties")]
    public float mouseSensitivitie = 10.0f;
    public Transform playerBody;

    private float XRotation = 0.0f;

    private bool isPlayingMiniGame = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GM.Instance.startedMiniGameOne.AddListener(GameStarted);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayingMiniGame) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivitie;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivitie;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void GameStarted()
    {
        isPlayingMiniGame = !isPlayingMiniGame;
    }
}

