using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{

    enum State
    {
        Patrolling,
        Chasing,
        Searching,
        Waiting,
        Attacking
    }

    State currentState;

    UnityEngine.AI.NavMeshAgent enemyAgent;

    public Transform playerTransform;

    [SerializeField] Transform  patrolAreaCenter;
    
    [SerializeField] Vector2 patrolAreaSize;

    public Transform[] points;

    [SerializeField]private int destPoint = 0;

    [SerializeField] float visionRange = 15;
    [SerializeField] float visionAngle = 50;

    Vector3 lastTargetPosition;

    [SerializeField]float searchTimer;

    [SerializeField]float searchWaitTime = 15;

    [SerializeField]float searchRadius = 30;

    private bool repeat = true;

    [SerializeField] float attackRange = 5;

    Animator _animator;

    //EnemyCombo _combo;

    // Start is called before the first frame update
    void Awake()
    {
        enemyAgent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        //playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        //_combo = GetComponent<EnemyCombo>();
    }

    void Start()
    {
        enemyAgent.autoBraking = false;
        currentState = State.Patrolling;
        enemyAgent.destination = points[destPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            
            case State.Chasing:
                Chase();
            break;

            case State.Searching:
                Search();
            break;
            
            case State.Waiting:
                Waiting();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    void Patrol()
    {
        _animator.SetBool("TenguStop", false);
        _animator.SetBool("TenguPatrolling", true);
        if(enemyAgent.remainingDistance < 0.5f)
        {
            //SetRandomPoint();
            currentState = State.Waiting;
        }

        if(OnRange() == true)
        {
            currentState = State.Chasing;
        }
    }

    void Chase()
    {
        _animator.SetBool("TenguStop", false);
        _animator.SetBool("TenguPatrolling", true);
        enemyAgent.destination = playerTransform.position;

        if(OnRange() == false)
        {
            currentState = State.Searching;
        }
        
        if(OnRangeAttack() == true)
        {
            currentState = State.Attacking;
        }
    }

    void Search()
    {
        _animator.SetBool("TenguStop", false);
        _animator.SetBool("TenguPatrolling", true);
        if(OnRange() == true)
        {
            searchTimer = 0;
            currentState = State.Chasing;
        }
        searchTimer += Time.deltaTime;

        if(searchTimer < searchWaitTime)
        {
            if(enemyAgent.remainingDistance < 0.5f)
            {
                Debug.Log("Buscando punto aleatorio");
                Vector3 randomSearchPoint = lastTargetPosition + Random.insideUnitSphere * searchRadius;

                randomSearchPoint.y = lastTargetPosition.y;

                enemyAgent.destination = randomSearchPoint;
            }
        }
        else
        {
            currentState = State.Patrolling;
        }
    }

    void Waiting()
    {
        if(repeat == true)
        {
            StartCoroutine("Esperar");
        }
    }

    IEnumerator Esperar()
    {
        _animator.SetBool("TenguPatrolling", false);
        _animator.SetBool("TenguStop", true);
        repeat = false;
        yield return new WaitForSeconds (5);
        GotoNextPoint();
        currentState = State.Patrolling;  
        repeat = true;
    }
    void Attacking()
    {
        //_combo.StartCombo();
        _animator.SetBool("AttackTengu", true);
        currentState = State.Chasing;
    }

    bool OnRangeAttack()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if(distanceToPlayer <= visionRange && angleToPlayer < visionAngle * 0.2f)
        {
            //return true;

            if(playerTransform.position == lastTargetPosition)
            {
                /*lastTargetPosition = playerTransform.position;
                currentState = State.Attacking;*/
                return true;
            }

            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionToPlayer, out hit, attackRange))
            {
                Debug.DrawRay(transform.position, directionToPlayer * attackRange, Color.green);

                if(hit.collider.CompareTag("Player"))
                {
                    lastTargetPosition = playerTransform.position;
                    return true;
                } 
            }

            return false;
        }

        return false;

    }

    void GotoNextPoint()
    {
        if(points.Length == 0)
        {
            return;
        }

        destPoint = (destPoint + 1) % points.Length;
        enemyAgent.destination = points[destPoint].position;
        
    }

    bool OnRange()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) <= visionRange)
        {
            return true;
        }

        return false;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if(distanceToPlayer <= visionRange && angleToPlayer < visionAngle * 0.5f)
        {
            return true;

            if(playerTransform.position == lastTargetPosition)
            {
                lastTargetPosition = playerTransform.position;
                return true;
            }

            RaycastHit hit;
            if(Physics.Raycast(transform.position, directionToPlayer, out hit, distanceToPlayer))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    return true;
                } 
            }

            return false;
        }

        return false;
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(patrolAreaCenter.position, new Vector3(patrolAreaSize.x, 0, patrolAreaSize.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.green;

        Vector3 fovLine1 = Quaternion.AngleAxis(visionAngle * 0.5f, transform.up) * transform.forward * visionRange;

        Vector3 fovLine2 = Quaternion.AngleAxis(-visionAngle * 0.5f, transform.up) * transform.forward * visionRange;

        Gizmos.DrawLine(transform.position, transform.position + fovLine1);

        Gizmos.DrawLine(transform.position, transform.position + fovLine2);
    }
}
