using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    bool alreadyPatrolling = false;
    public Transform[] waypoints;
    int waypointIndex;
    Vector3 target;
    //public Transform player;

    public GameObject playerRef;

    [HideInInspector]
    public bool canSeePlayer;

    Animator animator;

    public LayerMask obstructionMask, whatIsGround, whatIsPlayer;
    [HideInInspector]
    public float speed;
    public float rotationSpeed;
    public float max_health = 100f;
    public float cur_health = 0f;
    float damage = 5f;

    //Patroling
    [HideInInspector]
    public Vector3 walkPoint;
    [HideInInspector]
    public Vector3 pos_player;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float fovRadius;
    [Range(0, 360)]
    public float angle;
    public float sightRange, attackRange, tooCloseRange;
    private bool playerInSightRange, playerInAttackRange, playerTooClose;
    private bool desaparecido = false;

    private void Awake()
    {
        //player = GameObject.Find("Pawn").transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    void Start()
    {
        cur_health = max_health;
        speed = GetComponent<NavMeshAgent>().speed;
        animator = GetComponent<Animator>();
        UpdateDestination();
    }


    private void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isAttacking = animator.GetBool("isAttacking");

        if (canSeePlayer)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            canSeePlayer = playerInSightRange;
        } else
        {
            FieldOfViewCheck();
            playerInSightRange = canSeePlayer;
        }
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerTooClose = Physics.CheckSphere(transform.position, tooCloseRange, whatIsPlayer);

        if (cur_health <= 0)
        {
            agent.SetDestination(transform.position);
            playerInSightRange = false;
            playerInAttackRange = true;
            animator.SetTrigger("Die");
            if (!desaparecido) Invoke(nameof(Desaparecer), 2f);
            if (desaparecido) { StartCoroutine(Fade()); }
            Destroy(gameObject, 4.2f);
            
        }
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if ((playerInSightRange || playerTooClose) && !playerInAttackRange) { alreadyPatrolling = false; ChasePlayer(); }
        if (playerInSightRange && playerInAttackRange) { alreadyPatrolling = false; AttackPlayer(); }

    }

    private void Desaparecer()
    {
        desaparecido = true;
    }
    IEnumerator Fade()
    {
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.5f * Time.deltaTime)
        {
            Material[] m = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().materials;
            //Debug.Log(alpha);
            for (int i = 0; i < 3; i++)
            {
                Color c1 = m[i].color;
                c1.a = alpha;
                m[i] = m[i + 3];
                m[i].color = c1;

            }
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().materials = m;
            yield return null;
        }
    }


    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, fovRadius, whatIsPlayer);

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
        //CASO PARA PATRULLAR POR UNA ZONA, NO BORRAR
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
            //GameObject.Find("EnemigoSound").GetComponent<EnemigoSoundManager>().Caminar();
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
        Quaternion targetRotation = Quaternion.LookRotation(playerRef.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //pos_player = new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z);
        //transform.LookAt(pos_player);
        agent.SetDestination(playerRef.transform.position);
        
    }

    private void AttackPlayer()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        //Para que el enemigo no se mueva
        agent.SetDestination(transform.position);
        pos_player = new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z);
        transform.LookAt(pos_player);

        if (!alreadyAttacked)
        {
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


    public void TakeDamage(float amount)
    {
        if (cur_health > 0)
        {
            cur_health -= amount;
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, tooCloseRange);
    }

    void dealingDamage()
    {
        GetComponentInChildren<Sword>().atacando = true;
    }

    void notDealingDamage()
    {
        GetComponentInChildren<Sword>().atacando = false;
    }
}
