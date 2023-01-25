using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContoller : MonoBehaviour
{
    [SerializeField] private float stoppingDistance = 3;

    private float timeOfLastAttack = 0;
    private float attackRange = 5;
    private bool hasStopped = false;

    private NavMeshAgent agent = null;
    private EnemyStats stats = null;
    [SerializeField] private Transform player;
    [SerializeField] private Transform target;


    public float movementspeed = 8f;
    public float rotateSpeed = 100f;

    private bool isWandering = false;
    public bool isRotatingLeft = false;
    public bool isRotatingRight = false;
    public bool isWalking = false;

    Rigidbody rb;
    private void Awake()
    {
        
    }
    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {
        if(isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotateSpeed);
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotateSpeed);
        }
        if(isWalking == true)
        {
            rb.AddForce(transform.forward*movementspeed);
        }
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            MoveToTarget();
        }
           
        
    }
    private void MoveToTarget()
    {
        agent.SetDestination(player.position);
        Debug.Log("Moving towards player");
        RotateToTarget();

        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= agent.stoppingDistance)
        {
            if(!hasStopped)
            {
                hasStopped = true;
                timeOfLastAttack = Time.time;   
            }

           
            if (Time.time >= timeOfLastAttack + stats.attackSpeed)
            {
                timeOfLastAttack = Time.time;
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                AttackTarget(targetStats);
            }
            
        }
        else
        {
            if(hasStopped)
            {
                hasStopped= false;  
            }
        }
    }

    private void RotateToTarget()
    {

       Vector3 direction = target.position - transform.position;

       Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

       transform.rotation = rotation;
    }

    private void AttackTarget(CharacterStats statsToDamage)
    {
        stats.DealDamage(statsToDamage);
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection= Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isWandering = true;
        
        yield return new WaitForSeconds(walkWait);

        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking=false;

        yield return new WaitForSeconds(rotateWait);

        if(rotateDirection == 1)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }
        if (rotateDirection == 2)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }

        isWandering = false;
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        stats = GetComponent<EnemyStats>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = player;
        rb = GetComponent<Rigidbody>(); 
    }
}
