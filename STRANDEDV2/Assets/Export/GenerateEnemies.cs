using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject enemy;
    public int xPosition;
    public int zPosition;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCount < 2)
        {
            xPosition = Random.Range(1, 15);
            zPosition = Random.Range(1, 15);
            
            Instantiate(enemy, new Vector3(xPosition, 1, zPosition), Quaternion.identity);
            yield return new WaitForSeconds(1.0f);
            enemyCount += 1;
        }
    }
}
