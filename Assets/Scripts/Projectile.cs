using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	GameObject target;
	Vector3 direction;
	float speed = 10f;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player");
		Vector3 targetPosition = target.transform.position;
		// This way we don't aim at the target's feet...
		targetPosition.y += target.transform.localScale.y;
		direction = Vector3.Normalize(targetPosition - transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		// Space.World makes the translate use the world's coordinates and not the local's ones
		transform.Translate(direction * Time.deltaTime * speed, Space.World);
		transform.Rotate(new Vector3(1, 1, 1) * Time.deltaTime * 250);
	}

	private void OnTriggerEnter(Collider other) {
		// We hit our target
		if(other.gameObject.tag.Equals("Player")){
			target.SendMessage("TakeDamage", 1f);
			Destroy(gameObject);
		}
		// We missed our target
		else if(other.gameObject.tag.Equals("Player")){
			Destroy(gameObject);
		}
	}
}
