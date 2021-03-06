﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour {
    
	[SerializeField] int maxLife = 3;
	[SerializeField] GameObject weapon;
	[SerializeField] GameObject bladePrefab;
	int currentLife;
	float speed = 6f;
	Animator animator;
	bool dead;

    GameObject blade;

	// Use this for initialization
	void Start () {
		currentLife = maxLife;
		dead = false;
		animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		// We are dead, we fall to the ground D:
		if(dead){
			Quaternion rotation = new Quaternion(0F, 0F, 0F, 0F);
            // We only rotate around the Y axis
            rotation.eulerAngles = new Vector3(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            // This way the rotation is smooth by always having the same speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * 200f);
		}
	}

	public void Move(Vector3 direction) {
		transform.LookAt(transform.position + Vector3.Normalize(direction));
		transform.Translate(0, 0, Time.deltaTime * speed);
        // Camera follows player
		if(Time.timeScale > 0)
            GameObject.Find("Main Camera").transform.position = new Vector3(gameObject.transform.position.x, 30, gameObject.transform.position.z);
    }

	// getters and setters
	public float GetSpeed() { return speed; }

	public int GetCurrentLife() { return currentLife; }

	public void TakeDamage(int value) { 
		if(currentLife-value <= 0) {
			currentLife = 0; 
			if(!dead){
				animator.SetTrigger("dead");
				dead = true;
				SendMessage("PlayerDied");
				GameManager.instance.SendMessage("PlayerDied");
			}
		} 
		else {
			currentLife -= value;
        }
        Camera.main.SendMessage("CharacterHurt", currentLife);
    }

	// Called when an enemy got hit
	public void Hit(Transform trans){
		Vector3 direction = Vector3.Normalize(trans.position - transform.position);
		// We freeze y since we never want to rotate upwards
		direction.y = 0;
		transform.rotation = Quaternion.LookRotation(direction);
        GameObject blade = GameObject.Instantiate(bladePrefab, transform.position + new Vector3(-0.6f, 1.3f, 0.22f), Quaternion.identity);
        blade.GetComponent<Blade>().enemy = trans.gameObject;
		// Play the knife throw sound
		Camera.main.SendMessage("ThrowKnife");
		// Actually throw the knife
        animator.SetTrigger("attack");
		// We supposedly threw the knife so it shouldn't be in our hands :D
		weapon.SetActive(false);
    }

	// After the enemy got hit by the knife we get it back *MAGIC*
	public void GetKnifeBack(){
		weapon.SetActive(true);
	}
}
