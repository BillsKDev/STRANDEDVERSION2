using UnityEngine;
using UnityEngine.AI;

public class ZombieNavigation : MonoBehaviour
{
    private Transform playerTransform;
    private Transform target;
    private NavMeshPath path;

    private void Awake()
    {
        GetComponent<Health>().OnDied += HandleDied;
    }

    private void HandleDied()
    {
        this.enabled = false;
    }

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
        path = new NavMeshPath();
    }

    private void Update()
    {
        Vector3 targetPos = playerTransform.position;

        bool foundPath = NavMesh.CalculatePath(transform.position, targetPos, NavMesh.AllAreas, path);
        if (foundPath)
        {
            Vector3 nextDestination = path.corners[1];

            Vector3 directionToTarget = nextDestination - transform.position;
            Vector3 flatDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z);
            directionToTarget = flatDirection.normalized;

            var desiredRotation = Quaternion.LookRotation(directionToTarget);
            var finalRotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime);

            transform.rotation = finalRotation;
        }
    }
}