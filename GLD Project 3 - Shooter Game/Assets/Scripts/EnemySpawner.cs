using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] EnemyList;
    public GameObject SingleSpawn;

    public bool SpawnOnce;

    public float BeginSpawning;
    public float SpawnInterval;


    // Start is called before the first frame update
    void Start()
    {
        if (SpawnOnce)
            Invoke("SpawnEnemy", BeginSpawning);
        else
            InvokeRepeating("SpawnEnemyRandom", BeginSpawning, SpawnInterval);
    }

    private void OnEnable()
    {
        /*
        if (SpawnOnce)
        {
            if (SingleSpawn != null)
                SpawnEnemy(SingleSpawn);
            else
                SpawnEnemyRandom();

            StopAllCoroutines();
            Destroy(gameObject);
        }
        */
    }


    private void SpawnEnemyRandom()
    {
        int i = Random.Range(0, EnemyList.Length);
        GameObject newEnemy = Instantiate(EnemyList[i]) as GameObject;
        newEnemy.gameObject.transform.position = this.transform.position;
    }

    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(SingleSpawn) as GameObject;
        newEnemy.gameObject.transform.position = this.transform.position;

        StopAllCoroutines();
        Destroy(gameObject);
    }



}
