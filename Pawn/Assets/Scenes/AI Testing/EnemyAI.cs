using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour
{

    [Header("Enemy References")]
    public NavMeshAgent agent;
    public GameObject playerRef;
    public Transform[] waypoints;
    public LayerMask obstructionMask, whatIsGround, whatIsPlayer;


    int waypointIndex;
    bool alreadyPatrolling = false;
    Vector3 target;
    [HideInInspector]
    public bool canSeePlayer;
    Animator animator;
    [HideInInspector]
    public float speed;

    [Header("Enemy Parameters")]
    [Space(15)]
    public float chaseSpeedIncrease = 1;
    public float rotationSpeed = 10;
    public float max_health;
    public float cur_health = 0f;
    //float damage = 5f;

    //Patroling
    [HideInInspector]
    public Vector3 walkPoint; //Se usa para cuando quieres que un enemigo vaya por unos puntos aleatorios (de
                              //momento el modo SearchWalkPoint no se usa, pero podria en un futuro)
    [HideInInspector]
    public Vector3 pos_player; //Se usa para la hora de atacar coger la posicion de playerRef y ponerle 0 en y.
    //bool walkPointSet;
    [HideInInspector]
    public float walkPointRange;
    public float timeBetweenPoints;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    bool estoyAtacando = false;
    public GameObject projectile;
    
    //States
    [Header("Enemy Sight")]
    [Space(15)]
    public float fovRadius;
    [Range(0, 360)]
    public float angle;
    public float sightRange, attackRange, tooCloseRange;
    private bool playerInSightRange, playerInAttackRange, playerTooClose;
    [HideInInspector]
    public bool isWalking, isRunning, isAttacking, isHit;
    private bool desaparecido = false;
    private bool destruido = false;
    private Quaternion rotacion_inicial;

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
        rotacion_inicial = gameObject.transform.localRotation;
        waypointIndex = 0;
        IterateWaypointIndex();
        UpdateDestination();
    }


    private void Update()
    {
        isWalking = animator.GetBool("isWalking");
        isRunning = animator.GetBool("isRunning");
        isAttacking = animator.GetBool("isAttacking");
        
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
            animator.SetBool("isAttacking", false);
            
            if (!desaparecido) Invoke(nameof(Desaparecer), 2f);
            if (desaparecido) { StartCoroutine(Fade()); }
            Invoke(nameof(DestruirEnemigo), 4.2f);
            //Destroy(gameObject, 4.2f);
            
        }
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if ((playerInSightRange || playerTooClose) && !playerInAttackRange) { alreadyPatrolling = false; ChasePlayer(); }
        if (playerInSightRange && playerInAttackRange) { alreadyPatrolling = false; AttackPlayer(); }

    }

    private void DestruirEnemigo()
    {
        if (!destruido) { GameObject.Find("Text").GetComponent<EnemyCounter>().UpdateEnemies(); }
        destruido = true;
        Destroy(gameObject);
        
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
        NavMeshPath camino = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, camino);
        agent.SetPath(camino);
        if (timeBetweenPoints > 0.5) animator.SetBool("isWalking", true);
        //agent.SetDestination(target);
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
        animator.SetBool("isHit", false);

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
        //Debug.Log(Vector3.Distance(transform.position, target));
        if (Vector3.Distance(transform.position, target) < 1.5 || !alreadyPatrolling)
        {
            IterateWaypointIndex();
            //UpdateDestination();
            alreadyPatrolling = true;
            if (waypoints.Length <= 1)
            {
                //Invoke(nameof(ResetWalking), 4);
                animator.SetBool("isWalking", false);
                //Quaternion targetRotation = Quaternion.LookRotation(playerRef.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacion_inicial, 2 * Time.deltaTime);
            }
            else
            {
                if(timeBetweenPoints > 0.5) animator.SetBool("isWalking", false);
                Invoke(nameof(UpdateDestination), timeBetweenPoints);
            }
        }
        
    }

    //No borrar
    /*
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
        
    } */

    private void ChasePlayer()
    {
        GetComponent<NavMeshAgent>().speed = speed + chaseSpeedIncrease;
        
        animator.SetBool("isAttacking", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", true);
        float distanceToTarget = Vector3.Distance(transform.position, playerRef.transform.position);
        if (distanceToTarget <= tooCloseRange)
        {
            Quaternion targetRotation = Quaternion.LookRotation(playerRef.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        //pos_player = new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z);
        //transform.LookAt(pos_player);
        //NavMeshPath camino = new NavMeshPath();
        //NavMesh.CalculatePath(transform.position, playerRef.transform.position, NavMesh.AllAreas, camino);
        //agent.SetPath(camino);
        agent.SetDestination(playerRef.transform.position);
        //Debug.Log(agent.Warp(transform.position));
        //agent.Move(playerRef.transform.position - transform.position);
        
        
    }

    private void AttackPlayer()
    {
        
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        //Para que el enemigo no se mueva
        agent.SetDestination(transform.position);
        pos_player = new Vector3(playerRef.transform.position.x, transform.position.y, playerRef.transform.position.z);
        transform.LookAt(pos_player);
        //GameObject.Find("EnemigoSound").GetComponent<EnemigoSoundManager>().Caminar();

        if (!alreadyAttacked && !estoyAtacando)
        {
            animator.SetBool("isAttacking", true);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        
    }

    private void ResetAttack()
    {
        animator.SetBool("isHit", false);
        animator.SetBool("isAttacking", false);
        alreadyAttacked = false;
        //animator.SetBool("isHit", false);
    }

    private void ResetWalking()
    {
        animator.SetBool("isHit", false);
        animator.SetBool("isWalking", false);
    }


    public void TakeDamage(float amount)
    {
        if (cur_health > 0)
        {
            animator.SetBool("isHit", true);
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
    void inicioAtaque()
    {
        estoyAtacando = true;
    }

    void finAtaque()
    {
        estoyAtacando = false;
        //animator.SetBool("isAttacking", false);
    }
}
