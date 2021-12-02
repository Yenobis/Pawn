using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject[] vidas;
    private float horizontalMove;
    private float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;
    [SerializeField]private float playerSpeed;
    [SerializeField]private float fallVelocity;
    [SerializeField]private float fuerzaSalto;
    private Vector3 movePlayer;

    public float gravity = 9.8f;
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private float defaultSpeed;
    public float max_health = 100f;
    public float cur_health = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<CharacterController>();
        defaultSpeed = playerSpeed;
        cur_health = max_health;
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
    //Funci�n para determinar la direcci�n a la que mira la c�mara
    void camDirection() 
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;

    }
    //Funci�n para establecer la gravedad
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
    //Funci�n para realizar las habilidades del jugador 
    void HabilidadesJugador() 
    {
        //Debug.Log(player.isGrounded);
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
    public void TakeDamage(float amount)
    {
        if (cur_health > 0)
        {
            if (cur_health - amount >= 0)
            {
                cur_health -= amount;
            }
            else
            {
                cur_health = 0;
            }
        }
    }
}
