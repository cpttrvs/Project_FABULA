using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSpawner : MonoBehaviour
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
            manager.InstantiateEnemy(Enemy.Type.DRAGON, gameObject.transform.position);

            nbSpawned++;
        }
    }


}
