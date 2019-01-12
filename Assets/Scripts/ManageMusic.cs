using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMusic : MonoBehaviour
{

	public AudioSource background_music;
	public AudioSource boss_music;
	public AudioSource gameover_music;

	private bool is_boss_already_trigger = false;
	private bool is_boss_dead = false;

	private AudioSource current_music;

	// Start is called before the first frame update
	void Start()
	{
		current_music = background_music;
		current_music.Play(0);
	}

	public void fightMusic()
	{
		if (!is_boss_already_trigger)
		{
			current_music.Stop();
			current_music = boss_music;
			current_music.Play(0);
			is_boss_already_trigger = true;
		}
	}

	public void ambienceMusic()
	{
		if (is_boss_already_trigger && !is_boss_dead)
		{
			current_music.Stop();
			current_music = background_music;
			current_music.Play(0);
			is_boss_dead = true;
		}
	}

	public void gameOverMusic()
	{
		current_music.Stop();
		current_music = gameover_music;
		current_music.Play();
	}
}
