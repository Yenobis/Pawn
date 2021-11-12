using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;
    public float playerSpeed;
    public float fallVelocity;
    public float fuerzaSalto;
    private Vector3 movePlayer;

    public float gravity = 9.8f;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private float defaultSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<CharacterController>();
        defaultSpeed = playerSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0.0f, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer = movePlayer * playerSpeed;

        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();

        HabilidadesJugador();

        player.Move(movePlayer * Time.deltaTime);

    }
    //Función para determinar la dirección a la que mira la cámara
    void camDirection() 
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

    }
    //Función para establecer la gravedad
    void SetGravity() 
    {
        if (player.isGrounded) 
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else 
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        
    }
    //Función para realizar las habilidades del jugador 
    void HabilidadesJugador() 
    {
        if (player.isGrounded && Input.GetButtonDown("Jump")) {

            fallVelocity = fuerzaSalto;
            movePlayer.y = fallVelocity;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerSpeed = playerSpeed * 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerSpeed = defaultSpeed;
        }

    }
   
}
