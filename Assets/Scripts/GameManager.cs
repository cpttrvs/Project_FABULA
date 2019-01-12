using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	private Camera camera;
	[SerializeField] Canvas canvas;
	[SerializeField] GameObject fadeQuad;

	//player
	[SerializeField] GameObject playerGO;
	private Player player;
	private Controller controller;

	//enemies
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] GameObject foxPrefab;
	[SerializeField] GameObject wolfPrefab;
	[SerializeField] GameObject dragonPrefab;
	[SerializeField] GameObject dronePrefab;
	[SerializeField] GameObject gorillaPrefab;
	List<GameObject> enemies = new List<GameObject>();
	List<GameObject> visibleEnemies = new List<GameObject>();

	//UI
	private Canvas canvasUI;
    private Image writingImageUI;
    private Image writingBackgroundUI;
	private Text playerTextUI;
	private Text playerLifeUI;
	private Text statsNbErrors;
	private Text statsNbCorrectWords;
    private Text timerUI;

	//Stats
	public float timer = 0.0f;
	public int nbErrors = 0;
	public int nbCorrectWords = 0;
    public float wpm = 0f;

    //Game
	public bool gameover = false;
    public bool finished = false;
    [SerializeField] float timeScaleWriting = 0.75f;
    private float currentTimeScale;

	//Music
	public GameObject musicController;

	void Start () {
		StartCoroutine(FadeIn());
		Time.timeScale = 1f;
		camera = Camera.main;
		instance = this;
		Utility.InitializeVocabularyList();

		if(playerGO == null) {
			Debug.Log("GameManager : no player gameobject");
		} else {
			player = playerGO.GetComponent<Player>();
			if(player == null) Debug.Log("GameManager : no player");
			controller = playerGO.GetComponent<Controller>();
			if(controller == null) Debug.Log("GameManager : no controller");
		}
		if(camera == null) {
			Debug.Log("GameManager : no camera");
		} else {
			canvasUI = camera.GetComponentInChildren<Canvas>();
			if(canvasUI == null) {
				Debug.Log("GameManager : no canvas in camera");
			} else {
				foreach(Text t in canvasUI.GetComponentsInChildren<Text>()) {
					if(t.name == "playerText") {
                        playerTextUI = t;
					}
					else if(t.name == "playerLife") {
                        playerLifeUI = t;
                    }
					else if(t.name == "statsNbCorrectWords") {
                        statsNbCorrectWords = t;
                    }
					else if(t.name == "statsNbErrors") {
                        statsNbErrors = t;
                    }
                    else if(t.name == "timer")
                    {
                        timerUI = t;
                    }
				}
                foreach(Image i in canvasUI.GetComponentsInChildren<Image>())
                {
                    if (i.name == "writingImage")
                    {
                        writingImageUI = i;
                        writingImageUI.gameObject.SetActive(false);
                    }
                    else if(i.name == "writingBackground")
                    {
                        writingBackgroundUI = i;
                        writingBackgroundUI.gameObject.SetActive(false);
                    }
                }
			}
		}


		if(playerTextUI == null) { Debug.Log("GameManager : no playerText");}
		if(playerLifeUI == null) { Debug.Log("GameManager : no playerLife");}
		if(statsNbCorrectWords == null) { Debug.Log("GameManager : no statsNbCorrectWords");}
		if(statsNbErrors == null) { Debug.Log("GameManager : no statsNbErrors"); }
        if(timerUI == null) { Debug.Log("GameManager : no timer"); }
        if(writingImageUI == null) { Debug.Log("GameManager : no writingImage"); }
    }
	
	void Update () {
        // TESTING
		if(SceneManager.GetActiveScene().name.Equals("TestScene")){
			if(Input.GetKeyDown(KeyCode.F1)){
				InstantiateEnemy(Enemy.Type.GORILLA);
			}
			else if(Input.GetKeyDown(KeyCode.F2)){
				InstantiateEnemy(Enemy.Type.DRONE);
			}
			// Spawns one of each
			else if(Input.GetKeyDown(KeyCode.F3)){
				// No idea how to foreach on Enum...
				InstantiateEnemy(Enemy.Type.FOX);
				InstantiateEnemy(Enemy.Type.WOLF);
				InstantiateEnemy(Enemy.Type.DRAGON);
				InstantiateEnemy(Enemy.Type.DRONE);
				InstantiateEnemy(Enemy.Type.GORILLA);
			}
		}

		playerTextUI.text = controller.GetCurrentText();
		playerLifeUI.text = Glossary.STRING_LIFE + player.GetCurrentLife().ToString();
		statsNbCorrectWords.text = Glossary.STRING_NBCORRECTWORDS + nbCorrectWords;
		statsNbErrors.text = Glossary.STRING_NBERRORS + nbErrors;
        
		timer += Time.deltaTime;
        
		timerUI.text = timer.ToString("0.0");
        wpm = nbCorrectWords / (timer / 60f);

		if (enemies.Count == 0)
			musicController.GetComponent<ManageMusic>().ambienceMusic();
	}

	IEnumerator FadeIn(){
		Color fadeColor = fadeQuad.GetComponent<MeshRenderer>().material.color;
		CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
		// This time we start from black and go transparent
		fadeColor.a = 1;
		canvasGroup.alpha = 0;
		fadeQuad.GetComponent<MeshRenderer>().material.color = fadeColor;
		while(fadeColor.a > 0){
			fadeColor.a -= Time.deltaTime;
			canvasGroup.alpha += Time.deltaTime;
			fadeQuad.GetComponent<MeshRenderer>().material.color = fadeColor;
			yield return null;
		}
		yield return null;
	}

	public void InstantiateEnemy(Enemy.Type type){
		GameObject enemy;
		// Change the prefab depending on the enemy
		switch(type){
			case Enemy.Type.FOX:
				enemy = Instantiate(foxPrefab);
				break;
			case Enemy.Type.WOLF:
				enemy = Instantiate(wolfPrefab);
				break;
			case Enemy.Type.DRAGON:
				enemy = Instantiate(dragonPrefab);
				break;
			case Enemy.Type.DRONE:
				enemy = Instantiate(dronePrefab);
				break;
			case Enemy.Type.GORILLA:
				enemy = Instantiate(gorillaPrefab);
				break;
			default:
				enemy = Instantiate(enemyPrefab);
				break;
		}
		// TODO: CHANGE THAT!
		if(Random.value < 0.5f)
			enemy.transform.position = new Vector3(Random.Range(-10f, -12f), enemy.transform.position.y, Random.Range(8f, 10f));
		else
			enemy.transform.position = new Vector3(Random.Range(10f, 12f), enemy.transform.position.y, Random.Range(-8f, -10f));
		Enemy enemyScript = enemy.GetComponentInChildren<Enemy>();
		// Generate the enemy's vocabulary depending on its type
		enemyScript.Initiate(type);
	}

    public void InstantiateEnemy(Enemy.Type type, Vector3 pos)
    {
        GameObject enemy;
        // Change the prefab depending on the enemy
        switch (type)
        {
            case Enemy.Type.FOX:
                enemy = Instantiate(foxPrefab, new Vector3(pos.x, foxPrefab.transform.position.y, pos.z), Quaternion.identity);
                break;
            case Enemy.Type.WOLF:
                enemy = Instantiate(wolfPrefab, new Vector3(pos.x, wolfPrefab.transform.position.y, pos.z), Quaternion.identity);
                break;
            case Enemy.Type.DRAGON:
                enemy = Instantiate(dragonPrefab, new Vector3(pos.x, dragonPrefab.transform.position.y, pos.z), Quaternion.identity);
                break;
            case Enemy.Type.DRONE:
                enemy = Instantiate(dronePrefab, new Vector3(pos.x, dronePrefab.transform.position.y, pos.z), Quaternion.identity);
                break;
            case Enemy.Type.GORILLA:
                enemy = Instantiate(gorillaPrefab, new Vector3(pos.x, gorillaPrefab.transform.position.y, pos.z), Quaternion.identity);
                break;
            default:
                enemy = Instantiate(enemyPrefab, new Vector3(pos.x, enemyPrefab.transform.position.y, pos.z), Quaternion.identity);
                break;
        }
        Enemy enemyScript = enemy.GetComponentInChildren<Enemy>();
        // Generate the enemy's vocabulary depending on its type
        enemyScript.Initiate(type);
    }

    public void UpdateVisibility(GameObject go) {
		bool v = go.GetComponentInChildren<Enemy>().isVisible();
		
		//update the list of visible enemies
		if(v && !visibleEnemies.Contains(go)) {
			visibleEnemies.Add(go);
		}
		if(!v && visibleEnemies.Contains(go)) {
			visibleEnemies.Remove(go);
		}
	}

	public List<GameObject> GetVisibleEnemies(){
		return visibleEnemies;
	}

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

	public void AddEnemy(GameObject go) {
		if(!enemies.Contains(go)) {
			enemies.Add(go);
		}
	}

	public void RemoveEnemy(GameObject go) {
		if(enemies.Contains(go)) {
			enemies.Remove(go);
			if(visibleEnemies.Contains(go))
				visibleEnemies.Remove(go);
		}
	}

    public void WritingImage(bool state)
    {
        writingImageUI.gameObject.SetActive(state);
        //writingBackgroundUI.gameObject.SetActive(state);
        //if state is true, show the image and slow down deltaTime
        if (state)
        {
            Time.timeScale = timeScaleWriting;
            currentTimeScale = timeScaleWriting;
        } else
        {
            Time.timeScale = 1f;
            currentTimeScale = 1f;
        }
    }

	private void PlayerDied(){
		gameover = true;
		Time.timeScale = 0f;
		foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")){
			enemy.GetComponentInChildren<Enemy>().SendMessage("GameOver");
		}

		musicController.GetComponent<ManageMusic>().gameOverMusic();
	}
}
