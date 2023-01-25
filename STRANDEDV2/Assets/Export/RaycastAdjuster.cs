using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RaycastAdjuster : MonoBehaviour
{
    public GameObject[] itemstoPickFrom;
    public float raycastDistance = 100f;
    public float overlapTestBoxSize = 1f;
    public LayerMask spawnedObjectLayer;
    void Start()
    {
        PositionToRaycast();

    }

    void PositionToRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc(hit.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRotation, spawnedObjectLayer);

            Debug.Log("number of colliders found" + numberOfCollidersFound);

            if (numberOfCollidersFound == 0)
            {
                Debug.Log("spawned zombies");
                Pick(hit.point, spawnRotation);
            }
            else
            {
                Debug.Log("name of collider 0 found" + collidersInsideOverlapBox[0].name);
            }
        }
    }
    void Pick(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {
        int randomIndex = Random.Range(0, itemstoPickFrom.Length);
        GameObject clone = Instantiate(itemstoPickFrom[randomIndex], positionToSpawn, rotationToSpawn);
    }
}