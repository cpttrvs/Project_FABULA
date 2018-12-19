using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class Controller : MonoBehaviour {

	[SerializeField] GameObject gameManager;
	private GameManager gameManagerScript;

	private Player player;
	private string currentText = "";
	private bool textMode = false;
	private bool dead = false;
	
	// UI
	private Text keyboardLockUI;

	// SCORE...
	private List<Utility.Word> correctWords;
    private Utility.Word currentWord = null;

	void Start () {
		gameManagerScript = gameManager.GetComponent<GameManager>();
		player = GetComponent<Player>();
		keyboardLockUI = GameObject.Find("keyboardLock").GetComponent<Text>();
		correctWords = new List<Utility.Word>();
	}
	
	void Update () {
		if(!dead){		
			if(Input.GetButtonDown("Switch")) {
				textMode = !textMode;
                gameManagerScript.WritingImage(textMode);
                // resets every words on screen
                foreach(GameObject go in gameManagerScript.GetVisibleEnemies())
                {
                    go.GetComponent<Enemy>().UpdateCurrentWord();
                }

				if(textMode)
					keyboardLockUI.text = Glossary.STRING_KEYBOARDLOCK+"On";
				else
					keyboardLockUI.text = Glossary.STRING_KEYBOARDLOCK+"Off";
				currentText = "";
			}
			if(textMode) {
				//typing
				foreach(char c in Input.inputString) {
					if ((c == '\n') || (c == '\r')) {
						// enter or return : exit textMode and clear
						//textMode = !textMode;
						//currentText = "";
					} 
					else if(c == '\b' || c != ' ') { 
						// backspace or delete
						if(c == '\b'){
							if(currentText.Length != 0) {
								currentText = currentText.Substring(0, currentText.Length - 1);
							}
						}
						else{
							// add char to string
							currentText += char.ToUpper(c);

                            //check only when a character is added
                            CheckWords();
                        }
					} 	
				}
			}
			else if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
				//movement
				Vector3 move = Vector2.zero;

				move.x = Input.GetAxis("Horizontal");
				move.z = Input.GetAxis("Vertical");

				player.Move(move);
			}
		}
	}

	// Checks if the current word corresponds to one of the enemies' words
	private void CheckWords(){
		/*
			TODO: 	Change this 'cause for now it takes into account that multiple enemies
					can have the same word, which shouldn't be the case in the future...
		 */
		Utility.Word word = null;
		// Just for initialisation
		Enemy.Type type = Enemy.Type.SPIDER;

        //change it back to GetVisibleEnemies when fixed
        bool atleastOneHit = false;
		foreach(GameObject e in gameManagerScript.GetEnemies()) {
			if((word = e.GetComponentInChildren<Enemy>().VerifyWord(GetCurrentText())) != null){
				type = e.GetComponentInChildren<Enemy>().GetCreatureType();
			}
            if (word != null)
            {
                Debug.Log("Word: " + word.name);
                correctWords.Add(word);
                gameManagerScript.nbCorrectWords++; // stats
                atleastOneHit = true;
            }
        }
		
        if(atleastOneHit)
            SetCurrentText("");
    }

	public void PlayerDied(){
		dead = true;
	}

	public string GetCurrentText() { return currentText; }
	public void SetCurrentText(string s) { currentText = s; }

	public List<Utility.Word> GetCorrectWords(){
		return correctWords;
	}
}
