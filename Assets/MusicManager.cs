using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	public static MusicManager musicManager;
	public AudioMixerGroup audioMxrGroup;
	public List<Sound> musicList;
	private int currentSongIndex = 0;
	void Awake()
	{
		// Ensure only one manager exists
		if (musicManager == null)
		{
			musicManager = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);

		foreach (Sound sound in musicList)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.loop = sound.loop;

			sound.source.outputAudioMixerGroup = audioMxrGroup;
		}
	}

	public static IEnumerator FadeOut(Sound sound, float FadeTime)
	{
		float startVolume = sound.volume;

		while (sound.volume > 0)
		{
			sound.volume -= startVolume * Time.deltaTime / FadeTime;

			yield return null;
		}

		sound.source.Stop();
		sound.volume = startVolume;
	}

	public bool PlayNextSongInList()
	{
		if (currentSongIndex + 1 > musicList.Count) return false;

		FadeOut(musicList[currentSongIndex], 1);
		currentSongIndex += 1;
		musicList[currentSongIndex].source.Play();
		
		StartCoroutine(VerifyPlaying());
		return true;
	}

	IEnumerator VerifyPlaying()
	{
		while (true)
		{
			yield return new WaitForSeconds(1f);
			if (!musicList[currentSongIndex].source.isPlaying)
            {
				if (PlayNextSongInList()) Debug.Log("switched to next song");
				else Debug.Log("no songs left to play for this level");
			}
		}
	}
}
