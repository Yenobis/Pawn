using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private float horizontalMove;
    private float verticalMove;
    private Vector3 playerInput;
    public CharacterController player;
    [SerializeField]private float playerSpeed;
    [SerializeField]private float fallVelocity;
    [SerializeField]private float fuerzaSalto;
    private Vector3 movePlayer;
    private float defaultSpeed;

    public float gravity = 9.8f;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    private bool isOnSlope = false;
    private Vector3 hitNormal;
    [SerializeField]private float slideVelocity;
    [SerializeField] private float slopeForceDown;

    public float max_health = 100f;
    public float cur_health = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<CharacterController>();
        defaultSpeed = playerSpeed;
        cur_health = max_health;
    }
    public bool isAtMaxHealth()
    {
        return (cur_health==max_health);
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

        DeslizaAbajo();
    }
    //Función que detecta la pendiente donde se encuentra el jugador
    public void DeslizaAbajo()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y)* hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;
            movePlayer.y += slopeForceDown;
        }
    }
    //Función para realizar las habilidades del jugador 
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
        
        cur_health = Math.Max(cur_health - amount, 0);

    }
    public void HealDamage(float amount)
    {
        cur_health =Math.Min(cur_health + amount, max_health);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
}
