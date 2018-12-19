using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	[SerializeField] float speed;

	// Use this for initialization
	void Start () {
		//Time.timeScale = 4f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(-Time.deltaTime * speed, 0, 0);
	}

	private void OnTriggerExit(Collider other) {
		// We reposition it on the other side
		transform.position = new Vector3(60f, transform.position.y, transform.position.z);
	}
}
