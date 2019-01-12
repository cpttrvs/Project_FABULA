using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBank : MonoBehaviour {

    [SerializeField] AudioClip knifeThrow;
    [SerializeField] AudioClip popErrorSound;
    [SerializeField] AudioClip foxCry;
    [SerializeField] AudioClip foxHowl;
    [SerializeField] AudioClip wolfCry;
    [SerializeField] AudioClip wolfHowl;
    [SerializeField] AudioClip dragonHit;
    [SerializeField] AudioClip dragonDeath;
    [SerializeField] AudioClip gorillaBark;
    [SerializeField] AudioClip gorillaGroan;
    [SerializeField] AudioClip droneHit;
    [SerializeField] AudioClip droneDestroyed;
    [SerializeField] AudioClip characterHurt_1;
    [SerializeField] AudioClip characterHurt_2;
    [SerializeField] AudioClip characterHurt_3;
    [SerializeField] AudioClip characterHurt_4;
    [SerializeField] AudioClip characterDying;
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

    private void WolfCry() {
        audioSource.PlayOneShot(wolfCry);
    }

    private void WolfHowl() {
        audioSource.PlayOneShot(wolfHowl);
    }

    private void DragonHit() {
        audioSource.PlayOneShot(dragonHit);
    }

    private void DragonDeath() {
        audioSource.PlayOneShot(dragonDeath);
    }

    private void GorillaBark() {
        audioSource.PlayOneShot(gorillaBark);
    }

    private void GorillaGroan() {
        audioSource.PlayOneShot(gorillaGroan);
    }

    private void DroneHit() {
        audioSource.PlayOneShot(droneHit);
    }

    private void DroneDestroyed() {
        audioSource.PlayOneShot(droneDestroyed);
    }

    private void CharacterHurt(int hp) {
        switch (hp) {
            case 4:
                audioSource.PlayOneShot(characterHurt_1);
                break;
            case 3:
                audioSource.PlayOneShot(characterHurt_2);
                break;
            case 2:
                audioSource.PlayOneShot(characterHurt_3);
                break;
            case 1:
                audioSource.PlayOneShot(characterHurt_4);
                break;
            case 0:
                audioSource.PlayOneShot(characterDying);
                break;
        }
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
