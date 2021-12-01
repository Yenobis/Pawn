using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public NavMeshAgent agent;
    bool alreadyPatrolling = false;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;
    public Transform player;

    public GameObject playerRef;


    public bool canSeePlayer;

    Animator animator;

    public LayerMask obstructionMask, whatIsGround, whatIsPlayer;

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
        UpdateDestination();
    }


    private void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isAttacking = animator.GetBool("isAttacking");
        health = gameObject.GetComponent<HealthScript>().cur_health;

        if (canSeePlayer)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            canSeePlayer = playerInSightRange;
        } else
        {
            FieldOfViewCheck();
            playerInSightRange = canSeePlayer;
        }
        //FieldOfViewCheck();
        //Physics.CheckSphere(transform.position, sightRange, whatIsPlayer)
        //playerInSightRange = canSeePlayer;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (health <= 0)
        {
            agent.SetDestination(transform.position);
            playerInSightRange = false;
            playerInAttackRange = true;
            animator.SetTrigger("Die");
            Destroy(gameObject, 3f);
            
        }
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) { alreadyPatrolling = false; ChasePlayer(); }
        if (playerInSightRange && playerInAttackRange) { alreadyPatrolling = false; AttackPlayer(); }

    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, whatIsPlayer);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
    private void Patroling()
    {
        GetComponent<NavMeshAgent>().speed = speed;
        animator.SetBool("isAttacking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);

        /*
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
        */

        if (Vector3.Distance(transform.position, target) < 1 || !alreadyPatrolling)
        {
            IterateWaypointIndex();
            UpdateDestination();
            alreadyPatrolling = true;
        }
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
        pos_player = new Vector3(player.position.x, transform.position.y, player.position.z);
        agent.SetDestination(player.position);
        //transform.LookAt(pos_player);
    }

    private void AttackPlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        pos_player = new Vector3(player.position.x, transform.position.y, player.position.z);
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
    
    void dealingDamage()
    {
        GetComponentInChildren<Enemy_Sword>().atacando = true;
        //Debug.Log("ATACANDO");
    }

    void notDealingDamage()
    {
        GetComponentInChildren<Enemy_Sword>().atacando = false;
        //Debug.Log("NO ATACANDO");
    }
}
