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
    // True if the last input wasn't a typo, false otherwise
    private bool lastInput = true;
	
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
                        }
                        if(gameManagerScript.GetEnemies().Count > 0)
						    CheckWords();
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
		Utility.Word word = null;
		// Just for initialisation
		Enemy.Type type = Enemy.Type.SPIDER;
        
        Enemy.correctInput = false;
        bool fullMatch = false;
		foreach(GameObject e in gameManagerScript.GetEnemies()) {
			if((word = e.GetComponentInChildren<Enemy>().VerifyWord(GetCurrentText())) != null){
				type = e.GetComponentInChildren<Enemy>().GetCreatureType();
			}
            if (word != null)
            {
                correctWords.Add(word);
                fullMatch = true;
                gameManagerScript.nbCorrectWords++; // stats
            }
        }

        // We made a typo if none of the enemies words start with our input
        if (!Enemy.correctInput && !fullMatch && Input.inputString[0] != '\b') {
            Camera.main.SendMessage("Typo");
            // We count it as an error only if the last input wasn't one
            if(lastInput)
                GameManager.instance.nbErrors++;
        }
        
        // At least one hit
        if (fullMatch) {
            SetCurrentText("");
            lastInput = true;
        }
        else {
            lastInput = Enemy.correctInput;
        }
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
