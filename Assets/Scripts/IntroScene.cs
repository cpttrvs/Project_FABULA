using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {

	[SerializeField] Text titleUI;
	[SerializeField] Text startUI;
	[SerializeField] GameObject dragon;
	[SerializeField] GameObject player;
	[SerializeField] GameObject fadeQuad;
	// Keyboard sounds
	[SerializeField] List<AudioClip> audioClips;

	string targetText;
	bool freeze = false;
	bool zoomIn = false;
	Color fadeColor;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		targetText = titleUI.text;
		startUI.gameObject.SetActive(false);
		fadeColor = fadeQuad.GetComponent<MeshRenderer>().material.color;
		audioSource = Camera.main.GetComponent<AudioSource>();
		StartCoroutine(AnimateUI());
	}
	
	// Update is called once per frame
	void Update () {
		// Non Keypad ENTER pressed
		if(Input.GetKeyDown(KeyCode.Return)){
			zoomIn = true;
			//titleUI.gameObject.SetActive(false);
			//startUI.gameObject.SetActive(false);
		}
		if(zoomIn){
			/*
			// The camera rotates towards the player's head
			Quaternion rotation = Quaternion.LookRotation(player.transform.position+new Vector3(0, 2.5f, 0) - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2.5f * Time.deltaTime);
			// The camera zooms in
			if(Camera.main.fieldOfView > 1)
				Camera.main.fieldOfView--;
			*/
			// The screen turns black
			if(fadeColor.a < 1){
				fadeColor.a += Time.deltaTime;
				GetComponentInChildren<CanvasGroup>().alpha -= Time.deltaTime;
				fadeQuad.GetComponent<MeshRenderer>().material.color = fadeColor;
			}
			// We switch scenes
			else if(fadeColor.a > 1){
				SceneManager.LoadScene("IceLevel");
			}
		}

		if(!freeze)
			dragon.transform.Translate(0, 0, Time.deltaTime * 5f);
	}

	IEnumerator AnimateUI()
    {
		yield return new WaitForSeconds(2f);
		// Colors the title's letters just like during the gameplay
		for(int i=1; i<targetText.Length+1; i++){
			yield return new WaitForSeconds(0.4f);
			// If we aren't changing scenes
			if(!zoomIn){
        		audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
			}
			// IMO this way there's a better visual
			yield return new WaitForSeconds(0.1f);
			titleUI.text = Utility.GetWordColoring(targetText, targetText.Substring(0, i));
		}
		// We freeze the dragon
		freeze = true;
		yield return new WaitForSeconds(1f);
		// Blinking PRESS ENTER text
		while(!zoomIn){
			startUI.gameObject.SetActive(true);
			yield return new WaitForSeconds(1f);
			startUI.gameObject.SetActive(false);
			yield return new WaitForSeconds(1f);
		}
    }
}
