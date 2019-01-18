using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaScene : MonoBehaviour
{
    private float timer = 0;
    private float fixedTimer = 2.5f;
    [SerializeField] float spawnRate = 1f;

    private int nbSpawnsPerLevel = 5;
    private int currentLevel = 0;
    [SerializeField] float stepPerLevel = 0.1f;

    [SerializeField] List<GameObject> spawners = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        timer = spawnRate * fixedTimer - 2; // start spawning 2 seconds after launching ArenaScene
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= spawnRate * fixedTimer)
        {
            // random enemy, in a random spawn
            int r = Random.Range(0, 5);
            Enemy.Type t = Enemy.Type.WOLF;
            if (r == 0)
                t = Enemy.Type.DRAGON;
            if (r == 1)
                t = Enemy.Type.DRONE;
            if (r == 2)
                t = Enemy.Type.FOX;
            if (r == 3)
                t = Enemy.Type.GORILLA;
            if (r == 4)
                t = Enemy.Type.SPIDER;
            if (r == 5)
                t = Enemy.Type.WOLF;

            GameManager.instance.InstantiateEnemy(t, spawners[Random.Range(0, spawners.Count)].transform.position);
            timer = 0;


            // reduce the spawnRate to increase difficulty
            currentLevel++;
            if(currentLevel % nbSpawnsPerLevel == 0)
            {
                spawnRate -= stepPerLevel;

                if (spawnRate < 0.1)
                    spawnRate = 0.1f;

                currentLevel = 0;
                Debug.Log("Arena: increasing difficulty (" + stepPerLevel.ToString("0.00") + ")");
            }
        }
        timer += Time.deltaTime;
    }
}
