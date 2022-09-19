using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRanged : MonoBehaviour
{
    Transform target;
    [SerializeField] float chaseRange = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float shootRange = 10f;
    [SerializeField] int damage = 10;

    [SerializeField] ParticleSystem muzzleFlash;

    [SerializeField] Animator animator;

    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;
    bool isProvoked = false;
    EnemyHealth enemyHealth;
    PlayerHealth playerHealth;

    float secondsSinceLastShot = 0;
    [SerializeField] float secondsBetweenShots = 2f;
    [SerializeField] float secondsToWaitAfterShootingBeforeWalking = 5f;

    AudioPlayer audioPlayer;


    void Start()
    {
        target = FindObjectOfType<PlayerHealth>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        playerHealth = target.GetComponent<PlayerHealth>();
        audioPlayer = GetComponent<AudioPlayer>();
    }

    void Update()
    {
        if (enemyHealth.IsDead())
        {
            enabled = false;
            navMeshAgent.enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }
        else
        {
            Disengage();
        }
    }

    private void Disengage()
    {
        animator.SetBool("attack", false);
        animator.SetBool("move", false);
    }

    private void EngageTarget()
    {
        FaceTarget();

        secondsSinceLastShot += Time.deltaTime;
        if (distanceToTarget <= shootRange)
        {
            AttackTarget();
        }
        else if (secondsSinceLastShot >= secondsToWaitAfterShootingBeforeWalking)
        {
            secondsSinceLastShot = 0;
            ChaseTarget();
        }
    }

    private bool CanHitTarget()
    {
        RaycastHit hit;

        if (Vector3.Distance(transform.position, target.position) < shootRange)
        {
            if (Physics.Raycast(transform.position, (target.position - transform.position), out hit, shootRange))
            {
                if (hit.transform.tag == "Player")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ChaseTarget()
    {
        animator.SetBool("move", true);
        animator.SetBool("attack", false);
        navMeshAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        navMeshAgent.SetDestination(transform.position);
        animator.SetBool("attack", true);

        if (secondsSinceLastShot >= secondsBetweenShots && CanHitTarget())
        {
            audioPlayer.PlayByName("Shoot");
            secondsSinceLastShot = 0f;
            playerHealth.DecreaseHealth(damage);
            muzzleFlash.Play();
        }
        else if (secondsSinceLastShot >= secondsToWaitAfterShootingBeforeWalking)
        {
            ChaseTarget();
        }
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
