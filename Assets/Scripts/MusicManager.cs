using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Usage example; FindObjectOfType<MusicManager>();
namespace Detection
{
	public class MusicManager : MonoBehaviour
	{
		public static MusicManager musicManager;
		public AudioMixerGroup audioMxrGroup;
		public List<Sound> musicList;
		public bool randomize = false;

		private int currentSongIndex = 0;
		private List<Sound> haventPlayedList;

		void Awake()
		{
			haventPlayedList = new List<Sound>();

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

				haventPlayedList.Add(sound);
			}
		}

		public Sound GetCurrentSong()
		{
			if (musicList.Count == 0) return null;
			else return musicList[currentSongIndex];
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

		public bool PlayNextSongInOrder()
		{
			if (currentSongIndex + 1 > musicList.Count) return false;

			if (currentSongIndex == 0)
			{
				musicList[currentSongIndex].source.Play();
				StartCoroutine(VerifyPlaying());
				return true;
			}

			FadeOut(musicList[currentSongIndex], 1);
			musicList[++currentSongIndex].source.Play();
			gameObject.GetComponent<MusicAnalyzer>().UpdateSongPlaying();
			Debug.Log("played song " + musicList[currentSongIndex].source.name);

			StartCoroutine(VerifyPlaying());
			return true;
		}

		public bool PlayNextSongRandom()
		{
			if (haventPlayedList.Count == 0) return false;

			var random = new System.Random();
			int index = random.Next(haventPlayedList.Count);
			haventPlayedList[index].source.Play();
			gameObject.GetComponent<MusicAnalyzer>().UpdateSongPlaying();
			Debug.Log("played song " + haventPlayedList[index].source.name);
			haventPlayedList.RemoveAt(index);

			gameObject.GetComponent<MusicAnalyzer>().UpdateSongPlaying();
			return true;
		}

		IEnumerator VerifyPlaying()
		{
			while (true)
			{
				yield return new WaitForSeconds(1f);
				if (!musicList[currentSongIndex].source.isPlaying)
				{
					if (randomize)
					{
						if (PlayNextSongRandom()) Debug.Log("switched to next random song");
						else Debug.Log("no songs left to play");
					}
					else
					{
						if (PlayNextSongInOrder()) Debug.Log("switched to next song");
						else Debug.Log("no songs left to play for this level");
					}
				}
			}
		}
	}
}
