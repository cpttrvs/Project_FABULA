using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

    public GameObject enemy;

    private float speed = 50f;

    private Renderer enemyRenderer;

	// Use this for initialization
	void Start () {
        if (enemy.GetComponentInChildren<SkinnedMeshRenderer>() != null)
            enemyRenderer = enemy.GetComponentInChildren<SkinnedMeshRenderer>();
        else if (enemy.GetComponentInChildren<MeshRenderer>() != null)
            enemyRenderer = enemy.GetComponentInChildren<MeshRenderer>();
        else
            Debug.Log("Fuck: " + enemy.name);
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(enemyRenderer.bounds.center);
        transform.Rotate(-90, 180, 0);
        transform.Translate(0, Time.deltaTime * speed, 0);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.Equals(enemy)) {
            enemy.GetComponentInChildren<Enemy>().SendMessage("Hit");
            Destroy(gameObject);
        }
    }
}
