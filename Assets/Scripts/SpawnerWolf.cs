using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerWolf : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            GameManager manager = GameObject.Find("_GameManager").GetComponent<GameManager>();
            manager.InstantiateEnemy(Enemy.Type.WOLF, gameObject.transform.position);      
        }
    }


}
