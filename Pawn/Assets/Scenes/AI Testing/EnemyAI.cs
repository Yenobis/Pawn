using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    Animator animator;

    public LayerMask whatIsGround, whatIsPlayer;

    public float speed;
    public float health;
    float damage = 5f;

    //Patroling
    public Vector3 walkPoint;
    public Vector3 pos_player;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Pawn").transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    void Start()
    {
        speed = GetComponent<NavMeshAgent>().speed;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isAttacking = animator.GetBool("isAttacking");
        health = GameObject.Find("Enemigo").GetComponent<HealthScript>().cur_health;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (health <= 0) animator.SetTrigger("Die");
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }


    private void Patroling()
    {
        GetComponent<NavMeshAgent>().speed = speed;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);
        

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
        
    }

    private void ChasePlayer()
    {
        GetComponent<NavMeshAgent>().speed = speed + 1;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", true);
        pos_player = new Vector3(player.position.x, 0.5f, player.position.z);
        agent.SetDestination(player.position);
        //transform.LookAt(pos_player);
    }

    private void AttackPlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        pos_player = new Vector3(player.position.x, 0.5f, player.position.z);
        transform.LookAt(pos_player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            /* Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            */

            ///
            animator.SetBool("isAttacking", true);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        animator.SetBool("isAttacking", false);
        alreadyAttacked = false;
    }


    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Destroy(gameObject, .5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    // Start is called before the first frame update
    
    void dealingDamage()
    {
        GetComponentInChildren<Enemy_Sword>().atacando = true;
        Debug.Log("ATACANDO");
    }

    void notDealingDamage()
    {
        GetComponentInChildren<Enemy_Sword>().atacando = false;
        //Debug.Log("NO ATACANDO");
    }
}
