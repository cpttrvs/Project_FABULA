using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour {

    [SerializeField] List<AudioClip> audioClips;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void PlayWordAudioClip(string word) {
        Debug.Log("Playing audio clip for " + word);
        audioSource.PlayOneShot(GetAudioClipByName(word + "_" + Glossary.language.ToString().ToLower()));
    }

    private AudioClip GetAudioClipByName(string name) {
        foreach(AudioClip clip in audioClips) {
            if(clip.name.Equals(name))
                return clip;
        }
        Debug.Log("ERROR: Audio Source not found for "+name);
        return null;
    }
}
