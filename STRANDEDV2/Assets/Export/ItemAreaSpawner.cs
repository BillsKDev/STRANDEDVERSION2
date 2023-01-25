using System.Collections;
using UnityEngine;

public class ItemAreaSpawner : MonoBehaviour
{
    public GameObject itemToSpread;
    public GameObject boom;
    public int enemies;

    public float itemXSpread = 150;
    public float itemYSpread = 0;
    public float itemZSpread = 150;

    public bool night;
    public TimeProgressor isSpawning;
    public EnemyStats stats;


    private void Update()
    {

        if (isSpawning.nightTime)
        {
            StartCoroutine(SpreadItem());
        }
        if (isSpawning.dayTime)
        {
            if (gameObject.name == "Capsule(Clone)")
                Destroy(gameObject, 5);
        }

    }

    IEnumerator SpreadItem()
    {
        while (enemies < 100)
        {
            Vector3 randPosition = new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread)) + transform.position;
            GameObject clone = Instantiate(itemToSpread, randPosition, itemToSpread.transform.rotation);

            enemies++;

            yield return clone;
        }
    }
}