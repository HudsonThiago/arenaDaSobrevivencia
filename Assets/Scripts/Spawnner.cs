using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnner : MonoBehaviour
{
    [SerializeField] private GameObject monster;
    [SerializeField, Range(1, 10)] private float radius = 1;
    [SerializeField, Range(10, 60)] private int timeForSpawn = 20;
    [SerializeField, Range(10, 60*10)] private float initTime = 20;
    [SerializeField, Range(60, 60*10)] private float maxTime = 60*2;
    [SerializeField, Range(1, 5)] private int maxCount;
    private int cicles = 1;
    private bool controll = true;
    private Transform monsterSpawnParent;


    // Start is called before the first frame update
    void Start()
    {
        controll = true;
        monsterSpawnParent = GameObject.FindGameObjectWithTag("PlayerShoot").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Timer.getTimer() >= initTime && Timer.getTimer() <= maxTime)
        {
            if (Timer.getTimer() % timeForSpawn == 0)
            {
                spawn();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    private void spawn()
    {
        if (controll)
        {
            controll = !controll;
            for (int i = 0; i < cicles; i++)
            {
                Vector2 position = (Vector2)transform.position+(Random.insideUnitCircle * radius);
                Instantiate(monster, position, Quaternion.identity, monsterSpawnParent);
            }
            cicles += 1;
            if (cicles > maxCount)
            {
                cicles = maxCount;
            }
            StartCoroutine(spawnFinish());
        }
    }

    IEnumerator spawnFinish()
    {
        yield return new WaitForSeconds(1.2f);
        controll = true;
    }
}
