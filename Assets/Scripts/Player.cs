using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{


     private CharacterController controller;

    [Header("Movement Properties")]
    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHight = 3.0f;
    public Vector3 velocity;

    [Header("GroundDetection Properties")]
    public Transform groundCheck;
    public float groundRadius = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    private bool isPlayingMiniGame = false;

    // Start is called before the first frame update
    void Start()
    {
       controller = GetComponent<CharacterController>();
       GM.Instance.startedMiniGame.AddListener(GameStarted);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayingMiniGame) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * maxSpeed * Time.deltaTime);

        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    private void GameStarted()
    {
        isPlayingMiniGame = !isPlayingMiniGame;
    }
}
