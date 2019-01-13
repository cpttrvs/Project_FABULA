using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMusic : MonoBehaviour
{

	[SerializeField] AudioClip background_music;
    [SerializeField] AudioClip boss_music;
    [SerializeField] AudioClip gameover_music;

	private bool is_boss_already_trigger = false;
	private bool is_boss_dead = false;

	private AudioSource audio_source;

	// Start is called before the first frame update
	void Start()
	{
        audio_source = GetComponent<AudioSource>();
        audio_source.clip = background_music;
        audio_source.time = 7.5f;
		audio_source.Play();
	}

	public void fightMusic()
	{
		if (!is_boss_already_trigger)
		{
            audio_source.clip = boss_music;
            audio_source.time = 7.5f;
            audio_source.Play();
			is_boss_already_trigger = true;
		}
	}

	public void ambienceMusic()
	{
		if (is_boss_already_trigger && !is_boss_dead)
		{
            audio_source.clip = background_music;
            audio_source.Play();
			is_boss_dead = true;
		}
	}

	public void gameOverMusic()
	{
        audio_source.time = 7.5f;
        audio_source.clip = gameover_music;
        audio_source.Play();
	}
}
