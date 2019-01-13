using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public static bool GamePaused = false;

	[SerializeField] GameObject gameUI;
	[SerializeField] GameObject pauseMenuUI;
	[SerializeField] GameObject wordDisplayUI;
    [SerializeField] GameObject finishUI;

    [SerializeField] GameObject movementText;
    [SerializeField] GameObject enterText;

    [SerializeField] AudioSource music;

    // Contains the scrollBar and the scrollableList
    private GameObject layoutPanel;
	private GameObject scrollBar;
	private GameObject scrollableList;
	private GameObject modelPanel;
	// List of the words added to the scrollable list
	private List<Utility.Word> scrollableWords;

	void Start () {
		layoutPanel = wordDisplayUI.transform.Find("LayoutPanel").gameObject;
		scrollableList = layoutPanel.transform.Find("ScrollableList").gameObject;
		scrollBar = layoutPanel.transform.Find("Scrollbar").gameObject;
		modelPanel = scrollableList.transform.Find("InfoPanel").gameObject;
		scrollableWords = new List<Utility.Word>();
		//UpdateUITexts();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
            if (GamePaused) {
                Resume();
            }
            else {
                Pause();
            }
		}

        if(GameManager.instance.gameover)
        {
            if(!wordDisplayUI.gameObject.activeSelf)
                Finish();
        }
	}

	public void UpdateLanguage(string language){
		switch(language){
			case "FR":
				Glossary.language = Glossary.Language.FR;
				break;
			case "EN":
				Glossary.language = Glossary.Language.EN;
				break;
		}
		UpdateUITexts();
	}

	private void UpdateUITexts(){
		pauseMenuUI.transform.Find("ResumeButton").GetComponentInChildren<Text>().text = Glossary.STRING_RESUME;
		pauseMenuUI.transform.Find("Language").GetComponentInChildren<Text>().text = Glossary.STRING_LANGUAGE;
		pauseMenuUI.transform.Find("VocabularyButton").GetComponentInChildren<Text>().text = Glossary.STRING_VOCABULARY;
		pauseMenuUI.transform.Find("QuitButton").GetComponentInChildren<Text>().text = Glossary.STRING_QUIT;

        if(GameManager.instance.finished)
            finishUI.transform.Find("finishMessage").GetComponent<Text>().text = Glossary.STRING_WIN;
        else
            finishUI.transform.Find("finishMessage").GetComponent<Text>().text = Glossary.STRING_LOOSE;
        finishUI.transform.Find("finishWPM").GetComponent<Text>().text = Glossary.STRING_WPM + GameManager.instance.wpm;
        finishUI.transform.Find("finishNbCorrectWords").GetComponent<Text>().text = Glossary.STRING_NBCORRECTWORDS + GameManager.instance.nbCorrectWords;
        finishUI.transform.Find("finishNbErrors").GetComponent<Text>().text = Glossary.STRING_NBERRORS + GameManager.instance.nbErrors;

        finishUI.transform.Find("RestartButton").GetComponentInChildren<Text>().text = Glossary.STRING_RESTART;
        finishUI.transform.Find("VocabularyButton").GetComponentInChildren<Text>().text = Glossary.STRING_VOCABULARY;
        finishUI.transform.Find("QuitButton").GetComponentInChildren<Text>().text = Glossary.STRING_QUIT;
        
        wordDisplayUI.transform.Find("BackButton").GetComponentInChildren<Text>().text = Glossary.STRING_BACK;
		// Needed for the ForceRebuildLayoutImmediate, doesn't seem to work on inactive objects
		wordDisplayUI.SetActive(true);
		// Now we have to update all the buttons so they fit the new text sizes;
		foreach(Button btn in GetComponentsInChildren<Button>(true)){
			LayoutRebuilder.ForceRebuildLayoutImmediate(btn.GetComponent<RectTransform>());
		}
		wordDisplayUI.SetActive(false);

        movementText.GetComponent<TextMesh>().text = Glossary.STRING_MOVEMENT;
        enterText.GetComponent<TextMesh>().text = Glossary.STRING_ENTER;
	}

	public void Resume() {
        music.volume = 0.8f;
        GamePaused = false;
		pauseMenuUI.SetActive(false);
        finishUI.SetActive(false);
		wordDisplayUI.SetActive(false);
		gameUI.SetActive(true);
		// Default time scale
		Time.timeScale = 1f;	
	}

	public void Pause() {
        music.volume = 0.4f;
        wordDisplayUI.SetActive(false);
		gameUI.SetActive(true);
		pauseMenuUI.SetActive(true);
		// Only if we came from the game and not from other menus
		if(!GamePaused){
			GamePaused = true;
			pauseMenuUI.GetComponent<Animator>().SetTrigger("pause");
		}
		// Freeze time/game
		Time.timeScale = 0f;
		// Change the texts depending on the game's language
		UpdateUITexts();
	}

    public void Finish()
    {
        pauseMenuUI.SetActive(false);
        wordDisplayUI.SetActive(false);
        gameUI.SetActive(false);
        finishUI.SetActive(true);

        if (GameManager.instance.finished)
            finishUI.transform.Find("finishMessage").GetComponent<Text>().text = Glossary.STRING_WIN + "(" + GameManager.instance.timer.ToString("0.00") + ")";
        else
            finishUI.transform.Find("finishMessage").GetComponent<Text>().text = Glossary.STRING_LOOSE + "(" + GameManager.instance.timer.ToString("0.00") + ")";
        finishUI.transform.Find("finishWPM").GetComponent<Text>().text = Glossary.STRING_WPM + GameManager.instance.wpm.ToString("0.0");
        finishUI.transform.Find("finishNbCorrectWords").GetComponent<Text>().text = Glossary.STRING_NBCORRECTWORDS + GameManager.instance.nbCorrectWords;
        finishUI.transform.Find("finishNbErrors").GetComponent<Text>().text = Glossary.STRING_NBERRORS + GameManager.instance.nbErrors;

    }

    public void QuitGame(){
		Application.Quit();
	}

	public void ShowVocabulary() {
        music.volume = 0.2f;
        pauseMenuUI.SetActive(false);
		gameUI.SetActive(false);
        finishUI.SetActive(false);
		wordDisplayUI.SetActive(true);
		AddInfoPanels();
		HandleScrollbar();
	}

    public void Restart()
    {
        SceneManager.LoadScene("IceLevel");
    }

	// Hides/Shows the scrollbar depending on the size of the content
	private void HandleScrollbar(){
		//Debug.Log("Scroll: "+layoutPanel.GetComponent<ScrollRect>().verticalNormalizedPosition);
		if(layoutPanel.GetComponent<ScrollRect>().verticalNormalizedPosition > 0){
			layoutPanel.GetComponent<ScrollRect>().verticalScrollbar = scrollBar.GetComponent<Scrollbar>();
			scrollBar.SetActive(true);
		}
		else{
			layoutPanel.GetComponent<ScrollRect>().verticalScrollbar = null;
			scrollBar.SetActive(false);
		}
	}

	// Adds as many info panels as there were correct words
	private void AddInfoPanels(){
		/*
			Tried to change the info panel's language depending on the language of the game.
			To be able to do that we would've to change the whole implementation.
			Because for now we store the user's correct words and look for those words in the vocabulary to get their definitions.
			If you change the language throughout the game you won't be able to find the word's definition in the vocabulary since it's the wrong vocabulary language.
			To resolve that we would've to store the user's correct words + the current language of that word. 
			So that's a TODO!

			There's also the fact that if we were to change the language of the game when an enemy already spawned with its predefined words we get errors, for the same reason as above.
		*/

		/*
		// 1 'cause we have the model panel that is hidden
		if(scrollableList.transform.childCount > 1){
			// We delete them to recreate them, it's easier to handle the language being changed
			foreach(Transform infoPanel in scrollableList.transform){
				if(!infoPanel.gameObject.Equals(modelPanel.gameObject))
					Destroy(infoPanel.gameObject);
			}
		}
		*/
		// Could've been desactivated the last time
		modelPanel.SetActive(true);
		List<Utility.Word> playersWords = GameObject.FindGameObjectWithTag("Player").GetComponent<Controller>().GetCorrectWords();
		if(playersWords != null){
			for(int i=0; i<playersWords.Count; i++){
				// We only add a panel if the word isn't already present in the list
				if(!scrollableWords.Contains(playersWords.ElementAt(i))){
					scrollableWords.Add(playersWords.ElementAt(i));
					GameObject newPanel = Instantiate(modelPanel);
					// We add the panel to the scrollable list
					newPanel.transform.SetParent(scrollableList.transform, false);
					PopulateInfoPanel(newPanel, playersWords.ElementAt(i));
				}
			}
			// We disable our reference panel
			modelPanel.SetActive(false);
		}
	}

	// Populates the info panel with the word, its definition...
	private void PopulateInfoPanel(GameObject panel, Utility.Word wordInfo){
		Text[] texts = panel.GetComponentsInChildren<Text>();
		Text wordTxt = texts[0];
		Text translationTxt = texts[1];
		Text defintionTxt = texts[2];
		wordTxt.text = Utility.ToUpperOnFirstLetter(wordInfo.name);
		translationTxt.text = Utility.ToUpperOnFirstLetter(wordInfo.translation);
		defintionTxt.text = wordInfo.definition;

		string wordLanguage = wordInfo.language.ToString().ToLower();
		string translationLanguage = "";
		if(wordLanguage.Equals("fr"))
			translationLanguage = "en";
		else if(wordLanguage.Equals("en"))
			translationLanguage = "fr";

        Button[] buttons = panel.GetComponentsInChildren<Button>();
        Button wordBtn = buttons[0];
        Button translationBtn = buttons[1];
        wordBtn.onClick.AddListener(() => SpeakWord(wordInfo.name+"_"+wordLanguage));
        translationBtn.onClick.AddListener(() => SpeakWord(wordInfo.translation+"_"+translationLanguage));
    }

    public void SpeakWord(string word) {
        Camera.main.SendMessage("PlayWordAudioClip", word);
    }
}
