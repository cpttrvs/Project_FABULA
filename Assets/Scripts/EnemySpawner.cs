using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameManager gameManager;
    [SerializeField] bool finalStand;
    [SerializeField] Enemy.Type type;

	[SerializeField] GameObject musicController;

	// We only spawn a dragon the first time the player triggers it
	private bool spawnedDragon = false;
    private bool spawnedFinalFight = false;

    private void OnTriggerEnter(Collider collider) {
        if (collider.tag.Equals("Player")) {
            // It's the final fight, we spawn one of each
            if (finalStand && !spawnedFinalFight) {
                System.Random rand = new System.Random();
                float x = gameObject.transform.position.x;
                float y = gameObject.transform.position.y;
                float z = gameObject.transform.position.z;
                gameManager.InstantiateEnemy(Enemy.Type.FOX, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
                gameManager.InstantiateEnemy(Enemy.Type.WOLF, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
                gameManager.InstantiateEnemy(Enemy.Type.DRAGON, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
                gameManager.InstantiateEnemy(Enemy.Type.DRONE, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
                gameManager.InstantiateEnemy(Enemy.Type.GORILLA, new Vector3(x + (float)rand.NextDouble() * 15f - 7.5f, y, z + (float)rand.NextDouble() * 15f - 7.5f));
                spawnedFinalFight = true;

				musicController.GetComponent<ManageMusic>().fightMusic();
			}
            else if(!finalStand){
                // Fox and wolf spawns can be triggered multiple times, dragon spawns can't
                if (!type.Equals(Enemy.Type.DRAGON) || (type.Equals(Enemy.Type.DRAGON) && !spawnedDragon))
                    gameManager.InstantiateEnemy(type, transform.position);
                if (type.Equals(Enemy.Type.DRAGON) && !spawnedDragon)
                    spawnedDragon = true;
            }
        }
    }
}
