using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour {

    [SerializeField] AudioClip knifeThrow;
    [SerializeField] AudioClip popErrorSound;
    [SerializeField] AudioClip foxCry;
    [SerializeField] AudioClip foxHowl;
    [SerializeField] List<AudioClip> wordAudioClips;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FoxCry() {
        audioSource.PlayOneShot(foxCry);
    }

    private void FoxHowl() {
        audioSource.PlayOneShot(foxHowl);
    }

    private void Typo() {
        audioSource.PlayOneShot(popErrorSound);
    }

    private void ThrowKnife(){
        audioSource.PlayOneShot(knifeThrow);
    }

    private void PlayWordAudioClip(string filename) {
        Debug.Log("Playing audio clip for " + filename);
        audioSource.PlayOneShot(GetAudioClipByName(filename));
    }

    private AudioClip GetAudioClipByName(string name) {
        foreach(AudioClip clip in wordAudioClips) {
            if(clip.name.Equals(name))
                return clip;
        }
        Debug.Log("ERROR: Audio Source not found for "+name);
        return null;
    }
}
