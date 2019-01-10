using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour {

	public enum Type { SPIDER, WOLF, FOX, DRAGON, DRONE, GORILLA };
	public enum Mode { RANGE, MELEE };

    public static bool correctInput;

	// Only used by enemies attacking from afar
	[SerializeField] GameObject projectilePrefab;

	// Words
	[SerializeField] Stack<Utility.Word> words = new Stack<Utility.Word>();
	Utility.Word currentWord;

	// Behaviour
	GameObject target;
	Type type;
	Mode attackMode;
	// Probably have to change that!
	Transform realTransform;
	float moveSpeed = 1f;
	int maxLife = 0;
	int currentLife = 0;
	int damage = 1;
	bool visibility = false;
	float reloadTimer = 0f;
	float reloadTime = 2f;
	float attackRange = 0f;
	float attackSpeed = 2.5f;
	// Only used by enemies attacking in melee
	bool hit = false;
	bool gameover = false;
	float gameoverTimer = 0f;

	// UI
	Text currentWordUI;

	// Position in 2D (y being leveled)
	Vector3 targetPositionIn2D;

	void Awake() {
		GameManager.instance.SendMessage("AddEnemy", gameObject);
	}
	void Start () {
		maxLife = words.Count;
		currentLife = maxLife;
		target = GameObject.FindGameObjectWithTag("Player");
		ComputePositionsIn2D();
		// Rotate towards the target
		realTransform.LookAt(targetPositionIn2D);
		// When the enemy first comes into attack range it can attack right away
		reloadTimer = reloadTime;

		currentWordUI = gameObject.GetComponentInChildren<Text>();
		if(currentWordUI == null) {
			Debug.Log("Enemy (" + gameObject.name + "): no current word text");
		} else {
			UpdateCurrentWord();
			UpdateUI();
		}
	}

	bool attacking = false;
	
	void Update () {
		if(target != null){
			if(!gameover){
				ComputePositionsIn2D();
				// We constantly look at the target
				realTransform.LookAt(targetPositionIn2D);
				reloadTimer += Time.deltaTime;
			}
			if(!IsInAttackRange()){
				hit = false;
				Move();
			}
			// FOX, WOLF
			else if(IsInAttackRange() && attackMode == Mode.MELEE && !type.Equals(Type.GORILLA)){
				// Before hit
				if(IsInAttackRange() && reloadTimer > reloadTime && !hit && !gameover){
					realTransform.Translate(0, 0, Time.deltaTime * attackSpeed);
				}
				// After hit
				else if(IsInAttackRange() && hit){
					realTransform.Translate(0, 0, -Time.deltaTime * attackSpeed);
				}
			}
			// GORILLA
			else if(IsInAttackRange() && attackMode == Mode.MELEE && type.Equals(Type.GORILLA) && reloadTimer > reloadTime){
				// We stop moving
				GetComponent<Animator>().SetFloat("VSpeed", 0);
				// If the gorilla isn't yet attacking we do so
				if(!attacking && !gameover){
					GetComponent<Animator>().SetInteger("CurrentAction", 4);
					// The target takes damage
					target.SendMessage("TakeDamage", damage);
					attacking = true;
				}
				// Else we reset the animator's variable for the next attack
				else{
					GetComponent<Animator>().SetInteger("CurrentAction", 0);
					attacking = false;
					reloadTimer = 0;
				}
			}
			// DRAGON, DRONE
			else if(IsInAttackRange() && attackMode == Mode.RANGE){
				if(reloadTimer > reloadTime && !gameover){
					Vector3 position = realTransform.position;
					position = position + realTransform.forward * 1.4f;
					GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
					// Impossible to hide it in the dragon's hierarchy because the dragon's movement will affect the projectile
					//projectile.transform.parent = transform;
					projectile.hideFlags = HideFlags.HideInHierarchy;
					reloadTimer = 0f;
				}
			}
			if(gameover){
				gameoverTimer += Time.deltaTime;
				if(gameoverTimer > 1f){
					Move();
				}
			}
		}
		else{
			Debug.Log(name+" has no target!");
		}
		UpdateUI();
	}

	private void Move(){
		if(type.Equals(Type.GORILLA)){
			// Advance
			GetComponent<Animator>().SetFloat("VSpeed", 1);
			// We can use this to turn around the player, if needed
			//GetComponent<Animator>().SetFloat("HSpeed", 1);
		}
		else{
			realTransform.Translate(0, 0, Time.deltaTime * moveSpeed);
		}
	}

	/*
		Sets up the vocabulary and all the other parameters depending on the enemy's type
	*/
	public void Initiate(Enemy.Type type){
		this.type = type;	
		switch(type){
			case Enemy.Type.FOX:
				moveSpeed = 2.5f;
				attackMode = Mode.MELEE;
				attackRange = 2f;
				reloadTime = 2f;
				realTransform = transform;
				break;
			case Enemy.Type.WOLF:
				moveSpeed = 2.5f;
				attackMode = Mode.MELEE;
				attackRange = 3f;
				reloadTime = 2f;
				realTransform = transform;
				break;
			case Enemy.Type.DRAGON:
				moveSpeed = 3f;
				attackMode = Mode.RANGE;
				attackRange = 10f;
				reloadTime = 2.5f;
				realTransform = transform.parent;
				break;
			case Enemy.Type.DRONE:
				moveSpeed = 3f;
				attackMode = Mode.RANGE;
				attackRange = 10f;
				reloadTime = 2.5f;
				realTransform = transform;
				break;
			case Enemy.Type.GORILLA:
				moveSpeed = 1.5f;
				attackMode = Mode.MELEE;
				attackRange = 1.5f;
				reloadTime = 3f;
				realTransform = transform;
				break;
			default:
				moveSpeed = 2f;
				attackMode = Mode.MELEE;
				realTransform = transform;
				attackRange = 1f;
				break;
		}
		List<Utility.Word> vocabulary = new List<Utility.Word>(Utility.GetVocabulary(type, Glossary.language));
		// Move this elsewhere, specific for each enemy...
		int amount = 3;
		for(int i=0; i<amount; i++){
			// Select a random index
			int index = Random.Range(0, vocabulary.Count);
			// Add the word at that index to the enemy's vocabulary
			AddWord(vocabulary.ElementAt(index));
			// Remove the word from the list so it won't be used again
			vocabulary.RemoveAt(index);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if(!gameover && other.gameObject.tag.Equals("Player")){
			// We hit the target
			hit = true;
			// The target takes damage
			target.SendMessage("TakeDamage", damage);
			// We "reload"
			reloadTimer = 0f;
		}
	}

	private void GameOver(){
		gameover = true;
	}

	private void ComputePositionsIn2D(){
		// We level y so that we never look down or up but always straight!
		targetPositionIn2D = new Vector3(target.transform.position.x, realTransform.position.y, target.transform.position.z);
	}
	
	private bool IsInAttackRange(){
		return Vector3.Distance(targetPositionIn2D, realTransform.position) < attackRange;
	}

	private void UpdateUI(){
		currentWordUI.transform.position = Camera.main.WorldToScreenPoint(new Vector3(realTransform.position.x, 0, realTransform.position.z+realTransform.localScale.z/2+0.5f));
	}
	
	public void Hit() { 
        // Death
		if(currentLife - 1 <= 0) {
			currentLife = 0;
            DeathSound();
            // If the script isn't attached to the highest element of the gameobject
            if (gameObject.transform.parent != null)
                Destroy(gameObject.transform.parent.gameObject);
            else
			    Destroy(gameObject); 
		}
        else {
			currentLife -= 1;
            HurtSound();
		}
    }

    private void HurtSound() {
        switch (type) {
            case Enemy.Type.FOX:
                Camera.main.SendMessage("FoxCry");
                break;
            case Enemy.Type.WOLF:
                Camera.main.SendMessage("WolfCry");
                break;
            case Enemy.Type.DRAGON:
                Camera.main.SendMessage("DragonHit");
                break;
            case Enemy.Type.GORILLA:
                Camera.main.SendMessage("GorillaBark");
                break;
            case Enemy.Type.DRONE:
                Camera.main.SendMessage("DroneHit");
                break;
        }
    }

    private void DeathSound() {
        switch (type) {
            case Enemy.Type.FOX:
                Camera.main.SendMessage("FoxHowl");
            break;
            case Enemy.Type.WOLF:
                Camera.main.SendMessage("WolfHowl");
            break;
            case Enemy.Type.DRAGON:
                Camera.main.SendMessage("DragonDeath");
                break;
            case Enemy.Type.GORILLA:
                Camera.main.SendMessage("GorillaGroan");
                break;
            case Enemy.Type.DRONE:
                Camera.main.SendMessage("DroneDestroyed");
                break;
        }
    }

	private void OnBecameVisible() {
		visibility = true;
		GameManager.instance.SendMessage("UpdateVisibility", gameObject);
	}
	private void OnBecameInvisible() {
		visibility = false;
		if(gameObject != null){
			GameManager.instance.SendMessage("UpdateVisibility", gameObject);
			// For now they become invisible when the game is over so we destroy them
			if(gameover)
				Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		GameManager.instance.SendMessage("RemoveEnemy", gameObject);
	}

	// Target
	public void SetTarget(GameObject t) {
		target = t;
	}
	public GameObject GetTarget() { return target; }

	// Words
	public Utility.Word VerifyWord(string input) {
		input = input.ToUpper();
        currentWordUI.text = Utility.GetWordColoring(currentWord.name.ToUpper(), input);

        // If the user's input matches exactly the enemy's word
        if (currentWord.name.ToUpper() == input) {
			target.SendMessage("Hit", realTransform);
			Utility.Word tmp = currentWord;
            // Update the word straight away
            if(getCurrentLife() > 1) {        
                words.Pop(); 
                UpdateCurrentWord();
            }
            return tmp;
		}

        // The player's input matches at least the beginning of this word
        if (currentWord.name.ToUpper().StartsWith(input)) {
            // Thus it isn't accounted as an error or typo
            Enemy.correctInput = true;
        }
        
        return null;
	}
	
	public void UpdateCurrentWord() {
		if(words != null) {
			currentWord = words.Peek();
			currentWordUI.text = currentWord.name.ToUpper();
		}
	}

	public void AddWord(Utility.Word word) {
		word.language = Glossary.language;
		word.translation = Utility.GetWordTranslation(type, word);
		if(!words.Contains(word)) {
			words.Push(word);
			maxLife += 1;
			currentLife += 1;
		}
	}

	public Type GetCreatureType(){
		return type;
	}

	//Getters
	public int getCurrentLife() { return currentLife; }
	public int getDamage() { return damage; }
	public bool isVisible() { return visibility; }
}
