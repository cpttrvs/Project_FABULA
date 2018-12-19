using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossSpawner : MonoBehaviour
{
    public int nbSpawned = 0;
    public int nbToSpawn = 1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && nbSpawned < nbToSpawn)
        {
            GameManager manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
            System.Random rand = new System.Random();
            float x = gameObject.transform.position.x;
            float y = gameObject.transform.position.y;
            float z = gameObject.transform.position.z;
            manager.InstantiateEnemy(Enemy.Type.FOX, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
            //manager.InstantiateEnemy(Enemy.Type.WOLF, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
            manager.InstantiateEnemy(Enemy.Type.DRAGON, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
            manager.InstantiateEnemy(Enemy.Type.DRONE, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
            manager.InstantiateEnemy(Enemy.Type.GORILLA, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));

            nbSpawned++;
        }
    }

}
